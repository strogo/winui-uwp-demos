using System.Collections.Generic;
using System.Globalization;
using DevExpress.UI.Xaml.Editors;

namespace EditorsDemo {
    public class DateNavigatorViewModel : DevExpress.Mvvm.BindableBase {
        #region Consts
        const string predefinedCultures = "en-US,de-DE,fr-FR,ru-RU,ja-JP,zh-CN";
        #endregion

        #region Props
        CultureInfo selectedCulture = CultureInfo.CurrentUICulture;

        public IList<CultureInfo> CalendarCultures { get; private set; }

        public CultureInfo SelectedCulture {
            get { return selectedCulture; }
            set { SetProperty(ref selectedCulture, value); }
        }

        public CalendarSelectionMode SelectionMode {
            get { return GetProperty<CalendarSelectionMode>(); }
            set { SetProperty(value); }
        }
        #endregion

        public DateNavigatorViewModel() {
            InitializeCultures();

            SelectionMode = CalendarSelectionMode.Single;
        }

        void InitializeCultures() {
            CalendarCultures = new List<CultureInfo>();
            CalendarCultures.Insert(0, SelectedCulture);
            int count = 1;
            foreach(string cultureName in predefinedCultures.Split(",".ToCharArray())) {
                if(cultureName.Equals(SelectedCulture.Name))
                    continue;

                CalendarCultures.Insert(count++, new CultureInfo(cultureName));
            }
        }
    }
}
