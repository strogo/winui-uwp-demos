using DevExpress.UI.Xaml.Controls;
using FeatureDemo.Common;
using System;
using System.Collections.Generic;
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

namespace PdfViewerDemo {
    public sealed partial class ReportExplorerModule : DemoModuleView {
        public ReportExplorerModule() {
            this.InitializeComponent();
        }
    }

    public class ReportParametersSelector : DataTemplateSelector {
        public DataTemplate MultiColumnTemplate { get; set; }
        public DataTemplate MasterDetailTemplate { get; set; }
        public DataTemplate EmptyTemplate { get; set; }
        public DataTemplate TableTemplate { get; set; }
        public DataTemplate SubreportTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            if(item is MulticolumnParametersViewModel)
                return MultiColumnTemplate;
            if(item is EmptyParametersViewModel)
                return EmptyTemplate;
            if(item is TableReportParametersViewModel)
                return TableTemplate;
            if(item is SubreportParametersViewModel)
                return SubreportTemplate;
            return null;
        }
    }
}
