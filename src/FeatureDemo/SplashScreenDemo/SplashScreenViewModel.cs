using DevExpress.Mvvm;
using System.Windows.Input;
using System;
using System.Threading.Tasks;
using DevExpress.Mvvm.UI;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace SplashScreenDemo {
    public class SplashScreenViewModel : ViewModelBase {
        public ICommand ShowSplashScreenCommand { get; }
        public ICommand CancelSplashScreenCommand { get; }
        
        public bool IsProgressRingScenarioChecked {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        public bool IsProgressBarScenarioChecked {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        public bool IsCustomTemplateScenarioChecked {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        public bool IsCancelableSplashScreenScenarioChecked {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        ISplashScreenService CurrentSplashScreen { get; set; }
        public SplashScreenViewModel() {
            ShowSplashScreenCommand = new DelegateCommand(ShowSplashScreenCommandExecute);
            CancelSplashScreenCommand = new DelegateCommand(CancelSplashScreenExecute);
            IsProgressRingScenarioChecked = true;
        }
        protected void CancelSplashScreenExecute() {
            CurrentSplashScreen.HideSplashScreen();
        }
        protected void ShowSplashScreenCommandExecute() {
            if(IsProgressRingScenarioChecked)
                ShowSplashScreen(GetService<ISplashScreenService>());
            else if(IsProgressBarScenarioChecked)
                ShowSplashScreen(GetService<ISplashScreenService>("ProgressSplashScreen"));
            else if(IsCancelableSplashScreenScenarioChecked)
                ShowSplashScreen(GetService<ISplashScreenService>("CancelableSplashScreen"), true);
            else
                ShowSplashScreen(GetService<ISplashScreenService>("CustomSplashScreen"));
        }
        async void ShowSplashScreen(ISplashScreenService splashScreenService, bool infinit = false) {
            CurrentSplashScreen = splashScreenService;
            splashScreenService.ShowSplashScreen();
            if(infinit)
                return;
            for(int i = 0; i <= 100; i++) {
                switch(i) {
                    case 0:
                        splashScreenService.SetSplashScreenState("Starting...");
                        break;
                    case 50:
                        splashScreenService.SetSplashScreenState("Loading data...");
                        break;
                    case 80:
                        splashScreenService.SetSplashScreenState("Finishing...");
                        break;
                }
                splashScreenService.SetSplashScreenProgress(i, 100);
                await Task.Delay(50);
            }           
            splashScreenService.HideSplashScreen();
        }
    }
}