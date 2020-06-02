using DevExpress.Data.Mask;
using DevExpress.Core.Extensions;
using DevExpress.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Windows.Input;
using FeatureDemo.Common;

namespace ControlsDemo {
    public sealed partial class FlyoutFlowInfoModule : DemoModuleView {
        public FlyoutFlowInfoModule() {
            this.InitializeComponent();
        }

        void OpenInnerFlyout(object sender, RoutedEventArgs e) {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null)
                return;
            flyoutControl.PlacementTarget = this;
            flyoutControl.Style = GetFlyoutStyle(element.Name);
            flyoutControl.IsOpen = true;
        }
        void OpenFlyout(object sender, RoutedEventArgs e) {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null)
                return;
            flyoutControl.PlacementTarget = element;
            flyoutControl.Style = GetFlyoutStyle(element.Name);
            flyoutControl.IsOpen = true;
        }
        Style GetFlyoutStyle(string key) {
            return Resources[key] as Style;
        }
    }
}
