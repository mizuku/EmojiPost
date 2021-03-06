﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.IO;

using Prism.Mvvm;

using AyaStyle.DataServices.Entities;
using AyaStyle.Enums;
using AyaStyle.DataServices.Clients;
using AyaStyle.DataServices.Services;
using System.Text;

namespace AyaStyle.Models
{
    /// <summary>
    /// スタンプモデル
    /// </summary>
    public class StampModel : BindableBase, IDraggableItem
    {
        #region Fields
        /// <summary>
        /// スタンプ のエンティティモデル
        /// </summary>
        private StampEntity entity;

        #endregion

        #region Properties

        private string _stampName;
        /// <summary>
        /// スタンプ名 を取得または設定します。
        /// </summary>
        public string StampName
        {
            get => this._stampName;
            set => SetProperty(ref this._stampName, value);
        }

        private string _stampLocalName;
        /// <summary>
        /// スタンプ表示名 を取得または設定します。
        /// </summary>
        public string StampLocalName
        {
            get => this._stampLocalName;
            set => SetProperty(ref this._stampLocalName, value);
        }

        private int _canvasWidth;
        /// <summary>
        /// キャンバスの幅 を取得または設定します。
        /// </summary>
        public int CanvasWidth
        {
            get => this._canvasWidth;
            set => SetProperty(ref this._canvasWidth, value);
        }

        private int _canvasHeight;
        /// <summary>
        /// キャンバスの高さ を取得または設定します。
        /// </summary>
        public int CanvasHeight
        {
            get => this._canvasHeight;
            set => SetProperty(ref this._canvasHeight, value);
        }

        private BitmapSource _imageSourceBitmap;
        /// <summary>
        /// 元画像のビットマップオブジェクト を取得または設定します。
        /// </summary>
        public BitmapSource ImageSourceBitmap
        {
            get => this._imageSourceBitmap;
            private set => SetProperty(ref this._imageSourceBitmap, value);
        }


        private double _sourceRectTop;
        /// <summary>
        /// 元画像の矩形のTop を取得または設定します。
        /// </summary>
        public double SourceRectTop
        {
            get => this._sourceRectTop;
            set => SetProperty(ref this._sourceRectTop, value);
        }

        private double _sourceRectLeft;
        /// <summary>
        /// 元画像の矩形のLeft を取得または設定します。
        /// </summary>
        public double SourceRectLeft
        {
            get => this._sourceRectLeft;
            set => SetProperty(ref this._sourceRectLeft, value);
        }

        private double _sourceRectWidth;
        /// <summary>
        /// 元画像の矩形の幅 を取得または設定します。
        /// </summary>
        public double SourceRectWidth
        {
            get => this._sourceRectWidth;
            set => SetProperty(ref this._sourceRectWidth, value);
        }

        private double _sourceRectHeight;
        /// <summary>
        /// 元画像の矩形の高さ を取得または設定します。
        /// </summary>
        public double SourceRectHeight
        {
            get => this._sourceRectHeight;
            set => SetProperty(ref this._sourceRectHeight, value);
        }

        private double _clipRectTop;
        /// <summary>
        /// クリップ領域の矩形のTop を取得または設定します。
        /// </summary>
        public double ClipRectTop
        {
            get => this._clipRectTop;
            set => SetProperty(ref this._clipRectTop, value);
        }

        private double _clipRectLeft;
        /// <summary>
        /// クリップ領域の矩形のLeft を取得または設定します。
        /// </summary>
        public double ClipRectLeft
        {
            get => this._clipRectLeft;
            set => SetProperty(ref this._clipRectLeft, value);
        }

        private double _clipRectWidth;
        /// <summary>
        /// クリップ領域の矩形の幅 を取得または設定します。
        /// </summary>
        public double ClipRectWidth
        {
            get => this._clipRectWidth;
            set => SetProperty(ref this._clipRectWidth, value);
        }

        private double _clipRectHeight;
        /// <summary>
        /// クリップ領域の矩形の高さ を取得または設定します。
        /// </summary>
        public double ClipRectHeight
        {
            get => this._clipRectHeight;
            set => SetProperty(ref this._clipRectHeight, value);
        }

        private int _pixelOfFragments;
        /// <summary>
        /// １コマのピクセル数 を取得または設定します。
        /// </summary>
        public int PixelOfFragments
        {
            get => this._pixelOfFragments;
            set
            {
                if (SetProperty(ref this._pixelOfFragments, value))
                {
                    this.UpdateClipRectByPixelSize();
                }
            }
        }

        private BitmapSource _thumbnailBitmap;
        /// <summary>
        /// 結果のサムネイル画像のビットマップオブジェクト を取得または設定します。
        /// </summary>
        public BitmapSource ThumbnailBitmap
        {
            get => this._thumbnailBitmap;
            set => SetProperty(ref this._thumbnailBitmap, value);
        }

