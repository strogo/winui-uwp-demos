using DevExpress.Core.Native;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace FeatureDemo.Common {
    public class TitleLabel : Control {
        #region static
        public static readonly DependencyProperty IsNewProperty;
        public static readonly DependencyProperty IsUpdatedProperty;
        public static readonly DependencyProperty NewBackgroundProperty;
        public static readonly DependencyProperty UpdatedBackgroundProperty;
        public static readonly DependencyProperty GroupBackgroundProperty;
        public static readonly DependencyProperty IsGroupLabelProperty;
        public static readonly DependencyProperty GroupNameProperty;
        public static readonly DependencyProperty CaptionProperty;

        static TitleLabel() {
            DependencyPropertyRegistrator<TitleLabel>.New()
                .Register(nameof(IsNew), out IsNewProperty, false, (d) => d.Update())
                .Register(nameof(IsUpdated), out IsUpdatedProperty, false, (d) => d.Update())
                .Register(nameof(IsGroupLabel), out IsGroupLabelProperty, false, (d) => d.Update())
                .Register<string>(nameof(GroupName), out GroupNameProperty, null, (d) => d.Update())
                .Register<Brush>(nameof(NewBackground), out NewBackgroundProperty, null, (d) => d.Update())
                .Register<Brush>(nameof(UpdatedBackground), out UpdatedBackgroundProperty, null, (d) => d.Update())
                .Register<Brush>(nameof(GroupBackground), out GroupBackgroundProperty, null, (d) => d.Update())
                .Register<string>(nameof(Caption), out CaptionProperty);
        }
        #endregion
        #region dep props
        public bool IsNew {
            get { return (bool)GetValue(IsNewProperty); }
            set { SetValue(IsNewProperty, value); }
        }
        public bool IsUpdated {
            get { return (bool)GetValue(IsUpdatedProperty); }
            set { SetValue(IsUpdatedProperty, value); }
        }
        public bool IsGroupLabel {
            get { return (bool)GetValue(IsGroupLabelProperty); }
            set { SetValue(IsGroupLabelProperty, value); }
        }
        public string GroupName {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }
        public string Caption {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }
        public Brush NewBackground {
            get { return (Brush)GetValue(NewBackgroundProperty); }
            set { SetValue(NewBackgroundProperty, value); }
        }
        public Brush UpdatedBackground {
            get { return (Brush)GetValue(UpdatedBackgroundProperty); }
            set { SetValue(UpdatedBackgroundProperty, value); }
        }
        public Brush GroupBackground {
            get { return (Brush)GetValue(GroupBackgroundProperty); }
            set { SetValue(GroupBackgroundProperty, value); }
        }

        #endregion
        public TitleLabel() {
            DefaultStyleKey = typeof(TitleLabel);
        }
        private void Update() {
            if(IsGroupLabel) {
                Caption = GroupName;
                Visibility = Visibility.Visible;
                Background = GroupBackground;
                return;
            }
            if(IsNew) {
                Caption = "New";
                Visibility = Visibility.Visible;
                Background = NewBackground;
                return;
            }
            if(IsUpdated) {
                Caption = "Updated";
                Visibility = Visibility.Visible;
                Background = UpdatedBackground;
                return;
            }
            Visibility = Visibility.Collapsed;
        }
    }
}