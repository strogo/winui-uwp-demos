using DevExpress.Mvvm;
using FeatureDemo.DataModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.ApplicationModel.Resources.Core;
using Windows.Phone.UI.Input;
using System.Linq;
using DevExpress.Core;
using System;
using FeatureDemo.Common;

namespace FeatureDemo.ViewModel {
    public class DemoModuleGroupViewModel : NavigationViewModelBase {
        #region properties
        public MainViewModel MainViewModel { get; private set; }
        public DemoModuleGroup Group { get; private set; }
        public ObservableCollection<DemoModuleViewModel> DemoModules { get; private set; }
        public bool IsMultipleDemoModulesGroup { get; set; }

        ObservableCollection<DemoModuleViewModel> actualDemoModulesCore;
        public ObservableCollection<DemoModuleViewModel> ActualDemoModules {
            get {
                if(actualDemoModulesCore == null)
                    actualDemoModulesCore = new ObservableCollection<DemoModuleViewModel>();
                return actualDemoModulesCore;
            }
        }
        

        private DemoModuleViewModel SelectedDemoModuleCore = null;
        public DemoModuleViewModel SelectedDemoModule {
            get { return SelectedDemoModuleCore; }
            set { SetProperty(ref SelectedDemoModuleCore, value, OnSelectedDemoModuleChanged); }
        }
        public ObservableCollection<DemoModuleViewModel> HighlightedDemoModules { get; private set; }
        #endregion

        public DemoModuleGroupViewModel(MainViewModel mainViewModel, DemoModuleGroup group) {
            ((DevExpress.Mvvm.ISupportParentViewModel)this).ParentViewModel = mainViewModel;
            MainViewModel = mainViewModel;
            Group = group;            
            DemoModules = new ObservableCollection<DemoModuleViewModel>();
            HighlightedDemoModules = new ObservableCollection<DemoModuleViewModel>();
            bool isNewExist = false;
            bool isUpdatedExist = false;
            foreach(DemoModule demoModule in Group.DemoModules) {
                isNewExist = isNewExist || (!demoModule.IsHighlighted && demoModule.IsNew);
                isUpdatedExist = isUpdatedExist || demoModule.IsUpdated;
                DemoModuleViewModel demoModuleViewModel = new DemoModuleViewModel(this, demoModule);
                if(demoModule.IsHighlighted)
                    HighlightedDemoModules.Add(demoModuleViewModel);
                DemoModules.Add(demoModuleViewModel);
            }
            if(HighlightedDemoModules.Count != DemoModules.Count)
                HighlightedDemoModules.Add(new MoreDemoModuleViewModel(this, new DemoModule() { IsNew = isNewExist, IsUpdated = !isNewExist && isUpdatedExist }));
            IsMultipleDemoModulesGroup = DemoModules.Count != 1;
        }
        protected virtual void OnSelectedDemoModuleChanged(DemoModuleViewModel oldValue, DemoModuleViewModel newValue) {
            ObservableCollection<DemoModuleViewModel> demoModules = new ObservableCollection<DemoModuleViewModel>();
            ActualDemoModules.Clear();
            int demoModuleIndex = DemoModules.IndexOf(newValue);
            for(int i = 0; i < DemoModules.Count; i++) {
                ActualDemoModules.Add(DemoModules[demoModuleIndex]);
                demoModuleIndex = (demoModuleIndex + 1) % DemoModules.Count;
            }
        }
    }
}
