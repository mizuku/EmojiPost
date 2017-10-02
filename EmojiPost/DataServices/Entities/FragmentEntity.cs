using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace EmojiPost.DataServices.Entities
{
    /// <summary>
    /// スタンプの断片 のエンティティモデル
    /// </summary>
    [DataContract]
    [Table(name: "Fragments")]
    public class FragmentEntity : EntitiesBase
    {

        #region Properties

        /// <summary>
        /// 断片ID を取得または設定します。
        /// </summary>
        [Key, Required, Column(name: nameof(FragmentId), Order = 0, TypeName = "INTEGER")]
        [DisplayName("断片ID")]
        public int FragmentId { get; set; }

        /// <summary>
        /// スタンプID を取得または設定します。
        /// </summary>
        [Required, Column(name: nameof(StampId), Order = 1, TypeName = "INTEGER")]
        [DisplayName("スタンプID")]
        public int StampId { get; set; }

        /// <summary>
        /// ワークスペースID を取得または設定します。
        /// </summary>
        [Column(name: nameof(WorkspaceId), Order = 2, TypeName = "INTEGER")]
        [DisplayName("ワークスペースID")]
        public int? WorkspaceId { get; set; }

        /// <summary>
        /// emoji名 を取得または設定します。
        /// </summary>
        [Column(name: nameof(EmojiName), Order = 3, TypeName = "TEXT")]
        [DisplayName("emoji名")]
        public string EmojiName { get; set; }

        /// <summary>
        /// 画像 を取得または設定します。
        /// </summary>
        [Column(name: nameof(Image), Order = 4, TypeName = "BLOB")]
        [DisplayName("画像")]
        public byte[] Image { get; set; }

        /// <summary>
        /// 順序 を取得または設定します。
        /// </summary>
        [Column(name: nameof(OrderOfFragments), Order = 5, TypeName = "INTEGER")]
        [DisplayName("順序")]
        public int? OrderOfFragments { get; set; }
        
        /// <summary>
        /// 登録日 を取得または設定します。
        /// </summary>
        [Column(name: nameof(DateOfCreate), Order = 6, TypeName = "TEXT")]
        [DisplayName("登録日")]
        public string DateOfCreate { get; set; }

        /// <summary>
        /// 更新日 を取得または設定します。
        /// </summary>
        [Column(name: nameof(DateOfUpdate), Order = 7, TypeName = "TEXT")]
        [DisplayName("更新日")]
        public string DateOfUpdate { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}
