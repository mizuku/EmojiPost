using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace AyaStyle.DataServices.Entities
{
    /// <summary>
    /// ワークスペースのエンティティモデル
    /// </summary>
    [DataContract]
    [Table(name: "Workspaces")]
    public class WorkspaceEntity : EntitiesBase
    {

        #region Properties

        /// <summary>
        /// ワークスペースID を取得または設定します。
        /// </summary>
        [Key, Required, Column(name: nameof(WorkspaceId), Order = 0, TypeName = "INTEGER")]
        [DisplayName("ワークスペースID")]
        public int WorkspaceId { get; set; }

        /// <summary>
        /// Slackサブドメイン を取得または設定します。
        /// </summary>
        [Column(name: nameof(SubdomainOfSlack), Order = 1, TypeName = "TEXT")]
        [DisplayName("Slackサブドメイン")]
        public string SubdomainOfSlack { get; set; }

        /// <summary>
        /// SlackEメールアドレス を取得または設定します。
        /// </summary>
        [Column(name: nameof(MailAddressOfSlack), Order = 2, TypeName = "TEXT")]
        [DisplayName("SlackEメールアドレス")]
        public string MailAddressOfSlack { get; set; }

        /// <summary>
        /// Slackパスワード を取得または設定します。
        /// </summary>
        [Column(name: nameof(PasswordOfSlack), Order = 3, TypeName = "TEXT")]
        [DisplayName("Slackパスワード")]
        public string PasswordOfSlack { get; set; }

        /// <summary>
        /// 登録日 を取得または設定します。
        /// </summary>
        [Column(name: nameof(DateOfCreate), Order = 4, TypeName = "TEXT")]
        [DisplayName("登録日")]
        public string DateOfCreate { get; set; }

        /// <summary>
        /// 更新日 を取得または設定します。
        /// </summary>
        [Column(name: nameof(DateOfUpdate), Order = 5, TypeName = "TEXT")]
        [DisplayName("更新日")]
        public string DateOfUpdate { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}