        private EditState _editState;
        /// <summary>
        /// 編集状態 を取得または設定します。
        /// </summary>
        public EditState EditState
        {
            get => this._editState;
            set => SetProperty(ref this._editState, value);
        }

        private ObservableCollection<FragmentModel> _fragments;
        /// <summary>
        /// 変更の通知が可能なスタンプの断片コレクション を取得または設定します。
        /// </summary>
        public ObservableCollection<FragmentModel> Fragments
        {
            get => this._fragments;
            set
            {
                if (SetProperty(ref this._fragments, value))
                {
                    RaisePropertyChanged(nameof(StampString));
                }
            }
        }

        /// <summary>
        /// 元画像の矩形 を取得します。
        /// </summary>
        public Rect SourceRect
        {
            get => new Rect(this.SourceRectLeft, this.SourceRectTop, this.SourceRectWidth, this.SourceRectHeight);
        }

        /// <summary>
        /// クリップ領域の矩形 を取得します。
        /// </summary>
        public Rect ClipRect
        {
            get => new Rect(this.ClipRectLeft, this.ClipRectTop, this.ClipRectWidth, this.ClipRectHeight);
        }

        /// <summary>
        /// このスタンプを書き込むための文字列 を取得します。
        /// </summary>
        public string StampString
        {
            get
            {
                if (null == this.Fragments || false == this.Fragments.Any()) return string.Empty;

                StringBuilder builder = new StringBuilder();
                foreach (var f in this.Fragments)
                {
                    if (f.FragmentAddressX == 0 && f.FragmentAddressY != 0)
                    {
                        builder.Append('\n');
                    }
                    builder
                        .Append(':')
                        .Append(f.EmojiName)
                        .Append(':');
                }
                return builder.ToString();
            }
        }
        

        #endregion

        #region DI Services

        /// <summary>
        /// スタンプサービス を取得または設定します。
        /// </summary>
        public IStampService StampService { get; set; }

