using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DevExpress.Core;
using DevExpress.Core.Native;
using DevExpress.UI.Xaml.Layout;
using FeatureDemo.ViewModel;
using DevExpress.Utils;
using Microsoft.UI.Xaml;

namespace FeatureDemo.Common {
    public class HamburgerMenuThemeSwitcher : HamburgerSubMenu {
        public static readonly DependencyProperty SyncToThemeManagerOnCreateProperty;

        static HamburgerMenuThemeSwitcher() {
            new DependencyPropertyRegistrator<HamburgerMenuThemeSwitcher>()
                .Register(nameof(SyncToThemeManagerOnCreate), out SyncToThemeManagerOnCreateProperty, false);
        }

        public bool SyncToThemeManagerOnCreate {
            get { return (bool)GetValue(SyncToThemeManagerOnCreateProperty); }
            set { SetValue(SyncToThemeManagerOnCreateProperty, value); }
        }

        bool isLoadedEx = false;
        Locker selectionLocker = new Locker();

        public HamburgerMenuThemeSwitcher() {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            Loading += OnLoading;
        }

        void OnLoading(FrameworkElement sender, object args) {
            InitializeItemsSource();
            ThemeManager.RequestedThemeChanged += OnRequestedThemeChanged;
            ThemeManager.ThemeChanged += OnThemeChanged;
        }

        protected override void OnSelectedItemChanged(object oldValue) {
            base.OnSelectedItemChanged(oldValue);
            if(!selectionLocker.IsLocked && SelectedItem != null) {
                var item = SelectedItem as ColorSchemeInfo;
                ThemeManager.CurrentColorSchemeName = item.ColorSchemeName;
                ThemeManager.RequestedTheme = item.RequestedTheme;
            }
        }
        void OnUnloaded(object sender, RoutedEventArgs e) {
            isLoadedEx = false;
        }
        void OnLoaded(object sender, RoutedEventArgs e) {
            isLoadedEx = true;
            SyncSelectedItemWithThemeManager(false);
        }
        void OnThemeChanged(object sender, ThemeChangedEventArgs e) {
            if(isLoadedEx)
                SyncSelectedItemWithThemeManagerSafe();
        }
        void OnRequestedThemeChanged(object sender, EventArgs e) {
            if(isLoadedEx)
                SyncSelectedItemWithThemeManagerSafe();
        }
        void InitializeItemsSource() {
            ItemsSource = new List<ColorSchemeInfo>() {
                new ColorSchemeInfo(ThemeManager.GenericSchemeName, ElementTheme.Light),
                new ColorSchemeInfo(ThemeManager.GenericSchemeName, ElementTheme.Dark)
            };
            SyncSelectedItemWithThemeManager(SyncToThemeManagerOnCreate);
        }

        async void SyncSelectedItemWithThemeManagerSafe() {
            try {
                if(!Dispatcher.HasThreadAccess)
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => SyncSelectedItemWithThemeManager(false));
                else
                    SyncSelectedItemWithThemeManager(false);
            } catch(InvalidComObjectException) {
                return;
            }
        }
        void SyncSelectedItemWithThemeManager(bool selectFirstIfNotFound) {
            var themes = (ItemsSource as IEnumerable<ColorSchemeInfo>).Where(x => x.ColorSchemeName == ThemeManager.CurrentColorSchemeName);
            var itemToSelect = themes.FirstOrDefault(x => x.RequestedTheme == ThemeManager.RequestedTheme);
            if(selectFirstIfNotFound && itemToSelect == null)
                SelectedItem = (ItemsSource as IEnumerable<ColorSchemeInfo>).FirstOrDefault();
            else {
                selectionLocker.Lock();
                SelectedItem = itemToSelect;
                selectionLocker.Unlock();
            }
        }
    }
}
