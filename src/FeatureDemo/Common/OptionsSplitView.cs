using FeatureDemo.ViewModel;
using Windows.UI.Xaml.Controls;

namespace FeatureDemo.Common {
    public class OptionsSplitView : SplitView {
        public OptionsSplitView() {
            PaneClosed += OnPaneClosed;
        }

        private void OnPaneClosed(SplitView sender, object args) {
            if(DataContext is MainViewModel)
                ((MainViewModel)DataContext).IsOptionsVisible = false;
        }
    }
}
