using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace MapDemo {
    public class VisibleControl : Control {
        public static readonly DependencyProperty VisibleProperty = DependencyProperty.Register("Visible",
            typeof(bool), typeof(VisibleControl), new PropertyMetadata(true, new PropertyChangedCallback(VisiblePropertyChanged)));

        static void VisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            VisibleControl control = d as VisibleControl;
            if (control != null && !e.NewValue.Equals(e.OldValue)) {
                control.ChangeVisualState();
                control.VisibleChanged();
            }
        }

        public bool Visible {
            get { return (bool)GetValue(VisibleProperty); }
            set { SetValue(VisibleProperty, value); }
        }

        bool isPressedField = false;
        bool isMouseOverField = false;
        bool isFocusedField = false;

        public event RoutedEventHandler Click;

        void ChangeVisualState() {
            ChangeVisualState(true);
        }
        void ChangeVisualState(bool useTransitions) {
            if (!Visible)
                VisualStateManager.GoToState(this, "Invisible", useTransitions);
            else {
                VisualStateManager.GoToState(this, "Visible", useTransitions);
                if (isPressedField)
                    VisualStateManager.GoToState(this, "Pressed", useTransitions);
                else {
                    if (isMouseOverField)
                        VisualStateManager.GoToState(this, "MouseOver", useTransitions);
                    else
                        VisualStateManager.GoToState(this, "Normal", useTransitions);
                }
            }
            if (isFocusedField && Visible) {
                VisualStateManager.GoToState(this, "Focused", useTransitions);
                return;
            }
            VisualStateManager.GoToState(this, "Unfocused", useTransitions);
        }
        void OnClick() {
            if (Click != null)
                Click(this, new RoutedEventArgs());
        }
        protected override void OnPointerEntered(PointerRoutedEventArgs e) {
            base.OnPointerEntered(e);
            isMouseOverField = true;
            ChangeVisualState();
        }
        protected override void OnPointerExited(PointerRoutedEventArgs e) {
            base.OnPointerExited(e);
            isMouseOverField = false;
            isPressedField = false;
            ChangeVisualState();
        }
        protected override void OnGotFocus(RoutedEventArgs e) {
            base.OnGotFocus(e);
            isFocusedField = true;
            ChangeVisualState();
        }
        protected override void OnLostFocus(RoutedEventArgs e) {
            base.OnLostFocus(e);
            isFocusedField = false;
            ChangeVisualState();
        }
        protected override void OnPointerPressed(PointerRoutedEventArgs e) {
            base.OnPointerPressed(e);
            isPressedField = true;
            ChangeVisualState();
            OnClick();
        }
        protected override void OnPointerReleased(PointerRoutedEventArgs e) {
            base.OnPointerReleased(e);
            if (isPressedField) {
                isPressedField = false;
                ChangeVisualState();
            }
        }
        protected override void OnApplyTemplate() {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }
        protected virtual void VisibleChanged() {
        }
    }
}