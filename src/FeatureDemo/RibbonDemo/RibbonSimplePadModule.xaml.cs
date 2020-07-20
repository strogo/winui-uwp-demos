using DevExpress.Core;
using DevExpress.Themes.ColorKeys;
using DevExpress.Themes.Generic;
using DevExpress.UI.Xaml.Ribbon;
using FeatureDemo.Common;
using System;
using System.Collections.Generic;
using Microsoft.UI;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace RibbonDemo {
    public sealed partial class RibbonSimplePadModule : DemoModuleView {
        public RibbonViewModel ViewModel { get { return DataContext as RibbonViewModel; } }

        public RibbonSimplePadModule() {
            this.InitializeComponent();
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e) {
            var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
            foreach(var popup in popups) {
#if DEBUGTEST
                if(popup.Child is DevExpress.TestFramework.PopupWindow)
                    return;
#endif
                popup.IsOpen = false;
            }
            InputPane.GetForCurrentView().TryHide();
        }
    }
}
