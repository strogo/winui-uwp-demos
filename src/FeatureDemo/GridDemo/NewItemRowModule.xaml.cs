
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DevExpress.UI.Xaml.Grid;
using FeatureDemo.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using DevExpress.Core;
using System.ComponentModel;

namespace GridDemo {
    public class BoolToNewItemRowPosition : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            bool? isChecked = (bool?)value;
            if(isChecked.HasValue && isChecked.Value)
                return NewItemRowPosition.Top;
            else
                return NewItemRowPosition.Bottom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    public class ObservableCollectionConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return new ObservableCollection<Item>((Items)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    public sealed partial class NewItemRowModule : GridDemoUserControl {
        public NewItemRowModule() {
            this.InitializeComponent();
        }

        protected internal override GridControl Grid { get { return grid; } }

        private void grid_AddingNewRow(object sender, AddingNewEventArgs e) {
            e.NewObject = new Item() {
                Priority = FeatureDemo.Data.Priority.Low,
                Status = FeatureDemo.Data.Status.New,
                CreatedDate = DateTime.Now,
                FixedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
