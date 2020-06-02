using System;
using System.Collections.Generic;
using DevExpress.Core;
using DevExpress.Mvvm;

namespace SvgDemo {
    public class SvgDemoIconViewModel : BindableBase {
        public static List<SvgDemoIconViewModel> CreateDefaultIconSet(bool useThemeIcons) {
            List<SvgDemoIconViewModel> list;

            if(useThemeIcons) {
                list = new List<SvgDemoIconViewModel>() {
                new SvgDemoIconViewModel("Cut", "Cut", useThemeIcons),
                new SvgDemoIconViewModel("Edit", "Edit", useThemeIcons),
                new SvgDemoIconViewModel("Open", "Open", useThemeIcons),
                new SvgDemoIconViewModel("Picture", "Picture", useThemeIcons) };
            } else {
                list = new List<SvgDemoIconViewModel>() {
                    new SvgDemoIconViewModel("DXLogo", "Logo", useThemeIcons),
                    new SvgDemoIconViewModel("Cut", "Cut", useThemeIcons),
                    new SvgDemoIconViewModel("Edit", "Edit", useThemeIcons),
                    new SvgDemoIconViewModel("Mail", "Mail", useThemeIcons),
                    new SvgDemoIconViewModel("Preview", "Preview", useThemeIcons),
                    new SvgDemoIconViewModel("Refresh", "Refresh", useThemeIcons),
                    new SvgDemoIconViewModel("Save", "Save", useThemeIcons) };
            }
            return list;
        }

        public Uri Source { get; }
        public string Caption { get; }
        public SvgPalette Palette {
            get { return GetProperty<SvgPalette>(); }
            set { SetProperty(value); }
        }

        public SvgDemoIconViewModel(string iconName, string caption, bool useThemeIcons) {
            Source = new Uri(string.Format("ms-appx:///FeatureDemo;component/SvgDemo/{0}/{1}.svg", useThemeIcons ? "ThemedSvg" : "Images", iconName));
            Caption = caption;
        }
    }
}
