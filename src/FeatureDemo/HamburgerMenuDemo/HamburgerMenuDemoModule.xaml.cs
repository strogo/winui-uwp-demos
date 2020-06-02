using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.UI.Xaml.Layout;
using FeatureDemo.Common;
using FeatureDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using DevExpress.Utils;
using Windows.System;

namespace HamburgerMenuDemo {
    public sealed partial class HamburgerMenuDemoModule : DemoModuleView {
        public HamburgerMenuDemoModule() {
            this.InitializeComponent();
            Unloaded += HamburgerMenuDemoModule_Unloaded;
        }
        CoreApplicationView hamburgerMenuFrameView;
        private void HamburgerMenuDemoModule_Unloaded(object sender, RoutedEventArgs e) {
            CloseHamburgerMenuWindow();
        }
        private async void CloseHamburgerMenuWindow() {
            if(hamburgerMenuFrameView != null) {
                ApplicationView.GetForCurrentView().Consolidated -= HamburgerMenuDemoModule_Consolidated;
                await hamburgerMenuFrameView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Window.Current.Close());
                hamburgerMenuFrameView = null;
            }
        }
        private async void ShowHamburgerMenuWindow() {
            hamburgerMenuFrameView = CoreApplication.CreateNewView();
            ApplicationView.GetForCurrentView().Consolidated += HamburgerMenuDemoModule_Consolidated;
            int hamburgerMenuFrameViewId = 0;
            string description = ((DemoModuleViewModel)DataContext).DemoModule.About;
            string windowTitle = ((DemoModuleViewModel)DataContext).DemoModule.Title;
            await hamburgerMenuFrameView.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,
                            () => {
                                ApplicationView.GetForCurrentView().Consolidated += DemoView_Consolidated;
                                ApplicationView.GetForCurrentView().Title = windowTitle;                                
                                
                                var frame = new HamburgerMenuFrame(typeof(HamburgerMenuDemoModule_Menu));
                                var viewModel = new HamburgerMenuDemo_ViewModel() { Description = description, Title = windowTitle };
                                frame.DataContext = viewModel;
                                ((ISupportServices)viewModel).ServiceContainer.RegisterService(new DispatcherService());
                                ((ISupportServices)viewModel).ServiceContainer.RegisterService(new CloseWindowService(CloseDemoView));
                                frame.Navigate(typeof(HamburgerMenuDemo_SettingsPage), null);
                                Window.Current.Content = frame;
                                Window.Current.Activate();
                                hamburgerMenuFrameViewId = ApplicationView.GetForCurrentView().Id;
                            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(hamburgerMenuFrameViewId);
        }

        async void CloseDemoView() {
            ApplicationView.GetForCurrentView().Consolidated -= DemoView_Consolidated;
            await LaunchButton.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { LaunchButton.IsEnabled = true; this.hamburgerMenuFrameView = null; });
            Window.Current.Close();
        }

        void Button_Click(object sender, RoutedEventArgs e) {
            LaunchButton.IsEnabled = false;
            ShowHamburgerMenuWindow();
        }

        void HamburgerMenuDemoModule_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args) {
            Application.Current.Exit();
        }

        void DemoView_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args) {
            if(!CoreApplication.GetCurrentView().IsMain) {
                CloseDemoView();
            }            
        }
    }
}
