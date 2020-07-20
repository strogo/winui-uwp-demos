using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DevExpress.UI.Xaml.Controls.TextColorizer;
using FeatureDemo.Common;

namespace LayoutDemo {
    public class DataLayoutControlViewModel : DevExpress.Mvvm.BindableBase {
        public List<object> Items { get; private set; }
        public DataLayoutControlViewModel() {
            Data1 data1 = new Data1
            {
                IntProperty = 123,
                NullableIntProperty = 4567,
                DoubleProperty = 12345.12345,
                NullableDoubleProperty = 6789.6789,
                BoolProperty = true,
                CharProperty = 'Y',
                NullableCharProperty = 'N',
                NullableEnumProperty = null,
                StringProperty = "text",
                DateTimeProperty = DateTime.Today,
                NullableDateTimeProperty = DateTime.Today.AddDays(1),
                DecimalProperty = 12345.12345m,
                NullableDecimalProperty = 789.789m,
                //ComplexTypeProperty = new Point(123.45, 678.9),
                CurrencyProperty = 1234567.89m,
                MultilineTextProperty = "first line of text\r\nsecond line of text\r\nthird line of text",
                PasswordProperty = "password",
                PhoneNumberProperty = "1234567890"
            };
            Data2 data2 = new Data2
            {
                ID = 123456,
                FirstName = "Anita",
                LastName = "Benson",
                Age = 27,
                Gender = Gender.Female,
                Employer = "DevExpress Inc",
                SSN = "123-45-6789"
            };
            Data3 data3 = new Data3
            {
                FirstName = "Anita",
                LastName = "Benson",
                Group = "Inventory Management",
                Title = "Purchasing Manager",
                HireDate = new DateTime(2002, 2, 2),
                Salary = 65000m,
                Phone = "7138638137",
                Email = "Anita_Benson@example.com",
                AddressLine1 = "9602 South Main",
                AddressLine2 = "Moscow, 77025, Russia",
                Gender = Gender.Female,
                BirthDate = new DateTime(1985, 3, 18)
            };
            Data4 data4 = new Data4
            {
                FirstName = "Anita",
                LastName = "Benson",
                Group = "Inventory Management",
                Title = "Purchasing Manager",
                HireDate = new DateTime(2002, 2, 2),
                Salary = 65000m,
                Phone = "7138638137",
                Email = "Anita_Benson@example.com",
                AddressLine1 = "9602 South Main",
                AddressLine2 = "Moscow, 77025, Russia",
                Gender = Gender.Female,
                BirthDate = new DateTime(1985, 3, 18)
            };
            Items = new List<object>() { new DataInfo(data1), new DataInfo(data2), new DataInfo(data3)/*, new DataInfo(data4)*/ };
            SelectedDataItem = Items[0];
        }       
        private object _SelectedDataItem;
        public object SelectedDataItem {
            get { return _SelectedDataItem; }
            set {
                SetProperty(ref _SelectedDataItem, value);
            }
        }
        class DataInfo : DevExpress.Mvvm.BindableBase {
            private CodeLanguage _CodeLanguage;
            private object _Data;
            private Func<string> _GetCodeTextFunc;

            public DataInfo(object data) {
                Data = data;
            }
            public string DisplayName {
                get { return Data != null ? Data.ToString() : null; }
            }
            public Func<string> GetCodeTextFunc {
                get { return _GetCodeTextFunc; }
                private set {
                    SetProperty(ref _GetCodeTextFunc, value);
                }
            }
            public object Data {
                get { return _Data; }
                private set {
                    SetProperty(ref _Data, value, OnDataChanged);
                }
            }
            public CodeLanguage CodeLanguage {
                get { return _CodeLanguage; }
                private set {
                    SetProperty(ref _CodeLanguage, value);
                }
            }
            void OnDataChanged(object oldValue, object newValue) {
                GetCodeTextFunc = () => ReadCode(newValue.GetType());
                CodeLanguage = CodeLanguage.CS;
            }
            string ReadCode(Type newValue) {
                Stream s = AssemblyHelper.GetEmbeddedResourceStream(newValue.GetTypeInfo().Assembly, newValue.Name + ".cs", false);
                string str = null;
                if(s != null) {
                    using(StreamReader reader = new StreamReader(s)) {
                        str = reader.ReadToEnd();
                    }
                }
                return str;
            }
        }
    }
    public enum Gender { Male, Female }
}