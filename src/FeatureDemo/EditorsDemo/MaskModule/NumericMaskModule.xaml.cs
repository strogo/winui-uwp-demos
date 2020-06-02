using DevExpress.Mvvm;
using DevExpress.UI.Xaml.Editors;
using FeatureDemo.Common;
using System;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace EditorsDemo {
    public class NumericMaskViewModel : ViewModelBase {
        public TextEdit FocusedEditor { get { return GetProperty<TextEdit>(); } private set { SetProperty(value, OnFocusedEditorChanged); } }
        public TextInputMaskSettings TextInputSettings { get { return GetProperty<TextInputMaskSettings>(); } private set { SetProperty(value); } }
        public string Mask { get { return GetProperty<string>(); } set { SetProperty(value, OnMaskChanged); } }
        public double NegativeAlternativeValue { get { return GetProperty<double>(); } set { SetProperty(value); } }
        public ICommand OnLoadedCommand { get; private set; }
        public ICommand OnUnloadedCommand { get; private set; }
        public ICommand<object> OnEditorGotFocusCommand { get; private set; }
        
        DispatcherTimer timer;
        public NumericMaskViewModel() {
            OnEditorGotFocusCommand = new DelegateCommand<object>(OnEditorGotFocus);
            OnLoadedCommand = new DelegateCommand(OnLoaded);
            OnUnloadedCommand = new DelegateCommand(OnUnloaded);
            timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 2, 0) };
            NegativeAlternativeValue = 399.9;
        }
       
        void OnLoaded() {
            timer.Tick += OnTimerTick;
            timer.Start();
        }
        void OnUnloaded() {
            timer.Stop();
            timer.Tick -= OnTimerTick;
        }
        void OnEditorGotFocus(object editor) {
            FocusedEditor = editor as TextEdit;
        }
        void OnTimerTick(object sender, object e) {
            NegativeAlternativeValue = -NegativeAlternativeValue;
        }
        void OnFocusedEditorChanged() {
            TextInputSettings = FocusedEditor?.TextInputSettings as TextInputMaskSettings;
            Mask = TextInputSettings?.Mask;
        }
        async void OnMaskChanged() {
            if (TextInputSettings == null || Mask == TextInputSettings.Mask)
                return;
            string maskBackup = TextInputSettings.Mask;
            try {
                TextInputSettings.Mask = Mask;
            } catch {
                await GetService<IMessageBoxService>().ShowAsync("Invalid mask", "Error");
                TextInputSettings.Mask = maskBackup;
                Mask = maskBackup;
            }
        }
    }
    public sealed partial class NumericMaskModule : DemoSubModuleView {
        public NumericMaskModule() {
            InitializeComponent();
        }
    }
}
