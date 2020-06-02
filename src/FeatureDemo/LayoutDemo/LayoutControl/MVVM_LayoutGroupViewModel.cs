using System;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Media;

namespace LayoutDemo.LayoutControl {
    public sealed class Contact : DevExpress.Mvvm.BindableBase {
        public Contact() {
            var employee = FeatureDemo.Data.DataStorage.Employees[new Random().Next(0, FeatureDemo.Data.DataStorage.Employees.Count)];
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Photo = employee.ImageSource;
            PhoneNumbers = new ObservableCollection<PhoneNumber>{
                new PhoneNumber(){ Type="Work", Value=employee.Phone, IsRequired=true},
                new PhoneNumber(){ Type="Home"},
                new PhoneNumber(){ Type="Additional"},
            };
            Emails = new ObservableCollection<Email>{
                new Email(){ Type="Work", Value=employee.EmailAddress, IsRequired=true},
                new Email(){ Type="Home"},
            };
        }
        string firstNameCore;
        public string FirstName {
            get { return firstNameCore; }
            set { SetProperty(ref firstNameCore, value, (s1,s2) => RaisePropertiesChanged("FullName")); }
        }
        string lastNameCore;
        public string LastName {
            get { return lastNameCore; }
            set { SetProperty(ref lastNameCore, value, (s1, s2) => RaisePropertiesChanged("FullName")); }
        }
        ImageSource photoCore;
        public ImageSource Photo {
            get { return photoCore; }
            set { SetProperty(ref photoCore, value); }
        }
        public string FullName {
            get { return string.Join(", ", LastName, FirstName); }
        }
        public ObservableCollection<PhoneNumber> PhoneNumbers { get; private set; }
        public ObservableCollection<Email> Emails { get; private set; }
    }
    public sealed class PhoneNumber : DevExpress.Mvvm.BindableBase {
        public bool IsRequired { get; internal set; }
        string typeCore;
        public string Type {
            get { return typeCore; }
            set { SetProperty(ref typeCore, value); }
        }
        string valueCore;
        public string Value {
            get { return valueCore; }
            set { SetProperty(ref valueCore, value); }
        }
    }
    public sealed class Email : DevExpress.Mvvm.BindableBase {
        public bool IsRequired { get; internal set; }
        string typeCore;
        public string Type {
            get { return typeCore; }
            set { SetProperty(ref typeCore, value); }
        }
        string valueCore;
        public string Value {
            get { return valueCore; }
            set { SetProperty(ref valueCore, value); }
        }
    }
}