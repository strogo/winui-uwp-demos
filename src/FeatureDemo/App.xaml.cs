using System;
using System.Threading.Tasks;
using DevExpress.UI.Xaml.Layout;
using DevExpress.Utils;
using FeatureDemo.Common;
using FeatureDemo.View;
using FeatureDemo.ViewModel;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Microsoft.UI.Xaml;
using DevExpress.Mvvm.Native;
using System.Collections.Generic;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.DemoData.Models;
using System.Linq;
using SchedulerDemo;

namespace FeatureDemo {
    sealed partial class App : Application {
        public App() {
            DotNetNativeAssistant.IsEnabled = true;
            this.UnhandledException += App_UnhandledException;
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            SuspensionManager.RegisterKnownType(typeof(List<string>));
            DevExpress.UI.Xaml.Layout.Utils.ApplicationViewState.ApplicationViewMinWidth = 0d;
            //WorkData.SaveToXml();
        }
        Task<Windows.UI.Popups.IUICommand> task;
        void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e) {
            e.Handled = true;
            if(task != null && !task.IsCompleted)
                return;
            var dialog = new Windows.UI.Popups.MessageDialog(e.Exception.ToString());
            task = dialog.ShowAsync().AsTask();
        }
        MainViewModel viewModel;
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            if(args.UWPLaunchActivatedEventArgs.PreviousExecutionState == ApplicationExecutionState.Running) {
                Window.Current.Activate();
                return;
            }
            viewModel = new MainViewModel();
            DXFrame rootFrame = null;
            
            rootFrame = new HamburgerMenuFrame(typeof(HamburgerMenuPage)) { NavigationTypeProvider = new NavigationTypeProvider(), ViewModel = viewModel, DataContext = viewModel, UseLayoutRounding = true };
            Interaction.GetBehaviors(rootFrame).Add(new DevExpress.Mvvm.UI.NavigationService() { Frame = rootFrame });
            
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

            if(args.UWPLaunchActivatedEventArgs.PreviousExecutionState == ApplicationExecutionState.Terminated ||
                args.UWPLaunchActivatedEventArgs.PreviousExecutionState == ApplicationExecutionState.ClosedByUser) {
                await SuspensionManager.RestoreAsync();
            }

            if(rootFrame.Content == null) {
                if(!rootFrame.Navigate(typeof(MainPage), MainViewModel.MainScreenId)) {
                    throw new Exception("Failed to create initial page");
                }
            }
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetPreferredMinSize(new Windows.Foundation.Size { Width = 500, Height = 500 });

        }
        async private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
        private async void LaunchUrlAsync(string url) {
            await Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}