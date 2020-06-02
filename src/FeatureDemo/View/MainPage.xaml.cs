using DevExpress.Core.Native;
using DevExpress.Mvvm.UI.Native;
using DevExpress.UI.Xaml.Layout;
using FeatureDemo.Common;
using FeatureDemo.ViewModel;
using System;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace FeatureDemo.View {
    public sealed partial class MainPage : DXPage {
        #region static
        public static readonly DependencyProperty ViewModelProperty;
        static MainPage() {
            DependencyPropertyRegistrator<MainPage> registrator = new DependencyPropertyRegistrator<MainPage>();
            registrator.Register<MainViewModel>(nameof(ViewModel), ref ViewModelProperty, null);
        }
        #endregion
        #region dep props
        public MainViewModel ViewModel {
            get { return (MainViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion

        public MainPage() {
            InitializeComponent();
        }
        protected override void OnFrameViewModelChanged(object oldValue) {
            ViewModel = FrameViewModel as MainViewModel;
        }

        private void DemoModulesGrid_ItemClick(object sender, object e) {
            ((DemoModuleViewModel)e).MainViewModel.DemoModuleClickCommand.Execute(e);
        }
    }
    public sealed class DemoModuleTile : Button {
        public DemoModuleTile() {
            DefaultStyleKey = typeof(DemoModuleTile);
        }
    }
}
