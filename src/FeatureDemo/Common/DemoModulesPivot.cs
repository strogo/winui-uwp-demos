using System;
using DevExpress.Core.Native;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using FeatureDemo.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevExpress.Mvvm.Native;
using DevExpress.Core.Extensions;

namespace FeatureDemo.Common {
    public class DemoModulesPivot : Pivot {
        #region static
        public static readonly DependencyProperty DemoModuleIndexProperty;
        public static readonly DependencyProperty DemoModulesProperty;
        static DemoModulesPivot() {
            DependencyPropertyRegistrator<DemoModulesPivot>.New()
                .Register<int>(nameof(DemoModuleIndex), out DemoModuleIndexProperty, 0, (d) => d.OnDemoModuleIndexChanged())
                .Register<IEnumerable<DemoModuleViewModel>>(nameof(DemoModules), out DemoModulesProperty, null, (d) => d.OnDemoModulesChanged());
        }

        #endregion
        #region dep props
        public int DemoModuleIndex {
            get { return (int)GetValue(DemoModuleIndexProperty); }
            set { SetValue(DemoModuleIndexProperty, value); }
        }
        public IEnumerable<DemoModuleViewModel> DemoModules {
            get { return (IEnumerable<DemoModuleViewModel>)GetValue(DemoModulesProperty); }
            set { SetValue(DemoModulesProperty, value); }
        }
        #endregion
        #region props
        DemoModulePivotItem CurrentPivotItem { get; set; }
        bool PreferedOptionsVisibility { get; set; } = true;
        #endregion
        public DemoModulesPivot() {
            PivotItemLoading += OnPivotItemLoading;
            PivotItemUnloading += OnPivotItemUnloading;
            Unloaded += OnUnloaded;
            Items.Add(new DemoModulePivotItem() { Index = 0 });
            Items.Add(new DemoModulePivotItem() { Index = 1 });
            Items.Add(new DemoModulePivotItem() { Index = 2 });
        }

        private void OnUnloaded(object sender, RoutedEventArgs e) {
            if(CurrentPivotItem != null) {
                CurrentPivotItem.ViewModel = null;
                CurrentPivotItem = null;
            }
        }

