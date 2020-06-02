using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace NavigationDemo {
    public class MessageDialogModuleViewModel : ViewModelBase {
        public ICommand ShowMessageBoxCommand { get; private set; }        
        string output;
        public string Output {
            get { return output; }
            set { SetProperty(ref output, value); }
        }
        public string Message { get; set; }
        public string Title { get; set; }
        public string SelectedMessageDialogButtons { get; set; }
        public List<string> MessageDialogButtons { get; set; }
        public MessageDialogModuleViewModel() {            
            ShowMessageBoxCommand = new DelegateCommand(ShowMessageBox);
            MessageDialogButtons = Enum.GetNames(typeof(MessageButton)).Where(x => !x.Contains("AbortRetryIgnore") && !x.Contains("YesNoCancel")).ToList();
            SelectedMessageDialogButtons = MessageDialogButtons[1];
            Title = "Title";
            Message = "Message";
            Output = string.Empty;
        }
        async void ShowMessageBox() {
            IMessageBoxService dialogService = ServiceContainer.GetService<IMessageBoxService>();
            var buttons = (MessageButton)Enum.Parse(typeof(MessageButton), SelectedMessageDialogButtons);
            MessageResult result = await dialogService.ShowAsync(Message, Title, buttons);
            Output = result.ToString();
        }
    }
}