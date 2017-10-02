using System;
using System.IO;
using System.Linq;

using EmojiPost.DataServices.Clients;
using EmojiPost.DataServices.Utils;
using EmojiPost.DataServices.Entities;

namespace EmojiPost.DataServices.Services.Implementations
{

    /// <summary>
    /// ストレージサービスの実装
    /// </summary>
    public class StorageService : IStorageService
    {

        #region Implements IStorageService

        public void Create(DbProvider db)
        {
            db.ExecuteNonQuery(SqlHelper.MakeCreateDDL(typeof(SettingEntity)));
            db.ExecuteNonQuery(SqlHelper.MakeCreateDDL(typeof(WorkspaceEntity)));
            db.ExecuteNonQuery(SqlHelper.MakeCreateDDL(typeof(StampEntity)));
            db.ExecuteNonQuery(SqlHelper.MakeCreateDDL(typeof(FragmentEntity)));
        }

        public void Drop(DbProvider db)
        {
            db.ExecuteNonQuery(SqlHelper.MakeDropDDL(typeof(SettingEntity)));
            db.ExecuteNonQuery(SqlHelper.MakeDropDDL(typeof(WorkspaceEntity)));
            db.ExecuteNonQuery(SqlHelper.MakeDropDDL(typeof(StampEntity)));
            db.ExecuteNonQuery(SqlHelper.MakeDropDDL(typeof(FragmentEntity)));
        }

        public void Import(DbProvider db, Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Export(DbProvider db, Stream stream)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        public StorageService()
        {
        }

        #endregion

    }
}
