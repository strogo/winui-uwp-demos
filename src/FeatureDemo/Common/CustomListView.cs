using System;
using System.Linq;
using DevExpress.Core.Native;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.Native;

namespace FeatureDemo.Common {
    public class CustomListView : ListView {
        public static readonly DependencyProperty ItemClickCommandProperty;

        static CustomListView() {
            ItemClickCommandProperty = DependencyPropertyManager.Register<CustomListView, ICommand<object>>(nameof(ItemClickCommand), null);
        }

        public ICommand<object> ItemClickCommand {
            get { return (ICommand<object>)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public CustomListView() {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }
        protected virtual FrameworkElement FindParent(object sender, DependencyObject originalSource) {
            return LayoutTreeHelper.GetVisualParents(originalSource, (DependencyObject)sender).Where(CheckItemType(sender)).FirstOrDefault() as FrameworkElement;
        }
        protected Func<DependencyObject, bool> CheckItemType(object sender) {
            return d => d.GetType() == typeof(ListViewItem);
        }
        void OnUnloaded(object sender, RoutedEventArgs e) {
            Loaded -= OnLoaded;
            Unloaded -= OnUnloaded;
            Tapped -= OnTapped;
        }

        void OnLoaded(object sender, RoutedEventArgs e) {
            Tapped += OnTapped;
        }

        void OnTapped(object sender, TappedRoutedEventArgs e) {
            if (ItemClickCommand == null) return;
            var element = FindParent(sender, e.OriginalSource as DependencyObject);
            object result = null;
            if (element == null)
                result = null;
            else if (element.DataContext != null)
                result = element.DataContext;
            else
                result = (sender as IItemContainerMapping).With(x => x.ItemFromContainer(element));
            if (ItemClickCommand.CanExecute(result))
                ItemClickCommand.Execute(result);
        }
    }
}