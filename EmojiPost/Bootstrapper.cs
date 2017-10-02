using System.Linq;
using System.Windows;

using Prism.Autofac;
using Autofac;

using EmojiPost.Views;
using EmojiPost.Models;

namespace EmojiPost
{
    /// <summary>
    /// アプリケーションのブートストラッパー
    /// </summary>
    sealed class Bootstrapper : AutofacBootstrapper
    {

        #region Methods

        #endregion

        #region Overrides AutofacBootstrapper

        /// <summary>
        /// DIコンテナを構築します。
        /// </summary>
        /// <param name="builder">コンテナビルダー</param>
        protected override void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            base.ConfigureContainerBuilder(builder);

            // Serviceの登録
            builder
                .RegisterAssemblyTypes(typeof(EmojiPost.App).Assembly)
                .Where(x => x.IsInNamespace("EmojiPost.DataServices.Services.Implementations"))
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            // 非Singleton Modelの登録
            builder
                .RegisterAssemblyTypes(typeof(EmojiPost.App).Assembly)
                .Where(x => x.IsInNamespace("EmojiPost.Models"))
                .Where(x => x.Name.EndsWith("Model"))
                .Where(x => !typeof(ISingletonModel).IsAssignableFrom(x))
                .AsSelf();

            // Singleton Modelの登録
            builder
                .RegisterAssemblyTypes(typeof(EmojiPost.App).Assembly)
                .Where(x => x.IsInNamespace("EmojiPost.Models"))
                .Where(x => x.Name.EndsWith("Model"))
                .Where(x => typeof(ISingletonModel).IsAssignableFrom(x))
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();

            // ViewModelの登録
            // Property Injection で Autowiring しないなら何も書かなくても勝手に登録してくれる (6.3以降？)
            builder
                .RegisterAssemblyTypes(typeof(EmojiPost.App).Assembly)
                .Where(x => x.IsInNamespace("EmojiPost.ViewModels"))
                .Where(x => x.Name.EndsWith("ViewModel"))
                .AsSelf()
                // PropertiesAutowired()が肝らしい
                // http://docs.autofac.org/en/latest/register/prop-method-injection.html
                .PropertiesAutowired();

            // Viewの登録
            builder.Register(c => new Shell()).As<Shell>();

        }

        /// <summary>
        /// 既定のウインドウ <see cref="EmojiPost.Views.Shell"/> を作成します。
        /// </summary>
        /// <returns>Shellの実体</returns>
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Shell>();
        }

        /// <summary>
        /// 既定のウインドウ <see cref="EmojiPost.Views.Shell"/> を初期化します。
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Shell)Shell;
        }

#endregion

    }
}
