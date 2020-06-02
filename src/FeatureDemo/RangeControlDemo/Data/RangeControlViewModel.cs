using System;
using System.Collections;

namespace RangeControlDemo {
    public abstract class RangeControlViewModel {
        public int Count { get; set; }
        public double Start { get; set; }
        IEnumerable itemsSource;
        public IEnumerable ItemsSource { get { return itemsSource ?? (itemsSource = CreateItemsSource(Count)); } }
        protected abstract IEnumerable CreateItemsSource(int count);
        protected double GenerateStartValue(Random random) {
            return Start + random.NextDouble() * 100;
        }
        protected double GenerateAddition(Random random) {
            double factor = random.NextDouble();
            if (factor == 1)
                factor = 50;
            else if (factor == 0)
                factor = -50;
            return (factor - 0.5) * 50;
        }


    }
    public class NumericDataPoint {
        public int Value { get; set; }
        public double DisplayValue { get; set; }
    }
    public class DateTimeDataPoint {
        public DateTime Value { get; set; }
        public double DisplayValue { get; set; }
    }
}