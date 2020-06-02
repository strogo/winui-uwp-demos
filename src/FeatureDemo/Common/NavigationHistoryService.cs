using DevExpress.Mvvm.UI;

namespace FeatureDemo.Common {
    public interface INavigationHistoryService {
        void ClearHistory(int level);
    }
    public class NavigationHistoryService : ServiceBase, INavigationHistoryService {
        public void ClearHistory(int level) {
            ((DemoFrame)AssociatedObject).ClearHistory(level);
        }
    }
}