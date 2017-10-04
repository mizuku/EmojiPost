using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmojiPost.DataServices.Clients;
using EmojiPost.DataServices.Entities;

namespace EmojiPost.DataServices.Services
{
    /// <summary>
    /// スタンプ断片サービスのインタフェース
    /// </summary>
    public interface IFragmentService
    {
        #region Methods

        /// <summary>
        /// ストレージへスタンプ断片エンティティの内容を追加します。
        /// </summary>
        /// <param name="db">データベースプロバイダー</param>
        /// <param name="fragment">スタンプ断片エンティティ</param>
        /// <returns>追加件数</returns>
        int Insert(DbProvider db, FragmentEntity fragment);

        #endregion
    }
}
