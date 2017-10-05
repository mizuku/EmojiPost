using System.Collections.Generic;
using AyaStyle.DataServices.Entities;
using AyaStyle.DataServices.Clients;

namespace AyaStyle.DataServices.Services
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
