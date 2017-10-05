using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Prism.Mvvm;
using AyaStyle.DataServices.Services;
using AyaStyle.DataServices.Clients;

namespace AyaStyle.Models
{
    /// <summary>
    /// アプリケーションのルートモデル
    /// </summary>
    public class ApplicationModel : BindableBase, ISingletonModel
    {

        #region Properties

        /// <summary>
        /// アプリケーションの設定モデルを取得します。
        /// </summary>
        public SettingModel Setting { get; private set; }

        /// <summary>
        /// アプリケーションのメインモデルを取得します。
        /// </summary>
        public ShellModel Shell { get; private set; }

        #endregion

        #region DI Services

        /// <summary>
        /// ストレージサービス を取得または設定します。
        /// </summary>
        public IStorageService StorageService { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public void Initialize()
        {
            #region Setting
            var assembly = Assembly.GetEntryAssembly();
            var setting = ContainerProvider.Resolve<SettingModel>();
            setting.Initialize(new FileInfo(assembly.Location).DirectoryName, "data.dat");
            this.Setting = setting;
            #endregion

            this.PrepareStorage(false);

            // 起動時の設定を読み込む
            using (var db = new DbProvider(setting.StoragePath))
            {
                setting.LoadFromStorage(db);

                var shell = ContainerProvider.Resolve<ShellModel>();
                shell.Initialize(setting);
                this.Shell = shell;
            }
        }

        /// <summary>
        /// ストレージの初期化を行います。このメソッドはインスタンス生成時に実行されます。
        /// </summary>
        /// <param name="isOverride">ストレージが存在していたとき、上書きするならtrue, 上書きしないならfalse</param>
        public void PrepareStorage(bool isOverride)
        {
            var storageFile = new FileInfo(this.Setting.StoragePath);
            if (false == storageFile.Exists || isOverride)
            {
                if (storageFile.Exists)
                {
                    storageFile.Delete();
                }

                try
                {
                    using (var db = new DbProvider(storageFile.FullName))
                    {
                        this.StorageService.Create(db);
                    }
                }
                catch (Exception e)
                {
                    var fi = new FileInfo(storageFile.FullName);
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                    throw e;
                }
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        public ApplicationModel()
        {
        }

        #endregion

    }
}
