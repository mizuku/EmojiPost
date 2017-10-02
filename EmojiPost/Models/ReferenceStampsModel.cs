using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace EmojiPost.Models
{

    /// <summary>
    /// スタンプ一覧参照モデル
    /// </summary>
    public class ReferenceStampsModel : BindableBase, ISingletonModel
    {

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        public ReferenceStampsModel()
        {
        }

        #endregion
    }
}
