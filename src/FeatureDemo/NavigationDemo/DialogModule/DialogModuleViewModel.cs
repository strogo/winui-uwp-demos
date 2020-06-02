using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace NavigationDemo {
    public class DialogModuleViewModel : NavigationViewModelBase {
        public ICommand ShowAddNewUserMessageBoxCommand { get; private set; }
        public ICommand ShowEditMessageBoxCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ObservableCollection<UserViewModel> Users { get; set; }
        public UserViewModel SelectedUser { get; set; }
        public IDialogService DialogService { get { return ServiceContainer.GetService<IDialogService>(); } }
        public IMessageBoxService MessageBoxService { get { return ServiceContainer.GetService<IMessageBoxService>(); } }
        public DialogModuleViewModel() {
            ShowAddNewUserMessageBoxCommand = new DelegateCommand(ShowAddNewUserMessageBox);
            ShowEditMessageBoxCommand = new DelegateCommand(ShowEditMessageBox);
            RemoveCommand = new DelegateCommand(ShowRemoveMessageBox);
            Users = UserViewModel.CreateSampleUsers();
        }
        async void ShowRemoveMessageBox() {
            MessageResult dialogResults = await MessageBoxService.ShowAsync("Do you really want to delete this user?", "Delete", MessageButton.YesNo, MessageResult.Yes, MessageResult.No);
            if(dialogResults == MessageResult.Yes)
                Users.Remove(SelectedUser);
        }
        async void ShowEditMessageBox() {
            var viewModel = new UserViewModel(SelectedUser);
            MessageResult dialogResults = await DialogService.ShowDialogAsync(MessageButton.OKCancel, "Edit", "UseEditControl", viewModel);
            if(dialogResults == MessageResult.OK)
                SelectedUser.Update((UserViewModel)viewModel);
        }
        async void ShowAddNewUserMessageBox() {
            var viewModel = new UserViewModel() { Name = "User name", AllowRead = false, AllowWrite = false };
            MessageResult dialogResults = await DialogService.ShowDialogAsync(MessageButton.OKCancel, "Add a new user", "UseEditControl", viewModel);
            if(dialogResults == MessageResult.OK)
                Users.Add(viewModel);
        }
    }
}