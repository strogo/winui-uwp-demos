using DevExpress.Core;
using DevExpress.Core.Native;
using DevExpress.UI.Xaml.Layout;
using FeatureDemo.ViewModel;
using RibbonDemo;
using System.Collections.Generic;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using System;
using Color = Windows.UI.Color;

namespace FeatureDemo.Common {
    public static class AccentColorManager {
        static AccentColorInfo accentColor = null;
        public static AccentColorInfo AccentColorInfo {
            get { return accentColor; }
            set { PropertySetterHelper.SetProperty(ref accentColor, value, OnAccentColorChanged); }
        }
        public static event EventHandler AccentColorChanged;
        public static List<AccentColorInfo> ColorList { get; } = new List<AccentColorInfo>() {
                new AccentColorInfo("Default", Color.FromArgb(0xFF, 0x62, 0x62, 0x62), true) { IsDefault = true },
                new AccentColorInfo("Red", Color.FromArgb(0xFF, 0xE8, 0x11, 0x23), true),
                new AccentColorInfo("Green", Color.FromArgb(0xFF, 0x10, 0x89, 0x3E), false),
                new AccentColorInfo("Orange", Color.FromArgb(0xFF, 0xCA, 0x4F, 0x07), false),
                new AccentColorInfo("Blue", Color.FromArgb(0xFF, 0x00, 0x78, 0xD7), false),
                new AccentColorInfo("Turquoise", Color.FromArgb(0xFF, 0x20, 0x81, 0x84), false),
                new AccentColorInfo("Purple", Color.FromArgb(0xFF, 0x88, 0x17, 0x98), false),
                new AccentColorInfo("Gray", Color.FromArgb(0xFF, 0x80, 0x80, 0x80), false)
        };
        private static void OnAccentColorChanged() {
            if(AccentColorInfo.Name == "Default") {
                ThemeManager.ClearOverloads();
            }
            else {
                AccentColorizer.Apply(AccentColorInfo.AccentColor);
            }
            AccentColorChanged?.Invoke(null, null);
        }
        static AccentColorManager() {
            PropertySetterHelper.SetProperty(ref accentColor, ColorList[0]);
        }
    }
    public class HamburgerMenuAccentColorSelector : HamburgerSubMenu {
        public HamburgerMenuAccentColorSelector() {
            Loading += OnLoading;
            AccentColorManager.AccentColorChanged += OnAccentColorChanged;
        }
        private void OnAccentColorChanged(object sender, EventArgs e) {
            SelectedItem = AccentColorManager.AccentColorInfo;
        }
        
        void OnLoading(FrameworkElement sender, object args) {
            InitializeItemsSource();
            SelectedItem = AccentColorManager.AccentColorInfo;
        }
        void InitializeItemsSource() {
            ItemsSource = AccentColorManager.ColorList;
        }
        protected override void OnSelectedItemChanged(object oldValue) {
            base.OnSelectedItemChanged(oldValue);
            AccentColorManager.AccentColorInfo = ((AccentColorInfo)SelectedItem);
        }
    }
}