using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace EmojiPost.DataServices.Entities
{
    /// <summary>
    /// 設定のエンティティモデル
    /// </summary>
    [DataContract]
    [Table(name: "Settings")]
    public class SettingEntity
    {

        #region Properties

        /// <summary>
        /// 設定ID を取得または設定します。
        /// </summary>
        [Key, Required, Column(name: nameof(SettingId), Order = 0, TypeName = "INTEGER")]
        [DisplayName("設定ID")]
        public int SettingId { get; set; }

        /// <summary>
        /// 最後に作業したワークスペース を取得または設定します。
        /// </summary>
        [Column(name: nameof(LastOfWorkspaceId), Order = 1, TypeName = "INTEGER")]
        [DisplayName("最後に作業したワークスペース")]
        public int? LastOfWorkspaceId { get; set; }

        /// <summary>
        /// 最後に作業したスタンプ を取得または設定します。
        /// </summary>
        [Column(name: nameof(LastOfStampId), Order = 2, TypeName = "INTEGER")]
        [DisplayName("最後に作業したスタンプ")]
        public int? LastOfStampId { get; set; }

        /// <summary>
        /// 登録日 を取得または設定します。
        /// </summary>
        [Column(name: nameof(DateOfCreate), Order = 3, TypeName = "TEXT")]
        [DisplayName("登録日")]
        public string DateOfCreate { get; set; }

        /// <summary>
        /// 更新日 を取得または設定します。
        /// </summary>
        [Column(name: nameof(DateOfUpdate), Order = 4, TypeName = "TEXT")]
        [DisplayName("更新日")]
        public string DateOfUpdate { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}
