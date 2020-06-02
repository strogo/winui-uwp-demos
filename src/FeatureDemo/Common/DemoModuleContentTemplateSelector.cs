using DevExpress.Core.Extensions;
using DevExpress.Core.Native;
using DevExpress.Mvvm.Native;
using FeatureDemo.View;
using FeatureDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Core;
using Windows.Foundation;

namespace FeatureDemo.Common {
    public class DemoModulePresenter : Grid {
        #region static
        static bool IsOptionsPaneOpenState { get; set; }
        static bool SavedIsOptionsPaneOpened { get; set; }
        public static readonly DependencyProperty SelectedDemoModuleIndexProperty;
        public static readonly DependencyProperty DemoModuleViewModelProperty;
        public static readonly DependencyProperty OptionsVisibilityProperty;
        public static readonly DependencyProperty IsOptionsPaneOpenProperty;

        static DemoModulePresenter() {
            IsOptionsPaneOpenState = true;
            SavedIsOptionsPaneOpened = true;
            DependencyPropertyRegistrator<DemoModulePresenter>.New()
                .Register<int>(nameof(SelectedDemoModuleIndex), out SelectedDemoModuleIndexProperty, 0, (sender, oldValue) => sender.OnSelectedDemoModuleIndexChanged(oldValue))
                .Register<DemoModuleViewModel>(nameof(DemoModuleViewModel), out DemoModuleViewModelProperty, null, (sender, oldValue) => sender.OnDemoModuleViewModelChanged(oldValue))
                .Register(nameof(OptionsVisibility), out OptionsVisibilityProperty, Visibility.Collapsed)
                .Register<bool>(nameof(IsOptionsPaneOpen), out IsOptionsPaneOpenProperty, false, (sender)=>sender.OnIsOptionsPaneOpenChanged());
        }

        #endregion
        #region dep props
        public int SelectedDemoModuleIndex {
            get { return (int)GetValue(SelectedDemoModuleIndexProperty); }
            set { SetValue(SelectedDemoModuleIndexProperty, value); }
        }
        public DemoModuleViewModel DemoModuleViewModel {
            get { return (DemoModuleViewModel)GetValue(DemoModuleViewModelProperty); }
            set { SetValue(DemoModuleViewModelProperty, value); }
        }
        public Visibility OptionsVisibility {
            get { return (Visibility)GetValue(OptionsVisibilityProperty); }
            set { SetValue(OptionsVisibilityProperty, value); }
        }
        public bool IsOptionsPaneOpen{
            get { return (bool)GetValue(IsOptionsPaneOpenProperty); }
            set { SetValue(IsOptionsPaneOpenProperty, value); }
        }
        #endregion
        protected internal DemoModuleView DemoModuleView { get; set; }
        protected DemoModuleLoadingIndicator LoadingIndicator { get; set; }

        protected virtual void OnDemoModuleViewModelChanged(DemoModuleViewModel oldValue) {
            UpdateDemoModuleInstance();            
        }
        bool suppressAreOptionsCheckedLogic;
        protected virtual void OnIsOptionsPaneOpenChanged() {
            if(suppressAreOptionsCheckedLogic) return;
            IsOptionsPaneOpenState = IsOptionsPaneOpen;
            UpdateOptionsPaneOpen();
        }
        void SetIsOptionsPaneOpenSilently(bool value) {
            suppressAreOptionsCheckedLogic = true;
            IsOptionsPaneOpen = value;
            suppressAreOptionsCheckedLogic = false;
        }        
        protected virtual void OnSelectedDemoModuleIndexChanged(int oldValue) {
            UpdateDemoModuleInstance();
        }
        void ClearLoadingIndicator() {
            if(LoadingIndicator != null)
                Children.Remove(LoadingIndicator);
            LoadingIndicator = null;
        }
        void CreateLoadingIndicator() {
            if(LoadingIndicator == null) {
                LoadingIndicator = new DemoModuleLoadingIndicator();
                Children.Add(LoadingIndicator);
            }
        }
        bool IsModuleSelected() {
            return DemoModuleViewModel == null ? false : DemoModuleViewModel.MainViewModel.FilteredDemoModules.IndexOf(DemoModuleViewModel) == SelectedDemoModuleIndex;
        }
        async Task CreateDemoModuleInstance() {
            DemoModuleView newDemoModuleInstance = null;
            DemoModuleViewModel.IsLoaded = false;
            if(newDemoModuleInstance == null) {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => {
                    TypeInfo moduleTypeInfo = typeof(DemoModuleViewModel).GetTypeInfo().Assembly.DefinedTypes.First((ti) => ti.Name == DemoModuleViewModel.DemoModule.ViewTypeName);
                    if(moduleTypeInfo == null) return;
                    newDemoModuleInstance = (DemoModuleView)Activator.CreateInstance(moduleTypeInfo.AsType());
                });
            }
            
