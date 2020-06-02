using DevExpress.Mvvm;
using FeatureDemo.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace FeatureDemo {
    
    public class SelectionModeToBooleanConverter : IValueConverter {
        public bool Invert { get; set; } = false;
        public object Convert(object value, Type targetType, object parameter, string language) {
            ListViewSelectionMode selectionMode = (ListViewSelectionMode)value;
            bool result = selectionMode == ListViewSelectionMode.Multiple || selectionMode == ListViewSelectionMode.Extended;
            return Invert ? !result : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    public class SelectionModeVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            ListViewSelectionMode selectionMode = (ListViewSelectionMode)value;
            bool invert = bool.Parse(parameter.ToString());
            switch (selectionMode) {
                case ListViewSelectionMode.None:
                case ListViewSelectionMode.Single:
                    return invert ? Visibility.Collapsed : Visibility.Visible;
                    
                case ListViewSelectionMode.Multiple:
                case ListViewSelectionMode.Extended:
                    return !invert ? Visibility.Collapsed : Visibility.Visible;
                default:
                    return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
    public class SelectionModeComboBox : EnumComboBox<ListViewSelectionMode> { }
    
    public sealed partial class DXItemsControlModule : DemoModuleView {
        DXItemsControlViewModel ViewModel { get { return DataContext as DXItemsControlViewModel; } }
        public DXItemsControlModule() {
            DataContext = new DXItemsControlViewModel();
            this.InitializeComponent();
        }

        void SimpleChecked(object sender, RoutedEventArgs e) {
            ViewModel.SetSimpleSource.Execute(null);
        }

        void GroupedChecked(object sender, RoutedEventArgs e) {
            ViewModel.SetGroupedSource.Execute(null);
        }

    }
}
