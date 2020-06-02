using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.ComponentModel;
using System.Xml.Linq;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Data;
using DevExpress.UI.Xaml.Map;
using FeatureDemo.Common;
using INotifyPropertyChanged = Microsoft.UI.Xaml.Data.INotifyPropertyChanged;
using PropertyChangedEventHandler = Microsoft.UI.Xaml.Data.PropertyChangedEventHandler;
using PropertyChangedEventArgs = Microsoft.UI.Xaml.Data.PropertyChangedEventArgs;

namespace MapDemo {
    public sealed partial class PhotoGallery : DemoModuleView {
        CitiesViewModel ViewModel { get { return DataContext as CitiesViewModel; } }
        public PhotoGallery() {
            this.InitializeComponent();
            CitiesViewModel viewModel = new CitiesViewModel(map, Resources["citySmallIconTemplate"] as DataTemplate);
            DataContext = viewModel;
            map.Layers[0].ViewportChanged += TileLayer_ViewportChanged;
        }
        void TileLayer_ViewportChanged(object sender, ViewportChangedEventArgs e) {
            navWindow.LeftTop = e.TopLeft;
            navWindow.RightBottom = e.BottomRight;
        }
        void GalleryItemClick(object sender, RoutedEventArgs e) {
            PhotoGalleryItemControl item = sender as PhotoGalleryItemControl;
            if (item != null)
                ViewModel.SelectedPlace = item.DataContext as PlaceInfo;
        }
        void OnGalleryClose(object sender, RoutedEventArgs e) {
            ViewModel.SelectedCity = null;
        }
        void OnBackClick(object sender, RoutedEventArgs e) {
            ViewModel.SelectedCity = null;
        }
        void photoGallery_PointerReleased(object sender, PointerRoutedEventArgs e) {
            ViewModel.SelectedCity = null;
        }
        void placeControl_ShowNextSight(object sender, RoutedEventArgs e) {
            ViewModel.ShowNextSight();
        }
        void placeControl_ShowPreviousSight(object sender, RoutedEventArgs e) {
            ViewModel.ShowPrevSight();
        }
    }

    public enum ViewType { Map, Gallery, Detail }

    public class CitiesViewModel : INotifyPropertyChanged {
        ViewType viewType;
        ObservableCollection<MapCustomElement> citySmallIcons;
        ObservableCollection<MapCustomElement> cities;
        PlaceInfo selectedPlace = null;
        CityInfo selectedCity = null;
        Point cityPoint = new Point(0, 0);
        GeoPoint centerPoint = new GeoPoint(47.5, 2);
        double zoomLevel = 5.0;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewType ViewType {
            get { return viewType; }
            set {
                if (viewType == value)
                    return;
                viewType = value;
                Update();
                RaisePropertyChanged("ViewType");
                RaisePropertyChanged("IsMapView");
                RaisePropertyChanged("IsGalleryView");
                RaisePropertyChanged("IsDetailView");
            }
        }

        public PlaceInfo SelectedPlace {
            get { return selectedPlace; }
            set {
                if (selectedPlace == value)
                    return;
                selectedPlace = value;
                UpdateViewType();
                RaisePropertyChanged("SelectedPlace");
            }
        }

        public ObservableCollection<MapCustomElement> CitySmallIcons {
            get { return citySmallIcons; }
            set {
                if (citySmallIcons == value)
                    return;
                citySmallIcons = value;
                RaisePropertyChanged("CitySmallIcons");
            }
        }

        public ObservableCollection<MapCustomElement> Cities {
            get { return cities; }
            set {
                if (cities == value)
                    return;
                cities = value;
                RaisePropertyChanged("Cities");
            }
        }

        public CityInfo SelectedCity {
            get { return selectedCity; }
            set {
                if (selectedCity == value)
                    return;
                selectedCity = value;
                UpdateViewType();
                RaisePropertyChanged("SelectedCity");
            }
        }

        public Point CityPoint {
            get { return cityPoint; }
            set {
                if (cityPoint == value)
                    return;
                cityPoint = value;
                RaisePropertyChanged("CityPoint");
            }
        }
        public GeoPoint CenterPoint {
            get { return centerPoint; }
            set {
                if (centerPoint == value)
                    return;
                centerPoint = value;
                RaisePropertyChanged("CenterPoint");
            }
        }
        public double ZoomLevel {
            get { return zoomLevel; }
            set {
                if (zoomLevel == value)
                    return;
                zoomLevel = value;
                RaisePropertyChanged("ZoomLevel");
            }
        }

        public bool IsMapView { get { return this.ViewType == ViewType.Map; } }
        public bool IsGalleryView { get { return this.ViewType == ViewType.Gallery; } }
        public bool IsDetailView { get { return this.ViewType == ViewType.Detail; } }

        readonly MapControl map;

        LayerBase Layer { get { return map.Layers.Count > 0 ? map.Layers[0] : null; } }

