using FeatureDemo.Data;
using DevExpress.Data;
using DevExpress.UI.Xaml.Grid;
using DevExpress.Core.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Path = Windows.UI.Xaml.Shapes.Path;

namespace GridDemo {
    public sealed partial class GridRealTimeDataModule : GridDemoUserControl {        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(StockDataViewModel), typeof(GridRealTimeDataModule), new PropertyMetadata(null));

        
        static GridRealTimeDataModule() {

            Localizer.AddString(GridStringID.DefaultFixedTotalSummaryFormatString_Count, "COUNT");
            Localizer.AddString(GridStringID.DefaultFixedTotalSummaryFormatString_Max, "MAX OF {0}");
        }
        #region dep props
        public StockDataViewModel Model {
            get { return (StockDataViewModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
        #endregion
        public GridRealTimeDataModule() {
            this.InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            //LayoutUpdated += GridRealTimeDataModule_LayoutUpdated;
            //Application.Current.F
        }
        void OnLoaded(object sender, RoutedEventArgs e) {
            Model = new StockDataViewModel(grid);            
            Model.Resume();
            Application.Current.DebugSettings.EnableFrameRateCounter = false;
            grid.CustomSummary += grid_CustomSummary;
        }
        void OnUnloaded(object sender, RoutedEventArgs e) {
            grid.CustomSummary -= grid_CustomSummary;
            Model.Unload();
            Model = null;
        }

        //void GridRealTimeDataModule_LayoutUpdated(object sender, object e) {
        //}
        //protected override Size MeasureOverride(Size availableSize) {
        //    return base.MeasureOverride(availableSize);
        //}

        protected internal override GridControl Grid { get { return grid; } }
        public override void OnNavigatingFrom() {
            //model.Pause();
            base.OnNavigatingFrom();
        }
        public override void OnNavigatedTo() {
            base.OnNavigatedTo();
            //model.Resume();
        }

        void grid_CustomSummary(object sender, CustomSummaryEventArgs e) {
            if(e.SummaryProcess != CustomSummaryProcess.Finalize || Model == null)
                return;
            e.TotalValue = new RealLiveDataCustomSummary() { Negative = Model.NegativePriceHistoryList.ToList(), Positive = Model.PositivePriceCountList.ToList() };
        }
    }

    public class RealLiveDataCustomSummary {
        public List<double> Positive { get; set; }
        public List<double> Negative { get; set; }
    }

}
