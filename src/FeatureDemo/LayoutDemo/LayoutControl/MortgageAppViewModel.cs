
namespace LayoutDemo.LayoutControl {
    public sealed class MortgageAppViewModel : DevExpress.Mvvm.BindableBase {
        double purchasePrice;
        public double PurchasePrice {
            get { return purchasePrice; }
            set { SetProperty(ref purchasePrice, value, (v1, v2) => UpdateTotalCosts()); }
        }
        double prepaidItems;
        public double PrepaidItems {
            get { return prepaidItems; }
            set { SetProperty(ref prepaidItems, value, (v1, v2) => UpdateTotalCosts()); }
        }
        double closingCosts;
        public double ClosingCosts {
            get { return closingCosts; }
            set { SetProperty(ref closingCosts, value, (v1, v2) => UpdateTotalCosts()); }
        }
        double fees;
        public double Fees {
            get { return fees; }
            set { SetProperty(ref fees, value, (v1, v2) => UpdateTotalCosts()); }
        }
        double discount;
        public double Discount {
            get { return discount; }
            set { SetProperty(ref discount, value, (v1, v2) => UpdateTotalCosts()); }
        }
        double loanAmountWithoutFees;
        public double LoanAmountWithoutFees {
            get { return loanAmountWithoutFees; }
            set { SetProperty(ref loanAmountWithoutFees, value, (v1, v2) => UpdateLoanAmount()); }
        }
        double feesFinanced;
        public double FeesFinanced {
            get { return feesFinanced; }
            set { SetProperty(ref feesFinanced, value, (v1, v2) => UpdateLoanAmount()); }
        }
        double closingCostsPaidBySeller;
        public double ClosingCostsPaidBySeller {
            get { return closingCostsPaidBySeller; }
            set { SetProperty(ref closingCostsPaidBySeller, value, (v1, v2) => UpdateCashToFromBorrower()); }
        }
        double cashToFromBorrower;
        public double CashToFromBorrower {
            get { return cashToFromBorrower; }
            set { SetProperty(ref cashToFromBorrower, value); }
        }
        double totalCosts;
        public double TotalCosts {
            get { return totalCosts; }
            set { SetProperty(ref totalCosts, value, (v1, v2) => UpdateCashToFromBorrower()); }
        }
        double loanAmount;
        public double LoanAmount {
            get { return loanAmount; }
            set { SetProperty(ref loanAmount, value); }
        }
        double loanAmount2;
        public double LoanAmount2 {
            get { return loanAmount2; }
            set { SetProperty(ref loanAmount2, value, (v1, v2) => UpdateCashToFromBorrower()); }
        }
        void UpdateTotalCosts() {
            TotalCosts = PurchasePrice + PrepaidItems + ClosingCosts + Fees + Discount;
        }
        void UpdateLoanAmount() {
            LoanAmount = LoanAmount2 = LoanAmountWithoutFees + FeesFinanced;
        }
        void UpdateCashToFromBorrower() {
            CashToFromBorrower = TotalCosts - ClosingCostsPaidBySeller - LoanAmount2;
        }
    }
}