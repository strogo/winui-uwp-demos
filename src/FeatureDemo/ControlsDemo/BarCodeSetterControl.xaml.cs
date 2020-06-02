using DevExpress.UI.Xaml.Controls;
using DevExpress.XtraPrinting.BarCode;
using FeatureDemo.Common;
using System;
using System.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FeatureDemo.ControlsDemo {
    public sealed partial class BarCodeSetterControl : UserControl {
        public BarCodeSetterControl() {
            this.InitializeComponent();
        }
        public string Caption {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(BarCodeSetterControl), new PropertyMetadata(""));
        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(BarCodeSetterControl), new PropertyMetadata(""));
        public SymbologyBase Symbology {
            get { return (SymbologyBase)GetValue(SymbologyProperty); }
            set { SetValue(SymbologyProperty, value); }
        }
        public static readonly DependencyProperty SymbologyProperty =
            DependencyProperty.Register("Symbology", typeof(SymbologyBase), typeof(BarCodeSetterControl), new PropertyMetadata(null));
        public string PropertyName {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(BarCodeSetterControl), new PropertyMetadata(null));
        public IEnumerable ItemsSource {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(BarCodeSetterControl), new PropertyMetadata(null));
        public object EditValue {
            get { return (object)GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }
        public static readonly DependencyProperty EditValueProperty =
            DependencyProperty.Register("EditValue", typeof(object), typeof(BarCodeSetterControl), new PropertyMetadata(null));
    }
}
