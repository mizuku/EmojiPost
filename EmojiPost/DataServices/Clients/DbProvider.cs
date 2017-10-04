using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmojiPost.DataServices.Clients
{
    /// <summary>
    /// SQLite データベースアクセスのプロバイダー
    /// </summary>
    public class DbProvider : IDisposable
    {
        #region Properties

        /// <summary>
        /// データベースのタイムアウト時間
        /// </summary>
        public int Timeout { get; private set; }

        /// <summary>
        /// データベース接続
        /// </summary>
        private DbConnection Db { get => this.SQLite; }

        /// <summary>
        /// データベーストランザクション
        /// </summary>
        private DbTransaction Transaction { get; set; }

        /// <summary>
        /// SQLiteデータベース接続
        /// </summary>
        private SQLiteConnection SQLite { get; set; }

        #endregion

        #region Methods

        #region Execute

        /// <summary>
        /// SQLステートメントを実行します。
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="parameters">バインドパラメータ</param>
        /// <returns>実行結果</returns>
        public int ExecuteNonQuery(string sql, params (object value, DbType type)[] parameters)
        {
            using (var cmd = this.Db.CreateCommand())
            {
                this.BindParameters(cmd, sql, parameters);
                return cmd.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// SQLステートメントを実行します。ExecuteNonQueryの非同期版です。
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="parameters">バインドパラメータ</param>
        /// <returns>実行結果</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, params (object value, DbType type)[] parameters)
        {
            using (var cmd = this.Db.CreateCommand())
            {
                this.BindParameters(cmd, sql, parameters);
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// クエリを実行し、結果セットの列挙を取得します。
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="parameters">バインドパラメータ</param>
        /// <returns>結果セットの列挙</returns>
        public IEnumerable<ResultValue[]> ExecuteReader(string sql, params (object value, DbType type)[] parameters)
        {
            using (var cmd = this.Db.CreateCommand())
            {
                this.BindParameters(cmd, sql, parameters);
                using (var cursor = cmd.ExecuteReader())
                {
                    return this.FetchQueryResults(cursor);
                }
            }
        }

        /// <summary>
        /// クエリを実行し、結果セットの列挙を取得します。ExecuteReaderの非同期版です。
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="parameters">バインドパラメータ</param>
        /// <returns>結果セットの列挙</returns>
        public async Task<IEnumerable<ResultValue[]>> ExecuteReaderAsync(string sql, params (object value, DbType type)[] parameters)
        {
            using (var cmd = this.Db.CreateCommand())
            {
                this.BindParameters(cmd, sql, parameters);
                using (var cursor = await cmd.ExecuteReaderAsync())
                {
                    return this.FetchQueryResults(cursor);
                }
            }
        }

        /// <summary>
        /// クエリを実行し、結果の最初の列の最初の行を取得します。
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="parameters">バインドパラメータ</param>
        /// <returns>実行結果の最初の列の最初の行</returns>
        public object ExecuteScalar(string sql, params (object value, DbType type)[] parameters)
        {
            using (var cmd = this.Db.CreateCommand())
            {
                this.BindParameters(cmd, sql, parameters);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// クエリを実行し、結果の最初の列の最初の行を取得します。ExecuteScalarの非同期版です。
        /// </summary>
        /// <param name="sql">SQL文字列</param>
        /// <param name="parameters">バインドパラメータ</param>
        /// <returns>実行結果の最初の列の最初の行</returns>
        public async Task<object> ExecuteScalarAsync(string sql, params (object value, DbType type)[] parameters)
        {
            using (var cmd = this.Db.CreateCommand())
            {
                this.BindParameters(cmd, sql, parameters);
                return await cmd.ExecuteScalarAsync();
            }
        }


        /// <summary>
        /// コマンドにパラメータを設定します。
        /// </summary>
        /// <param name="command">DbCommand</param>
        /// <param name="sql">SQL文字列</param>
        /// <param name="parameters">バインドパラメータ</param>
        private void BindParameters(DbCommand command, string sql, params (object value, DbType type)[] parameters)
        {
            command.CommandText = sql;
            // パラメータを設定する
            if (null == parameters || !parameters.Any()) return;
            foreach (var (i, value, type) in parameters.Select((t, i) => (i, t.value, t.type)))
            {
                DbParameter param = command.CreateParameter();
                param.ParameterName = $"@{i}";
                param.Value = value;
                param.DbType = type;

                command.Parameters.Add(param);
            }
        }

        /// <summary>
        /// カーソルから順にクエリの結果を取得します。
        /// </summary>
        /// <param name="cursor">カーソル</param>
        /// <returns>クエリの結果</returns>
        private IEnumerable<ResultValue[]> FetchQueryResults(DbDataReader cursor)
        {
            List<ResultValue[]> resultSet = new List<ResultValue[]>();
            while (cursor.Read())
            {
                var rs = new ResultValue[cursor.FieldCount];
                for (int i = 0; i < cursor.FieldCount; i++)
                {
                    var v = new ResultValue();
                    v.Value = cursor.GetValue(i);
                    v.Name = cursor.GetName(i);
                    v.ValueType = cursor.GetFieldType(i);
                    rs[i] = v;
                }

                resultSet.Add(rs);
            }

            return resultSet;
        }

        #endregion

        #region Transaction

        /// <summary>
        /// トランザクションを開始します。
        /// </summary>
        public void BeginTransaction()
        {
            if (null != this.Transaction)
            {
                // ２重でトランザクションはNG
                throw new NotSupportedException("複数のトランザクションを同時に開始することはできません。");
            }
            this.Transaction = this.Db.BeginTransaction();
        }

        /// <summary>
        /// 現在のトランザクションをコミットします。
        /// </summary>
        public void Commit()
        {
            if (null != this.Transaction)
            {
                this.Transaction.Commit();
                this.Transaction.Dispose();
                this.Transaction= null;
            }
        }

        /// <summary>
        /// 現在のトランザクションをロールバックします。
        /// </summary>
        public void Rollback()
        {
            if (null != this.Transaction)
            {
                this.Transaction.Rollback();
                this.Transaction.Dispose();
                this.Transaction = null;
            }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// データソース名を指定して、このクラスのインスタンスを生成します。
        /// </summary>
        /// <param name="dataSource"></param>
        public DbProvider(string dataSource)
            : this(dataSource, 60)
        {
        }

        /// <summary>
        /// データソース名とタイムアウト時間を指定して、このクラスのインスタンスを生成します。
        /// </summary>
        public DbProvider(string dataSource, int timeout)
        {
            var db = new SQLiteConnection($"Data Source={dataSource}");
            this.Timeout = timeout;
            db.DefaultTimeout = timeout;
            db.Open();

            this.SQLite = db;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                }

                // アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // 大きなフィールドを null に設定します。
                if (null != this.Transaction)
                {
                    this.Transaction.Dispose();
                    this.Transaction = null;
                }
                if (null != this.SQLite)
                {
                    this.SQLite.Close();
                    this.SQLite.Dispose();
                    this.SQLite = null;
                }
                disposedValue = true;
            }
        }

        ~DbProvider()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(false);
        }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
