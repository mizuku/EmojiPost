using System;
using System.Collections.Generic;
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
            StampEntity entity = new StampEntity();
#if DEBUG
            if (999999 == stampId)
            {
                entity = new StampEntity()
                {
                    StampId = stampId,
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

        public void SaveAsLocal(string directory, StampEntity stamp, IEnumerable<FragmentEntity> fragments)
        {
            if (string.IsNullOrWhiteSpace(directory) || null == stamp
                || null == fragments || false == fragments.Any())
            {
                throw new ArgumentException();
            }

            var stampDirectory = $"{directory}{Path.DirectorySeparatorChar}{stamp.StampName}";
            if (false == Directory.Exists(stampDirectory))
            {
                Directory.CreateDirectory(stampDirectory);
            }
            var dir = new DirectoryInfo(stampDirectory);

            // 断片画像の書き込み
            var parent = $"{dir.FullName}{Path.DirectorySeparatorChar}";
            foreach (var f in fragments)
            {
                using (var imageStream = File.Open($"{parent}{f.EmojiName}.png", FileMode.Create, FileAccess.Write))
                {
                    imageStream.Write(f.Image, 0, f.Image.Length);
                    imageStream.Flush();
                }
            }

            // スタンプと断片をYAMLにシリアライズする
            using (var yamlWriter = new StreamWriter($"{parent}definitions.yaml"))
            {
                var serializer = new YamlDotNet.Serialization.Serializer();
                var graph = new
                {
                    title = stamp.StampName,
                    emojis = fragments.Select(f => new { name = f.EmojiName, src = $"{parent}{f.EmojiName}.png" }),
                };
                serializer.Serialize(yamlWriter, graph);
            }

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
