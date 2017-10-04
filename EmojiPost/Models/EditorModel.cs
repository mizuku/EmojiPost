using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Windows.Media.Imaging;
using Prism.Mvvm;
using EmojiPost.Enums;
using EmojiPost.DataServices.Clients;

namespace EmojiPost.Models
{
    /// <summary>
    /// スタンプ編集モデル
    /// </summary>
    public class EditorModel : BindableBase, ISingletonModel
    {

        #region Properties

        /// <summary>
        /// アプリケーションの設定モデル
        /// </summary>
        private SettingModel Setting { get; set; }

        private StampModel _currentStamp;
        /// <summary>
        /// 編集中のスタンプ を取得または設定します。
        /// </summary>
        public StampModel CurrentStamp
        {
            get => this._currentStamp;
            set
            {
                if (this._currentStamp != value) this.SetCurrentStamp(value);
            }
        }

        private int _canvasWidth = default(int);
        /// <summary>
        /// 編集中のキャンバスの幅 を取得します。このプロパティは読み取り専用です。
        /// </summary>
        public int CanvasWidth
        {
            get => this.CurrentStamp?.CanvasWidth ?? this._canvasWidth;
        }

        private int _canvasHeight = default(int);
        /// <summary>
        /// 編集中のキャンバスの高さ を取得します。このプロパティは読み取り専用です。
        /// </summary>
        public int CanvasHeight
        {
            get => this.CurrentStamp?.CanvasHeight ?? this._canvasHeight;
        }

        private string _stampName;
        /// <summary>
        /// 編集中のスタンプの名前 を取得します。このプロパティは読み取り専用です。
        /// </summary>
        public string StampName
        {
            get => this.CurrentStamp?.StampName ?? this._stampName;
        }

        private string _stampLocalName;
        /// <summary>
        /// 編集中のスタンプ表示名 を取得します。このプロパティは読み取り専用です。
        /// </summary>
        public string StampLocalName
        {
            get => this.CurrentStamp?.StampLocalName ?? this._stampLocalName;
        }

        private int _pixelOfFragments;
        /// <summary>
        /// 編集中の１コマのピクセル数 を取得します。このプロパティは読み取り専用です。
        /// </summary>
        public int PixelOfFragments
        {
            get => this.CurrentStamp?.PixelOfFragments ?? this._pixelOfFragments;
        }

        private BitmapSource _imageSourceBitmap;
        /// <summary>
        /// 編集中の元画像のビットマップオブジェクト を取得します。このプロパティは読み取り専用です。
        /// </summary>
        public BitmapSource ImageSourceBitmap
        {
            get => this.CurrentStamp?.ImageSourceBitmap ?? this._imageSourceBitmap;
        }
        

        #endregion

        #region Methods

        /// <summary>
        /// 初期化処理を行います。
        /// </summary>
        public void Initialize(SettingModel setting)
        {
            this.Setting = setting;

            // TODO 最後に編集していたスタンプをデフォルトで開くようにしたい
            // 初回は空で開く
            this.CurrentStamp = ContainerProvider.Resolve<StampModel>();
#if false
            using (var db = new DbProvider(setting.StoragePath))
            {
                this.CurrentStamp = StampModel.LoadById(db, 999999);
            }
#endif
        }

        /// <summary>
        /// 画像の入力ストリームより新しいスタンプを作成し、編集中のスタンプに設定します。
        /// </summary>
        /// <param name="inputStream">画像の入力ストリーム</param>
        public void CreateStamp(Stream sourceStream)
        {
            StampModel stamp;
            using (var db = new DbProvider(this.Setting.StoragePath))
            {
                db.BeginTransaction();

                if (this.CurrentStamp.EditState == EditState.Nothing)
                {
                    // TODO 今の編集中スタンプがまったく編集されていなければ削除？

                }
                else
                {
                    // TODO 今の編集中スタンプを仮保存

                }

                stamp = StampModel.CreateStamp(db, sourceStream);
                db.Commit();
            }
            this.CurrentStamp = stamp;
        }

        /// <summary>
        /// 文字列のパスが表す画像ファイルより新しいスタンプを作成し、編集中のスタンプに設定します。
        /// </summary>
        /// <param name="sourcePath">画像ファイルのパスを表す文字列</param>
        public void CreateStamp(string sourcePath)
        {
            using (var stream = File.Open(sourcePath, FileMode.Open, FileAccess.Read))
            {
                this.CreateStamp(stream);
            }
        }

        /// <summary>
        /// 編集中のスタンプを保存します。
        /// </summary>
        public void SaveStamp()
        {
            var stamp = this.CurrentStamp;

            using (var db = new DbProvider(this.Setting.StoragePath))
            {
                stamp.SaveStamp(db);
            }
        }

        /// <summary>
        /// スタンプ情報に従って画像の分割を行います。
        /// </summary>
        /// <param name="clipImage">分割する画像</param>
        public void DevideImage(BitmapSource clipImage)
        {
            this.CurrentStamp.DivideToFragments(clipImage);
        }

        /// <summary>
        /// 編集中のスタンプ の設定と初期化を行います。
        /// </summary>
        /// <param name="model">編集中のスタンプモデル</param>
        private void SetCurrentStamp(StampModel model)
        {
            if (null != this.CurrentStamp) this.CurrentStamp.PropertyChanged -= this.CurrentStamp_PropertyChanged;
            this._currentStamp = model;
            if (null != model)
            {
                model.PropertyChanged += this.CurrentStamp_PropertyChanged;
            }
            RaisePropertyChanged(nameof(this.CurrentStamp));
            RaisePropertyChanged(nameof(this.CanvasWidth));
            RaisePropertyChanged(nameof(this.CanvasHeight));
            RaisePropertyChanged(nameof(this.StampName));
            RaisePropertyChanged(nameof(this.StampLocalName));
            RaisePropertyChanged(nameof(this.PixelOfFragments));
            RaisePropertyChanged(nameof(this.ImageSourceBitmap));
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// 編集中スタンプのプロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">編集中スタンプモデル</param>
        /// <param name="e">イベント引数</param>
        private void CurrentStamp_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var stamp = sender as StampModel;
            // PropertyChangedを伝播する
            if (string.IsNullOrEmpty(e.PropertyName))
            {
                RaisePropertyChanged();
            }
            else switch (e.PropertyName)
            {
                case nameof(stamp.CanvasWidth):
                    RaisePropertyChanged(nameof(this.CanvasWidth));
                    break;
                case nameof(stamp.CanvasHeight):
                    RaisePropertyChanged(nameof(this.CanvasHeight));
                    break;
                case nameof(stamp.StampName):
                    RaisePropertyChanged(nameof(this.StampName));
                    break;
                case nameof(stamp.StampLocalName):
                    RaisePropertyChanged(nameof(this.StampLocalName));
                    break;
                case nameof(stamp.PixelOfFragments):
                    RaisePropertyChanged(nameof(this.PixelOfFragments));
                    break;
                case nameof(stamp.ImageSourceBitmap):
                    RaisePropertyChanged(nameof(this.ImageSourceBitmap));
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        public EditorModel()
        {
        }

        #endregion

    }
}
