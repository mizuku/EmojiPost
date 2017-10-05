using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AyaStyle.DataServices.Clients;
using AyaStyle.DataServices.Entities;
using AyaStyle.DataServices.Utils;
using System.Data;

namespace AyaStyle.DataServices.Services.Implementations
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
