using System.IO;

using AyaStyle.DataServices.Clients;

namespace AyaStyle.DataServices.Services
{

    /// <summary>
    /// ストレージサービスのインタフェース
    /// </summary>
    public interface IStorageService
    {
        #region Methods

        /// <summary>
        /// ストレージを初期化します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        void Create(DbProvider db);

        /// <summary>
        /// ストレージ内のテーブルを捨てます。
        /// </summary>
        /// <param name="db"></param>
        void Drop(DbProvider db);

        /// <summary>
        /// ストリームからデータをストレージへ書き込みます。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="stream">入力ストリーム</param>
        void Import(DbProvider db, Stream stream);

        /// <summary>
        /// ストレージの内容をストリームに書き込みます。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="stream">出力ストリーム</param>
        void Export(DbProvider db, Stream stream);

        #endregion

    }
}
