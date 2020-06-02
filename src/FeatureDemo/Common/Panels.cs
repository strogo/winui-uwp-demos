using DevExpress.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FeatureDemo.Common {
    public class RotatedHorizontalWrapPanel : Panel {
        #region Dependency Properties
        public static readonly DependencyProperty MinimumColumnsProperty;
        public static readonly DependencyProperty ItemWidthProperty;
        public static readonly DependencyProperty ItemHeightProperty;

        static RotatedHorizontalWrapPanel() {
            Type ownerType = typeof(RotatedHorizontalWrapPanel);
            MinimumColumnsProperty = DependencyProperty.Register("MinimumColumns", typeof(int), ownerType, new PropertyMetadata(-1, InvalidateMeasure));
            ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), ownerType, new PropertyMetadata(300.0, InvalidateMeasure));
            ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), ownerType, new PropertyMetadata(100.0, InvalidateMeasure));
        }
        static void InvalidateMeasure(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((UIElement)d).InvalidateMeasure();
        }
        #endregion

        public int MinimumColumns { get { return (int)GetValue(MinimumColumnsProperty); } set { SetValue(MinimumColumnsProperty, value); } }
        public double ItemWidth { get { return (double)GetValue(ItemWidthProperty); } set { SetValue(ItemWidthProperty, value); } }
        public double ItemHeight { get { return (double)GetValue(ItemHeightProperty); } set { SetValue(ItemHeightProperty, value); } }
        protected override Size MeasureOverride(Size availableSize) {
            if(Children.Count == 0) return new Size();
            Size childSize = new Size(ItemWidth, ItemHeight);
            foreach(UIElement child in Children)
                child.Measure(childSize);
            int rowsCount = (int)Math.Floor(availableSize.Height / childSize.Height);
            int columnsCount = (Children.Count + rowsCount - 1) / rowsCount;
            if(MinimumColumns >= 0 && columnsCount < MinimumColumns)
                columnsCount = MinimumColumns;
            return new Size(columnsCount * childSize.Width, availableSize.Height);
        }
        protected override Size ArrangeOverride(Size finalSize) {
            if(Children.Count == 0) return finalSize;
            Size childSize = new Size(ItemWidth, ItemHeight);
            int rowsCount = (int)Math.Floor(finalSize.Height / childSize.Height);
            int columnsCount = (Children.Count + rowsCount - 1) / rowsCount;
            if(MinimumColumns >= 0 && columnsCount < MinimumColumns)
                columnsCount = MinimumColumns;
            double width = columnsCount * childSize.Width;
            double x = 0.0;
            double y = 0.0;
            foreach(UIElement child in Children) {
                if(x + childSize.Width > width) {
                    x = 0.0;
                    y += childSize.Height;
                }
                child.Arrange(new Rect(x, y, childSize.Width, childSize.Height));
                x += childSize.Width;
            }
            return finalSize;
        }
    }
    public class DebugPanel : Panel {
        protected override Size MeasureOverride(Size availableSize) {
            double width = 0.0;
            double height = 0.0;
            foreach(UIElement child in Children) {
                child.Measure(availableSize);
                if(child.DesiredSize.Width > width)
                    width = child.DesiredSize.Width;
                if(child.DesiredSize.Height > height)
                    height = child.DesiredSize.Height;
            }
            return new Size(width, height);
        }
        protected override Size ArrangeOverride(Size finalSize) {
            foreach(UIElement child in Children) {
                child.Arrange(new Rect(new Point(), finalSize));
            }
            return finalSize;
        }
    }
}
