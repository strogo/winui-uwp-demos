using System;
using Windows.UI.Xaml.Data;

namespace NavigationDemo {
    public class StringFormatConverter : IValueConverter {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, string language) {
            try {
                string format = (parameter == null) ? null : parameter.ToString();
                if(String.IsNullOrEmpty(format)) {
                    format = "{0}";
                }
                return String.Format(format, value);
            }
            catch(Exception) {
                return Windows.UI.Xaml.DependencyProperty.UnsetValue;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
        #endregion
    }
}