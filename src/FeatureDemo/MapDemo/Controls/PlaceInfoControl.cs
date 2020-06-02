using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
namespace MapDemo {
    public class PlaceInfoControl : VisibleControl {
        ChangePlaceCommand changePlaceCommand;
        public static readonly DependencyProperty PlaceInfoProperty = DependencyProperty.Register("PlaceInfo",
            typeof(PlaceInfo), typeof(PlaceInfoControl), new PropertyMetadata(null, PlaceInfoPropertyChanged));

        static void PlaceInfoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            PlaceInfoControl placeInfoControl = d as PlaceInfoControl;
            if (placeInfoControl != null)
                placeInfoControl.TextScrollViewer.ChangeView(null, 0, null, true);
        }

        ScrollViewer textScrollViewer;

        public PlaceInfo PlaceInfo {
            get { return (PlaceInfo)GetValue(PlaceInfoProperty); }
            set { SetValue(PlaceInfoProperty, value); }
        }

        ScrollViewer TextScrollViewer { get { return textScrollViewer; } }
        public ChangePlaceCommand ChangePlaceCommand { get { return changePlaceCommand; } }

        public event RoutedEventHandler ShowNextPlace;
        public event RoutedEventHandler ShowPrevPlace;

        public PlaceInfoControl() {
            changePlaceCommand = new ChangePlaceCommand(this);
            DefaultStyleKey = typeof(PlaceInfoControl);
        }
        protected override void OnApplyTemplate() {
            base.OnApplyTemplate();
            ScrollViewer scrollViewer = this.GetTemplateChild("TestScrollViewer") as ScrollViewer;
            if (scrollViewer != null)
                textScrollViewer = scrollViewer;
        }
        public void OnShowNextPlace() {
            if (ShowNextPlace != null)
                ShowNextPlace(this, new RoutedEventArgs());
        }
        public void OnShowPreviousPlace() {
            if (ShowPrevPlace != null)
                ShowPrevPlace(this, new RoutedEventArgs());
        }
    }
    public class ChangePlaceCommand : ICommand {
        readonly PlaceInfoControl placeInfoControl;

        public ChangePlaceCommand(PlaceInfoControl placeInfoControl) {
            this.placeInfoControl = placeInfoControl;
        }

        public bool CanExecute(object parameter) {
            if (CanExecuteChanged != null)
                return placeInfoControl != null;
            return false;
        }
        public event System.EventHandler CanExecuteChanged;

        public void Execute(object parameter) {
            if (parameter == null)
                return;
            NavDirection navDirection = (NavDirection)parameter;
            switch (navDirection) {
                case NavDirection.Next: placeInfoControl.OnShowNextPlace();
                    break;
                case NavDirection.Previous:
                    placeInfoControl.OnShowPreviousPlace();
                    break;
            }
        }
    }

    public enum NavDirection { Next, Previous }

}