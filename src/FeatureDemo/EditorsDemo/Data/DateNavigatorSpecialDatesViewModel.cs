using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.UI.Xaml.Editors;
using DevExpress.UI.Xaml.Editors.Internal.Calendar;
using FeatureDemo.Data;

namespace EditorsDemo {
    public class DateNavigatorSpecialDatesViewModel : DevExpress.Mvvm.BindableBase {
        #region Props
        public CustomDatesCollection SpecialDates { get; private set; }
        public DateTime DisplayDate { get; private set; }
        public DateTime DisplayDateStart { get; private set; }
        public DateTime DisplayDateEnd { get; private set; }
        #endregion

        public DateNavigatorSpecialDatesViewModel() {
            SpecialDates = new CustomDatesCollection();
            SpecialDates.AddRange(DataStorage.Appointments.Select<Appointment, ICustomDateEvent>(e => new CustomDateEvent(e.Date, e)).ToList());
            IEnumerable<DateTime> dates = DataStorage.Appointments.Select(e => e.Date).OrderBy(e => e);
            DisplayDate = SpecialDates[0].Date;
            DisplayDateStart = dates.First().AddMonths(-2);
            DisplayDateEnd = dates.Last().AddMonths(2);
        }
    }
}