using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.UI.Xaml.Charts;
using DevExpress.UI.Xaml.Map;
using FeatureDemo.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Color = Windows.UI.Color;
using PropertyChangedEventHandler = Microsoft.UI.Xaml.Data.PropertyChangedEventHandler;
using INotifyPropertyChanged = Microsoft.UI.Xaml.Data.INotifyPropertyChanged;
using PropertyChangedEventArgs = Microsoft.UI.Xaml.Data.PropertyChangedEventArgs;

namespace ChartsDemo {
    public sealed partial class DashboardDemoModule : DemoModuleView {
        public DashboardDemoModule() {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }
        void OnLoaded(object sender, RoutedEventArgs e) {
            if(!vectorFileLayer.IsFileReadCompleted)
                vectorFileLayer.FileReadCompleted += OnShapeFileReadingCompleted;
            else SetMapItems();
        }
        void OnUnloaded(object sender, RoutedEventArgs e) {
            vectorFileLayer.FileReadCompleted -= OnShapeFileReadingCompleted;
            layoutRoot.DataContext = null;
        }
        void OnShapeFileReadingCompleted(object sender, EventArgs e) {
            SetMapItems();
        }
        void SetMapItems() {
            DashboardViewModel viewModel = (DashboardViewModel)Resources["viewModel"];
            viewModel.SetMapItems(vectorFileLayer.Items);
        }
    }

    public class DashboardViewModel : INotifyPropertyChanged {
        readonly Color[] palette = new Color[] { 
            Color.FromArgb(0xFF, 0x5F, 0x8B, 0x95), 
            Color.FromArgb(0xFF, 0x79, 0x96, 0x89),
            Color.FromArgb(0xFF, 0xA2, 0xA8, 0x75),
            Color.FromArgb(0xFF, 0xCE, 0xBB, 0x5F),
            Color.FromArgb(0xFF, 0xF2, 0xCB, 0x4E),
            Color.FromArgb(0xFF, 0xF1, 0xC1, 0x49),
            Color.FromArgb(0xFF, 0xE5, 0xA8, 0x4D),
            Color.FromArgb(0xFF, 0xD6, 0x86, 0x4E),
            Color.FromArgb(0xFF, 0xC5, 0x64, 0x50),
            Color.FromArgb(0xFF, 0xBA, 0x4D, 0x51),
            Color.FromArgb(0xFF, 0xAD, 0xAD, 0xAD),
            Color.FromArgb(0xFF, 0xE0, 0x99, 0x69)
        };
        readonly DevExpress.UI.Xaml.Map.ColorCollection countriesOnMapColors;
        readonly Palette outerDoughnutPalette;
        readonly Palette innerDoughnutPalette;
        readonly Palette urbanRuralDivisionDynamicPalette;
        readonly List<CountryStatisticInfo> countriesData = new List<CountryStatisticInfo>(12);
        
        CountryStatisticInfo selectedCountry;

