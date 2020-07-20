using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using FeatureDemo.Common;
using FeatureDemo.DataModel;
using Microsoft.UI.Xaml;
using DevExpress.UI.Xaml.Layout;
using DevExpress.Core.Native;
using Microsoft.UI;
using DevExpress.Core;
using System.Runtime.Serialization;
using Color = Windows.UI.Color;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace FeatureDemo.ViewModel {
    public class MainViewModel : NavigationViewModelBase, ISupportNavigationEvents, ISupportGoBack {
        public const string MainScreenId = "MainScreen";
        #region services
        INavigationService NavigationService { get { return ServiceContainer.GetService<INavigationService>(); } }
        #endregion
        #region commands
        public ICommand DemoModuleClickCommand { get; }
        public ICommand ShowPrevCommand { get; }
        public ICommand ShowNextCommand { get; }
        public ICommand HamburgerMenuItemClickCommand { get; }
        #endregion
        #region properties
        private bool showCommandBar = false;
        private bool showHamburgerButton = true;
        private bool isHamburgerMenuOpen = false;
        private bool prevNextEnabled = false;
        private bool showLabels = true;
        int selectedDemoModuleIndex = 0;
        object selectedHamburgerMenuItem;
        
        public bool PrevNextEnabled {
            get { return prevNextEnabled; }
            set { SetProperty(ref prevNextEnabled, value); }
        }
        public string WindowTitle { get { return "DevExpress WinUI UWP Controls Demo " + AssemblyInfo.VersionShort; } }
        public List<object> HamburgerMenuItems { get; }
        public object SelectedHamburgerMenuItem {
            get { return selectedHamburgerMenuItem; }
            set { SetProperty(ref selectedHamburgerMenuItem, value); }
        }
        public bool ShowCommandBar {
            get { return showCommandBar; }
            set { SetProperty(ref showCommandBar, value); }
        }
        public bool ShowHamburgerButton {
            get { return showHamburgerButton; }
            set { SetProperty(ref showHamburgerButton, value); }
        }
        public bool IsHamburgerMenuOpen {
            get { return isHamburgerMenuOpen; }
            set { SetProperty(ref isHamburgerMenuOpen, value, OnIsHamburgerMenuOpenChanged); }
        }
        public int SelectedDemoModuleIndex {
            get { return selectedDemoModuleIndex; }
            set { SetProperty(ref selectedDemoModuleIndex, value, OnSelectedDemoModuleIndexChanged); }
        }
        public bool ShowLabels {
            get { return showLabels; }
            set { SetProperty(ref showLabels, value); }
        }
        bool IsStateRestored { get; set; }
        NavigationLevel NavigationLevel { get; set; } = NavigationLevel.Main;
        List<string> navigationHistory;
        public ObservableCollection<DemoModuleViewModel> DemoModules { get; private set; }
        public ObservableCollection<DemoModuleViewModel> FilteredDemoModules { get; private set; }
        
        #endregion
        public MainViewModel() {
            navigationHistory = new List<string>() { MainScreenId };
            HamburgerMenuItems = new List<object>();
            DemoModuleClickCommand = new DelegateCommand<object>(DemoModuleClickCommandExecute);
            ShowNextCommand = new DelegateCommand<object>(ShowNextCommandExecute);
            ShowPrevCommand = new DelegateCommand<object>(ShowPrevCommandExecute);
            HamburgerMenuItemClickCommand = new DelegateCommand<object>(HamburgerMenuItemClickCommandExecute);
            InitializeDemoModulesAndHamburgerMenuItems();
        }
        void InitializeDemoModulesAndHamburgerMenuItems() {            
            HamburgerMenuItems.Add(new HamburgerMenuItem(this,
                                                         "Featured Demos",
                                                          "Overview",
                                                          MainScreenId
            ));
            
            DemoModules = new ObservableCollection<DemoModuleViewModel>();
            
            List<DemoModule> newModules = new List<DemoModule>();
            List<DemoModule> updatedModules = new List<DemoModule>();
            List<DemoModule> highlightedModules = new List<DemoModule>();
            List<DemoModule> modules = new List<DemoModule>();

            foreach(DemoModuleGroup demoModuleGroup in DataModel.DataModel.Current.DemoModuleGroups) {
                foreach(DemoModule demoModule in demoModuleGroup.DemoModules) {
                    if(demoModule.IsNew)
                        newModules.Add(demoModule);
                    else if(demoModule.IsUpdated)
                        updatedModules.Add(demoModule);
                    else if(demoModule.IsHighlighted)
                        highlightedModules.Add(demoModule);
                    else modules.Add(demoModule);                   
                }
                HamburgerMenuItems.Add(new HamburgerMenuItem(this, demoModuleGroup.Title, demoModuleGroup.Icon, demoModuleGroup.Name));
            }
            newModules.Union(updatedModules).Union(highlightedModules).Union(modules).Select(x => new DemoModuleViewModel(this, x)).ForEach(x=>DemoModules.Add(x));

            HamburgerMenuItems.Add(new HamburgerMenuDateNavigatorItem());
            HamburgerMenuItems.Add(new HamburgerMenuAccentColorSelectorItem());
            HamburgerMenuItems.Add(new HamburgerMenuThemeSwitcherItem());
            HamburgerMenuItems.Add(new HamburgerMenuHyperLink() { Title = "Privacy Policy", Uri = @"https://www.devexpress.com/Products/NET/Controls/Win10Apps/privacy-policy.xml", Placement = HamburgerMenuNavigationButtonPlacement.Bottom });
            HamburgerMenuItems.Add(new HamburgerMenuHyperLink() { Title = "Browse Online", Uri = @"https://www.devexpress.com/Products/NET/Controls/Win10Apps/", Placement = HamburgerMenuNavigationButtonPlacement.Top });

            FilteredDemoModules = new ObservableCollection<DemoModuleViewModel>();
            foreach(DemoModuleViewModel demoModule in DemoModules) {
                FilteredDemoModules.Add(demoModule);
            }
        }
        void NavigateToDemoModule(DemoModuleViewModel demoModule) {
            UpdateFilteredDemoModules(demoModule);
            SelectedDemoModuleIndex = 0;
            NavigationLevel = NavigationLevel.DemoModule;
            Navigate("DemoModulePage", demoModule.DemoModule.ViewTypeName);
        }
        void UpdateFilteredDemoModules(DemoModuleViewModel firstDemoModule) {
            int index = DemoModules.IndexOf(firstDemoModule);
            FilteredDemoModules.Clear();
            for(int i = index; i < DemoModules.Count; i++) {
                if(DemoModules[i].IsVisible)
                    FilteredDemoModules.Add(DemoModules[i]);
            }
            for(int i = 0; i < index; i++) {
                if(DemoModules[i].IsVisible)
                    FilteredDemoModules.Add(DemoModules[i]);
            }
            PrevNextEnabled = FilteredDemoModules.Count > 1;
        }
        void Navigate(string pageName, string navigationParameter) {
            NavigationService.Navigate(pageName, navigationParameter, null, false);
        }
        bool SelectHamburgerMenuItemByParameter(object parameter) {
            HamburgerMenuItem item = HamburgerMenuItems.OfType<HamburgerMenuItem>().FirstOrDefault(x => x.GroupName != null && x.GroupName == parameter as string);
            if(item != null) {
                SelectedHamburgerMenuItem = item;
                ApplyFilter(item.GroupName);
                return true;
            }
            return false;
        }
        void ApplyFilter(string filterString) {
            DemoModules.ForEach(x => x.IsVisible = x.Group.Name == filterString || filterString == MainScreenId && x.DemoModule.IsHighlighted || string.IsNullOrEmpty(filterString));
            ShowLabels = filterString == MainScreenId;
        }

        void RestoreState(object parameter, PageStateStorage storage) {
            List<string> savedNavigationHistory;
            if(storage.TryGetParameter("DemoNavigationHistory", out savedNavigationHistory)) {
                navigationHistory = savedNavigationHistory;
            };
            object hamburgerMenuItemGroupName = navigationHistory == null ? parameter : navigationHistory[navigationHistory.Count - 1];
            SelectHamburgerMenuItemByParameter(hamburgerMenuItemGroupName);
            if(hamburgerMenuItemGroupName.Equals(parameter)) return;
            DemoModuleViewModel demoModule = DemoModules.FirstOrDefault(x => x.DemoModule.ViewTypeName.Equals(parameter));
            if(demoModule != null) {
                UpdateFilteredDemoModules(demoModule);
                NavigationLevel = NavigationLevel.DemoModule;
                SelectedDemoModuleIndex = storage.GetParameter("SelectedDemoModuleIndex", SelectedDemoModuleIndex);
            } else {
                NavigationLevel = NavigationLevel.Main;
                Navigate("MainPage", MainScreenId);
            }
        }
        void SaveState(PageStateStorage storage) {
            storage.SaveParameter("SelectedDemoModuleIndex", SelectedDemoModuleIndex);
            storage.SaveParameter("NeedDisableNavigationOnSelectedHamburgerMenuItemChanged", true);
            storage.SaveParameter("DemoNavigationHistory", navigationHistory);
        }
        #region command handlers
        void ShowNextCommandExecute(object parameter) {
            SelectedDemoModuleIndex = (SelectedDemoModuleIndex + 1) % FilteredDemoModules.Count;
        }
        void ShowPrevCommandExecute(object parameter) {
            SelectedDemoModuleIndex = SelectedDemoModuleIndex == 0 ? FilteredDemoModules.Count - 1 : SelectedDemoModuleIndex - 1;
        }
        void HamburgerMenuItemClickCommandExecute(object parameter) {
            if(SelectHamburgerMenuItemByParameter(parameter)) {
                bool shouldResetScrollViewer = AddNavigationHistoryEntry(parameter.ToString());
                NavigationLevel = NavigationLevel.Main;
                Navigate("MainPage", parameter.ToString());
                if(shouldResetScrollViewer)
                    GetService<IResetScrollViewService>()?.Reset();
            }
        }
        bool AddNavigationHistoryEntry(string entry) {
            if(navigationHistory.Count != 0 && navigationHistory[navigationHistory.Count - 1] == entry)
                return false;
            navigationHistory.Add(entry);
            return true;
        }
        internal void DemoModuleClickCommandExecute(object parameter) {
            NavigateToDemoModule(parameter as DemoModuleViewModel);
        }
        #endregion
        #region property change handlers
        protected virtual void OnIsHamburgerMenuOpenChanged(bool oldValue, bool newValue) {

        }
        protected virtual void OnSelectedDemoModuleIndexChanged(int oldValue, int newValue) {
        }
        #endregion
        #region ISupportNavigationEvents
        void ISupportNavigationEvents.OnNavigated(DXNavigationEventArgs e) {
            if(IsStateRestored) return;
            RestoreState(e.Parameter, e.Storage);
            IsStateRestored = true;
        }
        void ISupportNavigationEvents.OnNavigating(DXNavigatingCancelEventArgs e) {
            SaveState(e.CurrentPageStorage);
        }
        #endregion
        #region ISupportGoBack
        public bool CanGoBack {
            get {
                return false;
                //return navigationHistory != null && navigationHistory.Count > 1 || NavigationLevel == NavigationLevel.DemoModule;
            }
        }

        public void GoBack() {
            string parameter = null;
            if(NavigationLevel == NavigationLevel.DemoModule) {
                parameter = navigationHistory[navigationHistory.Count - 1];
                NavigationLevel = NavigationLevel.Main;
            } else  if(navigationHistory != null && navigationHistory.Count > 1) {
                parameter = navigationHistory[navigationHistory.Count - 2];
                navigationHistory.RemoveAt(navigationHistory.Count - 1);
            }
            SelectHamburgerMenuItemByParameter(parameter);
            Navigate("MainPage", parameter);
        }

        #endregion
    }
    public class HamburgerMenuItem {
        public MainViewModel MainViewModel { get; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string GroupName { get; set; }
        public HamburgerMenuNavigationButtonPlacement Placement { get; set; }
        public HamburgerMenuItem(MainViewModel viewModel, string title, string icon, string groupName = null) {
            MainViewModel = viewModel;
            Title = title;
            Icon = "ms-appx:///Images/HamburgerMenu/" + icon + ".svg";
            GroupName = groupName;
        }
        public HamburgerMenuItem() {
        }
    }
    public class ColorSchemeInfo {
        public string PublicName { get { return ColorSchemeName + (RequestedTheme == ElementTheme.Light ? " Light" : " Dark"); } }
        public string ColorSchemeName { get; private set; }
        public ElementTheme RequestedTheme { get; private set; }
        public ColorSchemeInfo(string colorSchemeName, ElementTheme requestedTheme) {
            ColorSchemeName = colorSchemeName;
            RequestedTheme = requestedTheme;
        }
    }
    public class AccentColorInfo {
        public Color AccentColor { get; set; }
        public string Name { get; set; }
        public bool ShowInPreview { get; set; }
        public bool IsDefault { get; set; }
        public AccentColorInfo(string name, Color color, bool showInPreview) {
            Name = name;
            AccentColor = color;
            ShowInPreview = showInPreview;
        }
    }
    public class HamburgerMenuAccentColorSelectorItem {
        public string Title { get; set; }
        public string Icon { get; set; }
        public HamburgerMenuAccentColorSelectorItem() {
            Title = "Accent Color";
            Icon = "[F#Transparent W#24 H#24 M 0 0 l 24 0 0 24][F#White L#10 T#4 M 0 0 l 8 0 0 4 4 0 0 11 -12 0 z][F#White L#17.5 T#8 m 0 0 c -1.3 -3 -7 -1.9 -7 -1.9 l 4.9 4.6 -1 1.3 c -0.3 0.1 -1.5 5 2.1 5 3 0 2.3 -6 1 -9 z][F#Black L#16.75 T#9 m 0 0 c -1.1 -2.6 -5.2 -2.1 -5.2 -2.1 l 3.4 4.1 -0.2 1.6 c 0 0 -0.1 3.3 1.8 3.3 1.1 0 1.3 -4.3 0.2 -6.9 z][F#Black L#8.5 T#3.5 m 0 0 l -8.5 8.5 0 2.5 5 5 2.5 0 8.5 -8.5 z][F#White L#8.5 T#5 m 0 0 l 6 6 -7.5 7.5 -1.5 0 -4.5 -4.5 0 -1.5 z][F#White L#19 T#3 m 0 0 l 0 4 4 0 z][F#White L#5 T#9 m 0 1.5 a 1.5 1.5 0 0 1 3 0 a 1.5 1.5 0 0 1 -3 0]";
        }
    }
    public class HamburgerMenuDateNavigatorItem { }
    public class HamburgerMenuThemeSwitcherItem {
        public string Title { get; set; }
        public string Icon { get; set; }
        public HamburgerMenuThemeSwitcherItem() {
            Title = "Color Scheme";
            Icon = "ms-appx:///Images/HamburgerMenu/ColorSchemes.svg";
        }
    }
    public class HamburgerMenuHyperLink : HamburgerMenuItem {
        public string Uri { get; set; }
        public HamburgerMenuHyperLink() {
        }
    }
    public enum NavigationLevel { Main, DemoModule }
}

