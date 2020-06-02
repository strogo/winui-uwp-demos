using FeatureDemo.Common;
using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace RangeControlDemo {
    public sealed partial class RangeControlModule : DemoModuleView {
        public RangeControlModule() {
            this.InitializeComponent();
        }
    }

    public class FromDateConverter: IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if(value != null && value is DateTime) {
                DateTime date = (DateTime)value;
                string result = date.ToString("MM/dd/yyyy");
                return result;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
