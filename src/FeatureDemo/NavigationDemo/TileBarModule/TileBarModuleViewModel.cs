using DevExpress.Mvvm;
using System;
using System.Collections.ObjectModel;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media.Imaging;
using DevExpress.Core;
using DevExpress.UI.Xaml.Layout;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using DevExpress.Mvvm.Native;
using Color = Windows.UI.Color;

namespace NavigationDemo {
    public class TileBarViewModel : NavigationViewModelBase {
        public ObservableCollection<NavigationItem> Items { get; }
        protected IFlyoutService FlyoutService { get { return ServiceContainer.GetService<IFlyoutService>(); } }

        public Dock MenuPlacement {
            get { return GetProperty<Dock>(); }
            set { SetProperty(value, OnMenuPlacementChanged); }
        }
        public FlyoutShowDirection FlyoutShowDirection {
            get { return GetProperty<FlyoutShowDirection>(); }
            private set { SetProperty(value); }
        }
        public Orientation SubMenuOrientation {
            get { return GetProperty<Orientation>(); }
            set { SetProperty(value, OnSubMenuOrientationChanged); }
        }

        public TileBarViewModel() {
            MenuPlacement = Dock.Top;
            SubMenuOrientation = Orientation.Horizontal;
            Items = NavigationItemsDataProvider.ItemsCache ?? NavigationItemsDataProvider.GetItems(10, typeof(TileBarModuleDetailView));
        }

        protected override void OnNavigatedTo() {
            base.OnNavigatedTo();
            FlyoutService?.CloseFlyout();
        }
        void OnMenuPlacementChanged() {
            FlyoutShowDirection = MenuPlacement == Dock.Bottom ? FlyoutShowDirection.Inverted : FlyoutShowDirection.Default;
        }
        void OnSubMenuOrientationChanged() {
            Items?.ForEach(x => x.SubMenuOrientation = SubMenuOrientation);
        }
    }
    public class NavigationItem : BindableBase {
        public NavigationItem Parent { get; }
        public string Image { get; }
        public string Text { get; }
        public Color Color { get; }
        public bool ShowFlyout { get; }
        public Type NavigationTargetType { get; }
        public ObservableCollection<NavigationItem> SubItems { get; }
        public Orientation SubMenuOrientation {
            get { return GetProperty<Orientation>(); }
            set { SetProperty(value); }
        }

        public NavigationItem(string text, Type targetType, string image, Color color, bool showFlyout, NavigationItem parent) {
            Parent = parent;
            Text = text;
            NavigationTargetType = targetType;
            Color = color;
            Image = "ms-appx:///NavigationDemo/Images/" + image + ".svg";
            ShowFlyout = showFlyout;
            SubItems = new ObservableCollection<NavigationItem>();
            SubMenuOrientation = Orientation.Horizontal;
        }
    }

    static class NavigationItemsDataProvider {
        static ObservableCollection<NavigationItem> itemsCache;
        public static ObservableCollection<NavigationItem> ItemsCache { get { return itemsCache; } }

        static string[] images = new string[] {
            "Calc",
            "Rates",
            "Research",
            "Statistics",
            "System",
            "UserManagment",
            "ZillowLogo"
        };
        static Color[] colors = new Color[] {  Color.FromArgb(0xFF, 0x00, 0x87, 0x9C),
            Color.FromArgb(0xFF, 0xCC, 0x6D, 0x00), Color.FromArgb(0xFF, 0x00, 0x73, 0xC4),
            Color.FromArgb(0xFF, 0xCC, 0x6D, 0x00), Color.FromArgb(0xFF, 0x00, 0x73, 0xC4) };
        static Random random = new Random(DateTime.Now.Millisecond);

        static NavigationItem GenerateNavigationItem(string text, Type navigationTarget, NavigationItem parent = null) {
            return new NavigationItem(text, navigationTarget, images[random.Next(images.Length)],
                colors[random.Next(colors.Length)], parent == null && random.Next(2) == 1, parent);
        }
        public static ObservableCollection<NavigationItem> GetItems(int itemsCount, Type navigationTarget) {
            return GetItems(itemsCount, new List<Type>() { navigationTarget });
        }
        public static ObservableCollection<NavigationItem> GetItems(int itemsCount, IList<Type> navigationTargets) {
            var result = new ObservableCollection<NavigationItem>();
            for(var i = 0; i < itemsCount; i++) {
                var item = GenerateNavigationItem("Item " + i, navigationTargets[random.Next(navigationTargets.Count)]);
                if(item.ShowFlyout) {
                    for(var j = 0; j < 5; j++)
                        item.SubItems.Add(GenerateNavigationItem("SubItem " + j, navigationTargets[random.Next(navigationTargets.Count)]));
                }
                result.Add(item);
            }

            itemsCache = new ObservableCollection<NavigationItem>(result);
            return itemsCache;
        }
    }
}