            if(IsModuleSelected()) {
                DemoModuleView = newDemoModuleInstance;
                DemoModuleView.PaneClosing += OnOptionsClosing;
                SetIsOptionsPaneOpenSilently(IsOptionsPaneOpenState);
                WindowSizeChangedListener.OnCurrentWindowSizeChanged += OnCoreWindowSizeChanged;
                UpdateOptionsVisibility();
                UpdateOptionsView();
                Children.Add(DemoModuleView);
                ClearLoadingIndicator();
                DemoModuleViewModel.IsLoaded = true;
            }
        }
        void ClearDemoModuleInstance() {            
            if(DemoModuleView != null) {
                Children.Remove(DemoModuleView);
                DemoModuleView = null;
                WindowSizeChangedListener.OnCurrentWindowSizeChanged -= OnCoreWindowSizeChanged;
            }
        }
        protected async void UpdateDemoModuleInstance() {
            if(IsModuleSelected()) {
                await CreateDemoModuleInstance();                
            } else {
                ClearDemoModuleInstance();
                CreateLoadingIndicator();
            }
        }        
        public DemoModulePresenter() {
            LoadingIndicator = new DemoModuleLoadingIndicator();
            Children.Add(LoadingIndicator);
            Unloaded += OnUnloaded;
            RegisterPropertyChangedCallback(VisibilityProperty, (sender, dp) => ((DemoModulePresenter)sender).UpdateOptionsVisibility());
        }

        private void OnCoreWindowSizeChanged(object sender, WindowSizeChangedEventArgs e) {
            UpdateOptionsView(e.Size);
        }

        void OnUnloaded(object sender, RoutedEventArgs e) {
            Children.Clear();
            DemoModuleView = null;
            LoadingIndicator = null;
        }
        #region Options Adaptive Layout
        void UpdateOptionsVisibility() {
            if(Visibility == Visibility.Collapsed) {
                OptionsVisibility = Visibility.Collapsed;
                return;
            }
            if(DemoModuleView != null)
                OptionsVisibility = DemoModuleView.HasOptions.ToVisibility();
        }
        private void OnOptionsClosing(object sender, EventArgs e) {
            IsOptionsPaneOpen = false;
        }
        void UpdateOptionsView(Size windowSize = new Size()) {
            if(DemoModuleView == null) return;
            if(windowSize.Width == 0)
                windowSize = CoreWindow.GetForCurrentThread().Bounds.Size();
            SplitViewDisplayMode newMode = windowSize.Width <= 749 ? SplitViewDisplayMode.Overlay : SplitViewDisplayMode.Inline;
            if(newMode == DemoModuleView.OptionsViewMode) {
                DemoModuleView.IsOptionsPaneOpen = IsOptionsPaneOpen;
                return;
            }
            if(newMode == SplitViewDisplayMode.Inline) {
                SetIsOptionsPaneOpenSilently(SavedIsOptionsPaneOpened);
            } else {
                SavedIsOptionsPaneOpened = IsOptionsPaneOpen;
                IsOptionsPaneOpen = false;
            }
            DemoModuleView.OptionsViewMode = newMode;
        }
        void UpdateOptionsPaneOpen() {
            DemoModuleView.Do(x => x.IsOptionsPaneOpen = IsOptionsPaneOpen);
        }
        #endregion
    }
}