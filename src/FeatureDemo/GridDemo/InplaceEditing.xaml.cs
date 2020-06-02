using DevExpress.UI.Xaml.Grid;
using FeatureDemo.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Windows.System;

namespace GridDemo {
    public sealed partial class InplaceEditing : GridDemoUserControl {
        public InplaceEditing() {
            this.InitializeComponent();
        }
        protected internal override GridControl Grid { get { return grid; } }
    }
    public abstract class ObjectToImageConverter : IValueConverter {
        Dictionary<string, BitmapImage> images = new Dictionary<string, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, string language) {
            string path = GetImagePath(value);
            if(String.IsNullOrEmpty(path)) return null;
            BitmapImage image = null;
            if(!images.TryGetValue(path, out image)) {
                image = new BitmapImage(new Uri(path));
                images[path] = image;
            }
            return image;
        }
        protected abstract string GetImagePath(object value);

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
    public class PriorityToImageConverter : ObjectToImageConverter {
        protected override string GetImagePath(object value) {
            string path = "";
            if (value is FeatureDemo.Data.Priority)
                path = "ms-appx:///GridDemo/Icons/Priority/" + Enum.GetName(typeof(FeatureDemo.Data.Priority), value) + ".png";
            
            return path;
        }
    }
    public class StatusToImageConverter : ObjectToImageConverter {
        protected override string GetImagePath(object value) {
            string path = "";
            if (value is FeatureDemo.Data.Status)
                path = "ms-appx:///GridDemo/Icons/Status/" + Enum.GetName(typeof(FeatureDemo.Data.Status), value) + ".png";

            return path;
        }
    }
    public class DXSlider : Slider {
        public DXSlider() {
            DefaultStyleKey = typeof(DXSlider);
        }
        protected override void OnKeyDown(KeyRoutedEventArgs e) {
            if(e.Key == VirtualKey.Right && Value >= Maximum)
                return;
            if(e.Key == VirtualKey.Left && Value <= Minimum)
                return;
            base.OnKeyDown(e);
        }
    }
}
