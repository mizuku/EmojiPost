using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Interactivity.InteractionRequest;

using AyaStyle.ViewModels.Notifications;

namespace AyaStyle.Views
{
    /// <summary>
    /// 画像くり抜きトリガーアクション
    /// </summary>
    public class CropAction : TriggerAction<Canvas>
    {
        protected override void Invoke(object parameter)
        {
            var e = parameter as InteractionRequestedEventArgs;
            if (null == e) return;
            var notification = e.Context as CropNotification;
            if (null == notification) return;

            var clipRect = notification.ClipRect;
            var canvas = this.AssociatedObject as Canvas;

            // Measure > Arrange
            var size = new Size(canvas.ActualWidth, canvas.ActualHeight);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            var renderTarget = new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96.0d,
                96.0d,
                PixelFormats.Pbgra32);
            // Render
            renderTarget.Render(canvas);

            // Crop
            var bmp = new CroppedBitmap(renderTarget,
                new Int32Rect((int)clipRect.Left, (int)clipRect.Top, (int)clipRect.Width, (int)clipRect.Height));

            // 結果
            notification.ClipImage = bmp;
#if false
            using (var stream = new FileStream(@"C:\work\aya.png", FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(stream);
            }
#endif
            e.Callback?.Invoke();
        }
    }
}
