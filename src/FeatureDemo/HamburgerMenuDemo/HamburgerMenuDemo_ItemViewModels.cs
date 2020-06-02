using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.UI.Xaml.Layout;
using DevExpress.UI.Xaml.Layout.Internal;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace HamburgerMenuDemo {
    public abstract class HamburgerItemViewModelBase : BindableBase {
        public Type NavigationTargetType {
            get { return GetProperty<Type>(); }
            set { SetProperty(value); }
        }
        public object Content {
            get { return GetProperty<object>(); }
            set { SetProperty(value); }
        }
        public string Icon {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        public HamburgerMenuNavigationButtonPlacement Placement {
            get { return GetProperty<HamburgerMenuNavigationButtonPlacement>(); }
            set { SetProperty(value); }
        }

        public HamburgerItemViewModelBase() {
            Placement = HamburgerMenuNavigationButtonPlacement.Top;
        }
    }
    public abstract class HamburgerSubMenuItemViewModelBase : HamburgerItemViewModelBase {
        public HamburgerSubMenuMoreButtonVisibility MoreButtonVisibilityMode {
            get { return GetProperty<HamburgerSubMenuMoreButtonVisibility>(); }
            set { SetProperty(value); }
        }
        public bool IsStandaloneSelectionItemMode {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public HamburgerSubMenuItemViewModelBase() {
            MoreButtonVisibilityMode = HamburgerSubMenuMoreButtonVisibility.Auto;
        }
    }
    public class HamburgerSubMenuItemViewModel : HamburgerSubMenuItemViewModelBase {
        public IReadOnlyList<HamburgerSubItemViewModel> Items { get; }

        public HamburgerSubMenuItemViewModel(object content, string icon, IList<HamburgerSubItemViewModel> items) {
            Content = content;
            Icon = icon;
            Items = new ReadOnlyCollection<HamburgerSubItemViewModel>(items);
        }
    }
    public class HamburgerThemeSwitcherItemViewModel : HamburgerSubMenuItemViewModelBase {
        public HamburgerThemeSwitcherItemViewModel(object content, string icon) {
            Content = content;
            Icon = icon;
        }
    }
    public class HamburgerSubItemViewModel : HamburgerItemViewModelBase {
        public object RightContent {
            get { return GetProperty<object>(); }
            set { SetProperty(value); }
        }
        public bool ShowInPreview {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        public object PreviewContent {
            get { return GetProperty<object>(); }
            set { SetProperty(value); }
        }
        public IList<ContextMenuItem> ContextMenu {
            get { return GetProperty<IList<ContextMenuItem>>(); }
            set { SetProperty(value); }
        }
        public IList<ContextMenuItem> PreviewContextMenu {
            get { return GetProperty<IList<ContextMenuItem>>(); }
            set { SetProperty(value); }
        }
        public bool IsPinned { get; set; }
        public bool AllowSync { get; set; }
        public FolderCapabilities Capabilities { get; }

        public HamburgerSubItemViewModel(object content, object rightContent, Type navigationTargetType, bool showInPreview, object parentViewModel, FolderCapabilities capabilities) {
            Content = content;
            RightContent = rightContent;
            NavigationTargetType = navigationTargetType;
            ShowInPreview = showInPreview;
            Capabilities = capabilities;
            AllowSync = true;
            InitializeMenu(parentViewModel);
        }
        protected virtual void InitializeMenu(object parentViewModel) {
            var menu = new List<ContextMenuItem>() { new ToggleFavoritesContextMenuItem(this) { ShowSeparator = true } };
            var previewMenu = new List<ContextMenuItem>() { menu[0] };
            menu.Add(new NewSubfolderContextMenuItem(this));
            menu.Add(new RenameFolderContextMenuItem(this));
            menu.Add(new DeleteFolderContextMenuItem(this));
            menu.Add(new MoveFolderContextMenuItem(this) { ShowSeparator = true });

            previewMenu.Add(new RenameFolderPreviewContextMenuItem(this));
            previewMenu.Add(new DeleteFolderPreviewContextMenuItem(this) { ShowSeparator = true });
            
            menu.Add(new ClearFolderContextMenuItem(this));
            previewMenu.Add(menu.Last());
            menu.Add(new DisableSyncContextMenuItem(this) { ShowSeparator = true });
            previewMenu.Add(menu.Last());
            menu.Add(new PinToStartContextMenuItem(this));
            previewMenu.Add(menu.Last());

            foreach(ISupportParentViewModel child in menu)
                child.ParentViewModel = parentViewModel;
            foreach(ISupportParentViewModel child in previewMenu)
                child.ParentViewModel = parentViewModel;

            ContextMenu = menu;
            PreviewContextMenu = previewMenu;
        }
    }
    public class HamburgerLinkItemViewModel : HamburgerItemViewModelBase {
        public Uri NavigationUri {
            get { return GetProperty<Uri>(); }
            set { SetProperty(value); }
        }
        public HamburgerLinkItemViewModel(object content, string icon, Uri navigationUri) {
            Content = content;
            Icon = icon;
            NavigationUri = navigationUri;
        }
    }
    public class HamburgerNavigationItemViewModel : HamburgerItemViewModelBase {
        public bool HideMenuWhenSelected {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        public bool SelectOnClick {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        public ICommand Command {
            get { return GetProperty<ICommand>(); }
            set { SetProperty(value); }
        }
        public HamburgerNavigationItemViewModel(object content, string icon, Type navigationTargetType) {
            Content = content;
            Icon = icon;
            NavigationTargetType = navigationTargetType;
            SelectOnClick = true;
        }
    }

    [Flags]
    public enum FolderCapabilities {
        None = 0,
        Sync = 1,
        Move = 2,
        Clear = 4,
        SubFolder = 8
    }
}
