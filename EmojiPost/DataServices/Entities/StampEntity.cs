using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace AyaStyle.DataServices.Entities
{
    /// <summary>
    /// スタンプ のエンティティモデル
    /// </summary>
    [DataContract]
    [Table(name: "Stamps")]
    public class StampEntity : EntitiesBase
    {

        #region Properties

        /// <summary>
        /// スタンプID を取得または設定します。
        /// </summary>
        [Key, Required, Column(name: nameof(StampId), Order = 0, TypeName = "INTEGER")]
        [DisplayName("スタンプID")]
        public int StampId { get; set; }

        /// <summary>
        /// ワークスペースID を取得または設定します。
        /// </summary>
        [Column(name: nameof(WorkspaceId), Order = 1, TypeName = "INTEGER")]
        [DisplayName("ワークスペースID")]
        public int? WorkspaceId { get; set; }

        /// <summary>
        /// スタンプ名 を取得または設定します。
        /// </summary>
        [Column(name: nameof(StampName), Order = 2, TypeName = "TEXT")]
        [DisplayName("スタンプ名")]
        public string StampName { get; set; }

        /// <summary>
        /// スタンプ表示名 を取得または設定します。
        /// </summary>
        [Column(name: nameof(StampLocalName), Order = 3, TypeName = "TEXT")]
        [DisplayName("スタンプ表示名")]
        public string StampLocalName { get; set; }

        /// <summary>
        /// キャンバスの幅 を取得または設定します。
        /// </summary>
        [Required, Column(name: nameof(CanvasWidth), Order = 4, TypeName = "INTEGER")]
        [DisplayName("キャンバスの幅")]
        public int CanvasWidth { get; set; }

        /// <summary>
        /// キャンバスの高さ を取得または設定します。
        /// </summary>
        [Required, Column(name: nameof(CanvasHeight), Order = 5, TypeName = "INTEGER")]
        [DisplayName("キャンバスの高さ")]
        public int CanvasHeight { get; set; }

        /// <summary>
        /// 元画像 を取得または設定します。
        /// </summary>
        [Column(name: nameof(ImageSource), Order = 6, TypeName = "BLOB")]
        [DisplayName("元画像")]
        public byte[] ImageSource { get; set; }

        /// <summary>
        /// 元画像の矩形 を取得または設定します。
        /// </summary>
        [Column(name: nameof(SourceRect), Order = 7, TypeName = "TEXT")]
        [DisplayName("元画像の矩形")]
        public string SourceRect { get; set; }

        /// <summary>
        /// クリップ範囲の矩形 を取得または設定します。
        /// </summary>
        [Column(name: nameof(ClipRect), Order = 8, TypeName = "TEXT")]
        [DisplayName("クリップ範囲の矩形")]
        public string ClipRect { get; set; }

        /// <summary>
        /// １コマのピクセル数 を取得または設定します。
        /// </summary>
        [Column(name: nameof(PixelOfFragments), Order = 9, TypeName = "INTEGER")]
        [DisplayName("１コマのピクセル数")]
        public int? PixelOfFragments { get; set; }
        
        /// <summary>
        /// 結果のサムネイル画像 を取得または設定します。
        /// </summary>
        [Column(name: nameof(Thumbnail), Order = 10, TypeName = "BLOB")]
        [DisplayName("結果のサムネイル画像")]
        public byte[] Thumbnail { get; set; }

        /// <summary>
        /// 編集状態 を取得または設定します。
        /// </summary>
        [Column(name: nameof(EditState), Order = 11, TypeName = "INTEGER")]
        [DisplayName("編集状態")]
        public int? EditState { get; set; } = 0;

        /// <summary>
        /// 登録日 を取得または設定します。
        /// </summary>
        [Column(name: nameof(DateOfCreate), Order = 12, TypeName = "TEXT")]
        [DisplayName("登録日")]
        public string DateOfCreate { get; set; }

        /// <summary>
        /// 更新日 を取得または設定します。
        /// </summary>
        [Column(name: nameof(DateOfUpdate), Order = 13, TypeName = "TEXT")]
        [DisplayName("更新日")]
        public string DateOfUpdate { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}
