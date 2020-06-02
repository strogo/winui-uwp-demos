using System.Collections.Generic;
using FeatureDemo.Common;
using FeatureDemo.DataModel;

namespace FeatureDemo.ViewModel {
    public abstract class DemoModuleTab : DevExpress.Mvvm.BindableBase {
        public DemoModuleTab(DemoModuleViewModel demoModule) {
            DemoModule = demoModule;
        }
        public DemoModuleViewModel DemoModule { get; private set; }
        public string IconPath { get; set; }
        public string Label { get; set; }
    }
    public class DemoModuleContentTab : DemoModuleTab {
        public DemoModuleContentTab(DemoModuleViewModel demoModule) : base(demoModule) { }
    }
    public class DemoModuleCodeTab : DemoModuleTab {
        DemoModuleSource currentSourceCode;
        public DemoModuleCodeTab(DemoModuleViewModel demoModule)
            : base(demoModule) {
            ModuleSources = new List<DemoModuleSource>();
            List<string> sourcesList = new List<string>();
            //foreach(string fileName in sourcesList) {
            //    DemoModuleSource source = new DemoModuleSource(fileName, new DeferredValue<string>(() => DataProvider.Current.GetDemoModuleCode(DemoModuleData.ModuleInfo.DemoModule, fileName)));
            //    ModuleSources.Add(source);
            //}
            //CurrentModuleSource = ModuleSources.Count == 0 ? null : ModuleSources[0];
        }
        public List<DemoModuleSource> ModuleSources { get; private set; }
        public DemoModuleSource CurrentModuleSource { get { return currentSourceCode; } set { SetProperty<DemoModuleSource>(ref currentSourceCode, value); } }
    }
    public enum DemoModuleTabKind { None, Content, Code }
}
