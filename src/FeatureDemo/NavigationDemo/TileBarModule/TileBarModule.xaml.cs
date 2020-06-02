using System;
using System.Collections.Generic;
using DevExpress.Core;
using DevExpress.UI.Xaml.Grid;
using DevExpress.UI.Xaml.Layout;
using FeatureDemo.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace NavigationDemo
{
    public sealed partial class TileBarModule : DemoModuleView {
        public TileBarModule() {
            this.InitializeComponent();
        }
    }

    public class DockToThicknessConverter : IValueConverter {
        public Thickness HorizontalThickness { get; set; }
        public Thickness VerticalThickness { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language) {
            var dock = (Dock)value;
            if(dock == Dock.Bottom || dock == Dock.Top)
                return HorizontalThickness;
            else
                return VerticalThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
