using System.Windows;
using Autofac;

using AyaStyle.Models;

namespace AyaStyle
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public sealed partial class App : Application
    {

        #region Properties

        /// <summary>
        /// ブートストラッパー
        /// </summary>
        private Bootstrapper Bootstrapper { get; set; }

        /// <summary>
        /// アプリケーションモデル
        /// </summary>
        private ApplicationModel AppModel { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        private void Initialize()
        {
        }

        #endregion

        #region Override Application

        /// <summary>
        /// Startupイベントハンドラー
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Bootstrapper = new Bootstrapper();
            this.Bootstrapper.Run();
            ContainerProvider.InitializeContainer(this.Bootstrapper.Container);

            this.AppModel = ContainerProvider.Resolve<ApplicationModel>();
            this.AppModel.Initialize();

            Application.Current.MainWindow.Show();
        }

        #endregion

        #region Constructor

        public App()
        {
            this.Initialize();
        }

        #endregion

    }
}
