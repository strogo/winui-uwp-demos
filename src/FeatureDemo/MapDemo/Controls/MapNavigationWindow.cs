using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using DevExpress.UI.Xaml.Map;

namespace MapDemo {
    public class MapNavigationWindow : VisibleControl {
        public static readonly DependencyProperty MapCenterPointProperty = DependencyProperty.Register(nameof(MapCenterPoint),
            typeof(GeoPoint), typeof(MapNavigationWindow), new PropertyMetadata(null, PointPropertyChanged));
        public static readonly DependencyProperty LeftTopProperty = DependencyProperty.Register("LeftTop",
            typeof(GeoPoint), typeof(MapNavigationWindow), new PropertyMetadata(null));
        public static readonly DependencyProperty RightBottomProperty = DependencyProperty.Register("RightBottom",
            typeof(GeoPoint), typeof(MapNavigationWindow), new PropertyMetadata(null, PointPropertyChanged));

        bool isDrugInProcess = false;

        public GeoPoint MapCenterPoint {
            get { return (GeoPoint)GetValue(MapCenterPointProperty); }
            set { SetValue(MapCenterPointProperty, value); }
        }
        public GeoPoint LeftTop {
            get { return (GeoPoint)GetValue(LeftTopProperty); }
            set { SetValue(LeftTopProperty, value); }
        }
        public GeoPoint RightBottom {
            get { return (GeoPoint)GetValue(RightBottomProperty); }
            set { SetValue(RightBottomProperty, value); }
        }
        MapControl map;
        FrameworkElement locationRectangle;

        public MapNavigationWindow() {
            DefaultStyleKey = typeof(MapNavigationWindow);
        }

        static void PointPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            MapNavigationWindow navWindow = d as MapNavigationWindow;
            if (navWindow != null)
                navWindow.Invalidate();
        }
        void Invalidate() {
            if (map != null && locationRectangle != null) {
                Point leftTop = map.Layers[0].GeoToScreenPoint(LeftTop);
                Point rightBottom = map.Layers[0].GeoToScreenPoint(RightBottom);
                locationRectangle.Width = rightBottom.X - leftTop.X;
                locationRectangle.Height = rightBottom.Y - leftTop.Y;
                Canvas.SetLeft(locationRectangle, leftTop.X);
                Canvas.SetTop(locationRectangle, leftTop.Y);
            }
        }
        protected override void OnApplyTemplate() {
            base.OnApplyTemplate();
            map = GetTemplateChild("PART_Map") as MapControl;
            locationRectangle = GetTemplateChild("PART_LocationRect") as FrameworkElement;
        }
        protected override void OnPointerPressed(PointerRoutedEventArgs e) {
            base.OnPointerPressed(e);
            if (map != null) {
                isDrugInProcess = true;
                CapturePointer(e.Pointer);
                Point position = e.GetCurrentPoint(map).Position;
                MapCenterPoint = map.Layers[0].ScreenToGeoPoint(position);
            }
        }
        protected override void OnPointerReleased(PointerRoutedEventArgs e) {
            base.OnPointerReleased(e);
            ReleasePointerCapture(e.Pointer);
            isDrugInProcess = false;
        }
        protected override void OnPointerMoved(PointerRoutedEventArgs e) {
            base.OnPointerMoved(e);
            if (map != null && isDrugInProcess) {
                Point position = e.GetCurrentPoint(map).Position;
                MapCenterPoint = map.Layers[0].ScreenToGeoPoint(position);
            }
        }
        protected override Size MeasureOverride(Size availableSize) {
            double scaleX = (availableSize.Width - 8.0) / 512.0;
            double scaleY = (availableSize.Height - 8.0) / 512.0;
            ScaleTransform transform = new ScaleTransform() { ScaleX = scaleX, ScaleY = scaleY };
            if (map != null)
                map.RenderTransform = transform;
            Canvas canvas = GetTemplateChild("PART_Canvas") as Canvas;
            if (canvas != null)
                canvas.RenderTransform = transform;
            return base.MeasureOverride(availableSize);
        }
        protected override Size ArrangeOverride(Size finalSize) {
            Invalidate();
            if (locationRectangle != null)
                locationRectangle.Arrange(new Rect(new Point(0.0, 0.0), new Size(locationRectangle.Width, locationRectangle.Height)));
            return base.ArrangeOverride(finalSize);
        }
    }
}