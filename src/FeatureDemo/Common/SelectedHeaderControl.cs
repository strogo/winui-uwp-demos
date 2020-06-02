using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FeatureDemo.Common {
    public class SelectableControl : Control {
        public bool IsSelected {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(SelectableControl), new PropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedPropertyChanged)));

        private static void OnIsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((SelectableControl)d).OnIsSelectedChanged((bool)e.OldValue);
        }

        protected virtual void OnIsSelectedChanged(bool oldValue) {
            VisualStateManager.GoToState(this, IsSelected ? "Selected" : "Unselected", false);
        }
    }
}