        public CitiesViewModel(MapControl map, DataTemplate citySmallIconTemplate) {
            this.map = map;
            cities = new ObservableCollection<MapCustomElement>();
            citySmallIcons = new ObservableCollection<MapCustomElement>();
            LoadDataFromXML(citySmallIconTemplate);
        }
        void LoadDataFromXML(DataTemplate citySmallIconTemplate) {
            XDocument document = DataLoader.LoadFromXmlResource("MapDemo/Data/CitiesData.xml");
            if (document != null) {
                foreach (XElement element in document.Element("Cities").Elements()) {
                    string cityName = element.Element("CityName").Value;
                    GeoPoint cityLocation = new GeoPoint(Convert.ToDouble(element.Element("Latitude").Value, CultureInfo.InvariantCulture),
                        Convert.ToDouble(element.Element("Longitude").Value, CultureInfo.InvariantCulture));
                    CityInfo cityInfo = new CityInfo(cityName, cityLocation);
                    foreach (XElement placeElement in element.Element("Places").Elements()) {
                        string placeName = placeElement.Element("Name").Value;
                        GeoPoint placeLocation = new GeoPoint(Convert.ToDouble(placeElement.Element("Latitude").Value, CultureInfo.InvariantCulture),
                            Convert.ToDouble(placeElement.Element("Longitude").Value, CultureInfo.InvariantCulture));
                        string placeDescription = placeElement.Element("Description").Value;
                        Uri placeImageUri = new Uri(placeElement.Element("ImageUri").Value, UriKind.RelativeOrAbsolute);
                        BitmapImage image = new BitmapImage(placeImageUri);
                        cityInfo.Places.Add(new PlaceInfo(placeName, cityName, placeLocation, placeDescription, image));
                    }
                    CityInformationControl city = new CityInformationControl() { CityInfo = cityInfo };
                    Binding binding = new Binding() { Path = new PropertyPath("IsMapView"), Source = this };
                    city.SetBinding(CityInformationControl.VisibleProperty, binding);
                    MapCustomElement mapItem = new MapCustomElement() { Content = city, Location = cityLocation };
                    mapItem.PointerReleased += OnPointerReleased;
                    mapItem.PointerPressed += OnPointerPressed;
                    Cities.Add(mapItem);
                    CitySmallIcons.Add(new MapCustomElement() { Content = cityInfo, ContentTemplate = citySmallIconTemplate, Location = cityInfo.Location });
                }
            }
        }
        void UpdateViewType() {
            if (SelectedCity != null)
                ViewType = SelectedPlace != null ? ViewType.Detail : ViewType.Gallery;
            else
                ViewType = ViewType.Map;
        }
        void Update() {
            switch (ViewType) {
                case ViewType.Map:
                    ZoomLevel = 5;
                    break;
                case ViewType.Gallery:
                    ZoomLevel = 5;
                    CityPoint = Layer.GeoToScreenPoint(SelectedCity.Location);
                    break;
                case ViewType.Detail:
                    ZoomLevel = 17;
                    CenterPoint = SelectedPlace.Location;
                    break;
                default:
                    goto case ViewType.Map;
            }
        }
        void OnPointerReleased(object sender, PointerRoutedEventArgs e) {
            MapCustomElement element = sender as MapCustomElement;
            if (element != null) {
                SelectedPlace = null;
                SelectedCity = ((CityInformationControl)element.Content).CityInfo;
                UpdateViewType();
                e.Handled = true;
            }
        }
        void OnPointerPressed(object sender, PointerRoutedEventArgs e) {
            if (sender is MapCustomElement)
                e.Handled = true;
        }
        public void ShowNextSight() {
            if (SelectedCity != null && SelectedPlace != null) {
                int index = SelectedCity.Places.IndexOf(SelectedPlace) + 1;
                SelectedPlace = index < SelectedCity.Places.Count ? SelectedCity.Places[index] : SelectedCity.Places[0];
                CenterPoint = SelectedPlace.Location;
            }
        }
        public void ShowPrevSight() {
            if (SelectedCity != null && SelectedPlace != null) {
                int index = SelectedCity.Places.IndexOf(SelectedPlace) - 1;
                SelectedPlace = index < 0 ? SelectedCity.Places[SelectedCity.Places.Count - 1] : SelectedCity.Places[index];
                CenterPoint = SelectedPlace.Location;
            }
        }

        protected void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PlaceInfo {
        readonly string name;
        readonly string cityName;
        readonly GeoPoint location;
        readonly string description;
        readonly BitmapImage imageSource;

        public string Name { get { return name; } }
        public string CityName { get { return cityName; } }
        public GeoPoint Location { get { return location; } }
        public string Description { get { return description; } }
        public BitmapImage ImageSource { get { return imageSource; } }

        public PlaceInfo(string name, string cityName, GeoPoint location, string description, BitmapImage imageSource) {
            this.name = name;
            this.cityName = cityName;
            this.location = location;
            this.description = description;
            this.imageSource = imageSource;
        }
    }

    public class CityInfo {
        readonly string name;
        readonly GeoPoint location;
        readonly List<PlaceInfo> places = new List<PlaceInfo>();

        public string Name { get { return name; } }
        public GeoPoint Location { get { return location; } }
        public List<PlaceInfo> Places { get { return places; } }

        public CityInfo(string name, GeoPoint location) {
            this.name = name;
            this.location = location;
        }
    }

    public class PhotoGalleryResources {
        public BitmapImage CityInformationControlSource { get { return new BitmapImage(new Uri("ms-appx:///MapDemo/Data/Images/PhotoGallery/CityInformationControl.png", UriKind.RelativeOrAbsolute)); } }
        public BitmapImage LabelControlImageSource { get { return new BitmapImage(new Uri("ms-appx:///MapDemo/Data/Images/PhotoGallery/Label.png", UriKind.RelativeOrAbsolute)); } }
        public BitmapImage PlaceInfoControlPrevImageSource { get { return new BitmapImage(new Uri("ms-appx:///MapDemo/Data/Images/PhotoGallery/PrevPlace.png", UriKind.RelativeOrAbsolute)); } }
        public BitmapImage PlaceInfoControlNextImageSource { get { return new BitmapImage(new Uri("ms-appx:///MapDemo/Data/Images/PhotoGallery/NextPlace.png", UriKind.RelativeOrAbsolute)); } }

        public PhotoGalleryResources() {
        }
    }
}
