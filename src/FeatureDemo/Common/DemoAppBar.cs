using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace FeatureDemo.Common {
    public class DemoAppBar : ContentControl {


        public int SelectedDemoModuleIndex {
            get { return (int)GetValue(SelectedDemoModuleIndexProperty); }
            set { SetValue(SelectedDemoModuleIndexProperty, value); }
        }


        public static readonly DependencyProperty SelectedDemoModuleIndexProperty =
            DependencyProperty.Register("SelectedDemoModuleIndex", typeof(int), typeof(DemoAppBar), new PropertyMetadata(0, new PropertyChangedCallback(OnSelectedDemoModuleIndexPropertyChanged)));

        private static void OnSelectedDemoModuleIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((DemoAppBar)d).OnSelectedDemoModuleIndexChanged((int)e.OldValue);
        }

        protected virtual void OnSelectedDemoModuleIndexChanged(int oldValue) {

        }




        const int Duration = 200;
        public ICommand ExpandButtonClickCommand {
            get { return (ICommand)GetValue(ExpandButtonClickCommandProperty); }
            set { SetValue(ExpandButtonClickCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExpandButtonClickCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExpandButtonClickCommandProperty =
            DependencyProperty.Register("ExpandButtonClickCommand", typeof(ICommand), typeof(DemoAppBar), new PropertyMetadata(null));

        
        public bool IsOpen {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(DemoAppBar), new PropertyMetadata(false, new PropertyChangedCallback(OnIsOpenPropertyChanged)));

        private static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((DemoAppBar)d).OnIsOpenChanged((bool)e.OldValue);
        }

        protected virtual void OnIsOpenChanged(bool oldValue) {
            VisualStateManager.GoToState(this, IsOpen ? "Opened" : "Closed", true);
            if(IsOpen) {
                Storyboard storyboard = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = ((TranslateTransform)RenderTransform).Y;
                doubleAnimation.To = 0;
                doubleAnimation.Duration = new Windows.UI.Xaml.Duration(TimeSpan.FromMilliseconds(Duration));
                Storyboard.SetTarget(doubleAnimation, RenderTransform);
                Storyboard.SetTargetProperty(doubleAnimation, "Y");
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
            }
            else {
                Storyboard storyboard = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = ((TranslateTransform)RenderTransform).Y;
                doubleAnimation.To = ActualHeight - MinHeight;
                doubleAnimation.Duration = new Windows.UI.Xaml.Duration(TimeSpan.FromMilliseconds(Duration));
                Storyboard.SetTarget(doubleAnimation, RenderTransform);
                Storyboard.SetTargetProperty(doubleAnimation, "Y");
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
            }
        }

        public DemoAppBar() {
            DefaultStyleKey = typeof(DemoAppBar);
            ExpandButtonClickCommand = new DevExpress.Mvvm.DelegateCommand(ExpandButtonClickCommandExecute);
            RenderTransform = new TranslateTransform();
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
            ((TranslateTransform)RenderTransform).Y = IsOpen ? 0 : e.NewSize.Height - MinHeight;
            VisualStateManager.GoToState(this, IsOpen ? "Opened" : "Closed", false);
        }

        void ExpandButtonClickCommandExecute() {
            IsOpen = !IsOpen;

        }
        protected override void OnApplyTemplate() {            
            base.OnApplyTemplate();
        }
    }
}