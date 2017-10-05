using System;

namespace AyaStyle.Enums
{
    /// <summary>
    /// 編集状態の列挙型
    /// </summary>
    [Flags]
    public enum EditState
    {
        /// <summary>
        /// 未作成
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 編集中
        /// </summary>
        Editing = 1,
        /// <summary>
        /// 保存済
        /// </summary>
        Saved = 2,
        /// <summary>
        /// 登録完了
        /// </summary>
        Completed = 4,
    }
}
