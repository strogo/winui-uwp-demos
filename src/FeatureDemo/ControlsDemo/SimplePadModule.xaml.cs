using FeatureDemo.Common;
using System;
using Windows.UI.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace ControlsDemo {
    public sealed partial class SimplePadModule : DemoModuleView {
        public SimplePadModule() {
            this.InitializeComponent();
            Loading += OnLoading;
            Loaded += OnLoad;
            Unloaded += OnUnload;
        }
        void OnLoading(FrameworkElement sender, object args) {
            DataContext = new SimplePadViewModel();
        }
        void OnLoad(object sender, RoutedEventArgs e) {
            tb.ContextMenuOpening += RichEditBox_OnContextMenuOpening;
            bt.Click += ButtonBase_OnClick;
        }
        void OnUnload(object sender, RoutedEventArgs e) {
            DataContext = null;
            tb.ContextMenuOpening -= RichEditBox_OnContextMenuOpening;
            bt.Click -= ButtonBase_OnClick;
        }

        void RichEditBox_OnContextMenuOpening(object sender, ContextMenuEventArgs e) {
            e.Handled = true;
            flyout.IsOpen = true;
        }
        void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            tb.Focus(FocusState.Programmatic);
            flyout.IsOpen = true;
        }
    }
    public class TextAlignmentToBooleanConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            TextAlignment p;
            if (!Enum.TryParse(parameter as string, true, out p))
                return false;
            TextAlignment v = (TextAlignment)value;
            return p == v;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
