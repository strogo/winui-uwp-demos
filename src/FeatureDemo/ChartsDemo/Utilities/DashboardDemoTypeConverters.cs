using DevExpress.UI.Xaml.Map;
using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System.Linq;
namespace ChartsDemo {
    public class SelectedMapItemsConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if(value == null)
                return null;
            CountryStatisticInfo country = (CountryStatisticInfo)value;
            return country.Shapes;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            DashboardViewModel viewModel = (DashboardViewModel)parameter;
            IList<object> selecedShapes = (IList<object>)value;
            if(selecedShapes == null || selecedShapes.Count != 1)
                return null;
            MapItem selectedShape = (MapItem)selecedShapes[0];
            string selectedCountryName = (string)selectedShape.Attributes["NAME"].Value;
            CountryStatisticInfo selected = viewModel.Countries.FirstOrDefault(x => x.Name == selectedCountryName);
            return selected;
        }
    }

    public class PopulationDynamicConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if(value == null)
                return null;
            CountryStatisticInfo country = (CountryStatisticInfo)value;
            return country.PopulationDynamic;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return null;
        }
    }

    public class PopulationDynamicColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if(value == null)
                return null;
            DashboardViewModel viewModel = (DashboardViewModel)parameter;
            CountryStatisticInfo country = (CountryStatisticInfo)value;
            if (country.Brush == null){
                int countryIndex = viewModel.Countries.IndexOf(country);
                country.Brush = new SolidColorBrush(viewModel.DashboardPalette[countryIndex]);
            }
            return country.Brush;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return null;
        }
    }

    public class PopulationDynamicTitleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value == null)
                return null;
            CountryStatisticInfo country = (CountryStatisticInfo)value;
            return string.Format("Population Dynamics ({0})", country.Name);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return null;
        }
    }
}
