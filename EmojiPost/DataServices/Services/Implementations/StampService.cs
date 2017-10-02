using System;
using System.IO;
using System.Linq;

using EmojiPost.DataServices.Clients;
using EmojiPost.DataServices.Entities;
using EmojiPost.DataServices.Utils;

namespace EmojiPost.DataServices.Services.Implementations
{
    /// <summary>
    /// スタンプサービス の実装
    /// </summary>
    public class StampService : IStampService
    {

        #region Implements IStampService

        public StampEntity ReadEntities(DbProvider db, int stampId)
        {
            StampEntity entity;
#if DEBUG
            entity = new StampEntity()
            {
                StampId = 1,
                WorkspaceId = 1,
                StampName = "ayamini",
                StampLocalName = "SD丸山彩",
                CanvasWidth = 300,
                CanvasHeight = 300,
                DateOfCreate = DateTime.Today.ToString(),
                DateOfUpdate = DateTime.Today.ToString()
            };

            using (var stream = new MemoryStream(File.ReadAllBytes(@"C:\Users\morita\Pictures\ayamini_128.png")))
            {
                entity.ImageSource = stream.ToArray();
            }
#endif
            return entity;
        }

        public int Insert(DbProvider db, StampEntity stamp)
        {
            string sql = SqlHelper.MakeInsertDML(typeof(StampEntity));
            var parameters = SqlHelper.GetEntityBindparameters(stamp);

            return db.ExecuteNonQuery(sql, parameters);
        }

        public int Update(DbProvider db, StampEntity stamp)
        {
            string sql = SqlHelper.MakeUpdateDML(typeof(StampEntity));
            var parameters = SqlHelper.GetEntityBindparameters(stamp);

            return db.ExecuteNonQuery(sql, parameters);
        }

        public int NextStampId(DbProvider db)
        {
            string sql = @"SELECT
IFNULL(MAX(StampId), 0) + 1
FROM Stamps
";
            var result = db.ExecuteScalar(sql, null);
            if (null == result) throw new ApplicationException();

            return int.Parse(result.ToString());
        }

        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        public StampService()
        {
        }

        #endregion

    }
}
