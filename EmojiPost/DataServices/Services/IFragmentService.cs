using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmojiPost.DataServices.Clients;
using EmojiPost.DataServices.Entities;

namespace EmojiPost.DataServices.Services
{
    /// <summary>
    /// スタンプ断片サービスのインタフェース
    /// </summary>
    public interface IFragmentService
    {
        #region Methods

        /// <summary>
        /// ストレージへスタンプ断片エンティティの内容を追加します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="fragment">スタンプ断片エンティティ</param>
        /// <returns>追加件数</returns>
        int Insert(DbProvider db, FragmentEntity fragment);

        /// <summary>
        /// スタンプIDを指定してストレージのスタンプ断片データを削除します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="stampId">スタンプID</param>
        /// <returns>削除件数</returns>
        int DeleteByStampId(DbProvider db, int stampId);

        /// <summary>
        /// 次に割り振るスタンプ断片IDの値 を取得します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <returns>次に割り振るスタンプ断片IDの値</returns>
        /// <remarks>並列実行が行われると採番が重複する可能性があるが、アプリケーションの作り込みでカバーする</remarks>
        int NextFragmentId(DbProvider db);

        #endregion
    }
}
