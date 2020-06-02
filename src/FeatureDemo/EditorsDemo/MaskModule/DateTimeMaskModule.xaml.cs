using DevExpress.Mvvm;
using DevExpress.UI.Xaml.Editors;
using FeatureDemo.Common;
using System.Windows.Input;

namespace EditorsDemo {
    public class DateTimeMaskViewModel : ViewModelBase {
        public TextEdit FocusedEditor { get { return GetProperty<TextEdit>(); } private set { SetProperty(value, OnFocusedEditorChanged); } }
        public TextInputMaskSettings TextInputSettings { get { return GetProperty<TextInputMaskSettings>(); } private set { SetProperty(value); } }
        public string Mask { get { return GetProperty<string>(); } set { SetProperty(value, OnMaskChanged); } }
        public ICommand<object> OnEditorGotFocusCommand { get; private set; }
        
        public DateTimeMaskViewModel() {
            OnEditorGotFocusCommand = new DelegateCommand<object>(OnEditorGotFocus);
        }

        void OnEditorGotFocus(object editor) {
            FocusedEditor = editor as TextEdit;
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
    public sealed partial class DateTimeMaskModule : DemoSubModuleView {
        public DateTimeMaskModule() {
            this.InitializeComponent();
        }
    }
}
