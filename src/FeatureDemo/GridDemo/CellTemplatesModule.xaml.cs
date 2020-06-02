using DevExpress.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace GridDemo {
    public sealed partial class CellTemplatesModule : GridDemoUserControl {
        public CellTemplatesModule() {
            this.InitializeComponent();
            Unloaded += OnUnloaded;
        }
        void OnUnloaded(object sender, RoutedEventArgs e) {
            gridControl.DataContext = null;
            gridRoot.DataContext = null;
        }
        protected internal override GridControl Grid {
            get { return gridControl; }
        }        
    }
}