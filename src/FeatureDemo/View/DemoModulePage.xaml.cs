using DevExpress.UI.Xaml.Layout;
using FeatureDemo.Common;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using Microsoft.UI.Xaml.Media;
using FeatureDemo.ViewModel;
using DevExpress.Core.Extensions;
using System.Linq;
using DevExpress.Core.Native;
using System.Threading.Tasks;

namespace FeatureDemo.View {
    public sealed partial class DemoModulePage : DXPage {
        #region static
        public static readonly DependencyProperty ViewModelProperty;
        static DemoModulePage() {
            DependencyPropertyRegistrator<DemoModulePage> registrator = new DependencyPropertyRegistrator<DemoModulePage>();
            registrator.Register<MainViewModel>(nameof(ViewModel), ref ViewModelProperty, null);
        }
        #endregion
        #region dep props
        public MainViewModel ViewModel {
            get { return (MainViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion

        public DemoModulePage() {
            InitializeComponent();            
        }
        protected override void OnFrameViewModelChanged(object oldValue) {
            ViewModel = FrameViewModel as MainViewModel;
        }

        private async void DemoModuleLoadingIndicator_Loaded(object sender, RoutedEventArgs e) {
            await Task.Yield();
            pivot.Visibility = Visibility.Visible;
        }
    }
}