        public Color[] DashboardPalette {
            get { return palette; }
        }
        public List<CountryStatisticInfo> Countries {
            get{ return countriesData; }
        }
        public List<CountryStatisticInfo> Top10LargestCountriesData {
            get { return countriesData.FindAll(country => country.Name != "Others" && country.Name != "Top 10"); }
        }
        public List<CountryStatisticInfo> Top10TogetherAndOthersCountriesData {
            get { return countriesData.FindAll(country => country.Name == "Others" || country.Name == "Top 10"); }
        }
        public CountryStatisticInfo SelectedCountry {
            get { return selectedCountry; }
            set {
                if(selectedCountry == value)
                    return;
                selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
            }
        }
        public Palette OuterDoughnutPalette {
            get { return outerDoughnutPalette; }
        }
        public Palette InnerDoughnutPalette {
            get { return innerDoughnutPalette; }
        }
        public Palette UrbanRuralDivisionDynamicPalette {
            get { return urbanRuralDivisionDynamicPalette; }
        }
        public DevExpress.UI.Xaml.Map.ColorCollection CountriesOnMapColors {
            get { return countriesOnMapColors; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DashboardViewModel() {
            countriesData = CountriesInfoDataReader.Load();
            this.outerDoughnutPalette = CreateOuterDoughnutPalette();
            this.innerDoughnutPalette = CreateInnerDoughnutPalette();
            this.urbanRuralDivisionDynamicPalette = CreateUrbanRuralDivisionDynamicPalette();
            this.countriesOnMapColors = CreateColorCollectionForColorizer();
        }

        void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler propertyChangedEventHendler = PropertyChanged;
            if(propertyChangedEventHendler != null)
                propertyChangedEventHendler(this, new PropertyChangedEventArgs(propertyName));
        }
        DevExpress.UI.Xaml.Map.ColorCollection CreateColorCollectionForColorizer() {
            DevExpress.UI.Xaml.Map.ColorCollection colors = new DevExpress.UI.Xaml.Map.ColorCollection();
            for(int i = 0; i < 11; i++)
                colors.Add(this.palette[i]);
            return colors;
        }
        Palette CreateOuterDoughnutPalette() {
            CustomPalette palette = new CustomPalette();
            for(int i = 0; i < 10; i++)
                palette.Colors.Add(this.palette[i]);
            return palette;
        }
        Palette CreateInnerDoughnutPalette() {
            CustomPalette palette = new CustomPalette();
            palette.Colors.Add(this.palette[10]);
            palette.Colors.Add(this.palette[11]);
            return palette;
        }
        Palette CreateUrbanRuralDivisionDynamicPalette() {
            CustomPalette palette = new CustomPalette();
            palette.Colors.Add(Color.FromArgb(0xFF, 0xC5, 0x64, 0x50));
            palette.Colors.Add(Color.FromArgb(0xFF, 0x5F, 0x8B, 0x95));
            return palette;
        }

        public void SetMapItems(LayerMapItemCollection layerMapItemCollection) {
            List<object> top10Shapes = new List<object>(10);
            foreach(MapItem item in layerMapItemCollection) {
                string shapeName = (string)item.Attributes["NAME"].Value;
                CountryStatisticInfo countryInfo = countriesData.FirstOrDefault(info => info.Name == shapeName);
                countryInfo.Shapes = new List<object> { item };
                if(countryInfo.Name != "Others")
                    top10Shapes.Add(item);
                if(countryInfo.Name == "Russia")
                    SelectedCountry = countryInfo;
            }
            CountryStatisticInfo top10 = countriesData.FirstOrDefault(info => info.Name == "Top 10");
            top10.Shapes = top10Shapes;
        }
    }

   public class CountryStatisticInfo {
       readonly string name;
       readonly  double area;
       readonly List<PopulationStatisticByYear> statistic;
       
       List<object> shapes;
       Brush brush;

       public string Name {
           get { return name; }
       }
       public double AreaMSqrKilometers {
           get { return area; }
       }
       public List<PopulationStatisticByYear> PopulationDynamic { 
           get { return statistic; }
       }
       public List<object> Shapes {
           get { return shapes; }
           set {
               if(value == null)
                   throw new ArgumentNullException("value");
               shapes = value;
           }
       }
       public Brush Brush {
           get { return brush; }
           set {
               if(value == null)
                   throw new ArgumentNullException("value");
               brush = value;
           }
       }

       public CountryStatisticInfo(string name, double areaMSqrKilometers, List<PopulationStatisticByYear> statistic) {
           this.name = name;
           this.area = areaMSqrKilometers;
           this.statistic = statistic;
       }
   }
   
    public class PopulationStatisticByYear {
        int year;
        double populationMillionsOfPeople;
        double urbanPercent;

        public int Year {
            get { return year; }
        }
        public double PopulationMillionsOfPeople {
            get { return populationMillionsOfPeople; }
        }
        public double UrbanPercent {
            get { return urbanPercent; }
        }
        public double RuralPercent {
            get { return 100 - urbanPercent; }
        }

        public PopulationStatisticByYear(int year, double populationMillionsOfPeople, double urbanPercent) {
            SetYear(year);
            SetPopulation(populationMillionsOfPeople);
            SetUrbanPercent(urbanPercent);
        }

        void SetYear(int value) {
            if (1949 < value && value < 2051)
                this.year = value;
            else
                throw new ArgumentException("Years earlier than 1950 and later than 2050 are not considered.");
        }
        void SetUrbanPercent(double value) {
            if(0 <= value && value <= 100)
                this.urbanPercent = value;
            else
                throw new ArgumentException("Urban percentage must be greater than or equal to zero and less than or equal to 100.");
        }
        void SetPopulation(double population) {
            if(0 < population && population < 10000)
                this.populationMillionsOfPeople = population;
            else
                throw new ArgumentException("Population must be greater than zero and less than 10 billion inhabitants");
        }
    }
}