using DevExpress.Core.Extensions;
using DevExpress.Core.Native;
using DevExpress.Mvvm.Native;
using FeatureDemo.View;
using FeatureDemo.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.Web;

namespace FeatureDemo.Common {
    public class OptionGroupHeader : Control {
        #region static
        public static readonly DependencyProperty TextProperty;
        static OptionGroupHeader() {
            DependencyPropertyRegistrator<OptionGroupHeader>.New()
                .Register<FrameworkElement>(nameof(Text), out TextProperty, null);
        }
        #endregion
        #region dep props
        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion
        public OptionGroupHeader() {
            DefaultStyleKey = typeof(OptionGroupHeader);
        }
    }
    [ContentProperty(Name = nameof(Content))]
    public class OptionItem : Grid {
        #region static
        public static readonly DependencyProperty HeaderProperty;
        public static readonly DependencyProperty ContentProperty;
        public static readonly DependencyProperty HeaderStyleProperty;
        public static readonly DependencyProperty HorizontalHeaderMarginProperty;
        public static readonly DependencyProperty VerticalHeaderMarginProperty;
        public static readonly DependencyProperty HeaderOrientationProperty;
        static OptionItem() {
            DependencyPropertyRegistrator<OptionItem>.New()
                .Register<FrameworkElement>(nameof(Content), out ContentProperty, null, (s, oldValue) => s.OnContentChanged(oldValue))
                .Register<string>(nameof(Header), out HeaderProperty, null, (s, oldValue) => s.OnHeaderChanged(oldValue))
                .Register<Style>(nameof(HeaderStyle), out HeaderStyleProperty, null, s => s.OnHeaderStyleChanged())
                .Register<Thickness>(nameof(HorizontalHeaderMargin), out HorizontalHeaderMarginProperty, new Thickness())
                .Register<Thickness>(nameof(VerticalHeaderMargin), out VerticalHeaderMarginProperty, new Thickness())
                .Register<Orientation>(nameof(HeaderOrientation), out HeaderOrientationProperty, Orientation.Horizontal);

        }
        #endregion
        #region dep props
        public FrameworkElement Content {
            get { return (FrameworkElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public string Header {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public Style HeaderStyle {
            get { return (Style)GetValue(HeaderStyleProperty); }
            set { SetValue(HeaderStyleProperty, value); }
        }
        public Thickness HorizontalHeaderMargin {
            get { return (Thickness)GetValue(HorizontalHeaderMarginProperty); }
            set { SetValue(HorizontalHeaderMarginProperty, value); }
        }
        public Thickness VerticalHeaderMargin {
            get { return (Thickness)GetValue(VerticalHeaderMarginProperty); }
            set { SetValue(VerticalHeaderMarginProperty, value); }
        }
        public Orientation HeaderOrientation {
            get { return (Orientation)GetValue(HeaderOrientationProperty); }
            set { SetValue(HeaderOrientationProperty, value); }
        }

        #endregion
        internal TextBlock HeaderTextBlock { get; set; }
        public OptionItem() {
            Loading += OnLoading;
        }

        private void OnLoading(FrameworkElement sender, object args) {
            Style = (Style)Application.Current.Resources[typeof(OptionItem)];
            CreateLayout();    
        }
        void CreateLayout() {
            if(HeaderOrientation == Orientation.Horizontal) {
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = WinUICompatibility.CreateAutoGridLength() });
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = WinUICompatibility.CreateGridLength(1.0, GridUnitType.Star) });
                HeaderTextBlock.Do(x => x.Margin = HorizontalHeaderMargin);
            } else {
                this.RowDefinitions.Add(new RowDefinition() { Height = WinUICompatibility.CreateAutoGridLength() });
                this.RowDefinitions.Add(new RowDefinition() { Height = WinUICompatibility.CreateGridLength(1.0, GridUnitType.Star) });
                HeaderTextBlock.Do(x => x.Margin = VerticalHeaderMargin);
            }
        }
        void OnContentChanged(FrameworkElement oldValue) {
            oldValue.Do(x => Children.Remove(x));
            if(Content == null) return;
            Grid.SetColumn(Content, 1);
            Grid.SetRow(Content, 1);
            Children.Add(Content);
        }
        void OnHeaderChanged(string oldValue) {
            if(HeaderTextBlock == null) {
                HeaderTextBlock = new TextBlock();
                HeaderTextBlock.Style = HeaderStyle;
                Children.Add(HeaderTextBlock);
            }
            HeaderTextBlock.Text = Header;
        }
        void OnHeaderStyleChanged() {
            HeaderTextBlock.Do(x => x.Style = HeaderStyle);
        }
    }
    public class OptionsPanel : Panel {
        #region static
        public static readonly DependencyProperty GroupSpaceProperty;
        public static readonly DependencyProperty ItemSpaceProperty;
        public static readonly DependencyProperty FirstGroupItemSpaceProperty;        
        static OptionsPanel() {
            DependencyPropertyRegistrator<OptionsPanel>.New()
                .Register(nameof(GroupSpace), out GroupSpaceProperty, 0d)
                .Register(nameof(ItemSpace), out ItemSpaceProperty, 0d)
                .Register(nameof(FirstGroupItemSpace), out FirstGroupItemSpaceProperty, 0d);
        }
        #endregion
        #region dep props
        public double GroupSpace {
            get { return (double)GetValue(GroupSpaceProperty); }
            set { SetValue(GroupSpaceProperty, value); }
        }
        public double ItemSpace {
            get { return (double)GetValue(ItemSpaceProperty); }
            set { SetValue(ItemSpaceProperty, value); }
        }
        public double FirstGroupItemSpace {
            get { return (double)GetValue(FirstGroupItemSpaceProperty); }
            set { SetValue(FirstGroupItemSpaceProperty, value); }
        }
        #endregion
        public OptionsPanel() {
            Loading += OnLoading;
        }