        /// <summary>
        /// スタンプ断片サービス を取得または設定します。
        /// </summary>
        public IFragmentService FragmentService { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// スタンプIDを指定し、ストレージよりスタンプの情報を読み込みます。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="stampId">スタンプID</param>
        public void LoadFromStorageById(DbProvider db, int stampId)
        {
            var entity = this.StampService.ReadEntities(db, stampId);
            this.entity = entity;
            this.StampName = entity.StampName;
            this.StampLocalName = entity.StampLocalName;
            this.CanvasWidth = entity.CanvasWidth;
            this.CanvasHeight = entity.CanvasHeight;
            this.PixelOfFragments = entity.PixelOfFragments ?? this.PixelOfFragments;
            this.EditState = (EditState)Enum.ToObject(typeof(EditState), entity.EditState ?? 0);

            this.SetSourceRect(string.IsNullOrWhiteSpace(entity.SourceRect)
                ? this.SourceRect : Rect.Parse(entity.SourceRect));
            this.SetClipRect(string.IsNullOrWhiteSpace(entity.ClipRect)
                ? this.ClipRect : Rect.Parse(entity.ClipRect));
            // TODO Fragments

            if (null != entity.ImageSource && 0 < entity.ImageSource.Length)
            {
                using (var stream = new MemoryStream(entity.ImageSource))
                {
                    this.ImageSourceBitmap = new WriteableBitmap(BitmapFrame.Create(stream));
                }
            }
        }

        /// <summary>
        /// ストレージへこのスタンプの情報を保存します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        public void SaveStamp(DbProvider db, EditState editState = EditState.Saved)
        {
            // スタンプ断片はDelete>Insert
            this.FragmentService.DeleteByStampId(db, this.entity.StampId);
            var fragments = this.Fragments;
            if (null != fragments && fragments.Any())
            {
                foreach (var f in fragments)
                {
                    f.SaveFragment(db);
                }
            }

            // 更新
            this.EditState = editState;
            this.entity = this.ToEntities();
            this.entity.DateOfUpdate = DateTime.Now.ToString();
            this.StampService.Update(db, this.entity);
        }

        /// <summary>
        /// ローカルドライブへこのスタンプの情報を保存します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="directory">保存先のディレクトリ名</param>
        public void SaveAsLocal(DbProvider db, string directory)
        {
            // 出力前にストレージに保存 TODO アップロード済みのステータスを上書きしてもよいか？考える
            this.SaveStamp(db, EditState.Saved);

            var stamp = this.ToEntities();
            var fragments = this.GetFragmentEntities();

            // ファイル出力
            this.StampService.SaveAsLocal(directory, stamp, fragments);

            // TODO 編集状態をアップデート？ローカル保存のときは不要かも
        }

        /// <summary>
        /// このスタンプの情報をリロードします。
        /// </summary>
        public void Reload()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 画像を分割し、スタンプ断片情報を作成します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="clipImage">分割する画像</param>
        public void DivideToFragments(DbProvider db, BitmapSource clipImage)
        {
            if (null == clipImage || string.IsNullOrWhiteSpace(this.StampName) || this.PixelOfFragments <= 0)
                throw new ArgumentException("必要なスタンプ情報が未設定です。");

            var px = this.PixelOfFragments;
            var cx = (int)Math.Floor(this.ClipRectWidth / px);
            var cy = (int)Math.Floor(this.ClipRectHeight / px);

            // スタンプ断片IDを取得
            int nextFragmentId = this.FragmentService.NextFragmentId(db);

            // 分割した画像はFragmentModelとして保持
            var fragments = new List<FragmentModel>();
            int order = 0;
            for (int y = 0; y < cy; y++)
            {
                for (int x = 0; x < cx; x++)
                {
                    var fragmentImage = new CroppedBitmap(clipImage, new Int32Rect(x * px, y * px, px, px));
                    var f = ContainerProvider.Resolve<FragmentModel>();
                    f.FragmentId = nextFragmentId++;
                    f.StampId = this.entity.StampId;
                    f.EmojiName = $"{this.StampName}{y}{x}";
                    f.ImageBitmap = fragmentImage;
                    f.OrderOfFragments = order++;
                    f.StampSize = this.ClipRect.Size;
                    f.PixelOfFragments = this.PixelOfFragments;

                    fragments.Add(f);
                }
            }
            this.Fragments = new ObservableCollection<FragmentModel>(fragments);

            // 分割直後の状態でDBに書き込んでしまう
            this.SaveStamp(db, EditState.Editing);
        }

        /// <summary>
        /// 元画像を表すBitmapを設定します。
        /// </summary>
        /// <param name="sourceStream">元画像の入力ストリーム</param>
        public void SetImageSource(Stream sourceStream)
        {
            var bitmap = new WriteableBitmap(BitmapFrame.Create(sourceStream));
            this.ImageSourceBitmap = bitmap;
            this.SetSourceRect(new Rect(0d, 0d, bitmap.Width, bitmap.Height));
            this.CanvasWidth = (int)Math.Max(this.CanvasWidth, bitmap.Width);
            this.CanvasHeight = (int)Math.Max(this.CanvasHeight, bitmap.Height);
            this.SetClipRect(new Rect(
                Math.Floor((this.CanvasWidth - this.ClipRectWidth) / 2d),
                Math.Floor((this.CanvasHeight - this.ClipRectHeight) / 2d),
                this.ClipRectWidth,
                this.ClipRectHeight));
        }

        /// <summary>
        /// 元画像の矩形を設定します。
        /// </summary>
        /// <param name="rect">元画像の矩形</param>
        public void SetSourceRect(Rect rect)
        {
            this.SourceRectLeft = rect.Left;
            this.SourceRectTop = rect.Top;
            this.SourceRectWidth = rect.Width;
            this.SourceRectHeight = rect.Height;
        }

        /// <summary>
        /// クリップ領域の矩形を設定します。
        /// </summary>
        /// <param name="rect">クリップ領域の矩形</param>
        public void SetClipRect(Rect rect)
        {
            this.ClipRectLeft = rect.Left;
            this.ClipRectTop = rect.Top;
            this.ClipRectWidth = rect.Width;
            this.ClipRectHeight = rect.Height;
        }

        /// <summary>
        /// スタンプモデルからスタンプエンティティを取得します。
        /// </summary>
        /// <returns>このスタンプモデルの情報を反映したスタンプエンティティ</returns>
        public StampEntity ToEntities()
        {
            var e = this.entity ?? new StampEntity();
            e.StampName = this.StampName;
            e.StampLocalName = this.StampLocalName;
            e.CanvasWidth = this.CanvasWidth;
            e.CanvasHeight = this.CanvasHeight;
            e.PixelOfFragments = this.PixelOfFragments;
            e.SourceRect = this.SourceRect.ToString();
            e.ClipRect = this.ClipRect.ToString();
            e.EditState = (int)this.EditState;

            if (null != this.ImageSourceBitmap)
            {
                using (var stream = new MemoryStream())
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(this.ImageSourceBitmap));
                    encoder.Save(stream);
                    e.ImageSource = stream.ToArray();
                }
            }

            // TODO e.Thumbnail

            return e;
        }

