using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmojiPost.Models
{

    /// <summary>
    /// アプリケーションのメインモデル
    /// </summary>
    public class ShellModel : BindableBase, ISingletonModel
    {

        #region Properties

        /// <summary>
        /// アプリケーションの設定モデル を取得または設定します。
        /// </summary>
        private SettingModel Setting { get; set; }

        /// <summary>
        /// スタンプ編集モデル を取得します。
        /// </summary>
        public EditorModel Editor { get; private set; }

        /// <summary>
        /// 現在編集中のスタンプ を取得します。
        /// </summary>
        public StampModel CurrentStamp { get => this.Editor?.CurrentStamp; }

        #endregion

        #region Methods

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        /// <param name="setting">アプリケーションの設定モデル</param>
        public void Initialize(SettingModel setting)
        {
            this.Setting = setting;

            var editor = ContainerProvider.Resolve<EditorModel>();
            editor.Initialize(setting);
            editor.PropertyChanged += this.Editor_PropertyChanged;
            this.Editor = editor;
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// エディターのプロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">エディターモデル</param>
        /// <param name="e">イベント引数</param>
        private void Editor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var editor = sender as EditorModel;
            // PropertyChangedを伝播する
            if (string.IsNullOrEmpty(e.PropertyName))
            {
                RaisePropertyChanged(nameof(this.CurrentStamp));
            }
            else switch (e.PropertyName)
            {
                case nameof(editor.CurrentStamp):
                    RaisePropertyChanged(nameof(this.CurrentStamp));
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
        public ShellModel()
        {
        }

        #endregion

    }
}
