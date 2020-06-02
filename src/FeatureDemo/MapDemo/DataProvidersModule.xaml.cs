using System.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using DevExpress.UI.Xaml.Map;
using Microsoft.UI.Xaml;
using FeatureDemo.Common;

namespace MapDemo {
    public sealed partial class DataProvidersModule : DemoModuleView {
        #region static
        public static readonly DependencyProperty DataProviderProperty;
        public static readonly DependencyProperty MapBackgroundProperty;
        public static readonly DependencyProperty IsOpenStreetMapsCheckedProperty;
        public static readonly DependencyProperty IsBingMapsCheckedProperty;
        static DataProvidersModule() {
            DataProviderProperty = DependencyProperty.Register("DataProvider", typeof(MapDataProviderBase), typeof(DataProvidersModule), new PropertyMetadata(null));
            MapBackgroundProperty = DependencyProperty.Register("MapBackground", typeof(Brush), typeof(DataProvidersModule), new PropertyMetadata(null));
            IsOpenStreetMapsCheckedProperty = DependencyProperty.Register("IsOpenStreetMapsChecked", typeof(bool), typeof(DataProvidersModule), new PropertyMetadata(false, new PropertyChangedCallback(OnIsOpenStreetMapsCheckedPropertyChanged)));
            IsBingMapsCheckedProperty = DependencyProperty.Register("IsBingMapsChecked", typeof(bool), typeof(DataProvidersModule), new PropertyMetadata(false, new PropertyChangedCallback(OnIsBingMapsCheckedPropertyChanged)));
        }
        private static void OnIsOpenStreetMapsCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((DataProvidersModule)d).OnIsOpenStreetMapsCheckedChanged((bool)e.OldValue);
        }
        private static void OnIsBingMapsCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((DataProvidersModule)d).OnIsBingMapsCheckedChanged((bool)e.OldValue);
        }
        #endregion
        #region dep props

        public MapDataProviderBase DataProvider {
            get { return (MapDataProviderBase)GetValue(DataProviderProperty); }
            set { SetValue(DataProviderProperty, value); }
        }
        public Brush MapBackground {
            get { return (Brush)GetValue(MapBackgroundProperty); }
            set { SetValue(MapBackgroundProperty, value); }
        }
        public bool IsOpenStreetMapsChecked {
            get { return (bool)GetValue(IsOpenStreetMapsCheckedProperty); }
            set { SetValue(IsOpenStreetMapsCheckedProperty, value); }
        }
        public bool IsBingMapsChecked {
            get { return (bool)GetValue(IsBingMapsCheckedProperty); }
            set { SetValue(IsBingMapsCheckedProperty, value); }
        }
        #endregion

        BingMapDataProvider BingMapDataProvider { get { return (BingMapDataProvider)Resources["BingMapDataProvider"]; } }
        OpenStreetMapDataProvider OpenStreetMapDataProvider { get { return (OpenStreetMapDataProvider)Resources["OpenStreetMapDataProvider"]; } }
        SolidColorBrush BingBackground { get { return (SolidColorBrush)Resources["BingBackground"]; } }
        SolidColorBrush OpenStreetBackground { get { return (SolidColorBrush)Resources["OpenStreetBackground"]; } }

        public DataProvidersModule() {
            this.InitializeComponent();
            DataContext = this;
            Loaded += OnLoaded;
            
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            IsBingMapsChecked = true;
        }

        void OnIsOpenStreetMapsCheckedChanged(bool oldValue) {
            if(IsOpenStreetMapsChecked) {
                MapBackground = OpenStreetBackground;
                DataProvider = OpenStreetMapDataProvider;

            }
        }
        void OnIsBingMapsCheckedChanged(bool oldValue) {
            if(IsBingMapsChecked) {
                MapBackground = BingBackground;
                DataProvider = BingMapDataProvider;
            }
        }        
    }
}