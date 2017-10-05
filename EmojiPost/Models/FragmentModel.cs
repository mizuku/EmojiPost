using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using Prism.Mvvm;
using EmojiPost.DataServices.Entities;
using EmojiPost.DataServices.Clients;
using EmojiPost.DataServices.Services;
using System.IO;
using System.Windows;

namespace EmojiPost.Models
{
    /// <summary>
    /// スタンプ断片モデル
    /// </summary>
    public class FragmentModel : BindableBase
    {
        #region Fields

        /// <summary>
        /// スタンプ断片 のエンティティモデル。
        /// </summary>
        private FragmentEntity entity;

        #endregion

        #region Properties

        private int _fragmentId;
        /// <summary>
        /// スタンプ断片ID を取得または設定します。
        /// </summary>
        public int FragmentId
        {
            get => this._fragmentId;
            set => SetProperty(ref this._fragmentId, value);
        }

        private int _stampId;
        /// <summary>
        /// スタンプID を取得または設定します。
        /// </summary>
        public int StampId
        {
            get => this._stampId;
            set => SetProperty(ref this._stampId, value);
        }

        private string _emojiName;
        /// <summary>
        /// emoji名 を取得または設定します。
        /// </summary>
        public string EmojiName
        {
            get => this._emojiName;
            set => SetProperty(ref this._emojiName, value);
        }

        private BitmapSource _imageBitmap;
        /// <summary>
        /// 画像 を取得または設定します。
        /// </summary>
        public BitmapSource ImageBitmap
        {
            get => this._imageBitmap;
            set => SetProperty(ref this._imageBitmap, value);
        }

        private int? _orderOfFragments;
        /// <summary>
        /// 順序 を取得または設定します。
        /// </summary>
        public int? OrderOfFragments
        {
            get => this._orderOfFragments;
            set => SetProperty(ref this._orderOfFragments, value);
        }

        /// <summary>
        /// このスタンプ断片を所有するスタンプの大きさを取得または設定します。
        /// </summary>
        public Size StampSize { get; set; }

        /// <summary>
        /// このスタンプ断片の１辺のピクセル数を取得または設定します。
        /// </summary>
        public int PixelOfFragments { get; set; }

        /// <summary>
        /// このスタンプ断片の水平方向位置を取得します。
        /// </summary>
        public int FragmentAddressX
        {
            get
            {
                return (this.OrderOfFragments ?? 0) % (int)(this.StampSize.Width / this.PixelOfFragments);
            }
        }

        /// <summary>
        /// このスタンプ断片の垂直方向位置を取得します。
        /// </summary>
        public int FragmentAddressY
        {
            get
            {
                return (int)Math.Floor((this.OrderOfFragments ?? 0) / (this.StampSize.Width / this.PixelOfFragments));
            }
        }

        /// <summary>
        /// このスタンプ断片のピクセル上の開始位置Xを取得します。
        /// </summary>
        public int PositionX
        {
            get => this.FragmentAddressX * 20;
        }

        /// <summary>
        /// このスタンプ断片のピクセル上の開始位置Yを取得します。
        /// </summary>
        public int PositionY
        {
            get => this.FragmentAddressY * 20;
        }

        #endregion

        #region DI Services

        /// <summary>
        /// スタンプ断片サービス を取得または設定します。
        /// </summary>
        public IFragmentService FragmentService { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// スタンプ断片IDを指定し、ストレージよりスタンプ断片の情報を読み込みます。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="fragmentId">スタンプ断片ID</param>
        public void LoadFromStorageById(DbProvider db, int fragmentId)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// ストレージへこのスタンプ断片の情報を保存します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        public void SaveFragment(DbProvider db)
        {
            this.entity = this.ToEntities();
            this.entity.DateOfUpdate = DateTime.Now.ToString();
            this.FragmentService.Insert(db, this.entity);
        }

        /// <summary>
        /// スタンプ断片モデルからスタンプ断片エンティティを取得します。
        /// </summary>
        /// <returns>このスタンプ断片モデルの情報を反映したスタンプ断片エンティティ</returns>
        public FragmentEntity ToEntities()
        {
            var e = this.entity ?? new FragmentEntity();
            e.FragmentId = this.FragmentId;
            e.StampId = this.StampId;
            // TODO
            e.WorkspaceId = 1;
            e.EmojiName = this.EmojiName;
            e.OrderOfFragments = this.OrderOfFragments;
            e.DateOfCreate = string.IsNullOrEmpty(e.DateOfCreate) ? DateTime.Now.ToString() : e.DateOfCreate;

            if (null != this.ImageBitmap)
            {
                using (var stream = new MemoryStream())
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(this.ImageBitmap));
                    encoder.Save(stream);
                    e.Image = stream.ToArray();
                }
            }

            return e;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// スタンプ断片IDを指定し、スタンプ断片モデルを取得します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="stampId">スタンプ断片ID</param>
        /// <returns>スタンプ断片モデル</returns>
        public static FragmentModel LoadById(DbProvider db, int fragmentId)
        {
            var model = ContainerProvider.Resolve<FragmentModel>();
            model.LoadFromStorageById(db, fragmentId);
            return model;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        /// <param name="fragmentService">スタンプ断片サービス</param>
        public FragmentModel(IFragmentService fragmentService)
            : base()
        {
            this.FragmentService = fragmentService;

            this._fragmentId = 0;
            this._stampId = 0;
            this._emojiName = string.Empty;
            this._imageBitmap = null;
            this._orderOfFragments = null;
        }

        #endregion

    }
}
