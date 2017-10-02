using System.Collections.Generic;
using EmojiPost.DataServices.Entities;
using EmojiPost.DataServices.Clients;

namespace EmojiPost.DataServices.Services
{
    /// <summary>
    /// ワークスペースサービスのインタフェース
    /// </summary>
    public interface IWorkspaceService
    {

        #region Methods

        /// <summary>
        /// すべてのワークスペースを読み込みます。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <returns>ワークスペースエンティティの列挙</returns>
        IEnumerable<WorkspaceEntity> ReadAllEntities(DbProvider db);


        #endregion

    }
}
