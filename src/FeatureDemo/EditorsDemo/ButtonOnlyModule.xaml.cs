using DevExpress.Mvvm;
using FeatureDemo.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace EditorsDemo {
    public class ButtonsOnlyModuleViewModel : ViewModelBase {

        public DateTime DropDownDate {
            get { return GetProperty(() => DropDownDate); }
            set { SetProperty(() => DropDownDate, value); }
        }

        public DateTime SplitDate {
            get { return GetProperty(() => SplitDate); }
            set { SetProperty(() => SplitDate, value); }
        }
    }
    public sealed partial class ButtonOnlyModule : UserControl {
        public ButtonOnlyModule() {
            DataContext = new ButtonsOnlyModuleViewModel() { DropDownDate = DateTime.Now, SplitDate = DateTime.Now };
            this.InitializeComponent();
        }
    }
}
