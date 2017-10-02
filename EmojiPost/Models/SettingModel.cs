using EmojiPost.DataServices.Clients;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmojiPost.Models
{
    /// <summary>
    /// アプリケーションの設定モデル
    /// </summary>
    public class SettingModel : BindableBase, ISingletonModel
    {

        #region Properties

        /// <summary>
        /// 作業ディレクトリ を取得します。
        /// </summary>
        public string WorkDirectory { get; private set; }

        /// <summary>
        /// ストレージ名 を取得します。
        /// </summary>
        public string Storage { get; private set; }

        /// <summary>
        /// ストレージのフルパス名 を取得します。
        /// </summary>
        public string StoragePath { get => Path.Combine(this.WorkDirectory, this.Storage); }

        /// <summary>
        /// ワークスペースのコレクション を取得します。
        /// </summary>
        public List<WorkspaceModel> Workspaces { get; private set; }

        /// <summary>
        /// 現在のワークスペース を取得します。
        /// </summary>
        public WorkspaceModel CurrentWorkspace { get; }

        /// <summary>
        /// 現在作業中のスタンプID を取得します。
        /// </summary>
        public int CurrentStampId { get; }

        #endregion

        #region Methods

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        /// <param name="workDirectory">作業ディレクトリ</param>
        /// <param name="storage">ストレージ</param>
        public void Initialize(string workDirectory, string storage)
        {
            this.WorkDirectory = workDirectory;
            this.Storage = storage;
        }

        /// <summary>
        /// 指定されたプロバイダーを使用し、ストレージより設定を読み込みます。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        public void LoadFromStorage(DbProvider db)
        {

        }

        /// <summary>
        /// ストレージより設定を読み込みます。
        /// </summary>
        public void LoadFromStorage()
        {
            using (var db = new DbProvider(this.StoragePath))
            {
                this.LoadFromStorage(db);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        public SettingModel()
        {
        }

        #endregion

    }
}