        /// <summary>
        /// このスタンプが所有するスタンプ断片エンティティを取得します。
        /// </summary>
        /// <returns>このスタンプが所有するスタンプ断片エンティティ</returns>
        public IEnumerable<FragmentEntity> GetFragmentEntities()
        {
            if (null != this.Fragments && this.Fragments.Any())
            {
                return this.Fragments.Select(f => f.ToEntities());
            }
            else
            {
                return Enumerable.Empty<FragmentEntity>();
            }
        }

        /// <summary>
        /// ストレージへこのスタンプの情報を追加します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        private void AddStorage(DbProvider db)
        {
            this.StampService.Insert(db, this.entity);
        }

        /// <summary>
        /// エンティティの初期化を行います。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        private void InitializeEntities(DbProvider db)
        {
            var e = new StampEntity
            {
                StampId = this.StampService.NextStampId(db),
                // TODO
                WorkspaceId = 1,
                StampName = this.StampName,
                StampLocalName = this.StampLocalName,
                CanvasWidth = this.CanvasWidth,
                CanvasHeight = this.CanvasHeight,
                PixelOfFragments = this.PixelOfFragments,
                SourceRect = this.SourceRect.ToString(),
                ClipRect = this.ClipRect.ToString(),
                DateOfCreate = DateTime.Now.ToString(),
                DateOfUpdate = DateTime.Now.ToString()
            };

            this.entity = e;
        }
        
        /// <summary>
        /// ピクセルサイズの変更をクリップ領域の矩形に反映する。
        /// </summary>
        private void UpdateClipRectByPixelSize()
        {
            var ow = this.ClipRectWidth;
            var oh = this.ClipRectHeight;

            // 端数を切り捨てて新しいピクセルサイズによる断片数を算出
            int nx = (int)Math.Floor(ow / this.PixelOfFragments);
            int ny = (int)Math.Floor(oh / this.PixelOfFragments);
            // 算出した断片数 n と n + 1 のうち、より元の矩形の値に近似するほうを新しい矩形の大きさに適用する
            int x = Math.Abs(ow - (nx * this.PixelOfFragments)) < Math.Abs(ow - ((nx + 1) * this.PixelOfFragments)) ? nx : nx + 1;
            int y = Math.Abs(oh - (ny * this.PixelOfFragments)) < Math.Abs(oh - ((ny + 1) * this.PixelOfFragments)) ? ny : ny + 1;

            this.ClipRectWidth = Math.Max(x, 1) * this.PixelOfFragments;
            this.ClipRectHeight = Math.Max(y, 1) * this.PixelOfFragments;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// スタンプIDを指定し、スタンプモデルを取得します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="stampId">スタンプID</param>
        /// <returns>スタンプモデル</returns>
        public static StampModel LoadById(DbProvider db, int stampId)
        {
            var model = ContainerProvider.Resolve<StampModel>();
            model.LoadFromStorageById(db, stampId);
            return model;
        }

        /// <summary>
        /// 新しいスタンプモデルのインスタンスを作成します。スタンプの情報は作成時に仮保存されます。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="sourceStream">元画像の入力ストリーム</param>
        /// <returns>新しいスタンプモデル</returns>
        public static StampModel CreateStamp(DbProvider db, Stream sourceStream = null)
        {
            var stamp = ContainerProvider.Resolve<StampModel>();
            stamp.InitializeEntities(db);
            if (null != sourceStream)
            {
                stamp.SetImageSource(sourceStream);
            }
            // 初期化時点で仮保存
            stamp.AddStorage(db);

            return stamp;
        }

        #endregion

        #region Implements IDraggableItem

        public Size Coerce(Size size)
        {
            return new Size(Math.Floor(size.Width / this.PixelOfFragments) * this.PixelOfFragments,
                Math.Floor(size.Height / this.PixelOfFragments) * this.PixelOfFragments);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、コンストラクタです。
        /// </summary>
        /// <param name="stampService">スタンプサービス</param>
        /// <param name="fragmentService">スタンプ断片サービス</param>
        public StampModel(IStampService stampService, IFragmentService fragmentService)
            : base()
        {
            this.StampService = stampService;
            this.FragmentService = fragmentService;

            this._stampName = null;
            this._stampLocalName = null;
            this._canvasWidth = 200;
            this._canvasHeight = 200;
            this._imageSourceBitmap = null;
            this._pixelOfFragments = 20;
            this._thumbnailBitmap = null;
            this._editState = EditState.Nothing;
            this._fragments = null;
            this._sourceRectLeft = 0d;
            this._sourceRectTop = 0d;
            this._sourceRectWidth = 0d;
            this._sourceRectHeight = 0d;
            this._clipRectLeft = 10d;
            this._clipRectTop = 10d;
            this._clipRectWidth = 80d;
            this._clipRectHeight = 60d;
        }

        #endregion
    }
}