        private void OnLoading(FrameworkElement sender, object args) {
            Style = (Style)Application.Current.Resources[typeof(OptionsPanel)];
        }

        private Size LayoutItems(Func<UIElement, double, Size> layoutFunc) {
            Size retValue = new Size(0, 0);
            bool prevItemIsOptionGroup = false;
            bool isFirstChild = true;
            foreach(UIElement child in Children) {
                if(!isFirstChild) {
                    retValue.Height += child is OptionGroupHeader ? GroupSpace : (prevItemIsOptionGroup ? FirstGroupItemSpace : ItemSpace);
                } else {
                    isFirstChild = false;
                }
                Size childSize = layoutFunc(child, retValue.Height);
                retValue.Width = Math.Max(retValue.Width, child.DesiredSize.Width);
                retValue.Height += child.DesiredSize.Height;
                prevItemIsOptionGroup = child is OptionGroupHeader;
            }
            return retValue;
        }
        protected override Size MeasureOverride(Size availableSize) {
            if(Children.Count == 0)
                return base.MeasureOverride(availableSize);
            double itemHeaderWidth = 0;
            Size retVal = LayoutItems((child, offset) => {
                child.Measure(new Size(availableSize.Width, double.PositiveInfinity));
                OptionItem optionItem = child as OptionItem;

                if(optionItem != null && optionItem.HeaderOrientation == Orientation.Horizontal && optionItem.HeaderTextBlock != null)
                    itemHeaderWidth = Math.Max(itemHeaderWidth, optionItem.HeaderTextBlock.DesiredSize.Width);
                return child.DesiredSize;
            });
            Children.OfType<OptionItem>().Where(x => x.HeaderOrientation == Orientation.Horizontal && x.HeaderTextBlock != null).ForEach(x => x.ColumnDefinitions[0].Width = WinUICompatibility.CreateGridLength(itemHeaderWidth));
            return retVal;
        }
        protected override Size ArrangeOverride(Size finalSize) {
            if(Children.Count == 0)
                return base.ArrangeOverride(finalSize);
            LayoutItems((child, offset) => {
                child.Arrange(new Rect(0, offset, finalSize.Width, child.DesiredSize.Height));
                return child.DesiredSize;
            });            
            return finalSize;
        }
    }

    public class DemoWebView : Grid {


        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(DemoWebView), new PropertyMetadata(null, OnTextPropertyChanged));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((DemoWebView)d).OnTextChanged();
        }

        private void OnTextChanged() {
            webView.NavigateToString(Text);
        }

        WebView2 webView;
        public DemoWebView() {
            webView = new WebView2();
            Children.Add(webView);
        }
        class StreamUriResolver : IUriToStreamResolver {
            string Text { get; set; }
            public StreamUriResolver(string text) {
                Text = text;
            }
            public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri) {
                return GetContent().AsAsyncOperation();
            }

            private async Task<IInputStream> GetContent() {
                //
                try {
                    InMemoryRandomAccessStream res = new InMemoryRandomAccessStream();
                    using(DataWriter dataWriter = new DataWriter(res)) {
                        dataWriter.WriteString(Text);
                        await dataWriter.StoreAsync();
                        await dataWriter.FlushAsync();
                        dataWriter.DetachStream();
                    }
                    res.Seek(0);
                    return res;
                }
                catch(Exception) { throw new Exception("Invalid path"); }
            }
        }
    }
}
