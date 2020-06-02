using System.ComponentModel;
using System.Runtime.CompilerServices;
using DevExpress.UI.Xaml.Grid;
using Microsoft.UI.Xaml;

using PropertyChangedEventHandler = Microsoft.UI.Xaml.Data.PropertyChangedEventHandler;
using INotifyPropertyChanged = Microsoft.UI.Xaml.Data.INotifyPropertyChanged;
using PropertyChangedEventArgs = Microsoft.UI.Xaml.Data.PropertyChangedEventArgs;

namespace GridDemo {
    public class GridSearchPanelViewModel : DependencyObject, INotifyPropertyChanged {
        public static readonly DependencyProperty IsShowModeDefaultProperty =
            DependencyProperty.Register("IsShowModeDefault", typeof(bool), typeof(GridSearchPanelViewModel), new PropertyMetadata(false, (d, e) => (d as GridSearchPanelViewModel).OnUpdateShowMode()));
        public static readonly DependencyProperty IsShowModeAlwaysProperty =
            DependencyProperty.Register("IsShowModeAlways", typeof(bool), typeof(GridSearchPanelViewModel), new PropertyMetadata(true, (d, e) => (d as GridSearchPanelViewModel).OnUpdateShowMode()));
        public static readonly DependencyProperty IsShowModeNeverProperty =
            DependencyProperty.Register("IsShowModeNever", typeof(bool), typeof(GridSearchPanelViewModel), new PropertyMetadata(false, (d, e) => (d as GridSearchPanelViewModel).OnUpdateShowMode()));

        public static readonly DependencyProperty IsSearchModeAlwaysProperty =
            DependencyProperty.Register("IsSearchModeAlways", typeof(bool), typeof(GridSearchPanelViewModel), new PropertyMetadata(true, (d, e) => (d as GridSearchPanelViewModel).OnUpdateSearchMode()));
        public static readonly DependencyProperty IsSearchModeFindOnEnterProperty =
            DependencyProperty.Register("IsSearchModeFindOnEnter", typeof(bool), typeof(GridSearchPanelViewModel), new PropertyMetadata(false, (d, e) => (d as GridSearchPanelViewModel).OnUpdateSearchMode()));

        public GridSearchPanelViewModel() {
            OnUpdateShowMode();
            OnUpdateSearchMode();
        }

        public bool IsShowModeDefault {
            get { return (bool)GetValue(IsShowModeDefaultProperty); }
            set { SetValue(IsShowModeDefaultProperty, value); }
        }
        public bool IsShowModeAlways {
            get { return (bool)GetValue(IsShowModeAlwaysProperty); }
            set { SetValue(IsShowModeAlwaysProperty, value); }
        }
        public bool IsShowModeNever {
            get { return (bool)GetValue(IsShowModeNeverProperty); }
            set { SetValue(IsShowModeNeverProperty, value); }
        }

        public bool IsSearchModeAlways {
            get { return (bool)GetValue(IsSearchModeAlwaysProperty); }
            set { SetValue(IsSearchModeAlwaysProperty, value); }
        }
        public bool IsSearchModeFindOnEnter {
            get { return (bool)GetValue(IsSearchModeFindOnEnterProperty); }
            set { SetValue(IsSearchModeFindOnEnterProperty, value); }
        }

        ShowSearchPanelMode showMode;
        public ShowSearchPanelMode ShowMode {
            get { return showMode; }
            set { SetProperty(ref showMode, value); }
        }
        FindMode searchMode;
        public FindMode SearchMode {
            get { return searchMode; }
            set { SetProperty(ref searchMode, value); }
        }

        void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
            storage = value;
            OnPropertyChanged(propertyName);
        }

        void OnUpdateShowMode() {
            if(IsShowModeDefault) {
                ShowMode = ShowSearchPanelMode.Default;
            }
            if(IsShowModeAlways) {
                ShowMode = ShowSearchPanelMode.Always;
            }
            if(IsShowModeNever) {
                ShowMode = ShowSearchPanelMode.Never;
            }
        }
        void OnUpdateSearchMode() {
            if(IsSearchModeAlways) {
                SearchMode = FindMode.Always;
            }
            if(IsSearchModeFindOnEnter) {
                SearchMode = FindMode.FindOnEnter;
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            var temp = PropertyChanged;
            if(temp != null) {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
