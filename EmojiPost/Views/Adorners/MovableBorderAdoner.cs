using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using AyaStyle.Models;

namespace AyaStyle.Views.Adorners
{
    /// <summary>
    /// 要素の移動を可能とするアドナー
    /// </summary>
    public class MovableBorderAdorner : Adorner
    {

        #region Fields

        private bool isDrag;
        private Point dragOffset;
        private IInputElement adornedElementParent;

        #endregion

        #region Methods

        /// <summary>
        /// ドラッグが終了したときの処理
        /// </summary>
        private void DragFinished()
        {
            if (this.isDrag)
            {
                this.dragOffset = new Point(0.0d, 0.0d);
                this.isDrag = false;
            }
            this.ReleaseMouseCapture();
        }

        #endregion

        #region Override Adorner

        /// <summary>
        /// アドナーを初期化します。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var parent = VisualTreeHelper.GetParent(this.AdornedElement) as IInputElement;
            if (null != parent)
            {
                this.adornedElementParent = parent;
            }
            else
            {
                throw new ApplicationException("このアドナーを装飾することのできないオブジェクトです。");
            }
        }

        /// <summary>
        /// アドナーを描画します。
        /// </summary>
        /// <param name="drawingContext">描画コンテキスト</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            var adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
            
            //var renderBrush = (Brush)Application.Current.FindResource("PrimaryHueLightBrush");
            var renderBrush = (Brush)Application.Current.FindResource("AccentTransparentBrush");
            var renderPen = new Pen((Brush)Application.Current.FindResource("PrimaryHueMidBrush"), 2.0);
            renderPen.DashStyle = DashStyles.Dash;

            drawingContext.DrawRectangle(renderBrush, renderPen, adornedElementRect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.isDrag = true;
            this.dragOffset = e.GetPosition(this.AdornedElement);
            this.CaptureMouse();
            Mouse.OverrideCursor = Cursors.ScrollAll;

            base.OnPreviewMouseDown(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            this.DragFinished();

            base.OnPreviewMouseUp(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
            base.OnLostMouseCapture(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (this.isDrag)
            {
                var pos = Mouse.GetPosition(this.adornedElementParent);

                Canvas.SetLeft(this.AdornedElement, pos.X - dragOffset.X);
                Canvas.SetTop(this.AdornedElement, pos.Y - dragOffset.Y);
            }

            base.OnPreviewMouseMove(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            this.DragFinished();

            base.OnLostFocus(e);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 装飾するUI要素を指定し、このクラスのインスタンスを生成するコンストラクタです。
        /// </summary>
        /// <param name="adornedElement">装飾するUI要素</param>
        public MovableBorderAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
        }

        #endregion
    }
}
