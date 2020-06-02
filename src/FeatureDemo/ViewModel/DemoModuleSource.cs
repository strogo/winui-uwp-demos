using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DevExpress.Mvvm.Native;

namespace FeatureDemo.ViewModel {
    public class DemoModuleSource {
        const string DesktopDeviceFamilyResoruceQualifier = "DeviceFamily_Desktop";
        const string MobileDeviceFamilyResoruceQualifier = "DeviceFamily_Mobile";
        public DemoModuleSource(string fileName) {
            FileName = fileName;
            GetCodeTextFunc = ()=>GetDemoModuleCode(fileName);
        }
        public string GetDemoModuleCode(string fileName) {
            fileName = "." + fileName.Replace("/", ".");
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            List<string> resourceNameList = new List<string>();
            foreach(string resourceName in assembly.GetManifestResourceNames()) {
                if(resourceName.EndsWith(fileName, StringComparison.Ordinal)) {
                    resourceNameList.Add(resourceName);
                }
            }
            if(resourceNameList.Count == 0)
                return string.Empty;
            

            string deviceFamily = null;
            switch(DeviceFamilyHelper.DeviceFamily) {
                case DeviceFamily.Desktop:
                    deviceFamily = DesktopDeviceFamilyResoruceQualifier;
                    break;
                case DeviceFamily.Mobile:
                    deviceFamily = MobileDeviceFamilyResoruceQualifier;
                    break;
            }
            if(deviceFamily == null)
                return string.Empty;

            string resourceToLoad = null;
            foreach(string r in resourceNameList) {
                if(r.Contains(deviceFamily)) {
                    resourceToLoad = r;
                    break;
                }
            }
            if(resourceToLoad == null) {
                foreach(string r in resourceNameList) {
                    if(!r.Contains(MobileDeviceFamilyResoruceQualifier) && !r.Contains(DesktopDeviceFamilyResoruceQualifier)) {
                        resourceToLoad = r;
                        break;
                    }
                }
            }
            if(resourceToLoad == null)
                return string.Empty;

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resourceToLoad))) {
                return reader.ReadToEnd();
            }            
        }

        public string FileName { get; private set; }
        public Func<string> GetCodeTextFunc { get; private set; }
    }
}
