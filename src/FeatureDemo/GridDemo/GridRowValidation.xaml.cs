using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.UI.Xaml.Grid;
using DevExpress.UI.Xaml.Grid.Native;
using FeatureDemo.Data;
using FeatureDemo.Common;

namespace GridDemo {
    public class ExceptionModeComboBox : EnumComboBox<ExceptionMode> { }
    public sealed partial class GridRowValidation : GridDemoUserControl {
        public GridRowValidation() {
            this.InitializeComponent();
        }

        protected internal override GridControl Grid { get { return grid; } }

        private void ValidateDemoRow(object sender, GridRowValidationEventArgs e) {
            e.Handled = true;
            ValidationInvoices row = e.Row as ValidationInvoices;
            if (row != null) {
                e.IsValid = row.OrderDate < row.RequiredDate;
                if (!e.IsValid)
                    e.ErrorContent = "\"Order Date\" must be precede \"Required Date\"";
            }
        }

        private void grid_InvalidRowException(object sender, InvalidRowExceptionEventArgs e) {
            ValidationInvoices row = e.Row as ValidationInvoices;
            if (row != null) {
                if (row.OrderDate >= row.RequiredDate) {
                    e.ExceptionMode = ExceptionMode.DisplayError;
                    e.WindowCaption = "Error";
                    e.ErrorText = "\"Order Date\" must be precede \"Required Date\"";
                    return;
                }
            }
        }

        private void ProductNameValidate(object sender, GridCellValidationEventArgs e) {
            if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString()))
                e.IsValid = false;
            if (!e.IsValid) {
                e.Handled = true;
                e.ErrorType = DevExpress.UI.Xaml.Editors.Native.ErrorType.Warning;
                e.ErrorContent = "Product Name can't be empty";
            }
        }

        private void colOrderIDValidate(object sender, GridCellValidationEventArgs e) {
            if (e.Value == null) {
                e.IsValid = false;
                e.ErrorContent = "Order ID can't be empty";
                e.ErrorType = DevExpress.UI.Xaml.Editors.Native.ErrorType.Information;
                e.Handled = true;
                return;
            }
            int id;
            e.IsValid = int.TryParse(e.Value.ToString(), out id);
            if (!e.IsValid) {
                e.ErrorContent = "Order ID must be integer";
                e.ErrorType = DevExpress.UI.Xaml.Editors.Native.ErrorType.Critical;
                e.Handled = true;
            }
            if (id <= 0) {
                e.IsValid = false;
                e.ErrorContent = "Order ID must be greater than 0";
                e.ErrorType = DevExpress.UI.Xaml.Editors.Native.ErrorType.Default;
                e.Handled = true;
            }
        }

        void colCustomerID_Validate(object sender, GridCellValidationEventArgs e) {
            if (e.Value == null || string.IsNullOrEmpty(e.Value.ToString())) {
                e.ErrorContent = "Customer ID can't be empty";
                e.ErrorType = DevExpress.UI.Xaml.Editors.Native.ErrorType.Information;
                e.Handled = true;
                e.IsValid = false;
                return;
            }
            string id = e.Value.ToString();
            //if (id.Length != 5) {
            //    e.ErrorContent = "Customer ID must be 5-characters long";
            //    e.ErrorType = DevExpress.UI.Xaml.Editors.Native.ErrorType.Information;
            //    e.Handled = true;
            //    e.IsValid = false;
            //    return;
            //}
            if (id.ToUpper() != id) {
                e.ErrorContent = "Customer ID must contain only upper case letters";
                e.ErrorType = DevExpress.UI.Xaml.Editors.Native.ErrorType.Information;
                e.Handled = true;
                e.IsValid = false;
            }
        }

        private void colFreight_Validate(object sender, GridCellValidationEventArgs e) {
            if (e.Value == null) {
                e.IsValid = false;
                e.ErrorContent = "Freight can't be empty";
                e.ErrorType = DevExpress.UI.Xaml.Editors.Native.ErrorType.Information;
                e.Handled = true;
                return;
            }
            decimal freight;
            e.IsValid = decimal.TryParse(e.Value.ToString(), out freight);
            if (!e.IsValid) {
                e.ErrorContent = "Freight must be decimal";
                e.ErrorType = DevExpress.UI.Xaml.Editors.Native.ErrorType.Critical;
                e.Handled = true;
            }
            if (freight <= 0) {
                e.IsValid = false;
                e.ErrorContent = "Freight must be greater than 0";
                e.ErrorType = DevExpress.UI.Xaml.Editors.Native.ErrorType.Default;
                e.Handled = true;
            }
        }
    }
}
