using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmojiPost.DataServices.Utils
{
    /// <summary>
    /// SQL作成ヘルパー
    /// </summary>
    public static class SqlHelper
    {

        #region Extension Methods

        /// <summary>
        /// string から DbType への変換を行います。
        /// </summary>
        /// <param name="s">文字列</param>
        /// <returns>DbType</returns>
        public static DbType ToDbType(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException();
            string typeName = s.ToLowerInvariant();

            switch (typeName)
            {
                case "integer":
                case "int":
                case "number":
                    return DbType.Int32;
                case "blob":
                    return DbType.Binary;
                case "double":
                case "float":
                case "real":
                    return DbType.Double;
                default:
                    return DbType.String;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// テーブルCreate文を作成します。
        /// </summary>
        /// <param name="type">Createするテーブルのエンティティモデルの型。</param>
        /// <returns>Create文を表す文字列</returns>
        /// <remarks>Key属性, Required属性, Column属性にのみ対応</remarks>
        public static string MakeCreateDDL(Type type)
        {
            var tableAttr = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            if (null == tableAttr)
            {
                throw new ArgumentException();
            }

            var attrs = GetSortedColumns(type);

            var columns = new List<string>();
            var keys = new List<string>();
            foreach (var attr in attrs)
            {
                string nullable = (null == attr.Item2 && null == attr.Item3) ? "" : " NOT NULL";
                columns.Add($"{attr.Item1.Name} {attr.Item1.TypeName}{nullable}");

                if (null != attr.Item2)
                {
                    keys.Add(attr.Item1.Name);
                }
            }

            string primaryKey = string.Empty;
            if (keys.Any())
            {
                primaryKey = $", PRIMARY KEY({string.Join(",", keys)})";
            }
            return $"CREATE TABLE {tableAttr.Name}({string.Join(", ", columns)}{primaryKey})";
        }

        /// <summary>
        /// テーブルDrop文を作成します。
        /// </summary>
        /// <param name="type">Dropするテーブルのエンティティモデルの型。</param>
        /// <returns>Drop文を表す文字列</returns>
        public static string MakeDropDDL(Type type)
        {
            var tableAttr = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            if (null == tableAttr)
            {
                throw new ArgumentException();
            }

            return $"DROP TABLE {tableAttr.Name}";
        }

        /// <summary>
        /// テーブルInsert文を作成します。
        /// </summary>
        /// <param name="type">Insertするテーブルのエンティティモデルの型。</param>
        /// <returns>Insert文を表す文字列</returns>
        public static string MakeInsertDML(Type type)
        {
            var tableAttr = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            if (null == tableAttr)
            {
                throw new ArgumentException();
            }

            var attrs = GetSortedColumns(type);
            var columns = attrs.Select(a => a.Item1.Name).ToArray();
            var binds = attrs.Select((a, i) => $"@{i}").ToArray();

            return $"INSERT INTO {tableAttr.Name}({string.Join(", ", columns)}) VALUES({string.Join(", ", binds)})";
        }

        /// <summary>
        /// テーブルUpdate文を作成します。
        /// </summary>
        /// <param name="type">Updateするテーブルのエンティティモデルの型。</param>
        /// <returns>Update文を表す文字列</returns>
        public static string MakeUpdateDML(Type type)
        {
            var tableAttr = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            if (null == tableAttr)
            {
                throw new ArgumentException();
            }

            var attrs = GetSortedColumns(type);

            var columns = new List<string>();
            var keys = new List<string>();
            foreach (var attr in attrs.Select((a, i) => new { i, a }))
            {
                bool isKey = (null != attr.a.Item2);
                string s = $"{attr.a.Item1.Name} = @{attr.i}";
                if (isKey)
                {
                    keys.Add(s);
                }
                else
                {
                    columns.Add(s);
                }
            }

            return $"UPDATE {tableAttr.Name} SET {string.Join(", ", columns)} WHERE {string.Join(" AND ", keys)}";
        }

        /// <summary>
        /// エンティティからソート済みのバインドパラメータを取得します。
        /// </summary>
        /// <typeparam name="T">エンティティの型</typeparam>
        /// <param name="entity">バインドパラメータを取得するエンティティ</param>
        /// <returns>エンティティから取り出したパラメータ</returns>
        public static (object, DbType)[] GetEntityBindparameters<T>(T entity)
        {
            if (null == entity)
            {
                throw new ArgumentException();
            }

            // 速度的にアレなのでExpressionTreeをコンパイルしてDictionary<Type, ExpressionTree>にキャッシュしたい
            var attrs = GetSortedColumns(typeof(T));
            return attrs.Select(a => (a.Item4.GetValue(entity), a.Item1.TypeName.ToDbType())).ToArray();
        }

        /// <summary>
        /// 型を指定し、その型のプロパティが持つColumnをOrderの順序で取得します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static (ColumnAttribute, KeyAttribute, RequiredAttribute, PropertyInfo)[] GetSortedColumns(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            var columns = new List<(ColumnAttribute, KeyAttribute, RequiredAttribute, PropertyInfo)>();
            foreach (var p in properties)
            {
                var columnAttr = p.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                if (null != columnAttr)
                {
                    var keyAttr  = p.GetCustomAttribute(typeof(KeyAttribute)) as KeyAttribute;
                    var requiredAttr = p.GetCustomAttribute(typeof(RequiredAttribute)) as RequiredAttribute;
                    columns.Add((columnAttr, keyAttr, requiredAttr, p));
                }
            }
            if (!columns.Any())
            {
                throw new ArgumentException();
            }
            return columns.OrderBy(a => a.Item1.Order).ToArray();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static SqlHelper()
        {
        }

        #endregion

    }
}
