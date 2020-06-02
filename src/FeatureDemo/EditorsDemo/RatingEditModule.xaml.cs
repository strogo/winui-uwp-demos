using System;
using Microsoft.UI.Xaml.Data;
using FeatureDemo.Common;
using DevExpress.UI.Xaml.Editors;

namespace EditorsDemo {
    public partial class RatingEditModule : DemoModuleView {
        public RatingEditModule() {
            InitializeComponent();
        }
    }
    public class PrecisionToEditMaskConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var precision = (RatingPrecision)value;
            if((RatingPrecision)value == RatingPrecision.Full)
                return "d";

            return "n1";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
