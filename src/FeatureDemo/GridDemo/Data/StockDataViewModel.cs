using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.UI.Xaml.Grid;
using FeatureDemo.Data;
using Microsoft.UI.Xaml;

namespace GridDemo {
    public class StockDataViewModel {
        List<StockItem> data = new StockItemsData().DataSource.ToList();
        DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        Random rnd = new Random();
        GridControl grid;

        double[] positivePriceHistory = new double[StockItem.HistoryLength];
        double[] negativePriceHistory = new double[StockItem.HistoryLength];

        public StockDataViewModel(GridControl grid) {
            this.grid = grid;
            for(int i = 0; i < StockItem.HistoryLength; i++)
                UpdateItems();
            timer.Tick += OnTimerRaized;
        }
        public void Unload() {
            timer.Tick -= OnTimerRaized;
        }
        void OnTimerRaized(object sender, object e) {
            UpdateItems();
        }
        void UpdateItems() {
            StepLeftCustomSummaryInfo();
            grid.BeginDataUpdate();
            foreach(StockItem item in data) {
                item.UpdatePrice(rnd);
                if(Math.Sign(item.DeltaPrice) > 0) positivePriceHistory[StockItem.HistoryLength - 1]++; else negativePriceHistory[StockItem.HistoryLength - 1]++;
            }
            grid.EndDataUpdate();
        }

        void StepLeftCustomSummaryInfo() {
            for(int i = 1; i < StockItem.HistoryLength; i++) {
                positivePriceHistory[i - 1] = positivePriceHistory[i];
                negativePriceHistory[i - 1] = negativePriceHistory[i];
            }
            positivePriceHistory[StockItem.HistoryLength - 1] = 0;
            negativePriceHistory[StockItem.HistoryLength - 1] = 0;
        }

        public void Pause() {
            if(timer.IsEnabled)
                timer.Stop();
        }
        public void Resume() {
            if(!timer.IsEnabled)
                timer.Start();
        }

        public List<StockItem> Data { get { return data; } }
        public double[] PositivePriceCountList { get { return positivePriceHistory; } }
        public double[] NegativePriceHistoryList { get { return negativePriceHistory; } }
    }

}
