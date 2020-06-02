using System.Reflection;
using System.Collections.Generic;
using DevExpress.UI.Xaml.Layout;

namespace FeatureDemo {
    public class NavigationTypeProvider : TypeProviderBase {
        public override IEnumerable<System.Reflection.Assembly> Assemblies {
            get {
                yield return typeof(App).GetTypeInfo().Assembly;
            }
        }
    }
}