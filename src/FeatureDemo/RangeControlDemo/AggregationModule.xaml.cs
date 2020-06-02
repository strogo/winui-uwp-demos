using FeatureDemo.Common;
using System;
using Windows.UI.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
namespace RangeControlDemo {
    public sealed partial class AggregationModule : DemoModuleView {
        public AggregationModule() {
            this.InitializeComponent();
            Unloaded += OnUnload;
            DataContext = new DateTimeViewModel() {
                Start = 10000,
                Count = 50000,
                Step = TimeSpan.FromHours(3),
                VisibleStart = new DateTime(2015, 1, 1),
                VisibleEnd = new DateTime(2018, 1, 1),
                SelectionStart = new DateTime(2016, 1, 1),
                SelectionEnd = new DateTime(2017, 1, 1)
            };
        }
        void OnUnload(object sender, RoutedEventArgs e) {
            rangeControl.Content = null;
            DataContext = null;
        }
    }
}
