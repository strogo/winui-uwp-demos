using DevExpress.UI.Xaml.Grid;
using FeatureDemo.Common;
using System;

namespace GridDemo {
    public sealed partial class GridSearchPanelModule : GridDemoUserControl {
        public GridSearchPanelModule() {
            this.InitializeComponent();
        }
        protected internal override GridControl Grid {
            get { return grid; }
        }
    }
}
