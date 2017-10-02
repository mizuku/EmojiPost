using System;
using System.Collections.Generic;
using System.Linq;

namespace EmojiPost.DataServices.Clients
{

    /// <summary>
    /// クエリ実行結果の値の型
    /// </summary>
    public class ResultValue
    {

        /// <summary>
        /// 値
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 値の列名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 値の型
        /// </summary>
        public Type ValueType { get; set; }

    }
}
