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
        public ReactiveCommand SaveStampCommand { get; private set; }

        /// <summary>
        /// 画像くり抜きコマンド
        /// </summary>
        public ReactiveCommand CropCommand { get; private set; }

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

        private EditorModel _editor;
        /// <summary>
        /// スタンプ編集モデル を取得または設定します。
        /// </summary>
        public EditorModel Editor
        {
            get => this._editor;
            set
            {
                if (this._editor != value) this.SetEditor(value);
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
            this._editor = model;
            if (null != model)
            {
                #region Setup ReactiveProperty
                this.CurrentStamp = model.ToReactivePropertyAsSynchronized(m => m.CurrentStamp);

                this.SaveStampCommand = model.ObserveProperty(m => m.ImageSourceBitmap)
                    .Select(p => null != p)
                    .ToReactiveCommand();
                this.SaveStampCommand.Subscribe(() => this.SaveStamp());

                var p0 = model.ObserveProperty(m => m.ImageSourceBitmap);
                var p1 = model.ObserveProperty(m => m.StampName);
                var p2 = model.ObserveProperty(m => m.PixelOfFragments);
                this.CropCommand = p0.CombineLatest(p1, p2,
                    (pp0, pp1, pp2) => null != pp0 && !string.IsNullOrWhiteSpace(pp1) && 0 < pp2)
                    .ToReactiveCommand();
                this.CropCommand.Subscribe(() =>
                {
                    this.CropRequest.Raise(
                        new CropNotification(this.CurrentStamp.Value.ClipRect),
                        n => this.Editor.DevideImage(n.ClipImage));
                });
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
            this.Editor.CreateStamp(filePath);
        }

        /// <summary>
        /// スタンプを保存します。
        /// </summary>
        private void SaveStamp()
        {
            this.Editor.SaveStamp();
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

            this.OpenImageFileCommand = Observable.Return<bool>(true)
                .ToReactiveCommand<string>();
            this.OpenImageFileCommand.Subscribe((s) => this.OpenImageFile(s));
        }

        #endregion

    }
}
