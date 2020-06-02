using FeatureDemo.Common;
using System;
using Windows.Foundation.Metadata;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FeatureDemo.ControlsDemo {
    public sealed partial class BarCodeSample : DemoModuleView {
        public BarCodeSample() {
            this.InitializeComponent();
            DataContext = ViewModel = new BarCodeSampleViewModel();
        }
        public BarCodeSampleViewModel ViewModel { get; set; }
    }
}
