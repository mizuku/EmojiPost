using System.Windows;

namespace EmojiPost.Models
{
    /// <summary>
    /// ドラッグ操作の可能なアイテムであることを表すインタフェース
    /// </summary>
    public interface IDraggableItem
    {
        /// <summary>
        /// 指定されたsizeに基づく、このアイテムに適用可能な大きさを返します。
        /// </summary>
        /// <param name="size"><see cref="System.Windows.Size"/></param>
        /// <returns>このアイテムに適用可能な大きさを持つ<see cref="System.Windows.Size"/></returns>
        Size Coerce(Size size);
    }
}