        private void OnPivotItemUnloading(Pivot sender, PivotItemEventArgs args) {
            DemoModulePivotItem item = (DemoModulePivotItem)args.Item;
            PreferedOptionsVisibility = item.PreferedOptionsVisibility;
            item.ViewModel = null;
        }
        bool loadingByDemoModuleIndexChanged;
        private void OnPivotItemLoading(Pivot sender, PivotItemEventArgs args) {
            if(DemoModules == null) return;
            DemoModulePivotItem item = (DemoModulePivotItem)args.Item;
            
            if(!loadingByDemoModuleIndexChanged && CurrentPivotItem != null) {
                int oldIndex = CurrentPivotItem.Index;
                int newIndex = item.Index;
                int count = DemoModules.Count();
                SetDemoModuleIndexSilently((DemoModuleIndex + count + ((newIndex - oldIndex + 2) % 3 == 0 ? 1 : -1)) % count);                
            }
            loadingByDemoModuleIndexChanged = false;
            CurrentPivotItem = item;
            CurrentPivotItem.PreferedOptionsVisibility = PreferedOptionsVisibility;
            if(DemoModules != null) {
                item.ViewModel = DemoModules.ElementAt(DemoModuleIndex);
            }
        }
        int GetNextContainerIndex() { return (SelectedIndex + 1) % Items.Count; }        
        protected virtual void OnDemoModuleIndexChanged() {
            if(suppressDemoModuleIndexChangedLogic || CurrentPivotItem == null) return;
            loadingByDemoModuleIndexChanged = true;
            SelectedIndex = GetNextContainerIndex();
        }
        bool suppressDemoModuleIndexChangedLogic;
        protected virtual void OnDemoModulesChanged() {
            if(DemoModules != null && CurrentPivotItem != null) {
                CurrentPivotItem.ViewModel = DemoModules.ElementAt(DemoModuleIndex);
            }
        }
        protected void SetDemoModuleIndexSilently(int index) {
            suppressDemoModuleIndexChangedLogic = true;
            DemoModuleIndex = index;
            suppressDemoModuleIndexChangedLogic = false;
        }
    }
    public class DemoModulePivotItem : PivotItem {
        const double MinContentWidth = 546;
        #region static
        public static readonly DependencyProperty IsOptionsPaneOpenProperty;
        public static readonly DependencyProperty OptionsButtonVisibilityProperty;
        public static readonly DependencyProperty IsCodeVisibleProperty;
        public static readonly DependencyProperty IsOptionsButtonEnabledProperty;
        static DemoModulePivotItem() {
            DependencyPropertyRegistrator<DemoModulePivotItem>.New()
                .Register<Visibility>(nameof(OptionsButtonVisibility), out OptionsButtonVisibilityProperty, Visibility.Visible)
                .Register<bool>(nameof(IsOptionsPaneOpen), out IsOptionsPaneOpenProperty, true, (s) => s.OnIsOptionsPaneOpenChanged())
                .Register<bool>(nameof(IsCodeVisible), out IsCodeVisibleProperty, false, (s) => s.OnIsCodeVisibleChanged())
                .Register<bool>(nameof(IsOptionsButtonEnabled), out IsOptionsButtonEnabledProperty, true);
        }
        #endregion
        #region dep props
        public Visibility OptionsButtonVisibility {
            get { return (Visibility)GetValue(OptionsButtonVisibilityProperty); }
            set { SetValue(OptionsButtonVisibilityProperty, value); }
        }
        public bool IsOptionsPaneOpen {
            get { return (bool)GetValue(IsOptionsPaneOpenProperty); }
            set { SetValue(IsOptionsPaneOpenProperty, value); }
        }
        public bool IsCodeVisible {
            get { return (bool)GetValue(IsCodeVisibleProperty); }
            set { SetValue(IsCodeVisibleProperty, value); }
        }
        public bool IsOptionsButtonEnabled {
            get { return (bool)GetValue(IsOptionsButtonEnabledProperty); }
            set { SetValue(IsOptionsButtonEnabledProperty, value); }
        }

        #endregion
        public DemoModuleView DemoModuleView { get; set; }
        internal bool PreferedOptionsVisibility { get; set; }
        bool OptionsButtonWasAffected { get; set; }
        public int Index { get; set; }
        DemoModuleViewModel viewModel;
        public DemoModuleViewModel ViewModel {
            get { return viewModel; }
            set { PropertySetterHelper.SetProperty(ref viewModel, value, OnViewModelChanged); }
        }
        Grid DemoModuleContainer { get; set; }
        public DemoModulePivotItem() {
            DefaultStyleKey = typeof(DemoModulePivotItem);
            DataContext = null;
           
        }
        protected override void OnApplyTemplate() {
            base.OnApplyTemplate();
            DemoModuleContainer = GetTemplateChild("demoModuleContainer") as Grid;
        }
        private void OnViewModelChanged(DemoModuleViewModel oldValue) {
            DataContext = ViewModel;
            if(ViewModel == null) {
                if(DemoModuleView != null) {
                    DemoModuleView.Initialized -= OnDemoModuleViewInitialized;
                    DemoModuleView.SplitView.Do(x => x.PaneClosing -= OnOptionsPaneClosing);
                    DemoModuleView.SplitView.Do(x => x.IsPaneOpen = x.DisplayMode == SplitViewDisplayMode.Inline ? x.IsPaneOpen : false);
                    if(DemoModuleView.HideOptionsInitially)
                        SetIsOptionsPaneOpenSilently(false);
                    
                    DemoModuleContainer.Children.Remove(DemoModuleView);
                }
                WindowSizeChangedListener.OnCurrentWindowSizeChanged -= OnCoreWindowSizeChanged;
                DemoModuleView = null;
                return;
            }
            TypeInfo moduleTypeInfo = typeof(DemoModuleViewModel).GetTypeInfo().Assembly.DefinedTypes.First((ti) => ti.Name == ViewModel.DemoModule.ViewTypeName);
            DemoModuleView = (DemoModuleView)Activator.CreateInstance(moduleTypeInfo.AsType());
            DemoModuleView.Initialized += OnDemoModuleViewInitialized;
            IsCodeVisible = false;
            IsOptionsButtonEnabled = true;
            DemoModuleContainer.Children.Add(DemoModuleView);
        }

