using System;
using System.Collections;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DevExpress.Mvvm.UI.Native;
using DevExpress.UI.Xaml.Gauges.Native;

namespace MapDemo {
    public class PhotoGalleryControl : VisibleControl {
        Point anchorPoint = new Point(0, 0);

        public static readonly DependencyProperty ActualItemsProperty = DependencyProperty.Register("ActualItems",
            typeof(ObservableCollection<object>), typeof(PhotoGalleryControl), new PropertyMetadata(null));
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IEnumerable), typeof(PhotoGalleryControl), new PropertyMetadata(null, new PropertyChangedCallback(ItemsSourcePropertyChanged)));
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate",
            typeof(DataTemplate), typeof(PhotoGalleryControl), new PropertyMetadata(null));
        public static readonly DependencyProperty AnchorPointProperty = DependencyProperty.Register("AnchorPoint",
            typeof(Point), typeof(PhotoGalleryControl), new PropertyMetadata(new Point(0, 0)));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string), typeof(PhotoGalleryControl), new PropertyMetadata(string.Empty));

        static void ItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            PhotoGalleryControl gallery = d as PhotoGalleryControl;
            if (gallery != null)
                gallery.Update();
        }

        public ObservableCollection<object> ActualItems {
            get { return (ObservableCollection<object>)GetValue(ActualItemsProperty); }
        }
        public IEnumerable ItemsSource {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public DataTemplate ItemTemplate {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public Point AnchorPoint {
            get { return (Point)GetValue(AnchorPointProperty); }
            set { SetValue(AnchorPointProperty, value); }
        }
        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        ItemsControl elements = null;
        Panel LayoutPanel { get { return LayoutHelper.FindElementByType<Panel>(elements); } }

        public PhotoGalleryControl() {
            DefaultStyleKey = typeof(PhotoGalleryControl);
        }
        void Update() {
            ObservableCollection<object> items = new ObservableCollection<object>();
            if (ItemsSource != null)
                foreach (object item in ItemsSource)
                    items.Add(item);
            SetValue(ActualItemsProperty, items);
        }
        void Invalidate() {
            if (LayoutPanel != null)
                LayoutPanel.InvalidateMeasure();
        }
        protected override void OnApplyTemplate() {
            base.OnApplyTemplate();
            elements = GetTemplateChild("PART_Elements") as ItemsControl;
        }
        protected override void VisibleChanged() {
            base.VisibleChanged();
        }
    }
    public class PhotoGalleryPanel : Panel {
        const double defaultWidth = 300.0;
        const double defaultHeight = 300.0;

        int rowCount = 0;
        int columnCount = 0;

        Point TransformedPoint { get { return AppHelper.RootVisual.TransformToVisual(this).TransformPoint(AnchorPoint); } }

        PhotoGalleryControl Gallery { get { return DataContext as PhotoGalleryControl; } }
        Point AnchorPoint { get { return Gallery != null ? Gallery.AnchorPoint : new Point(0, 0); } }

        void CalculateLayout(Size constraint, int elementCount) {
            if(constraint.Height == 0 || constraint.Width == 0) {
                rowCount = 1;
                columnCount = elementCount;
                return;
            }
            double sizeRatio = constraint.Width / constraint.Height * 0.5;
            rowCount = (int)Math.Max(1, Math.Round(Math.Sqrt(elementCount / sizeRatio)));
            columnCount = (int)Math.Ceiling((double)elementCount / (double)rowCount);
        }
        Rect CalculateElementLayout(Size elementSize, int elementNumber) {
            double x = (elementNumber % columnCount) * elementSize.Width;
            double y = Math.Ceiling((double)(elementNumber / columnCount)) * elementSize.Height;
            return new Rect(x, y, elementSize.Width, elementSize.Height);
        }

        protected override Size MeasureOverride(Size availableSize) {
            double constraintWidth = double.IsInfinity(availableSize.Width) ? defaultWidth : availableSize.Width;
            double constraintHeight = double.IsInfinity(availableSize.Height) ? defaultHeight : availableSize.Height;
            Size constraint = new Size(constraintWidth, constraintHeight);
            CalculateLayout(constraint, Children.Count);
            Size elementSize = new Size(constraint.Width / columnCount, constraint.Height / rowCount);
            double width = 0, height = 0;
            for (int i = 0; i < Children.Count; i++) {
                Children[i].Measure(elementSize);
                width = Math.Max(width, Children[i].DesiredSize.Width);
                height = Math.Max(height, Children[i].DesiredSize.Height);
            }
            return new Size(columnCount * width, rowCount * height);
        }
        protected override Size ArrangeOverride(Size arrangeSize) {
            for (int i = 0; i < Children.Count; i++) {
                Children[i].Opacity = 1.0;
                Children[i].Arrange(CalculateElementLayout(Children[i].DesiredSize, i));
            }
            return arrangeSize;
        }
    }
}