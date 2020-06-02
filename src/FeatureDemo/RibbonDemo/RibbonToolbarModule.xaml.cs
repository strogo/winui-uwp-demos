using FeatureDemo.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace RibbonDemo {
    public sealed partial class RibbonToolbarModule : DemoModuleView {
        public RibbonViewModel ViewModel { get { return DataContext as RibbonViewModel; } }
        public RibbonToolbarModule() {
            this.InitializeComponent();
            Unloaded += OnUnloaded;
        }

        void OnUnloaded(object sender, RoutedEventArgs e) {
            var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
            foreach(var popup in popups) {
                popup.IsOpen = false;
            }
            InputPane.GetForCurrentView().TryHide();
        }
    }
}
