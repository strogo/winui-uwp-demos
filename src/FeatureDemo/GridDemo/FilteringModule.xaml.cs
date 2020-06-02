using System;
using DevExpress.Data.Filtering;
using DevExpress.UI.Xaml.Grid;
using FeatureDemo.Common;

namespace GridDemo {
    public sealed partial class FilteringModule : GridDemoUserControl {
        protected internal override GridControl Grid { get { return grid; } }

        public FilteringModule() {
            this.InitializeComponent();
            grid.ExpandAllGroups();

            var orderDate = new OperandProperty("UnboundOrderDate");
            var unitPrice = new OperandProperty("UnitPrice");
            var today = DateTime.Today;
            var nextMonth = today.AddMonths(1);
            var filter = orderDate >= new DateTime(today.Year, today.Month, 1) & orderDate < new DateTime(nextMonth.Year, nextMonth.Month, 1);

            grid.FilterCriteria = filter;
        }
    }
}
