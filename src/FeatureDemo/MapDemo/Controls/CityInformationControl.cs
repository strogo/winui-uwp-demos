using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;

namespace MapDemo {
    public class CityInformationControl : VisibleControl {
        public static readonly DependencyProperty CityInfoProperty = DependencyProperty.Register("CityInfo",
            typeof(CityInfo), typeof(CityInformationControl), new PropertyMetadata(null, new PropertyChangedCallback(CityInfoPropertyChanged)));
        public static readonly DependencyProperty CityImageSourceProperty = DependencyProperty.Register("CityImageSource",
            typeof(BitmapImage), typeof(CityInformationControl), new PropertyMetadata(null));

        public CityInfo CityInfo {
            get { return (CityInfo)GetValue(CityInfoProperty); }
            set { SetValue(CityInfoProperty, value); }
        }
        public BitmapImage CityImageSource {
            get { return (BitmapImage)GetValue(CityImageSourceProperty); }
            set { SetValue(CityImageSourceProperty, value); }
        }

        static void CityInfoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            CityInformationControl control = d as CityInformationControl;
            if (control != null && !e.NewValue.Equals(e.OldValue))
                control.StartImageChanging();
        }

        TimeSpan imageChangeInterval;
        DispatcherTimer imageChangeTimer;
        int currentPlaceIndex;

        public TimeSpan ImageChangeInterval {
            get { return imageChangeInterval; }
            set { imageChangeInterval = value; }
        }

        public CityInformationControl() {
            DefaultStyleKey = typeof(CityInformationControl);
            imageChangeInterval = TimeSpan.FromSeconds(2.0);
            imageChangeTimer = new DispatcherTimer() { Interval = imageChangeInterval };
            imageChangeTimer.Tick += ImageChangeTimer_Tick;
        }

        void StartImageChanging() {
            currentPlaceIndex = 0;
            if (CityInfo != null && CityInfo.Places.Count > 0)
                CityImageSource = CityInfo.Places[currentPlaceIndex].ImageSource;
            imageChangeTimer.Start();
        }
        void ImageChangeTimer_Tick(object sender, object e) {
            if (CityInfo != null && CityInfo.Places.Count > 0) {
                currentPlaceIndex++;
                if (currentPlaceIndex > CityInfo.Places.Count - 1)
                    currentPlaceIndex = 0;
                CityImageSource = CityInfo.Places[currentPlaceIndex].ImageSource;
                imageChangeTimer.Interval = imageChangeInterval;
            }
            else
                imageChangeTimer.Stop();
        }
    }
}
