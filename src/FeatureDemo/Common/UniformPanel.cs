using DevExpress.Core.Extensions;
using System;
using System.Linq;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Collections.Generic;
using DevExpress.Core.Native;
using DevExpress.UI.Xaml.Internal;
using FeatureDemo.View;
using DevExpress.Mvvm.Native;
using System.Runtime.InteropServices.WindowsRuntime;

namespace FeatureDemo.Common {
    public class UniformPanel : Panel {
        #region static
        public static readonly DependencyProperty SpaceProperty;
        public static readonly DependencyProperty MaxItemWidthProperty;

        static UniformPanel() {
            SpaceProperty = DependencyProperty.Register(nameof(Space), typeof(double), typeof(UniformPanel), new PropertyMetadata(0d));
            MaxItemWidthProperty = DependencyProperty.Register(nameof(MaxItemWidth), typeof(double), typeof(UniformPanel), new PropertyMetadata(0d));
        }
        protected static double CalcRealSize(double width, int colCount, double space) {
            if(colCount == 0) return 0;
            return Math.Round((width - (colCount - 1) * space) / colCount);
        }
        #endregion
        #region dep props
        public double Space {
            get { return (double)GetValue(SpaceProperty); }
            set { SetValue(SpaceProperty, value); }
        }
        public double MaxItemWidth {
            get { return (double)GetValue(MaxItemWidthProperty); }
            set { SetValue(MaxItemWidthProperty, value); }
        }
        #endregion

        protected double itemWidth = 0d;
        protected double itemHeight = 0d;
        protected int rowCount = 0;
        protected override Size MeasureOverride(Size availableSize) {
            if(Children.Count == 0)
                return base.MeasureOverride(availableSize);
            int visibleItemCount = Children.Count(x => x.Visibility == Visibility.Visible);

            double minCount = (availableSize.Width + Space) / (MaxItemWidth + Space);
            int colCount = (int)minCount;
            if(minCount - Math.Truncate(minCount) != 0d) {
                colCount++;
            }
            colCount = Math.Min(visibleItemCount, colCount);
            double width = Math.Min(MaxItemWidth, Math.Truncate((availableSize.Width - (colCount - 1) * Space) / colCount));
            double height = 0;

            foreach(UIElement element in Children) {
                element.Measure(new Size(width, double.PositiveInfinity));
                if(element.Visibility == Visibility.Collapsed)
                    continue;
                height = Math.Max(height, element.DesiredSize.Height);
            }
            if(visibleItemCount == 0)
                return new Size(0, 0);
            rowCount = visibleItemCount / colCount + ((visibleItemCount % colCount) == 0 ? 0 : 1);
            itemWidth = width;
            itemHeight = height;
            return new Size(colCount * width + Space * (colCount - 1), rowCount * height + Space * (rowCount - 1));
        }

        protected override Size ArrangeOverride(Size finalSize) {
            Rect arrangeRect = new Rect(0, 0, itemWidth, itemHeight);
            foreach(UIElement element in Children.Where(x => x.Visibility == Visibility.Visible)) {
                element.Arrange(arrangeRect);
                arrangeRect.X += itemWidth + Space;
                if(arrangeRect.X >= finalSize.Width) {
                    arrangeRect.X = 0;
                    arrangeRect.Y += itemHeight + Space;
                }
            }
            return finalSize;
        }
    }
    public class UniformItemsPanel : UniformPanel, IScrollSnapPointsInfo {
        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty ItemContainerStyleProperty;

        static UniformItemsPanel() {
            ItemContainerStyleProperty = DependencyPropertyManager.Register<UniformItemsPanel, Style>(nameof(ItemContainerStyle), null);
            ItemsSourceProperty = DependencyPropertyManager.Register<UniformItemsPanel, object>(nameof(ItemsSource),
                null, (d, e) => d.OnItemsSourceChanged());
        }