        private void OnOptionsPaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args) {
            SetIsOptionsPaneOpenSilently(false);
        }
        private void OnDemoModuleViewInitialized(object sender, EventArgs e) {
            OptionsButtonWasAffected = false;
            DemoModuleView.Initialized -= OnDemoModuleViewInitialized;
            UpdateOptionsButtonVisibility();
            if(DemoModuleView.SplitView == null) return;
            DemoModuleView.SplitView.Do(x => x.PaneClosing += OnOptionsPaneClosing);            
            DemoModuleView.SplitView.DisplayMode = GetSplitViewDisplayMode();
            SetIsOptionsPaneOpenSilently(DemoModuleView.SplitView.DisplayMode != SplitViewDisplayMode.Overlay && PreferedOptionsVisibility && !DemoModuleView.HideOptionsInitially);
            DemoModuleView.SplitView.IsPaneOpen = IsOptionsPaneOpen;
            WindowSizeChangedListener.OnCurrentWindowSizeChanged += OnCoreWindowSizeChanged;
        }
        void UpdateOptionsButtonVisibility() {
            OptionsButtonVisibility = DemoModuleView.HasOptions.ToVisibility();
        }
        private void OnCoreWindowSizeChanged(object sender, WindowSizeChangedEventArgs e) {
            if(DemoModuleView == null || DemoModuleView.SplitView == null) return;
            double width = e.Size.Width;
            SplitViewDisplayMode newMode = GetSplitViewDisplayMode(width);
            if(newMode == DemoModuleView.SplitView.DisplayMode) return;
            DemoModuleView.SplitView.DisplayMode = newMode;
            bool isOptionsPaneOpen = false;
            if(newMode == SplitViewDisplayMode.Inline) {
                isOptionsPaneOpen = OptionsButtonWasAffected ? PreferedOptionsVisibility : PreferedOptionsVisibility && !DemoModuleView.HideOptionsInitially;
            }

            SetIsOptionsPaneOpenSilently(isOptionsPaneOpen);
            DemoModuleView.SplitView.IsPaneOpen = isOptionsPaneOpen;
        }
        SplitViewDisplayMode GetSplitViewDisplayMode(double width = double.NaN) {
            if(DemoModuleView == null || DemoModuleView.SplitView == null) return SplitViewDisplayMode.Inline;
            if(DemoModuleView.ShowOptionsInOverlay) return SplitViewDisplayMode.Overlay;
            if(double.IsNaN(width))
                width = Windows.UI.Core.CoreWindow.GetForCurrentThread().Bounds.Size().Width;
            return width - 44 - DemoModuleView.OptionsPaneWidth <= MinContentWidth ? SplitViewDisplayMode.Overlay : SplitViewDisplayMode.Inline;
        }
        bool suppressIsOptionsPaneOpenChangedLogic;
        private void OnIsCodeVisibleChanged() {
            if(IsCodeVisible && ViewModel != null) {
                ViewModel.InitializeSourceList();
            }
            IsOptionsButtonEnabled = !IsCodeVisible;
        }
        private void OnIsOptionsPaneOpenChanged() {
            if(suppressIsOptionsPaneOpenChangedLogic) return;
            OptionsButtonWasAffected = true;
            if(DemoModuleView.SplitView.DisplayMode == SplitViewDisplayMode.Inline)
                PreferedOptionsVisibility = IsOptionsPaneOpen;
            DemoModuleView.SplitView.IsPaneOpen = IsOptionsPaneOpen;
        }
        private void SetIsOptionsPaneOpenSilently(bool value) {
            suppressIsOptionsPaneOpenChangedLogic = true;
            IsOptionsPaneOpen = value;
            suppressIsOptionsPaneOpenChangedLogic = false;
        }
    }
}