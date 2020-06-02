using Microsoft.UI.Xaml.Input;

namespace MapDemo {
    public class PhotoGalleryItemControl : VisibleControl {
        public PhotoGalleryItemControl() {
            DefaultStyleKey = typeof(PhotoGalleryItemControl);
            PointerReleased += OnPointerReleased;
        }
        void OnPointerReleased(object sender, PointerRoutedEventArgs e) {
            e.Handled = true;
        }
    }
}