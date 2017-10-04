using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmojiPost.DataServices.Clients;
using EmojiPost.DataServices.Entities;
using EmojiPost.DataServices.Utils;
using System.Data;

namespace EmojiPost.DataServices.Services.Implementations
{
    /// <summary>
    /// スタンプ断片サービス の実装
    /// </summary>
    public class FragmentService : IFragmentService
    {
        #region Implements IFragmentService

        public int Insert(DbProvider db, FragmentEntity fragment)
        {
            string sql = SqlHelper.MakeInsertDML(typeof(FragmentEntity));
            var parameters = SqlHelper.GetEntityBindparameters(fragment);

            return db.ExecuteNonQuery(sql, parameters);
        }

        public int DeleteByStampId(DbProvider db, int stampId)
        {
            string sql = @"DELETE FROM Fragments
WHERE StampId = @0
";
            return db.ExecuteNonQuery(sql, (stampId, DbType.Int32));
        }

        public int NextFragmentId(DbProvider db)
        {
            string sql = @"SELECT
IFNULL(MAX(FragmentId), 0) + 1
FROM Fragments
";
            var result = db.ExecuteScalar(sql, null);
            if (null == result) throw new ApplicationException();

            return int.Parse(result.ToString());
        }

        #endregion
    }
}
