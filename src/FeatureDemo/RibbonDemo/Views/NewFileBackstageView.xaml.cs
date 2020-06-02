using Microsoft.UI.Xaml.Controls;

namespace RibbonDemo {
    public sealed partial class NewFileBackstageView : UserControl {
        public BackstageNewTabViewModel ViewModel { get { return DataContext as BackstageNewTabViewModel; } }

        public NewFileBackstageView() {
            this.InitializeComponent();
        }
    }
}
