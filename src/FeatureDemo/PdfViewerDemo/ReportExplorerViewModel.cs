using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Input;
using DevExpress.Core.ReportsClient;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using Windows.UI.Popups;
using Designer = DevExpress.Core.Extensions.UIElementExtensions;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace PdfViewerDemo {
    public class ReportExplorerViewModel : DevExpress.Mvvm.BindableBase {
        object document;
        public object Document {
            get { return document; }
            set { SetProperty(ref document, value); }
        }

        public ICommand LoadDocumentCommand { get; private set; }

        bool canStartLoading = true;
        public bool CanStartLoading {
            get { return canStartLoading; }
            set { SetProperty(ref canStartLoading, value); }
        }

        bool isLoading = false;
        public bool IsLoading {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        public List<ReportItemViewModel> AvailableReports { get; private set; }
        ReportItemViewModel selectedReport;
        public ReportItemViewModel SelectedReport {
            get { return selectedReport; }
            set { SetProperty(ref selectedReport, value); }
        }

        public ReportExplorerViewModel() {
            LoadDocumentCommand = new DevExpress.Mvvm.DelegateCommand(OnLoadDocumentCommand);
            AvailableReports = new List<ReportItemViewModel> {
                new ReportItemViewModel { DisplayName = "Multicolumn Report", 
                                          ReportName = "XtraReportsDemos.MulticolumnReport.Report, DevExpress.DemoReports.v14.2",
                                          Parameters = new MulticolumnParametersViewModel() },
                new ReportItemViewModel { DisplayName = "Master-Detail Report",
                                          ReportName = "XtraReportsDemos.MasterDetailReport.Report, DevExpress.DemoReports.v14.2",
                                          Parameters = new EmptyParametersViewModel() },
                new ReportItemViewModel { DisplayName = "Merged Report", 
                                          ReportName = "XtraReportsDemos.ReportMerging.MergedReport, DevExpress.DemoReports.v14.2",
                                          Parameters = new EmptyParametersViewModel() },
                new ReportItemViewModel { DisplayName = "Table Report",
                                          ReportName = "XtraReportsDemos.TableReport.Report, DevExpress.DemoReports.v14.2",
                                          Parameters = new TableReportParametersViewModel() },
                new ReportItemViewModel { DisplayName = "Subreport",
                                          ReportName = "XtraReportsDemos.Subreports.MasterReport, DevExpress.DemoReports.v14.2",
                                          Parameters = new SubreportParametersViewModel() },                                          
            };
            SelectedReport = AvailableReports.First();
            OnLoadDocumentCommand();
        }

        async void OnLoadDocumentCommand() {
            CanStartLoading = false;
            IsLoading = true;
            bool error = false;
            try {
                EndpointAddress endPoint = new EndpointAddress("http://demos.devexpress.com/DemoCenter/Silverlight/DemoReportService.svc");
                var facade = new ReportServiceFacade(endPoint);
                Document = await facade.ExportToPdfAsync(selectedReport.Parameters.ReportParameters, SelectedReport.ReportName);
            } catch {
                IsLoading = false;
                error = true;
            }
            if (error && !Designer.IsInDesignMode())
                await new MessageDialog("An error occurred while loading the report.").ShowAsync();
            CanStartLoading = true;
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

    public class ReportItemViewModel {
        public string DisplayName { get; set; }
        public string ReportName { get; set; }
        public IReportParamatersViewModel Parameters { get; set; }
    }
}
