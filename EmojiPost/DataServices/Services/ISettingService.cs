using AyaStyle.DataServices.Clients;
using AyaStyle.DataServices.Entities;

namespace AyaStyle.DataServices.Services
{
    /// <summary>
    /// アプリケーション設定サービスのインタフェース
    /// </summary>
    public interface ISettingService
    {

        #region Methods

        /// <summary>
        /// ストレージへ初期化用のデフォルト設定を追加します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <returns>追加した設定エンティティ</returns>
        SettingEntity InsertDefaultSetting(DbProvider db);

        /// <summary>
        /// 設定IDの昇順で先頭のエンティティを読み込みます。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <returns>先頭の設定エンティティ</returns>
        SettingEntity ReadFirstEntity(DbProvider db);

        /// <summary>
        /// ストレージへ設定エンティティの内容を追加します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="setting">設定エンティティ</param>
        /// <returns>追加件数</returns>
        int Insert(DbProvider db, SettingEntity setting);

        /// <summary>
        /// ストレージへ設定エンティティの内容を更新します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="setting">設定エンティティ</param>
        /// <returns>更新件数</returns>
        int Update(DbProvider db, SettingEntity setting);

        #endregion

    }
}
