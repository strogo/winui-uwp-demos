using System;
using System.Windows.Input;
using DevExpress.Mvvm;
using Windows.Graphics.Printing;
using Windows.UI.Core;
using Windows.UI.Popups;
using Microsoft.UI.Xaml;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace PdfViewerDemo {
    public class SimplePdfViewerViewModel : BindableBase {
        public ICommand PrintDocumentCommand { get; private set; }
        public IPrintDocumentSource PrintDocumentSource { get; set; }
        CoreDispatcher dispatcher = Window.Current.Dispatcher;

        public SimplePdfViewerViewModel() {
            if(ViewModelBase.IsInDesignMode)
                return;
            
            PrintDocumentCommand = new DelegateCommand(PrintDocument);
        }

        async void PrintDocument() {
            PrintManager.GetForCurrentView().PrintTaskRequested += printManager_PrintTaskRequested;
            await PrintManager.ShowPrintUIAsync();
            PrintManager.GetForCurrentView().PrintTaskRequested -= printManager_PrintTaskRequested;
        }

        void printManager_PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args) {
            PrintTask printTask = null;            
            printTask = args.Request.CreatePrintTask("PdfViewer Demo Print Task", sourceRequested => {
                printTask.Completed += async (s, e) => {
                    if(e.Completion == PrintTaskCompletion.Failed) {
                        await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () => {
                            await new MessageDialog("Printing error.").ShowAsync();
                        });
                    }
                };
                sourceRequested.SetSource(PrintDocumentSource);
            });
        }
    }
}
