using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraPrinting.BarCode.Native;
using DevExpress.Mvvm.Native;

namespace FeatureDemo.ControlsDemo {
    public class BarCodeSampleViewModel : DevExpress.Mvvm.BindableBase {
        bool showText;
        bool autoModule;
        string text;
        double barCodeModule;
        BarCodeSymbology barCodeSymbology;
        public BarCodeSampleViewModel() {
            this.Text = "101";
            this.BarCodeModule = 3;
            this.AutoModule = true;
            this.ShowText = true;
            this.BarCodeTypes = GetBarCodeTypes();
            this.BarCodeSymbology = this.BarCodeTypes.First();
        }
        public bool ShowText {
            get { return showText; }
            set { SetProperty(ref showText, value); } 
        }
        public bool AutoModule {
            get { return autoModule; }
            set { SetProperty(ref autoModule, value); } 
        }
        public string Text {
            get { return text; }
            set { SetProperty(ref text, value); }
        }
        public double BarCodeModule {
            get { return barCodeModule; }
            set { SetProperty(ref barCodeModule, value); }
        }
        public BarCodeSymbology BarCodeSymbology {
            get { return barCodeSymbology; }
            set { SetProperty(ref barCodeSymbology, value); }
        }
        IEnumerable<BarCodeSymbology> GetBarCodeTypes() {
            DeviceFamily deviceFamily = DeviceFamilyHelper.DeviceFamily;
            var symbologies = Enum.GetValues(typeof(BarCodeSymbology)).Cast<BarCodeSymbology>();
            return symbologies.TakeWhile(x => !IsSymbology2D(x)).ToArray();
        }
        public IEnumerable<BarCodeSymbology> BarCodeTypes { get; private set; }

        static bool IsSymbology2D(BarCodeSymbology symbology) {
            return symbology == BarCodeSymbology.QRCode || 
                symbology == BarCodeSymbology.DataMatrix || 
                symbology == BarCodeSymbology.DataMatrixGS1 || 
                symbology == BarCodeSymbology.PDF417;
        }
        class SymbologyComparer : IComparer<BarCodeSymbology> {
            public int Compare(BarCodeSymbology x, BarCodeSymbology y) {
                bool first2d = IsSymbology2D(x);
                bool second2d = IsSymbology2D(y);
                if(first2d == second2d)
                    return 0;
                if(first2d)
                    return -1;
                return 1;            
            }
        }
    }
}
