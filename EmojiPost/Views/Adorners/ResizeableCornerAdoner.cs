using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using EmojiPost.Models;

namespace EmojiPost.Views.Adorners
{
    /// <summary>
    /// 要素のリサイズを可能とするアドナー
    /// </summary>
    public class ResizeableCornerAdorner : Adorner
    {

        #region Fields

        private bool isDrag;
        private Point dragStart;
        private double dragStartWidth;
        private double dragStartHeight;
        private double magnify;

        #endregion

        #region Properties

        /// <summary>
        /// アドナーの装飾位置
        /// </summary>
        public Corners Corner { get; set; }

        #endregion

        #region Methods
        
        /// <summary>
        /// マウスカーソルを設定します。
        /// </summary>
        private void SetMouseCursor()
        {
            switch (this.Corner)
            {
                case Corners.TopLeft:
                    Mouse.OverrideCursor = Cursors.SizeNWSE;
                    break;
                case Corners.TopRight:
                    Mouse.OverrideCursor = Cursors.SizeNESW;
                    break;
                case Corners.BottomLeft:
                    Mouse.OverrideCursor = Cursors.SizeNESW;
                    break;
                case Corners.BottomRight:
                    Mouse.OverrideCursor = Cursors.SizeNWSE;
                    break;
            }
        }

        /// <summary>
        /// ドラッグが終了したときの処理
        /// </summary>
        private void DragFinished()
        {
            if (this.isDrag)
            {
                this.dragStart = new Point(0.0d, 0.0d);
                this.dragStartWidth = 0.0d;
                this.dragStartHeight = 0.0d;
                this.magnify = 1.0d;
                this.isDrag = false;
            }
            this.ReleaseMouseCapture();
        }

        #endregion

        #region Override Adorner

        /// <summary>
        /// アドナーを描画します。
        /// </summary>
        /// <param name="drawingContext">描画コンテキスト</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            var adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            var renderBrush = (Brush)Application.Current.FindResource("PrimaryHueLightBrush");
            var renderPen = new Pen((Brush)Application.Current.FindResource("PrimaryHueDarkBrush"), 1.0);

            Point cornerPos;
            switch (this.Corner)
            {
                case Corners.TopLeft:
                    cornerPos = adornedElementRect.TopLeft;
                    break;
                case Corners.TopRight:
                    cornerPos = adornedElementRect.TopRight;
                    break;
                case Corners.BottomLeft:
                    cornerPos = adornedElementRect.BottomLeft;
                    break;
                case Corners.BottomRight:
                    cornerPos = adornedElementRect.BottomRight;
                    break;
                default:
                    throw new NotSupportedException("Cornerプロパティが割り当てられていません");
            }

            double renderRadius = 5.0d;
            drawingContext.DrawEllipse(renderBrush, renderPen, cornerPos, renderRadius, renderRadius);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            // ドラッグ開始時
            this.isDrag = true;
            this.magnify = ContainerProvider.Resolve<EditorSettingReferenceModel>().Magnify;
            var parent = this.Parent as AdornerLayer;
            this.dragStart = Mouse.GetPosition(parent);
            var el = this.AdornedElement as FrameworkElement;
            if (null != el)
            {
                this.dragStartWidth = el.Width;
                this.dragStartHeight = el.Height;
            }

            this.CaptureMouse();
            this.SetMouseCursor();

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
                var parent = this.Parent as AdornerLayer;
                var dragEnd = Mouse.GetPosition(parent);

                var el = this.AdornedElement as FrameworkElement;
                if (null != el)
                {
                    var moveX = (dragEnd.X - dragStart.X) / this.magnify;
                    var moveY = (dragEnd.Y - dragStart.Y) / this.magnify;
                    switch (this.Corner)
                    {
                        case Corners.TopLeft:
                            {
                                var w = Math.Max(dragStartWidth - moveX, el.MinWidth);
                                var h = Math.Max(dragStartHeight - moveY, el.MinHeight);
                                var size = new Size(w, h);
                                var s = (el.DataContext as IDraggableItem)?.Coerce(size) ?? size;
                                el.Width = s.Width;
                                el.Height = s.Height;
                                // 最小サイズを下回るときに場所だけ変わらないように差分で移動
                                Canvas.SetLeft(el, dragStartWidth + (this.dragStart.X / this.magnify) - s.Width);
                                Canvas.SetTop(el, dragStartHeight + (this.dragStart.Y / this.magnify) - s.Height);
                            }
                            break;
                        case Corners.TopRight:
                            {
                                var w = Math.Max(dragStartWidth + moveX, el.MinWidth);
                                var h = Math.Max(dragStartHeight - moveY, el.MinHeight);
                                var size = new Size(w, h);
                                var s = (el.DataContext as IDraggableItem)?.Coerce(size) ?? size;
                                el.Width = s.Width;
                                el.Height = s.Height;
                                // 最小サイズを下回るときに場所だけ変わらないように差分で移動
                                Canvas.SetTop(el, dragStartHeight + (this.dragStart.Y / this.magnify) - s.Height);
                            }
                            break;
                        case Corners.BottomLeft:
                            {
                                var w = Math.Max(dragStartWidth - moveX, el.MinWidth);
                                var h = Math.Max(dragStartHeight + moveY, el.MinHeight);
                                var size = new Size(w, h);
                                var s = (el.DataContext as IDraggableItem)?.Coerce(size) ?? size;
                                el.Width = s.Width;
                                el.Height = s.Height;
                                // 最小サイズを下回るときに場所だけ変わらないように差分で移動
                                Canvas.SetLeft(el, dragStartWidth + (this.dragStart.X / this.magnify) - s.Width);
                            }
                            break;
                        case Corners.BottomRight:
                            {
                                var w = Math.Max(dragStartWidth + moveX, el.MinWidth);
                                var h = Math.Max(dragStartHeight + moveY, el.MinHeight);
                                var size = new Size(w, h);
                                var s = (el.DataContext as IDraggableItem)?.Coerce(size) ?? size;
                                el.Width = s.Width;
                                el.Height = s.Height;
                            }
                            break;
                        default:
                            break;
                    }
                }

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
        /// 装飾するUI要素と装飾位置を指定し、このクラスのインスタンスを生成するコンストラクタです。
        /// </summary>
        /// <param name="adornedElement">装飾するUI要素</param>
        /// <param name="corner">装飾位置</param>
        public ResizeableCornerAdorner(UIElement adornedElement, Corners corner)
            : base(adornedElement)
        {
            this.Corner = corner;
        }

        #endregion
    }
}
