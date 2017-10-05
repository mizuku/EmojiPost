using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using AyaStyle.Views.Adorners;

namespace AyaStyle.Views.Controls
{
    /// <summary>
    /// ドラッグ可能なコントロールのコンテナー
    /// </summary>
    public class DraggableItemsControl : Selector
    {
        #region Fields

        /// <summary>
        /// このコンテナーに対応するアドナーレイヤー
        /// </summary>
        private AdornerLayer adornerLayer;
        /// <summary>
        /// 子要素とアドナーの関連を管理
        /// </summary>
        private Dictionary<UIElement, Adorner[]> elementAdorners = new Dictionary<UIElement, Adorner[]>();
        /// <summary>
        /// くり抜き領域のアドナー
        /// </summary>
        private Adorner clipperAdorner;

        #endregion

        #region Methods

        /// <summary>
        /// 要素が Clipper であるかどうかを取得します。
        /// </summary>
        /// <param name="element">要素</param>
        /// <returns>elementがClipperだったときtrue, Clipperでなかったときfalse</returns>
        private bool IsClipperElement(DependencyObject element)
        {
            var el = element as FrameworkElement;
            return "Clipper" == el?.Name;
        }

        #endregion

        #region Overrides Selector

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.adornerLayer = AdornerLayer.GetAdornerLayer(this);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var el = element as UIElement;
            if (this.IsClipperElement(el))
            {
                this.clipperAdorner = new ClipperAdorner(el);
                this.adornerLayer.Add(this.clipperAdorner);
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            var el = element as UIElement;
            if (this.IsClipperElement(el))
            {
                this.adornerLayer.Remove(this.clipperAdorner);
            }

            base.ClearContainerForItemOverride(element, item);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var target = e.OriginalSource as FrameworkElement;
            if (this.Items.Contains(target))
            {
                this.SelectedItem = target;
                // キーボードフォーカスが欲しいのでフォーカスしておく
                this.Focus();
            }
            else
            {
                this.SelectedItem = null;
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            // ハンドル削除
            foreach (var item in e.RemovedItems)
            {
                var element = item as UIElement;
                if (null != element && this.elementAdorners.ContainsKey(element))
                {
                    var adorners = this.elementAdorners[element];
                    this.elementAdorners.Remove(element);
                    foreach (var ad in adorners)
                    {
                        adornerLayer.Remove(ad);
                    }
                }
            }
            // ハンドル追加
            foreach (var item in e.AddedItems)
            {
                var element = item as UIElement;
                if (null != element)
                {
                    Adorner[] adorners;
                    if (this.IsClipperElement(element))
                    {
                        // Clipperは移動とリサイズ
                        adorners = new Adorner[]
                        {
                            new MovableBorderAdorner(element),
                            new ResizeableCornerAdorner(element, Corners.TopLeft),
                            new ResizeableCornerAdorner(element, Corners.TopRight),
                            new ResizeableCornerAdorner(element, Corners.BottomLeft),
                            new ResizeableCornerAdorner(element, Corners.BottomRight),
                        };
                    }
                    else
                    {
                        // Clipperでなければ移動のみ
                        adorners = new []
                        {
                            new MovableBorderAdorner(element)
                        };
                    }
                    this.elementAdorners.Add(element, adorners);
                    foreach (var ad in adorners)
                    {
                        adornerLayer.Add(ad);
                    }
                }
            }

        }

        #endregion

        #region Constructor
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static DraggableItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DraggableItemsControl), new FrameworkPropertyMetadata(typeof(DraggableItemsControl)));
        }
        #endregion

    }
}
