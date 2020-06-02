using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.UI.Xaml.Charts;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using FeatureDemo.Common;
using DevExpress.Core.Native;

namespace ChartsDemo {
    public sealed partial class RealTimeDataModule : DemoModuleView {
        #region static
        public static readonly DependencyProperty DataProperty;
        static RealTimeDataModule() {
            DataProperty = DependencyProperty.Register("Data", typeof(SinDataGenerator), typeof(RealTimeDataModule), new PropertyMetadata(null));
        }
        #endregion
        #region dep props
        public SinDataGenerator Data {
            get { return (SinDataGenerator)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        #endregion

        DispatcherTimer timer;

        public RealTimeDataModule() {            
            this.InitializeComponent();
            Data = new SinDataGenerator();
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 1000/60) };
            timer.Tick += OnTimerTick;            
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            MinMargin = WinUICompatibility.CreateThickness(5);
        }

        void OnLoaded(object sender, RoutedEventArgs e) {
            DataContext = this;
            timer.Start();
        }

        void OnUnloaded(object sender, RoutedEventArgs e) {
            DataContext = null;
            timer.Stop();
        }

        void OnTimerTick(object sender, object e) {
            Data.Refresh();
        }
    }

    public class SinDataGenerator : ChartDataAdapter {
        const int PointsCount = 500;
        const double Divider = 250;
        const int NewPointsCount = 1;

        double count = 0;
        Random random = new Random();
        List<Point> points = new List<Point>();

        protected override int RowsCount { get { return points.Count; } }

        public SinDataGenerator() {
            for (int i = 0; i < PointsCount; i++) {
                points.Add(new Point(i, GetValue(count)));
                IncreaseCount();
            }
        }
        void IncreaseCount() {
            count++;
        }
        double GetValue(double count) {
            return (Math.Sin((count / Divider) * 2.0 * Math.PI) + random.NextDouble() - 0.5) * 33;
        }
        protected override DateTime GetDateTimeValue(int index, ChartDataMemberType dataMember) {
            return DateTime.MinValue;
        }
        protected override object GetKey(int index) {
            return null;
        }
        protected override double GetNumericalValue(int index, ChartDataMemberType dataMember) {
            if (dataMember == ChartDataMemberType.Argument)
                return points[index].X;
            return points[index].Y;
        }
        protected override string GetQualitativeValue(int index, ChartDataMemberType dataMember) {
            return null;
        }
        protected override ActualScaleType GetScaleType(ChartDataMemberType dataMember) {
            return ActualScaleType.Numerical;
        }
        public void Refresh() {
            for (int i = 0; i < NewPointsCount; i++) {
                points.RemoveAt(0);
                points.Add(new Point(count, GetValue(count)));
                IncreaseCount();
            }
            OnDataChanged(ChartDataUpdateType.Reset, -1);  
        }
    }
}
