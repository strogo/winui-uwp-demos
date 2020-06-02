using DevExpress.Core.Native;
using DevExpress.UI.Xaml.Layout;
using FeatureDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FeatureDemo.View {
    public sealed partial class DemoModuleGroupPage : DXPage {
        #region static
        public static readonly DependencyProperty ViewModelProperty;
        static DemoModuleGroupPage() {
            DependencyPropertyRegistrator<DemoModuleGroupPage> registrator = new DependencyPropertyRegistrator<DemoModuleGroupPage>();
            registrator.Register<MainViewModel>(nameof(ViewModel), ref ViewModelProperty, null);
        }
        #endregion
        #region dep props
        public MainViewModel ViewModel {
            get { return (MainViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion

        public DemoModuleGroupPage() {
            InitializeComponent();
        }
        protected override void OnFrameViewModelChanged(object oldValue) {
            ViewModel = FrameViewModel as MainViewModel;
        }
    }
}
