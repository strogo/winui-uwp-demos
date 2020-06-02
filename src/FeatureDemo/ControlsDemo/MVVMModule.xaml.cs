using System;
using System.Collections.Generic;
using DevExpress.Core;
using Windows.UI.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using FeatureDemo.Common;

namespace ControlsDemo {
    public sealed partial class MVVMModule : DemoModuleView {
        public MVVMModule() {
            this.InitializeComponent();
            Loaded += OnLoad;
            Unloaded += OnUnload;
        }
        void OnLoad(object sender, RoutedEventArgs e) {
            DataContext = CreateViewModel();
        }
        void OnUnload(object sender, RoutedEventArgs e) {
            DataContext = null;
        }

        ViewModel CreateViewModel() {
            var model = new ViewModel();
            model.Items = new List<Item>();
            model.Items.Add(new Item() { Label = "New", Index = 0, Uri = new Uri(@"ms-appx:///ControlsDemo/Images/New_32x32.png") });
            model.Items.Add(new Item() { Label = "Open", Index = 1, Uri = new Uri(@"ms-appx:///ControlsDemo/Images/Open_32x32.png") });
            model.Items.Add(new Container() {
                Label = "Save",
                Uri = new Uri(@"ms-appx:///ControlsDemo/Images/Save_32x32.png"),
                Items = new List<Item>() {
                    new Item() { Label = "Save", Uri = new Uri(@"ms-appx:///ControlsDemo/Images/Save_32x32.png") },
                    new Item() { Label = "Save All", Uri = new Uri(@"ms-appx:///ControlsDemo/Images/SaveAll_32x32.png") },
                    new Item() { Label = "Save as...", Uri = new Uri(@"ms-appx:///ControlsDemo/Images/SaveAs_32x32.png") }, 
                    new Item() { Label = "Save and New", Uri = new Uri(@"ms-appx:///ControlsDemo/Images/SaveAndNew_32x32.png") },
                    new Item() { Label = "Save and Close", Uri = new Uri(@"ms-appx:///ControlsDemo/Images/SaveAndClose_32x32.png") },
                }
            });
            model.Items.Add(new Item() {Label = "Close", Index = 4, Uri = new Uri(@"ms-appx:///ControlsDemo/Images/Close_32x32.png")});
            model.Items.Add(new Item() {Label = "Print", Index = 3, Uri = new Uri(@"ms-appx:///ControlsDemo/Images/Print_32x32.png")});
            return model;
        }
    }
    public class MVVMModuleSelector : DataTemplateSelector {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            var fe = (FrameworkElement)container;
            if (item is Container)
                return (DataTemplate)fe.FindResource("containerTemplate");
            if (item is Item)
                return (DataTemplate)fe.FindResource("itemTemplate");
            return base.SelectTemplateCore(item, container);
        }
    }
}
