
using Prism.Mvvm;
using AyaStyle.Models;

namespace AyaStyle.ViewModels
{
    /// <summary>
    /// アプリケーションのメインビューモデル
    /// </summary>
    public class ShellViewModel : BindableBase
    {
        #region Properties

        private ShellModel _shell;
        /// <summary>
        /// アプリケーションのメインモデル を取得または設定します。
        /// </summary>
        public ShellModel Shell
        {
            get => this._shell;
            set => SetProperty(ref this._shell, value);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        public ShellViewModel()
        {
        }

        #endregion
    }
}
