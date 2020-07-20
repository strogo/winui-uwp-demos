using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DevExpress.UI.Xaml.Controls.Internal.Pdf;
using DevExpress.UI.Xaml.Ribbon;
using PdfViewerDemo;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace RibbonDemo {
    public sealed partial class RibbonReportExplorerModule : UserControl {
        public RibbonReportExplorerModule() {
            this.InitializeComponent();
        }
        
        private void PdfViewer_Loaded(object sender, RoutedEventArgs e) {
            //DXVisualizer.DXVisualTreeVisualizer.Show(this);
        }
    }
}
