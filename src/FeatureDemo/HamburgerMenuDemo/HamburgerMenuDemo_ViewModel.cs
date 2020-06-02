using System;
using DevExpress.Mvvm;
using DevExpress.UI.Xaml.Layout;
using System.Windows.Input;
using FeatureDemo.Common;
using System.Collections.Generic;
using DevExpress.UI.Xaml.Layout.Internal;
using System.Collections.ObjectModel;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace HamburgerMenuDemo {
    public class HamburgerMenuDemo_ViewModel : NavigationViewModelBase {
        bool IsInitializing { get; set; }
        public ICommand CloseWindowCommand { get; private set; }

        public string CheckBoxState {
            get { return this.GetProperty<string>(); }
            set { this.SetProperty(value); }
        }
        public string SelectedRadioButtonGlyph {
            get { return this.GetProperty<string>(); }
            set { this.SetProperty(value); }
        }
        public bool IsCalendarRadioButtonChecked {
            get { return this.GetProperty<bool>(); }
            set { this.SetProperty(value, UpdateSelectedRadioButton); }
        }
        public bool IsMailRadioButtonChecked {
            get { return this.GetProperty<bool>(); }
            set { this.SetProperty(value, UpdateSelectedRadioButton); }
        }
        public bool IsCheckBoxChecked {
            get { return this.GetProperty<bool>(); }
            set { this.SetProperty(value, UpdateCheckBoxState); }
        }
        public string Description {
            get { return this.GetProperty<string>(); }
            set { this.SetProperty(value); }
        }
        public string Title {
            get { return this.GetProperty<string>(); }
            set { this.SetProperty(value); }
        }
        public string Header {
            get { return this.GetProperty<string>(); }
            set { this.SetProperty(value); }
        }
        public bool AllowCustomizingWindowTitle {
            get { return this.GetProperty<bool>(); }
            set { this.SetProperty(value); }
        }
        public bool ShowMenuOnEmptySpaceBarClick {
            get { return this.GetProperty<bool>(); }
            set { this.SetProperty(value); }
        }

        public HamburgerMenuAvailableViewStates AvailableViewStates {
            get { return this.GetProperty<HamburgerMenuAvailableViewStates>(); }
            set { this.SetProperty(value); }
        }
        public bool IsFlyoutViewStateEnabled {
            get { return this.GetProperty<bool>(); }
            set {
                this.SetProperty(value, OnIsFlyoutViewStateEnabledChanged);
            }
        }
        public bool IsOverlayViewStateEnabled {
            get { return this.GetProperty<bool>(); }
            set { this.SetProperty(value, OnIsOverlayViewStateEnabledChanged); }
        }
        public bool IsInlineViewStateEnabled {
            get { return this.GetProperty<bool>(); }
            set { this.SetProperty(value, OnIsInlineViewStateEnabledChanged); }
        }
        public IReadOnlyList<HamburgerItemViewModelBase> Items { get; }

        protected bool IsAvailableViewStateEmpty() {
            return !IsInlineViewStateEnabled && !IsOverlayViewStateEnabled && !IsFlyoutViewStateEnabled;
        }
        protected void OnIsFlyoutViewStateEnabledChanged() {
            if(!IsFlyoutViewStateEnabled && IsAvailableViewStateEmpty()) {
                this.GetService<IDispatcherService>().BeginInvoke(() => IsFlyoutViewStateEnabled = true);
                return;
            }
            UpdateAvailableViewState();
        }
        protected void OnIsOverlayViewStateEnabledChanged() {
            if(!IsOverlayViewStateEnabled && IsAvailableViewStateEmpty()) {
                this.GetService<IDispatcherService>().BeginInvoke(() => IsOverlayViewStateEnabled = true);
                return;
            }
            UpdateAvailableViewState();
        }
        protected void OnIsInlineViewStateEnabledChanged() {
            if(!IsInlineViewStateEnabled && IsAvailableViewStateEmpty()) {
                this.GetService<IDispatcherService>().BeginInvoke(() => IsInlineViewStateEnabled = true);
                return;
            }
            UpdateAvailableViewState();
        }
        void UpdateSelectedRadioButton() {
            SelectedRadioButtonGlyph = IsCalendarRadioButtonChecked ? "\uE787" : "\uE715";
        }
        void UpdateCheckBoxState() {
            CheckBoxState = IsCheckBoxChecked ? "Checked" : "Unchecked";
        }

        protected void UpdateAvailableViewState() {
            HamburgerMenuAvailableViewStates state = (HamburgerMenuAvailableViewStates)0;
            if(IsInlineViewStateEnabled)
                state = state | HamburgerMenuAvailableViewStates.Inline;
            if(IsOverlayViewStateEnabled)
                state = state | HamburgerMenuAvailableViewStates.Overlay;
            if(IsFlyoutViewStateEnabled)
                state = state | HamburgerMenuAvailableViewStates.Flyout;
            AvailableViewStates = state;
        }

        public HamburgerMenuDemo_ViewModel() {
            IsInitializing = true;
            Header = "Menu Header";
            IsMailRadioButtonChecked = true;
            UpdateCheckBoxState();
            CloseWindowCommand = new DelegateCommand(OnCloseWindowCommandExecute);
            AllowCustomizingWindowTitle = true;
            ShowMenuOnEmptySpaceBarClick = true;
            IsInlineViewStateEnabled = true;
            IsOverlayViewStateEnabled = true;
            IsFlyoutViewStateEnabled = true;
            Items = new ReadOnlyCollection<HamburgerItemViewModelBase>(CreateMenuItems());
            IsInitializing = false;
        }

        protected virtual IList<HamburgerItemViewModelBase> CreateMenuItems() {
            var subItems = new List<HamburgerSubItemViewModel>() {
                new HamburgerSubItemViewModel("Outbox", "12", typeof(HamburgerMenuDemo_OutboxPage), false, this, FolderCapabilities.None),
                new HamburgerSubItemViewModel("Inbox", "142", typeof(HamburgerMenuDemo_InboxPage), true, this, FolderCapabilities.SubFolder),
                new HamburgerSubItemViewModel("Drafts", "3", typeof(HamburgerMenuDemo_DraftsPage), true, this, FolderCapabilities.SubFolder),
                new HamburgerSubItemViewModel("Junk", null, typeof(HamburgerMenuDemo_JunkPage), false, this, FolderCapabilities.Move | FolderCapabilities.Clear | FolderCapabilities.SubFolder),
                new HamburgerSubItemViewModel("Sent", "475", typeof(HamburgerMenuDemo_SentPage), true, this, FolderCapabilities.SubFolder | FolderCapabilities.Clear) { PreviewContent = "Sent Items" },
                new HamburgerSubItemViewModel("Trash", "15", typeof(HamburgerMenuDemo_TrashPage), false, this, FolderCapabilities.Sync | FolderCapabilities.Clear | FolderCapabilities.SubFolder)
            };
            var items = new List<HamburgerItemViewModelBase>() {
                new HamburgerNavigationItemViewModel("New Mail", "&#xE710;", typeof(HamburgerMenuDemo_NewMailPage)) { HideMenuWhenSelected = true },
                new HamburgerSubMenuItemViewModel("Folders", "&#xE8B7;", subItems),
                new HamburgerLinkItemViewModel("Additional Information", null, new Uri(@"https://documentation.devexpress.com/#Win10Apps/CustomDocument12019")),
                new HamburgerThemeSwitcherItemViewModel("Color Scheme", "ms-appx:///Images/HamburgerMenu/ColorSchemes.svg") {
                    IsStandaloneSelectionItemMode = true,
                    MoreButtonVisibilityMode = HamburgerSubMenuMoreButtonVisibility.Hidden,
                    Placement = HamburgerMenuNavigationButtonPlacement.Bottom
                },
                new HamburgerNavigationItemViewModel("Settings", "&#xE713;", typeof(HamburgerMenuDemo_SettingsPage)) {
                    Placement = HamburgerMenuNavigationButtonPlacement.Bottom
                },
                new HamburgerNavigationItemViewModel("Close Window", "&#xE711;", null) {
                    Placement = HamburgerMenuNavigationButtonPlacement.Bottom,
                    SelectOnClick = false,
                    Command = CloseWindowCommand
                }
            };
            return items;
        }
        async void OnCloseWindowCommandExecute() {
            if(await GetService<IMessageBoxService>().ShowAsync("Do you want to close the demo?", "Hamburger Menu Demo", MessageButton.YesNo) == MessageResult.Yes) {
                GetService<ICloseWindowService>().Close();
            }
        }
    }
}