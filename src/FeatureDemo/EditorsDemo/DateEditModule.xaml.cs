using FeatureDemo.Common;
using System;
using Microsoft.UI.Xaml.Controls;

namespace EditorsDemo {
    public sealed partial class DateEditModule : DemoModuleView {
        public DateEditModule() {
            this.InitializeComponent();
            datePicker.DateTime = DateTime.Now;
            dateNavigator.DateTime = DateTime.Now;
            radialTimeEdit.DateTime = DateTime.Now;
        }
    }
}
