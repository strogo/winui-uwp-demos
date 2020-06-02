using DevExpress.Mvvm;
using FeatureDemo.DataModel;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using FeatureDemo.Common;
using Windows.Phone.UI.Input;
using Windows.ApplicationModel.Resources.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace FeatureDemo.ViewModel {    
    public class DemoModuleViewModel : NavigationViewModelBase {
        #region properties
        private ObservableCollection<DemoModuleSource> sourceListCore = new ObservableCollection<DemoModuleSource>();
        private DemoModuleSource selectedSourceCore = null;
        bool isVisible = true;

        public DemoModuleGroup Group { get; protected set; }
        public DemoModule DemoModule { get; protected set; }
        public MainViewModel MainViewModel { get; protected set; }
        public ObservableCollection<DemoModuleSource> SourceList {
            get { return sourceListCore; }
            set { SetProperty(ref sourceListCore, value); }
        }
        public DemoModuleSource SelectedSource {
            get { return selectedSourceCore; }
            set { SetProperty(ref selectedSourceCore, value); }
        }
        public bool IsVisible{
            get { return isVisible; }
            set { SetProperty(ref isVisible, value); }
        }
        #endregion
        public DemoModuleViewModel(MainViewModel viewModel, DemoModule demoModule) {
            ((DevExpress.Mvvm.ISupportParentViewModel)this).ParentViewModel = viewModel;
            MainViewModel = viewModel;
            Group = demoModule.Group;
            DemoModule = demoModule;
        }        
        public void InitializeSourceList() {
            if(SourceList.Count != 0) {
                if(SelectedSource == null)
                    SelectedSource = SourceList[0];
                return;
            }
            
            SourceList.Add(new DemoModuleSource(DemoModule.ViewTypeName + ".xaml"));
            SourceList.Add(new DemoModuleSource(DemoModule.ViewTypeName + ".xaml.cs"));
            if(DemoModule.Sources != null) {
                foreach(string source in DemoModule.Sources)
                    SourceList.Add(new DemoModuleSource(source));
            }
            
            SelectedSource = SourceList[0];
        }
    }
}