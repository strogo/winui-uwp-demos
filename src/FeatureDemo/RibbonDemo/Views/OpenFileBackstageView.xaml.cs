using Microsoft.UI.Xaml.Controls;

namespace RibbonDemo {
    public sealed partial class OpenFileBackstageView : UserControl {
        public BackstageOpenTabViewModel ViewModel { get { return DataContext as BackstageOpenTabViewModel; } }

        public OpenFileBackstageView() {
            this.InitializeComponent();
        }
    }
}