        public object ItemsSource {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public Style ItemContainerStyle {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        ItemsSourceController<FrameworkElement, UniformItemsPanel> controller;

        public UniformItemsPanel() {
            controller = new ItemsSourceController<FrameworkElement, UniformItemsPanel>(this, (o) => {
                var container = CreateContainer(o);
                container.DataContext = o;
                if(ItemContainerStyle != null) container.Style = ItemContainerStyle;
                Children.Add(container);
                return container;
            });
            Loading += OnLoading;
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
            TokenTable.InvocationList?.Invoke(this, null);
        }

        private void OnLoading(FrameworkElement sender, object args) {
            scroll = this.VisualParents().OfType<ScrollViewer>().FirstOrDefault();
            scroll.Do(x => x.SizeChanged += OnScrollViewerSizeChanged);
        }

        private void OnScrollViewerSizeChanged(object sender, SizeChangedEventArgs e) {
            TokenTable.InvocationList?.Invoke(this, null);
        }

        public virtual FrameworkElement CreateContainer(object item) {
            var container = new DemoModuleTile();
            container.Click += Container_Click;
            return container;
        }
        ScrollViewer scroll = null;
        int firstVisibleRow = 0;
        protected override Size MeasureOverride(Size availableSize) {
            var result = base.MeasureOverride(availableSize);
            if(scroll != null) 
                firstVisibleRow = Math.Max(0, (int)Math.Round((scroll.VerticalOffset - this.Margin.Top) / (itemHeight + Space)));
            return result;
        }

        private void Container_Click(object sender, RoutedEventArgs e) {
            ItemClick?.Invoke(this, (sender as FrameworkElement).DataContext);
        }

        public event EventHandler<object> ItemClick;

        protected virtual void OnItemsSourceChanged() {
            controller.ItemsSource = ItemsSource;
        }
        #region IScrollSnapPointsInfo
        EventRegistrationTokenTable<EventHandler<object>> tokenTable = null;
        EventRegistrationTokenTable<EventHandler<object>> TokenTable {
            get { return EventRegistrationTokenTable<EventHandler<object>>.GetOrCreateEventRegistrationTokenTable(ref tokenTable); }
        }
        public bool AreHorizontalSnapPointsRegular { get { return true; } }

        public bool AreVerticalSnapPointsRegular { get { return false; } }
        List<float> snapPoints = new List<float>();


#pragma warning disable 0067
        public event EventHandler<object> HorizontalSnapPointsChanged;
#pragma warning restore 0067
        public event EventHandler<object> VerticalSnapPointsChanged {
            add { return TokenTable.AddEventHandler(value); }
            remove { TokenTable.RemoveEventHandler(value); }
        }
        public IReadOnlyList<float> GetIrregularSnapPoints(Orientation orientation, SnapPointsAlignment alignment) {
            if(scroll == null) return new List<float>();
            int lastCount = snapPoints.Count;
            snapPoints.Clear();
            snapPoints.Add(0);
            float snapPoint = 0;
            for(int i = 1; i < rowCount; i++) {
                snapPoint = (float)(i * (itemHeight + Space) + Margin.Top - 0.5 * Space);
                if(snapPoint >= scroll.ScrollableHeight) {
                    snapPoint = (float)scroll.ScrollableHeight;
                    snapPoints.Add(snapPoint);
                    break;
                } else {
                    snapPoints.Add(snapPoint);
                }
            }
            if(snapPoint != scroll.ScrollableHeight)
                snapPoints.Add((float)scroll.ScrollableHeight);
            return snapPoints;
        }

        public float GetRegularSnapPoints(Orientation orientation, SnapPointsAlignment alignment, out float offset) {
            throw new NotImplementedException();
        }
        #endregion
    }
    public class ContentControlUniformPanel : UniformItemsPanel {
        public override FrameworkElement CreateContainer(object item) {
            return new ContentControl();
        }
    }
}