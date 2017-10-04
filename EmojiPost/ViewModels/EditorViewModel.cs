using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Reactive.Linq;
using Prism.Mvvm;
using Prism.Interactivity.InteractionRequest;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using EmojiPost.Models;
using EmojiPost.ViewModels.Notifications;

namespace EmojiPost.ViewModels
{
    /// <summary>
    /// スタンプ編集ビューモデル
    /// </summary>
    public class EditorViewModel : BindableBase
    {

        #region Properties

        private int _magnifyPercent;
        /// <summary>
        /// 拡縮率のパーセント表示 を取得または設定します。
        /// </summary>
        public int MagnifyPercent
        {
            get { return this._magnifyPercent; }
            set
            {
                var val = (int)Math.Floor(value / 5.0d) * 5;
                if (SetProperty(ref this._magnifyPercent, val))
                {
                    this.Magnify.Value = Math.Round(val / 100.0d, 2);
                }
            }
        }

        /// <summary>
        /// 拡縮率 を取得または設定します。
        /// </summary>
        public ReactiveProperty<double> Magnify { get; private set; }

        /// <summary>
        /// 編集中のスタンプモデル を取得または設定します。
        /// </summary>
        public ReactiveProperty<StampModel> CurrentStamp { get; private set; }

        /// <summary>
        /// 画像くり抜きリクエスト を取得します。
        /// </summary>
        public InteractionRequest<CropNotification> CropRequest { get; } = new InteractionRequest<CropNotification>();

        /// <summary>
        /// 編集元画像ファイルを開くコマンド を取得します。
        /// </summary>
        public ReactiveCommand<string> OpenImageFileCommand { get; }

        /// <summary>
        /// スタンプを保存するコマンド を取得します。
        /// </summary>
        public ReactiveCommand SaveStampCommand { get; }

        /// <summary>
        /// 画像くり抜き通知オブジェクト
        /// </summary>
        private CropNotification CropNotification { get; set; }

        public ReactiveCommand TestCommand { get; private set; }

        public BitmapSource ClipImage
        {
            get => this.CropNotification.ClipImage;
        }

        #endregion

        #region DI Models

        private EditorSettingReferenceModel _editorSettingReference;
        /// <summary>
        /// 編集画面の設定参照モデル を取得または設定します。
        /// </summary>
        public EditorSettingReferenceModel EditorSettingReference
        {
            get => this._editorSettingReference;
            set
            {
                if (this._editorSettingReference != value) this.SetEditorSettingReference(value);
            }
        }

        private EditorModel _editorModel;
        /// <summary>
        /// スタンプ編集モデル を取得または設定します。
        /// </summary>
        public EditorModel EditorModel
        {
            get => this._editorModel;
            set
            {
                if (this._editorModel != value) this.SetEditor(value);
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// 編集画面の設定参照モデル をこのビューモデルに対して設定および初期化を行います。
        /// </summary>
        /// <param name="model">編集画面の設定参照モデル</param>
        private void SetEditorSettingReference(EditorSettingReferenceModel model)
        {
            this._editorSettingReference = model;
            if (null != model)
            {
                #region Setup ReactiveProperty
                this.Magnify = model.ToReactivePropertyAsSynchronized(m => m.Magnify);
                #endregion
            }
        }

        /// <summary>
        /// スタンプ編集モデル をこのビューモデルに対して設定および初期化を行います。
        /// </summary>
        /// <param name="model">スタンプ編集モデル</param>
        private void SetEditor(EditorModel model)
        {
            this._editorModel = model;
            if (null != model)
            {
                #region Setup ReactiveProperty
                this.CurrentStamp = model.ToReactivePropertyAsSynchronized(m => m.CurrentStamp);
                #endregion
            }
        }

        /// <summary>
        /// 画像ファイルを開きます。
        /// </summary>
        /// <param name="filePath">画像ファイルのパス</param>
        private void OpenImageFile(string filePath)
        {
            // 新しいスタンプとして開く
            this.EditorModel.CreateStamp(filePath);
        }

        /// <summary>
        /// スタンプを保存します。
        /// </summary>
        private void SaveStamp()
        {
            this.EditorModel.SaveStamp();
        }
        
        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        public EditorViewModel()
        {
            this.Magnify = new ReactiveProperty<double>(1.0d);
            this.MagnifyPercent = 100;

            this.CropRequest = new InteractionRequest<CropNotification>();
            this.CropNotification = new CropNotification();

            this.OpenImageFileCommand = Observable.Return<bool>(true).ToReactiveCommand<string>();
            this.OpenImageFileCommand.Subscribe((s) => this.OpenImageFile(s));

            this.SaveStampCommand = Observable.Return<bool>(true).ToReactiveCommand();
            this.SaveStampCommand.Subscribe(() => this.SaveStamp());

            this.TestCommand = Observable.Return<bool>(true).ToReactiveCommand();
            this.TestCommand.Subscribe(() =>
            {
                this.CropNotification.ClipRect = new Rect(
                    this.CurrentStamp.Value.ClipRectLeft,
                    this.CurrentStamp.Value.ClipRectTop,
                    this.CurrentStamp.Value.ClipRectWidth,
                    this.CurrentStamp.Value.ClipRectHeight);
                this.CropRequest.Raise(this.CropNotification, n => RaisePropertyChanged(nameof(this.ClipImage)));
            });
        }

        #endregion

    }
}
