using FeatureDemo.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using FeatureDemo.ViewModel;

namespace DXBrushesDemo {
    public sealed partial class DXBrushesDemoModule : DemoModuleView {


        public object SelectedColor {
            get { return (object)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(object), typeof(DXBrushesDemoModule), new PropertyMetadata(null, OnSelectedColorChanged));

        public List<AccentColorInfo> ColorList { get; }
        public DXBrushesDemoModule() {
            ColorList = AccentColorManager.ColorList;
            this.InitializeComponent();
            Loaded += DXBrushesDemoModule_Loaded;
            Unloaded += DXBrushesDemoModule_Unloaded;
        }
        static bool disableSelectedColorChangedLogic;
        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(e.OldValue == null || disableSelectedColorChangedLogic) return;            
            AccentColorManager.AccentColorInfo = (AccentColorInfo)e.NewValue;
        }
        private void DXBrushesDemoModule_Unloaded(object sender, RoutedEventArgs e) {
            AccentColorManager.AccentColorChanged -= OnAccentColorChanged;
        }
        void SetSelectedColorSilently(AccentColorInfo color) {
            disableSelectedColorChangedLogic = true;
            SelectedColor = color;
            disableSelectedColorChangedLogic = false;
        }
        private void OnAccentColorChanged(object sender, EventArgs e) {
            SetSelectedColorSilently(AccentColorManager.AccentColorInfo);
        }

        private void DXBrushesDemoModule_Loaded(object sender, RoutedEventArgs e) {
            AccentColorManager.AccentColorChanged += OnAccentColorChanged;
            SetSelectedColorSilently(AccentColorManager.AccentColorInfo);
        }
    }
}
