using System.Windows;
using System.Windows.Media.Imaging;

using Prism.Interactivity.InteractionRequest;

namespace EmojiPost.ViewModels.Notifications
{
    /// <summary>
    /// 画像くり抜きの通知オブジェクト
    /// </summary>
    public class CropNotification : INotification
    {

        #region Properties

        /// <summary>
        /// くり抜き領域を表す<see cref="Rect"/>を取得または設定します。
        /// </summary>
        public Rect ClipRect { get; set; }

        /// <summary>
        /// くり抜き結果の画像を取得または設定します。
        /// </summary>
        public BitmapSource ClipImage { get; set; } 

        #endregion

        #region Implements INotification

        public string Title { get; set; }
        public object Content { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        public CropNotification()
        {
        }

        #endregion

    }
}
