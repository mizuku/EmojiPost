using System;
using Autofac;

namespace AyaStyle
{
    /// <summary>
    /// アプリケーションのDIコンテナプロバイダー
    /// </summary>
    public static class ContainerProvider
    {
        #region Properties

        /// <summary>
        /// DIコンテナ を取得します。
        /// </summary>
        public static IContainer Container { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// DIコンテナを初期化します。一度のみ実行することができます。
        /// </summary>
        /// <param name="container"></param>
        public static void InitializeContainer(IContainer container)
        {
            if (null != ContainerProvider.Container)
            {
                throw new NotSupportedException("すでにDIコンテナは初期化されています。");
            }
            ContainerProvider.Container = container;
        }

        /// <summary>
        /// DIコンテナよりインスタンスを取得します。
        /// </summary>
        /// <typeparam name="T">インスタンスを取得する型</typeparam>
        /// <returns>インスタンス</returns>
        public static T Resolve<T>()
        {
            return ContainerProvider.Container.Resolve<T>();
        }

        #endregion

        #region Constructor
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static ContainerProvider()
        {
            ContainerProvider.Container = null;
        }
        #endregion
    }
}
