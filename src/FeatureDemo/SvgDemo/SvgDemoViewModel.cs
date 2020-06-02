using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Media;

namespace SvgDemo {
    public class SvgDemoViewModel : NavigationViewModelBase {
        public List<SvgDemoIconViewModel> Icons { get; private set; }
        public SvgDemoIconViewModel SelectedIcon {
            get { return GetProperty<SvgDemoIconViewModel>(); }
            set { SetProperty(value); }
        }
        public Stretch IconStretch {
            get { return GetProperty<Stretch>(); }
            set { SetProperty(value); }
        }
        public double Width {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
        public double Height {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public SvgDemoViewModel() {
            Width = 150;
            Height = 150;
            IconStretch = Stretch.Uniform;
            Icons = SvgDemoIconViewModel.CreateDefaultIconSet(false);
            SelectedIcon = Icons[0];
        }
    }
}