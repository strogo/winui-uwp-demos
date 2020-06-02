using Microsoft.UI.Xaml;
namespace MapDemo {
    public class PhotoGalleryButton : VisibleControl {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(object), typeof(PhotoGalleryButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate",
            typeof(DataTemplate), typeof(PhotoGalleryButton), new PropertyMetadata(null));

        public object Content {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public DataTemplate ContentTemplate {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public PhotoGalleryButton() {
            DefaultStyleKey = typeof(PhotoGalleryButton);
        }
    }
}