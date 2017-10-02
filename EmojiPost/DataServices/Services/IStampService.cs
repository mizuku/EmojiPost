﻿using EmojiPost.DataServices.Clients;
using EmojiPost.DataServices.Entities;

namespace EmojiPost.DataServices.Services
{
    /// <summary>
    /// スタンプサービスのインタフェース
    /// </summary>
    public interface IStampService
    {

        #region Methods

        /// <summary>
        /// スタンプIDを指定し、該当のスタンプエンティティを読み込みます。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="stampId">スタンプID</param>
        /// <returns>スタンプエンティティ</returns>
        StampEntity ReadEntities(DbProvider db, int stampId);

        /// <summary>
        /// ストレージへスタンプエンティティの内容を追加します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="stamp">スタンプエンティティ</param>
        /// <returns>追加件数</returns>
        int Insert(DbProvider db, StampEntity stamp);

        /// <summary>
        /// ストレージへスタンプエンティティの内容を更新します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="stamp">スタンプエンティティ</param>
        /// <returns>更新件数</returns>
        int Update(DbProvider db, StampEntity stamp);

        /// <summary>
        /// 次に割り振るスタンプIDの値 を取得します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <returns>次に割り振るスタンプIDの値</returns>
        /// <remarks>並列実行が行われると採番が重複する可能性があるが、アプリケーションの作り込みでカバーする</remarks>
        int NextStampId(DbProvider db);

        #endregion

    }
}
