using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace FeatureDemo.Common {
    public class BarCodeEnumToListConveter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            Type type = Type.GetType((string)parameter);
            return Enum.GetValues(type).Cast<object>().Where(x => x.ToString() != "Binary").ToList();
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}