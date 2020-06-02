using DevExpress.Mvvm.Native;
using DevExpress.UI.Xaml.Layout;
using System.Collections.Generic;
using System.Reflection;

namespace NavigationDemo {
    public interface IFlyoutService {
        void CloseFlyout();
    }
    public class TileBarService : DevExpress.Mvvm.UI.ServiceBase, IFlyoutService {
        public TileBar TileBar { get; set; }
        TileBar GetTargetObject() {
            return TileBar ?? AssociatedObject as TileBar;
        }
        #region ITileBarService Members
        public void CloseFlyout() {
            GetTargetObject().Do(x => x.CloseFlyout());
        }
        #endregion
    }
    class DetailViewTypePropvider : TypeProviderBase {
        public override IEnumerable<Assembly> Assemblies {
            get {
                return new[] { GetType().GetTypeInfo().Assembly };
            }
        }
    }
}
