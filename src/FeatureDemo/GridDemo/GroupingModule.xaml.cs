using DevExpress.Data;
using DevExpress.Data.Native;
using DevExpress.UI.Xaml.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace GridDemo {
    public sealed partial class GroupingModule : GridDemoUserControl {
        public GroupingModule() {
            this.InitializeComponent();
            grid.Loaded += gridLoaded;            
        }
        protected internal override GridControl Grid { get { return grid; } }
        void gridLoaded(object sender, RoutedEventArgs e) {
            grid.Loaded -= gridLoaded;
            grid.ExpandGroupRow(-1);
            grid.ExpandGroupRow(-2);
        }
    }
}
