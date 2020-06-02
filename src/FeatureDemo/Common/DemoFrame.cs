using System;
using DevExpress.UI.Xaml.Layout;
using Windows.Phone.UI.Input;

namespace FeatureDemo.Common {
    public class DemoFrame : DXFrame {
        public DemoFrame() {
            DefaultStyleKey = typeof(DemoFrame);
        }

        public void ClearHistory(int level) {
            ForwardStack.Clear();
            while(BackStack.Count > level)
                BackStack.RemoveAt(level);
        }
    }    
}