using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Input;
using DevExpress.Core.Native;
using DevExpress.Core.ReportsClient;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using DevExpress.Mvvm;
using DevExpress.UI.Xaml.Controls;
using DevExpress.UI.Xaml.Ribbon;
using Windows.UI.Popups;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Designer = DevExpress.Core.Extensions.UIElementExtensions;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace RibbonDemo {
    public class RibbonTextSearchBox : Control, IRibbonItem {
        public static readonly DependencyProperty OwnerProperty;
        public static readonly DependencyProperty ShrinkPriorityProperty;
        public static readonly DependencyProperty CollapsePriorityProperty;
        public static readonly DependencyProperty CollapsedGroupNameProperty;
        public static readonly DependencyProperty ShowInFlyoutOnlyProperty;

        static RibbonTextSearchBox() {
            ShowInFlyoutOnlyProperty = DependencyPropertyManager.Register<RibbonTextSearchBox, bool>("ShowInFlyoutOnly", false);
            OwnerProperty = DependencyPropertyManager.Register<RibbonTextSearchBox, PdfViewerControl>("Owner", null);
            CollapsePriorityProperty = DependencyPropertyManager.Register<RibbonTextSearchBox, CollapsePriority>("CollapsePriority", CollapsePriority.Normal);
            ShrinkPriorityProperty = DependencyPropertyManager.Register<RibbonTextSearchBox, CollapsePriority>("ShrinkPriority", CollapsePriority.Normal);
            CollapsedGroupNameProperty = DependencyPropertyManager.Register<RibbonTextSearchBox, string>("CollapsedGroupName", null);
        }

		public bool ShowInFlyoutOnly {
            get { return (bool)GetValue(ShowInFlyoutOnlyProperty); }
            set { SetValue(ShowInFlyoutOnlyProperty, value); }
        }
		public string CollapsedGroupName {
            get { return (string)GetValue(CollapsedGroupNameProperty); }
            set { SetValue(CollapsedGroupNameProperty, value); }
        }
		public PdfViewerControl Owner {
            get { return (PdfViewerControl)GetValue(OwnerProperty); }
            set { SetValue(OwnerProperty, value); }
        }
		public CollapsePriority CollapsePriority {
            get { return (CollapsePriority)GetValue(CollapsePriorityProperty); }
            set { SetValue(CollapsePriorityProperty, value); }
        }
        public CollapsePriority ShrinkPriority {
            get { return (CollapsePriority)GetValue(ShrinkPriorityProperty); }
            set { SetValue(ShrinkPriorityProperty, value); }
        }
        public RibbonTextSearchBox() {
            DefaultStyleKey = typeof(RibbonTextSearchBox);
        }
        public bool ClosePopupOnClick { get { return false; } }
        public bool HasContent { get { return false; } }
        public RibbonItemStyle ItemStyle { get { return RibbonItemStyle.Default; } }
        public FrameworkElement Visual { get { return this; } }

    }
    public class RibbonReportParametersSelector : DataTemplateSelector {
        public DataTemplate MultiColumnTemplate { get; set; }
        public DataTemplate MasterDetailTemplate { get; set; }
        public DataTemplate EmptyTemplate { get; set; }
        public DataTemplate TableTemplate { get; set; }
        public DataTemplate SubreportTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item) {
            if(item is ReportItemViewModel) {
                if((item as ReportItemViewModel).Parameters is MulticolumnParametersViewModel)
                    return MultiColumnTemplate;
                if((item as ReportItemViewModel).Parameters is EmptyParametersViewModel)
                    return EmptyTemplate;
                if((item as ReportItemViewModel).Parameters is TableReportParametersViewModel)
                    return TableTemplate;
                if((item as ReportItemViewModel).Parameters is SubreportParametersViewModel)
                    return SubreportTemplate;
            }
            return null;
        }
    }
    public class RibbonReportExplorerViewModel : BindableBase {
        object document;
        public object Document {
            get { return document; }
            set { SetProperty(ref document, value); }
        }

        public ICommand LoadDocumentCommand { get { return loadCommand; } }

        bool canStartLoading = true;
        DelegateCommand<object> loadCommand;

        public bool CanStartLoading {
            get { return canStartLoading; }
            set { SetProperty(ref canStartLoading, value); }
        }

        bool isLoading = false;
        public bool IsLoading {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        public PdfViewMode ViewMode {
            get { return GetProperty(() => ViewMode); }
            set { SetProperty(() => ViewMode, value); }
        }

        public List<ReportItemViewModel> AvailableReports { get; private set; }
        ReportItemViewModel selectedReport;
        public ReportItemViewModel SelectedReport {
            get { return selectedReport; }
            set { SetProperty(ref selectedReport, value); }
        }

        public RibbonReportExplorerViewModel() {
            loadCommand = new DelegateCommand<object>(OnLoadDocumentCommand, OnCanExecuteLoadDocumentCommand);
            AvailableReports = new List<ReportItemViewModel> {
                new ReportItemViewModel { DisplayName = "Multicolumn Report",
                                          ReportName = "XtraReportsDemos.MulticolumnReport.Report, DevExpress.DemoReports.v13.1",
                                          Parameters = new MulticolumnParametersViewModel(),
                                          LoadDocumentCommand = loadCommand },
                new ReportItemViewModel { DisplayName = "Master-Detail Report",
                                          ReportName = "XtraReportsDemos.MasterDetailReport.Report, DevExpress.DemoReports.v13.1",
                                          Parameters = new EmptyParametersViewModel(),
                                          LoadDocumentCommand = loadCommand },
                new ReportItemViewModel { DisplayName = "Merged Report",
                                          ReportName = "XtraReportsDemos.ReportMerging.MergedReport, DevExpress.DemoReports.v13.1",
                                          Parameters = new EmptyParametersViewModel(),
                                          LoadDocumentCommand = loadCommand },
                new ReportItemViewModel { DisplayName = "Table Report",
                                          ReportName = "XtraReportsDemos.TableReport.Report, DevExpress.DemoReports.v13.1",
                                          Parameters = new TableReportParametersViewModel(),
                                          LoadDocumentCommand = loadCommand },
                new ReportItemViewModel { DisplayName = "Subreport",
                                          ReportName = "XtraReportsDemos.Subreports.MasterReport, DevExpress.DemoReports.v13.1",
                                          Parameters = new SubreportParametersViewModel(),
                                          LoadDocumentCommand = loadCommand },
            };
            SelectedReport = AvailableReports.First();
            OnLoadDocumentCommand(AvailableReports[0]);
        }

        private bool OnCanExecuteLoadDocumentCommand(object arg) {
            return CanStartLoading;
        }

        async void OnLoadDocumentCommand(object report) {
            CanStartLoading = false;
            loadCommand.RaiseCanExecuteChanged();
            IsLoading = true;
            bool error = false;
            try {
                EndpointAddress endPoint = new EndpointAddress("http://demos.devexpress.com/DemoCenter/Silverlight/DemoReportService.svc");
                var facade = new ReportServiceFacade(endPoint);
                Document = await facade.ExportToPdfAsync((report as ReportItemViewModel).Parameters.ReportParameters, (report as ReportItemViewModel).ReportName);
            } catch {
                IsLoading = false;
                error = true;
            }
            if(error && !Designer.IsInDesignMode())
                await new MessageDialog("An error occurred while loading the report.").ShowAsync();
            CanStartLoading = true;
            loadCommand.RaiseCanExecuteChanged();
        }
    }

    public interface IReportParamatersViewModel {
        List<ReportParameter> ReportParameters { get; }
    }

    public class EmptyParametersViewModel : IReportParamatersViewModel {
        public List<ReportParameter> ReportParameters {
            get { return new List<ReportParameter>(); }
        }
    }

    public class TableReportParametersViewModel : IReportParamatersViewModel {
        public List<ReportParameter> ReportParameters {
            get {
                return new List<ReportParameter> {
                   new ReportParameter { Path = "OrderIdParameter", Value = OrderId },
                   new ReportParameter { Path = "MaxRowCountParameter", Value = MaxRowsPerPage }
                };
            }
        }
        public TableReportParametersViewModel() {
            OrderId = 11077;
            MaxRowsPerPage = 10;
        }
        public int OrderId { get; set; }
        public int MaxRowsPerPage { get; set; }
    }

    public class SubreportParametersViewModel : IReportParamatersViewModel {
        public List<ReportParameter> ReportParameters {
            get {
                return new List<ReportParameter> {
                   new ReportParameter { Path = "subreport1.fromDateParameter", Value = From },
                   new ReportParameter { Path = "subreport1.toDateParameter", Value = To }
                };
            }
        }
        public SubreportParametersViewModel() {
            From = new DateTime(2002, 11, 20);
            To = new DateTime(2002, 12, 20);
        }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class MulticolumnParametersViewModel : IReportParamatersViewModel {
        public List<ReportParameter> ReportParameters {
            get {
                return new List<ReportParameter> {
                   new ReportParameter { Path = "ColumnLayoutParameter", Value = IsHorizontal }
                };
            }
        }
        public MulticolumnParametersViewModel() {
            IsHorizontal = true;
        }
        public bool IsHorizontal { get; set; }
    }

    public class ReportItemViewModel : BindableBase {
        public string DisplayName { get; set; }
        public string ReportName { get; set; }
        public IReportParamatersViewModel Parameters { get; set; }
        public ICommand LoadDocumentCommand { get; internal set; }
    }
}
