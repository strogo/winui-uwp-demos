using DevExpress.UI.Xaml.Layout;
using Microsoft.UI.Xaml.Navigation;

namespace NavigationDemo {
    public sealed partial class TileBarModuleDetailView : DXPage {
        public TileBarModuleDetailView() {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            DataContext = e.Parameter;
        }
    }
}
