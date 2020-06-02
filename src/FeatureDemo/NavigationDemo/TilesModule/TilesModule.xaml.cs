using FeatureDemo.Common;
using FeatureDemo.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace NavigationDemo {
    public sealed partial class TilesModule : DemoModuleView {
        public TilesModule() {
            this.InitializeComponent();
        }
    }

    public class EmployeeTileTemplateSelector : DataTemplateSelector {
        public DataTemplate EmptyTemplate { get; set; }
        public DataTemplate ItemTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item) {
            if(item is Employee)
                return ItemTemplate;
            return EmptyTemplate;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            return SelectTemplateCore(item);
        }
    }
}
