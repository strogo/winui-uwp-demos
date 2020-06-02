using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RangeControlDemo;

namespace RangeControlDemo {
    public class DateTimeViewModel : RangeControlViewModel {
        DateTime? selectionStart;
        DateTime? selectionEnd;
        DateTime? visibleStart;
        DateTime? visibleEnd;
        readonly Random random = new Random();
        DateTime start = new DateTime(2014, 1, 1);
        public TimeSpan Step { get; set; }
        public DateTime VisibleStart {
            get { return visibleStart.HasValue ? visibleStart.Value : start + TimeSpan.FromTicks(((long)random.Next(Count / 5) + Count / 5) * Step.Ticks); }
            set { visibleStart = value; }
        }
        public DateTime VisibleEnd {
            get { return visibleEnd.HasValue ? visibleEnd.Value : VisibleStart + TimeSpan.FromTicks(((long)random.Next(Count / 5) + Count / 5) * Step.Ticks); }
            set { visibleEnd = value; }
        }
        public DateTime SelectionStart {
            get { return selectionStart.HasValue ? selectionStart.Value : start + TimeSpan.FromTicks(((long)random.Next(Count / 5) + Count / 5) * Step.Ticks); }
            set { selectionStart = value; }
        }
        public DateTime SelectionEnd {
            get { return selectionEnd.HasValue ? selectionEnd.Value : SelectionStart + TimeSpan.FromTicks(((long)random.Next(Count / 5) + Count / 5) * Step.Ticks); }
            set { selectionEnd = value; }
        }
        protected override IEnumerable CreateItemsSource(int count) {
            var points = new List<DateTimeDataPoint>();

            double value = GenerateStartValue(random);
            points.Add(new DateTimeDataPoint() { Value = start, DisplayValue = value });
            for (int i = 1; i < count; i++) {
                value += GenerateAddition(random);
                start = start + Step;
                points.Add(new DateTimeDataPoint() { Value = start, DisplayValue = value });
            }
            return points;
        }
    }
}
