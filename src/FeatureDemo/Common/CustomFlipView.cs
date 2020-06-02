using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Core;
using DevExpress.Core.Native;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FeatureDemo.ViewModel;
using System.Reflection;
using Windows.UI.Xaml.Input;

namespace FeatureDemo.Common {
    public class CustomFlipView : FlipView {
        public CustomFlipView() {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }
        void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            ClearValue(DataContextProperty);
        }
        void OnUnloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            DataContext = null;
        }
        protected override void OnPointerWheelChanged(Windows.UI.Xaml.Input.PointerRoutedEventArgs e) { }
        protected override DependencyObject GetContainerForItemOverride() {
            return new CustomFlipViewItem();
        }
    }
    public class CustomFlipViewItem : FlipViewItem {
        public CustomFlipViewItem() {
            DefaultStyleKey = typeof(CustomFlipViewItem);
        }
        protected override void OnKeyDown(KeyRoutedEventArgs e) {
            e.Handled = e.Key == Windows.System.VirtualKey.Left || e.Key == Windows.System.VirtualKey.Right
                || e.Key == Windows.System.VirtualKey.PageDown || e.Key == Windows.System.VirtualKey.PageUp
                || e.Key == Windows.System.VirtualKey.End || e.Key == Windows.System.VirtualKey.Home;
        }
    }
}
