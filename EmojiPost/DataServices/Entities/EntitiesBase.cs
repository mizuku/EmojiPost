using EmojiPost.DataServices.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmojiPost.DataServices.Entities
{
    /// <summary>
    /// エンティティモデルの抽象クラス
    /// </summary>
    [DataContract]
    public abstract class EntitiesBase
    {
        #region Methods

        #endregion

        #region Constructor

        /// <summary>
        /// 既定のコンストラクタ
        /// </summary>
        public EntitiesBase()
        {
        }

        #endregion
    }
}
