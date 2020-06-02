using System;
using DevExpress.Core;
using DevExpress.UI.Xaml.Editors;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using DevExpress.UI.Xaml.Layout;
using DevExpress.UI.Xaml.Scheduler;

namespace FeatureDemo.Common {
    public class GroupTypeComboBox : EnumComboBox<SchedulerGroupType> { }
    public class AppointmentsDisplayModeComboBox : EnumComboBox<AppointmentsDisplayMode> { }
    public class OrientationComboBox : EnumComboBox<Orientation> { }
    public class PlacementComboBox : EnumComboBox<Dock> { }
    public class RatingPrecisionComboBox : EnumComboBox<RatingPrecision> { }
    public class CalendarSelectionModeComboBox : EnumComboBox<CalendarSelectionMode> { }
    public class StretchComboBox : EnumComboBox<Stretch> { }
    public class MasterDetailPageContentSplitterModeComboBox : EnumComboBox<MasterDetailPageContentSplitterMode> { }
    public class DoubleComboBox : ComboBox {
        public DoubleComboBox() {
            ItemsSource = new double[] { double.NaN, 10d, 20d, 30d, 50d, 100d, 150d, 200d };
        }
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item) {
            base.PrepareContainerForItemOverride(element, item);
            ((ComboBoxItem)element).Content = item.ToString();
            ((ComboBoxItem)element).DataContext = item;


        }
    }
    [Bindable]
    public abstract class EnumComboBox<TEnum> : ComboBox {
        public EnumComboBox() {
            ItemsSource = Enum.GetValues(typeof(TEnum));
        }
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item) {
            base.PrepareContainerForItemOverride(element, item);
            ((ComboBoxItem)element).Content = item.ToString();
            ((ComboBoxItem)element).DataContext = item;
        }
    }
}
