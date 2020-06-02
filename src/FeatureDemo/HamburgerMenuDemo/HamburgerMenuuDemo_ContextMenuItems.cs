using System;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using Microsoft.UI.Xaml;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;


namespace HamburgerMenuDemo {
    public abstract class ContextMenuItem : ViewModelBase {
        public bool ShowSeparator {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        public object Content {
            get { return GetProperty<object>(); }
            protected set { SetProperty(value); }
        }
        public Visibility Visibility {
            get { return GetProperty<Visibility>(); }
            set { SetProperty(value); }
        }

        public ICommand Command { get; }
        protected HamburgerSubItemViewModel Owner { get; }

        public ContextMenuItem(HamburgerSubItemViewModel owner, object content) {
            Owner = owner;
            Visibility = Visibility.Visible;
            Content = content;
            Command = CreateCommand();
            Command.CanExecuteChanged += OnCommandCanExecuteChanged;
            UpdateProperties();
        }

        public virtual void UpdateProperties() { }
        protected void RaiseCanExecuteChanged() {
            ((IDelegateCommand)Command).RaiseCanExecuteChanged();
        }
        protected virtual ICommand CreateCommand() {
            return new DelegateCommand<object>(Execute, CanExecuteCore);
        }
        protected abstract void ExecuteCore(object parameter);
        protected virtual bool CanExecuteCore(object parameter) {
            return true;
        }
        void Execute(object parameter) {
            ExecuteCore(parameter);
            RaiseCanExecuteChanged();
        }
        void OnCommandCanExecuteChanged(object sender, object e) {
            UpdateProperties();
        }

    }
    public abstract class SwitchableContextMenuItemBase : ContextMenuItem {
        protected abstract bool UseDefaultContent { get; }
        object content;
        object altContent;

        public SwitchableContextMenuItemBase(HamburgerSubItemViewModel owner, object content, object altContent)
            : base(owner, null) {
            this.content = content;
            this.altContent = altContent;
            UpdateProperties();
        }

        public override void UpdateProperties() {
            Content = UseDefaultContent ? content : altContent;
        }
    }
    public class PinToStartContextMenuItem : SwitchableContextMenuItemBase {
        protected override bool UseDefaultContent { get { return !Owner.IsPinned; } }
        public PinToStartContextMenuItem(HamburgerSubItemViewModel owner)
            : base(owner, "Pin to Start", "Unpin from Start") {

        }
        protected override void ExecuteCore(object parameter) {
            Owner.IsPinned = !Owner.IsPinned;
        }
    }
    public class ToggleFavoritesContextMenuItem : SwitchableContextMenuItemBase {
        protected override bool UseDefaultContent { get { return !Owner.ShowInPreview; } }

        public ToggleFavoritesContextMenuItem(HamburgerSubItemViewModel owner)
            : base(owner, "Add to Favorites", "Remove from Favorites") {

        }
        protected override void ExecuteCore(object parameter) {
            Owner.ShowInPreview = !Owner.ShowInPreview;
        }
    }
    public class ClearFolderContextMenuItem : ContextMenuItem {
        public ClearFolderContextMenuItem(HamburgerSubItemViewModel owner)
            : base(owner, "Clear folder") { }

        public override void UpdateProperties() {
            Visibility = Owner.Capabilities.HasFlag(FolderCapabilities.Clear) ? Visibility.Visible : Visibility.Collapsed;
        }
        protected override void ExecuteCore(object parameter) {
            Owner.RightContent = null;
        }
        protected override bool CanExecuteCore(object parameter) {
            return Owner.RightContent != null;
        }
    }
    public class DisableSyncContextMenuItem : ContextMenuItem {
        public DisableSyncContextMenuItem(HamburgerSubItemViewModel owner)
            : base(owner, "Don't Sync this Folder") {
        }
        public override void UpdateProperties() {
            Visibility = Owner.AllowSync && Owner.Capabilities.HasFlag(FolderCapabilities.Sync) ? Visibility.Visible : Visibility.Collapsed;
        }
        protected override void ExecuteCore(object parameter) {
            Owner.AllowSync = false;
        }
        protected override bool CanExecuteCore(object parameter) {
            return Owner.AllowSync;
        }
    }
    public abstract class MessageBoxContextMenuItemBase : ContextMenuItem {
        public MessageBoxContextMenuItemBase(HamburgerSubItemViewModel owner, object content)
            : base(owner, content) {
        }

        protected override async void ExecuteCore(object parameter) {
            var service = GetService<IMessageBoxService>();
            await service.ShowAsync(string.Format("The \"{0}\" item of the \"{1}\"'s context menu was clicked.", Content, Owner.Content));
        }
    }
    public abstract class FolderContextMenuItemBase : MessageBoxContextMenuItemBase {
        public FolderContextMenuItemBase(HamburgerSubItemViewModel owner, object content)
            : base(owner, content) {
        }
        
        protected override bool CanExecuteCore(object parameter) {
            return Owner.Capabilities.HasFlag(FolderCapabilities.Move);
        }
        public override void UpdateProperties() {
            Visibility = Owner.Capabilities.HasFlag(FolderCapabilities.Move) || Owner.Capabilities.HasFlag(FolderCapabilities.SubFolder) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
    public class RenameFolderContextMenuItem : FolderContextMenuItemBase {
        public RenameFolderContextMenuItem(HamburgerSubItemViewModel owner)
            : base(owner, "Rename") {
        }
    }
    public class DeleteFolderContextMenuItem : FolderContextMenuItemBase {
        public DeleteFolderContextMenuItem(HamburgerSubItemViewModel owner)
            : base(owner, "Delete") {
        }
    }
    public class MoveFolderContextMenuItem : FolderContextMenuItemBase {
        public MoveFolderContextMenuItem(HamburgerSubItemViewModel owner)
            : base(owner, "Move") {
        }
    }
    public class RenameFolderPreviewContextMenuItem : RenameFolderContextMenuItem {
        public RenameFolderPreviewContextMenuItem(HamburgerSubItemViewModel owner) : base(owner) { }
        public override void UpdateProperties() {
            Visibility = Owner.Capabilities.HasFlag(FolderCapabilities.Move) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
    public class DeleteFolderPreviewContextMenuItem : DeleteFolderContextMenuItem {
        public DeleteFolderPreviewContextMenuItem(HamburgerSubItemViewModel owner) : base(owner) { }
        public override void UpdateProperties() {
            Visibility = Owner.Capabilities.HasFlag(FolderCapabilities.Move) ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public class NewSubfolderContextMenuItem : MessageBoxContextMenuItemBase {
        public NewSubfolderContextMenuItem(HamburgerSubItemViewModel owner)
            : base(owner, "Create new subfolder") {
        }
        
        protected override bool CanExecuteCore(object parameter) {
            return Owner.Capabilities.HasFlag(FolderCapabilities.SubFolder);
        }
        public override void UpdateProperties() {
            Visibility = Owner.Capabilities.HasFlag(FolderCapabilities.Move) || Owner.Capabilities.HasFlag(FolderCapabilities.SubFolder) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
