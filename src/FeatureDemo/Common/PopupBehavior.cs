using DevExpress.Mvvm.UI.Interactivity;
using System.Diagnostics;
using Windows.UI.Xaml;
using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using DevExpress.UI.Xaml.Charts;

namespace FeatureDemo.Common {
    public class PopupBehavior : Behavior<FrameworkElement> {
        long visibilityToken;
        bool inputPaneVisible = false;
        protected override void OnAttached() {
            base.OnAttached();
            visibilityToken = AssociatedObject.RegisterPropertyChangedCallback(UIElement.VisibilityProperty, VisibilityChanged);
        }

        void VisibilityChanged(DependencyObject sender, DependencyProperty dp) {
            var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
            if(AssociatedObject.Visibility == Visibility.Collapsed) {
                if(inputPaneVisible) {
                    InputPane.GetForCurrentView().TryShow();
                    inputPaneVisible = false;
                }
                foreach(var popup in popups) {
                    if(popup.IsOpen && IsPopupSupported(popup.Child) && popup.IsHitTestVisible == false && popup.Opacity == 0) {
                        popup.IsHitTestVisible = true;
                        popup.Opacity = 1;
                    }
                }
            } else {
                var inputPane = InputPane.GetForCurrentView();
                if(inputPane.OccludedRect.Height > 0)
                    inputPaneVisible = inputPane.TryHide();
                foreach(var popup in popups) {
                    if(popup.IsOpen && IsPopupSupported(popup.Child)) {
                        popup.IsHitTestVisible = false;
                        popup.Opacity = 0;
                    }
                }
            }
        }

        private bool IsPopupSupported(UIElement child) {
            return child is Canvas || child is ToolTipContentPresenter;
        }

        protected override void OnDetaching() {
            base.OnDetaching();
            AssociatedObject.UnregisterPropertyChangedCallback(UIElement.VisibilityProperty, visibilityToken);
        }
    }
}