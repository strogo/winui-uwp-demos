using System.Collections.ObjectModel;

namespace NavigationDemo {
    public class UserViewModel : DevExpress.Mvvm.BindableBase {
        public static ObservableCollection<UserViewModel> CreateSampleUsers() {
            ObservableCollection<UserViewModel> users = new ObservableCollection<UserViewModel>();
            users.Add(new UserViewModel() { Name = "Anthony Peterson", AllowRead = false, AllowWrite = false });
            users.Add(new UserViewModel() { Name = "Cindy Haneline", AllowRead = true, AllowWrite = false });
            users.Add(new UserViewModel() { Name = "Albert Walker", AllowRead = false, AllowWrite = false });
            users.Add(new UserViewModel() { Name = "Rachel Scott", AllowRead = true, AllowWrite = true });
            users.Add(new UserViewModel() { Name = "Vernon Rounds", AllowRead = true, AllowWrite = true });
            users.Add(new UserViewModel() { Name = "Andrew Carter", AllowRead = true, AllowWrite = true });
            return users;
        }
        public UserViewModel() { }
        internal UserViewModel(UserViewModel other) {
            this.Name = other.Name;
            this.AllowRead = other.AllowRead;
            this.AllowWrite = other.AllowWrite;
        }
        internal void Update(UserViewModel user) {
            this.Name = user.Name;
            this.AllowRead = user.AllowRead;
            this.AllowWrite = user.AllowWrite;
        }
        string name;
        public string Name {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        bool allowRead;
        public bool AllowRead {
            get { return allowRead; }
            set { SetProperty(ref allowRead, value); }
        }
        bool allowWrite;
        public bool AllowWrite {
            get { return allowWrite; }
            set { SetProperty(ref allowWrite, value); }
        }
    }
}