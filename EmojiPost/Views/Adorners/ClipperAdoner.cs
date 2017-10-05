using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace AyaStyle.Views.Adorners
{
    /// <summary>
    /// くり抜き領域を装飾するアドナー
    /// </summary>
    public class ClipperAdorner : Adorner
    {

        #region Override Adorner

        /// <summary>
        /// アドナーを描画します。
        /// </summary>
        /// <param name="drawingContext">描画コンテキスト</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            var adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
            var renderPen = new Pen((Brush)Application.Current.FindResource("PrimaryHueDarkBrush"), 1.0);
            drawingContext.DrawRectangle(null, renderPen, adornedElementRect);
        }


        #endregion

        #region Constructor

        /// <summary>
        /// 装飾するUI要素を指定し、このクラスのインスタンスを生成するコンストラクタです。
        /// </summary>
        /// <param name="adornedElement">装飾するUI要素</param>
        public ClipperAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
        }

        #endregion
    }
}
