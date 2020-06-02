using DevExpress.Mvvm;
using Microsoft.UI.Xaml.Controls;
using DevExpress.UI.Xaml.Editors;
using Microsoft.UI.Xaml;

namespace EditorsDemo {
    public class RatingCustomStyleSelector {
        public RatingEditViewModel ViewModel { get; set; }
        public Style DefaultStyle { get; set; }
        public Style CustomStyle { get; set; }

        public Style GetStyle() {
            if(ViewModel.IsCustomStyleChecked)
                return CustomStyle;
            return DefaultStyle;
        }
    }

    public class RatingEditViewModel : ViewModelBase {
        public RatingCustomStyleSelector RatingItemStyleSelector {
            get { return GetProperty<RatingCustomStyleSelector>(); }
            set { SetProperty(value); OnRatingItemStyleSelectorUpdated(value); }
        }
        public double Value {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
        public int ItemsCount {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }
        public double ItemWidth {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
        public double ItemHeight {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
        public Orientation Orientation {
            get { return GetProperty<Orientation>(); }
            set { SetProperty(value); }
        }
        public RatingPrecision Precision {
            get { return GetProperty<RatingPrecision>(); }
            set { SetProperty(value); OnPrecisionChanged(); }
        }
        public bool IsDefaultStyleChecked {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); OnViewChanged(); }
        }
        public bool IsCustomStyleChecked {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); OnViewChanged(); }
        }
        public Style CalculatedRatingItemStyle {
            get { return GetProperty<Style>(); }
            private set { SetProperty(value); }
        }
        public bool CustomTextVisible {
            get { return GetProperty<bool>(); }
            private set { SetProperty(value); }
        }
        public double Increment {
            get { return GetProperty<double>(); }
            private set { SetProperty(value); }
        }

        public RatingEditViewModel() {
            Value = 1;
            ItemsCount = 5;
            ItemWidth = 30;
            ItemHeight = 30;
            Orientation = Orientation.Horizontal;
            Precision = RatingPrecision.Full;
            IsDefaultStyleChecked = true;
            CustomTextVisible = false;
        }

        void OnPrecisionChanged() {
            Increment = GetIncrement(Precision);
        }
        double GetIncrement(RatingPrecision precision) {
            switch(precision) {
                case RatingPrecision.Half:
                    return 0.5d;
                case RatingPrecision.Exact:
                    return 0.1d;
                default:
                    return 1.0d;
            }
        }
        void OnViewChanged() {
            CalculatedRatingItemStyle = RatingItemStyleSelector != null ? RatingItemStyleSelector.GetStyle() : null;
            CustomTextVisible = IsCustomStyleChecked;
        }
        void OnRatingItemStyleSelectorUpdated(RatingCustomStyleSelector newValue) {
            newValue.ViewModel = this;
            OnViewChanged();
        }
    }
}