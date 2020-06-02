using DevExpress.Core.Extensions;
using DevExpress.Mvvm.UI;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DevExpress.Mvvm.Native;

namespace FeatureDemo.Common {
    public interface IShowOptionsService {
        void ShowOptions();
        void HideOptions();
    }
    public class ShowOptionsService : ServiceBase, IShowOptionsService {
        Pivot GetPivot() { return AssociatedObject.VisualChildren().OfType<Pivot>().First(); }
        SplitView GetSplitView() { return AssociatedObject.VisualChildren().OfType<SplitView>().First(); }
        public void HideOptions() {
            if(DeviceFamilyHelper.DeviceFamily == DeviceFamily.Mobile) {
                SplitView splitView = GetSplitView();
                splitView.IsPaneOpen = false;
                ((OptionsControl)splitView.Pane).ContentTemplate = null;
                ((OptionsControl)splitView.Pane).DataContext = null;

            }
        }

        public void ShowOptions() {
            if(DeviceFamilyHelper.DeviceFamily == DeviceFamily.Mobile) {
                Pivot pivot = GetPivot();
                SplitView splitView = GetSplitView();
                FrameworkElement selectedContainer = pivot.ContainerFromIndex(pivot.SelectedIndex) as FrameworkElement;
                DemoModuleViewControl demoModuleViewControl = selectedContainer.VisualChildren().OfType<DemoModuleViewControl>().First();
                ((OptionsControl)splitView.Pane).DemoModule = demoModuleViewControl.DemoModulePresenter.DemoModuleViewModel;
                ((OptionsControl)splitView.Pane).ContentTemplate = demoModuleViewControl.OptionsPaneContent;
                ((OptionsControl)splitView.Pane).DataContext = demoModuleViewControl.DataContext;
                splitView.IsPaneOpen = true;
            }
        }
    }
}