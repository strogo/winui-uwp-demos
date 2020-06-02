using System;
using DevExpress.UI.Xaml.Map;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using FeatureDemo.Common;

namespace MapDemo {
    public sealed partial class ShapefileSupportModule : DemoModuleView {
        #region static
        public static readonly DependencyProperty IsGDPCheckedProperty;
        public static readonly DependencyProperty IsPopulationCheckedProperty;
        public static readonly DependencyProperty IsPoliticalCheckedProperty;
        public static readonly DependencyProperty ActiveColorizerProperty;
        public static readonly DependencyProperty ToolTipPatternProperty;
        static ShapefileSupportModule() {
            IsGDPCheckedProperty = DependencyProperty.Register("IsGDPChecked", typeof(bool), typeof(ShapefileSupportModule), new PropertyMetadata(false, new PropertyChangedCallback(OnIsGDPCheckedPropertyChanged)));
            IsPopulationCheckedProperty = DependencyProperty.Register("IsPopulationChecked", typeof(bool), typeof(ShapefileSupportModule), new PropertyMetadata(false, new PropertyChangedCallback(OnIsPopulationCheckedPropertyChanged)));
            IsPoliticalCheckedProperty = DependencyProperty.Register("IsPoliticalChecked", typeof(bool), typeof(ShapefileSupportModule), new PropertyMetadata(false, new PropertyChangedCallback(OnIsPoliticalCheckedPropertyChanged)));
            ActiveColorizerProperty = DependencyProperty.Register("ActiveColorizer", typeof(MapColorizer), typeof(ShapefileSupportModule), new PropertyMetadata(null));
            ToolTipPatternProperty = DependencyProperty.Register("ToolTipPattern", typeof(string), typeof(ShapefileSupportModule), new PropertyMetadata(null));
        }
        private static void OnIsGDPCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ShapefileSupportModule)d).OnIsGDPCheckedChanged((bool)e.OldValue);
        }
        private static void OnIsPopulationCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ShapefileSupportModule)d).OnIsPopulationCheckedChanged((bool)e.OldValue);
        }
        private static void OnIsPoliticalCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ShapefileSupportModule)d).OnIsPoliticalCheckedChanged((bool)e.OldValue);
        }
        #endregion
        #region dep props
        public bool IsGDPChecked {
            get { return (bool)GetValue(IsGDPCheckedProperty); }
            set { SetValue(IsGDPCheckedProperty, value); }
        }
        public bool IsPopulationChecked {
            get { return (bool)GetValue(IsPopulationCheckedProperty); }
            set { SetValue(IsPopulationCheckedProperty, value); }
        }
        public bool IsPoliticalChecked {
            get { return (bool)GetValue(IsPoliticalCheckedProperty); }
            set { SetValue(IsPoliticalCheckedProperty, value); }
        }
        public MapColorizer ActiveColorizer {
            get { return (MapColorizer)GetValue(ActiveColorizerProperty); }
            set { SetValue(ActiveColorizerProperty, value); }
        }
        public string ToolTipPattern {
            get { return (string)GetValue(ToolTipPatternProperty); }
            set { SetValue(ToolTipPatternProperty, value); }
        }

        #endregion


        readonly MapColorizer populationColorizer;
        readonly MapColorizer gdpColorizer;
        readonly MapColorizer politicalColorizer;
        
        public ShapefileSupportModule() {
            this.InitializeComponent();
            this.DataContext = this;
            this.gdpColorizer = Resources["gdpColorizer"] as MapColorizer;
            this.populationColorizer = Resources["populationColorizer"] as MapColorizer;
            this.politicalColorizer = Resources["politicalColorizer"] as MapColorizer;

            IsGDPChecked = true;
        }
        private void OnIsGDPCheckedChanged(bool oldValue) {
            if(IsGDPChecked) {
                ActiveColorizer = gdpColorizer;
                ToolTipPattern = "{NAME}: ${GDP_MD_EST:0,0}M";
            }
        }

        private void OnIsPopulationCheckedChanged(bool oldValue) {
            if(IsPopulationChecked) {
                ActiveColorizer = populationColorizer;
                ToolTipPattern = "{NAME}: {POP_EST:0,0}";
            }
        }
        private void OnIsPoliticalCheckedChanged(bool oldValue) {
            if(IsPoliticalChecked) {
                ActiveColorizer = politicalColorizer;
                ToolTipPattern = "{NAME}";
            }
        }
    }
}
