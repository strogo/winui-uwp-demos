using System;
using System.Collections.Generic;
using DevExpress.Mvvm.Native;

namespace FeatureDemo.DataModel {
    public enum DXReleaseVersion {
        Before171, V171, V172, V181, V182, V191, V192, V201, V202
    }
    public class DemoModule {
        public static readonly DXReleaseVersion CurrentVersion = (DXReleaseVersion)Enum.Parse(typeof(DXReleaseVersion), "V" + AssemblyInfo.VersionId);
        public IEnumerable<string> Sources { get; set; }
        public string ViewTypeName { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public string Description { get; set; }
        public bool IsHighlighted { get; set; }
        public bool IsNew => CurrentVersion == IntroducedIn;
        public bool IsUpdated => CurrentVersion == UpdatedIn;
        public DXReleaseVersion IntroducedIn { get; set; } = DXReleaseVersion.Before171;
        public DXReleaseVersion UpdatedIn { get; set; } = DXReleaseVersion.Before171;
        public DemoModuleGroup Group { get; set; }
        public string FullImagePath { get { return "ms-appx:///Images/Modules/" + ViewTypeName + ".png"; } }
        public DemoModule(string viewTypeName, string title, string about, string description = null) {
            ViewTypeName = viewTypeName;
            Title = title;
            About = about;
            Description = description ?? about;
        }
    }

    public class DemoModuleGroup {
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Icon { get; private set; }
        public List<DemoModule> DemoModules { get; private set; }
        public DemoModuleGroup(string name, string title, string icon, List<DemoModule> demoModules) {
            Name = name;
            Title = title;
            Icon = icon;
            DemoModules = demoModules;
            DemoModules.ForEach(x => x.Group = this);
        }
    }
    public class DataModel {
        private static DataModel current;
        public static DataModel Current { get { return current ?? (current = new DataModel()); } }
        private List<DemoModuleGroup> demoModuleGroups;
        public List<DemoModuleGroup> DemoModuleGroups { get { return demoModuleGroups ?? (demoModuleGroups = new List<DemoModuleGroup>()); } }
        public DataModel() {
            InitializeDataForDesktop();
        }
        private void InitializeDataForDesktop() {
            #region Scheduler
            var schedulerGroup = new DemoModuleGroup("Scheduler", "Scheduler", "Scheduler", new List<DemoModule>() {
                new DemoModule("SchedulerDemoModule", "Scheduler Control", "This demo shows our UWP Scheduler Control that can display appointments in a variety of views inspired by popular calendar and scheduling applications: from a single-day schedule to a month-wide view. The demo allows you to create new appointments or select existing appointments and edit them. When editing, explore available appointment recurrence patterns.", "This demo uses SchedulerControl to display appointments.") {
                    IsHighlighted = true, IntroducedIn=DXReleaseVersion.V182, UpdatedIn = DXReleaseVersion.V191, Sources = new string[] { "ViewModels/SchedulingDemoViewModel.cs" } }
            });
            #endregion
            #region Ribbon
            var ribbonGroup = new DemoModuleGroup("Ribbon", "Ribbon & Toolbars", "Ribbon", new List<DemoModule>() {
                new DemoModule("RibbonSimplePadModule", "Ribbon", "This module demonstrates the DevExpress UWP Ribbon Control which brings Microsoft Office inspired navigation and command UI to your applications.")
                    { IsHighlighted = true, Sources = new string[] { "RibbonSimplePadViewModel.cs", "RibbonDemoCommonStyles.xaml", "DemoRichEditor/DemoRichEditControl.cs", "DemoRichEditor/DemoRichEditInterfaces.cs", "DemoRichEditor/DemoRichEditorBehavior.cs", "DemoRichEditor/DemoRichEditPreviewControl.cs", "ViewModels/BackstageNewTabViewModel.cs", "ViewModels/BackstageOpenTabViewModel.cs", "ViewModels/FontStyleGroupViewModel.cs", "ViewModels/InsertGroupViewModel.cs", "ViewModels/ParagraphStyleGroupViewModel.cs", "Views/NewFileBackstageView.xaml", "Views/NewFileBackstageView.xaml.cs", "Views/OpenFileBackstageView.xaml", "Views/OpenFileBackstageView.xaml.cs" } },
                new DemoModule("RibbonToolbarModule",
                               "Ribbon Toolbar",
                               "This module demonstrates the DevExpress UWP RibbonToolbar Control. A lightweight version of the feature-rich Ribbon Control, the RibbonToolbarControl does not include caption, Backstage View, and is not integrated into the window's title bar, thus providing an elegant solution for creating compact menus for controls",
                               "This module demonstrates the DevExpress UWP RibbonToolbar Control.")
                    { IsHighlighted = true, Sources = new string[] {
                        "RibbonSimplePadViewModel.cs",
                        "RibbonDemoCommonStyles.xaml",
                        "DemoRichEditor/DemoRichEditControl.cs",
                        "DemoRichEditor/DemoRichEditInterfaces.cs",
                        "DemoRichEditor/DemoRichEditorBehavior.cs",
                        "ViewModels/FontStyleGroupViewModel.cs",
                        "ViewModels/InsertGroupViewModel.cs",
                        "ViewModels/ParagraphStyleGroupViewModel.cs" } },
                new DemoModule("ToolbarModule",
                               "Toolbar Control",
                               "This module demonstrates the DexExpress UWP Toolbar Control - lightweight one-tab toolbar.",
                               "This module demonstrates the DevExpress UWP Toolbar Control.")
                    { IsHighlighted = true }
                //new DemoModule() { ViewTypeName = "RibbonReportExplorerModule", Title = "Report Explorer", About = "In this demo, the PDF Viewer control displays reports generated by the DevExpress Report Service. Use the Options pane to select different reports and customize report parameters.", IsHighlighted = true, Image = "PdfViewerReports.png", IsNew = true }
            });
            #endregion
            #region Layout
            var layoutGroup = new DemoModuleGroup("Layout", "Layout", "Layout", new List<DemoModule>() {
                //TODO: WinUI
                //new DemoModule("HamburgerMenuDemoModule", "Hamburger Menu", "This example demonstrates the features available to you when using the DevExpress Hamburger Menu control.\n\n    - Adaptive layout\n    - Item types: navigation buttons, radio groups, checkboxes, submenus and hyperlinks\n    - Window title customization\n    - Automatic synchronization with the currently selected page", "This example demonstrates the features available to you when using the DevExpress Hamburger Menu control.")
                //    { IsHighlighted = true, UpdatedIn = DXReleaseVersion.V171, Sources = new string[] { "HamburgerMenuDemo_ViewModel.cs", "HamburgerMenuDemo_ItemViewModels.cs", "HamburgerMenuuDemo_ContextMenuItems.cs", "HamburgerMenuDemoModule_Menu.xaml", "HamburgerMenuDemoModule_Menu.xaml.cs", "HamburgerMenuDemoModule.xaml", "HamburgerMenuDemoModule.xaml.cs", "HamburgerMenuDemo_SettingsPage.xaml", "HamburgerMenuDemo_SettingsPage.xaml.cs", "HamburgerMenuDemo_InboxPage.xaml", "HamburgerMenuDemo_InboxPage.xaml.cs" } },
                new DemoModule("MVVM_LayoutGroupDemoModule", "MVVM-Style Layout", "The LayoutControl allows you to use the MVVM design pattern in order to populate groups with items from View Models.\nIn this demo, the Phones and Emails groups are populated with items from bound collections.", "The LayoutControl allows you to use the MVVM design pattern in order to populate groups with items from View Models."),
                new DemoModule("DataLayoutControlModule", "Data Layout Control", "The DataLayoutControl automatically generates a data entry form based on the object assigned to its CurrentItem property.\n In this demo you can explore the features and options available in the control.", "The DataLayoutControl automatically generates a data entry form based on the object assigned to its CurrentItem property.")
                    { Sources = new string[] { "DataLayoutControlViewModel.cs"} },
                new DemoModule("ContentContainersModule", "Content Containers", "This module demonstrates content containers and their key settings."),
                new DemoModule("MortgageAppDemoModule", "LayoutControl: Mortgage Application", "This demo uses the LayoutControl to set up a an automatically arranged and customizable entry form. Take note of expandable groups and the automatic editor alignment."),
                new DemoModule("LayoutGroupOptionsDemoModule", "LayoutControl: LayoutGroup Options", "This example demonstrates key LayoutGroup options that will help you build your own data entry forms."),
                new DemoModule("LayoutItemOptionsDemoModule", "LayoutControl: LayoutItem Options", "This example demonstrates key LayoutItem options that will help you build your own data entry forms."),
                new DemoModule("DialogModule", "MVVM-Style Dialogs ", "This demo shows how to define custom dialogs in XAML and then invoke them via commands declared in a View Model (DialogModuleViewModel).\nThe View Model contains asynchronous methods that invoke dialogs and perform actions in response to various dialog results.", "This demo shows how to define custom dialogs in XAML and then invoke them via commands declared in a View Model (DialogModuleViewModel).")
                    { Sources = new string[] { "DialogModule/UseEditControl.xaml", "DialogModule/UseEditControl.xaml.cs" } },
                new DemoModule("MessageDialogModule", "MVVM-Style Message Dialog", "This demo shows how to define a Message Dialog in XAML and invoke it via a command declared in a View Model (MessageDialogModuleViewModel).\nThe View Model contains an asynchronous method that calls the Message Dialog and performs specific actions after it is closed.", "This demo shows how to define a Message Dialog in XAML and invoke it via a command declared in a View Model (MessageDialogModuleViewModel).")
                    {Sources = new string[] { "MessageDialogModule/MessageDialogModuleViewModel.cs" } },
                new DemoModule("TilesModule", "Tiles", "This module demonstrates how to build a Windows Store UI experience for your apps."),
                new DemoModule("TileBarModule", "TileBar", "The TileBar control displays a set of tiles within its container and allows you to introduce a simple and straightforward navigation experience. In this demo, the TileBar is populated with items from a bound collection.")
                    { IsHighlighted = true, Sources = new string[] { "TileBarModule/TileBarModuleDetailView.xaml", "TileBarModule/TileBarModuleDetailView.xaml.cs", "TileBarModule/TileBarService.cs" } },
                new DemoModule("MasterDetailPageContentDemoModule", "Master-Detail Pattern", "This demo shows the master-detail navigation pattern. When a user selects a list item, the details pane is automatically updated with the corresponding content. Horizontal resizing operations automatically toggle between stacked and side-by-side display styles. The side-by-side view style displays both master and detail panes with a slider between them.", "This demo shows the master-detail navigation pattern inspired by the Windows 10 Mail app. Horizontal resizing operations automatically toggle between stacked and side-by-side display styles.")
                    { IntroducedIn = DXReleaseVersion.V172, IsHighlighted = true, Sources = new string[] { "MasterDetailPageContentDemoStyles.xaml", "MasterDetailPageContentDemoViewModel.cs"} }
            });
            #endregion
            #region Utility Controls
            var controlsGroup = new DemoModuleGroup("Controls", "Utility Controls", "Controls", new List<DemoModule>() {
                new DemoModule("DXItemsControlModule", "DXComboBox and DXListView Controls", "This module demonstrates the DevExpress DXComboBox and DXListView controls.")
                    { IntroducedIn = DXReleaseVersion.V192, IsHighlighted = true, Sources = new string[] { "Data/DXItemsControlViewModel.cs" }  },
                new DemoModule("SvgDemoModule", "SVG Icon Control", "This module demonstrates the DevExpress SVG Icon Control. The icon's color changes depending on two factors: the currently applied theme and the accent color selected in the menu on the left.")
                    { IntroducedIn = DXReleaseVersion.V171, IsHighlighted = true, Sources = new string[] { "SvgDemoViewModel.cs", "SvgDemoIconViewModel.cs" } },
                new DemoModule("SvgThemingDemoModule", "SVG Icon Palettes", "This demo illustrates the capabilities to color SVG icons using palettes. You can specify a global palette that is used to color all SVG icons in your application. Additionally you can apply a custom palette to individual icons that overrides certain colors of the global palette.", "This demo illustrates the capabilities to color SVG icons using palettes.")
                    { IntroducedIn = DXReleaseVersion.V172, IsHighlighted = true, Sources = new string[] { "SvgDemoViewModel.cs", "SvgDemoIconViewModel.cs" } },
                new DemoModule("DXBrushesDemoModule", "Applying DevExpress Brushes to Custom Controls", "This module shows how to apply DevExpress Hamburger Menu theme colors to the standard ListView control. Click the Show Code item at the top right to see how the ListView's colors are set in XAML. Try changing the theme and accent color using the Hamburger Menu to see the ListView is repainted accordingly.", "This module shows how to apply DevExpress Hamburger Menu theme colors to the standard ListView control.")
                    { IntroducedIn = DXReleaseVersion.V171, IsHighlighted = true},
                new DemoModule("ContextMenuModule", "Context Menu", "This module demonstrates the Context Menu. Switch between menu templates in the Options panel.") 
                    { IntroducedIn = DXReleaseVersion.V171, IsHighlighted = true, Sources = new string[] { "Data/ContextMenuViewModel.cs" } },
                new DemoModule("SimplePadModule", "Radial Context Menu", "Inspired by Microsoft OneNote, the DevExpress UWP Radial Menu Control allows you to provide quick access to contextual commands in touch-centric interfaces."),
                new DemoModule("FlyoutFlowInfoModule", "Flyout", "This module demonstrates the features available in the DevExpress UWP Flyout control. It can be displayed as a hint or popup panel displayed to either side of the screen."),
                //new DemoModule() { ViewTypeName = "MVVMModule", Title = "Context Menu MVVM Demo", About = "Inspired by Microsoft OneNote, the DevExpress UWP Radial Menu allows you to extend your applications and create contextual and quick access touch-centric interfaces." },
                new DemoModule("BarCodeEmployees", "QR Code Business Card", "This example uses the BarCode Control to generate QR codes within business cards. Each QR code contains the corresponding contact's personal information.")
                    { Sources = new string[] { "ControlsDemo/Data/EmployeViewModel.cs" } },
                new DemoModule("BarCodeSample2D", "BarCode 2D", "This example demonstrates four types of two-dimensional barcodes created using the BarCode Control. You can set barcode values and customize related display options."),
                new DemoModule("BarCodeSample", "Linear BarCode", "This example demonstrates one-dimensional barcodes created using the BarCodeControl. You can switch between individual barcode types, set barcode values and change corresponding display options.", "This example demonstrates one-dimensional barcodes created using the BarCodeControl."),
                new DemoModule("SplashScreenModule", "Splash Screen", "This module demonstrates how to display a splash screen. Use the radio buttons to try the two predefined styles or a manually created template.")
                    { IsHighlighted = true, Sources = new string[] { "SplashScreenDemo/SplashScreenViewModel.cs" } },
                new DemoModule("RangeControlModule", "Range Control Clients", "This module demonstrates available controls that can be linked to the Range Control.")
                    { IsHighlighted = true },
                new DemoModule("AggregationModule", "Data Aggregation", "This demo shows how to use the Range Control’s Data Aggregation feature for statistical analysis.")
                    { IsHighlighted = true }
            });
            #endregion
            #region Charts
            var chartsGroup = new DemoModuleGroup("Charts", "Charts", "Charts", new List<DemoModule>() {
                new DemoModule("DashboardDemoModule", "Dashboard", "This module uses the MVVM pattern to create a Population dashboard that includes chart and map controls.")
                    { IsHighlighted = true },
                new DemoModule("BigAmountOfDataModule", "Large Data Source", "This module shows the DevExpress Chart Control’s performance when bound to a large data source.")
                    { IsHighlighted = true },
                new DemoModule("RealTimeDataModule", "Real-Time Data", "This module shows the DevExpress Chart Control’s performance when bound to a frequently-updated data source."),
                new DemoModule("PieDonutSeriesViewModule", "Pie and Donut", "This module demonstrates Pie and Donut series view types."),
                new DemoModule("BarSeriesViewModule", "Bar and Column", "This module demonstrates the Bar series view type."),
                new DemoModule("AreaSeriesViewModule", "Area", "This module demonstrates the Area series view type."),
                new DemoModule("LineSeriesViewModule", "Line", "This module demonstrates the Line series view type."),
                new DemoModule("ScatterLineSeriesViewModule", "Scatter Line", "This module demonstrates the Scatter Line series view type."),
                new DemoModule("PointSeriesViewModule", "Point", "This module demonstrates the Point series view type."),
                new DemoModule("FunnelSeriesViewModule", "Funnel", "This module demonstrates the Funnel series view type.") }
            );
            #endregion
            #region Gauges
            var gaugesGroup = new DemoModuleGroup("Gauges", "Gauges", "Gauges", new List<DemoModule>() {
                new DemoModule("HouseClimateDashboardModule", "House Climate Dashboard", "In this demo, gauges are used to create a house climate dashboard that tracks house temperature which needs to stay at a specified level and also visualizes real-time energy consumption.")
                    { IsHighlighted = true },
                new DemoModule("CircularGaugeModule", "Circular Gauge", "In this module, Circular gauges show current time in different time zones."),
                new DemoModule("LinearGaugeModule", "Linear Gauge", "This module uses level bars and markers in a Linear Gauge to indicate web site visitor trends.")
            });
            #endregion
            //TODO: WinUI
            //#region Maps
            //var mapsGroup = new DemoModuleGroup("Maps", "Maps", "Maps", new List<DemoModule>() {
            //        new DemoModule("PhotoGallery", "Photo Gallery", "This module demonstrates how to integrate a Photo Gallery into the DevExpress UWP Map control."),
            //        new DemoModule("ShapefileSupportModule", "Shapefile Support", "This module loads Shapefile vector elements into the DevExpress UWP Map Control."),
            //        new DemoModule("DataProvidersModule", "Map Data Providers", "This module shows the use of both Bing Maps and OpenStreetMap data providers with the DevExpress UWP Map Control.")
            //});
            //#endregion
            #region Grid
            var gridGroup = new DemoModuleGroup("Grid", "Grid", "Grid", new List<DemoModule>() {
                new DemoModule("RowAlternationDemo", "Row Alternation", "This module demonstrates row alternation capabilities of the DevExpress Universal Grid Control. To enhance readability, you can highlight alternating grid rows with a different style using the AlternateRowBackground and AlternationCount properties. The AlternateRowBackgroundPalette property allows you to define multiple alternating row colors." , "This module demonstrates row alternation capabilities.")
                    { IntroducedIn = DXReleaseVersion.V172, Sources = new string[] { }},
                new DemoModule("GridRowValidation", "Input Validation", "This module demonstrates validation capabilities of the DevExpress Universal Grid Control. Validation exceptions will be raised when attempting to specify an empty product name or to set the order date after the required or shipped dates. The grid validation mechanism first checks whether the cell value satisfies the validation attributes for the corresponding property defined in the view model. Then the validation events are raised for the cell, its column and lastly its row.", "This module demonstrates validation capabilities of the DevExpress Universal Grid Control.")
                    { IntroducedIn = DXReleaseVersion.V171, Sources = new string[] { "GridDemo/Data/ValidationViewModel.cs", }},
                new DemoModule("InplaceEditing", "Cell Editors", "This module shows how to embed data editor controls (text box, date edit, track bar) into grid cells for in-place editing.")
                    { Sources = new [] { "GridDemo/Themes/InplaceEditingStyles.xaml", "GridDemo/Themes/CustomControlStyles.xaml" } },
                new DemoModule("NewItemRowModule", "New Item Row", "Displayed either above or below all data/group rows, the New Item row enables end-users to add new records to the data source.")
                    { IntroducedIn = DXReleaseVersion.V181, Sources = new [] { "GridDemo/Themes/InplaceEditingStyles.xaml", "GridDemo/Themes/CustomControlStyles.xaml" } },
                new DemoModule("CellTemplatesModule", "Custom Cell Templates", "This demo shows how to customize data cells using templates.")
                    { Sources = new [] { "GridDemo/Themes/CustomControlStyles.xaml" } },
                new DemoModule("FilteringModule", "Data Filtering", "This module allows you to explore the data filtering options provided by the DevExpress UWP Grid Control.") 
                    { UpdatedIn = DXReleaseVersion.V181, Sources = new [] { "GridDemo/Data/FilteringViewModel.cs", "GridDemo/Themes/FilteringStyles.xaml" } },
                new DemoModule("ConditionalFormattingModule", "Conditional Formatting", "The GridControl allows you to apply conditional formatting and change the appearance of individual cells and row based on specific conditions.")
                    { IntroducedIn = DXReleaseVersion.V192, IsHighlighted = true, },
                new DemoModule("GridRealTimeDataModule", "Real-Time Data", "This module demonstrates the DevExpress Grid Control’s automatic refresh feature with a frequently-updated data source.")
                    { Sources = new [] { "GridDemo/Data/StockDataViewModel.cs", "GridDemo/Themes/GridRealTimeDataStyles.xaml" } },
                new DemoModule("GroupingModule", "Data Grouping and Summary Alignment", "This demo shows how to group data and align group summaries under the corresponding columns.")
                    { Sources = new [] { "GridDemo/Controls/SalesByYearData.cs", "GridDemo/Themes/GroupingStyles.xaml" } },
                new DemoModule("MultiSelection", "Multi-Row Selection", "This module demonstrates multiple-row selection. The total summary uses only values from selected rows for calculation.")
                    { Sources = new string[] { "Data/MultiSelectionViewModel.cs" }},
                new DemoModule("CellSelection", "Multi-Cell Selection", "This module demonstrates multiple cell selection in the DevExpress UWP Grid Control.")
                    { Sources = new string[] { "GridDemo/Controls/SalesByYearData.cs" }},
                new DemoModule("GridSearchPanelModule", "Grid Search Panel", "This module demonstrates the built-in Search Panel which provides end-users with an easy way to locate information within the grid.")
                    { Sources = new [] { "GridDemo/Data/GridSearchPanelViewModel.cs" } },
                new DemoModule("VirtualSourcesModule", "Virtual Data Sources", "This module shows how to use a GridControl with an infinite virtual data source.")
                    { IntroducedIn = DXReleaseVersion.V182, IsHighlighted = true, Sources = new string[] { "GridDemo/Data/VirtualSourcesViewModel.cs", "GridDemo/Data/IssuesData.cs" } },
            });
            #endregion
            #region Editors
            var editorsGroup = new DemoModuleGroup("Editors", "Editors", "Editors", new List<DemoModule>() {
                new DemoModule("RatingPhotoGallery", "Rating Photo Gallery", "This module demonstrates the rating editor.")
                    { IsHighlighted = false, Sources = new string[] { "RatingPhotoGalleryViewModel.cs" } },
                new DemoModule("RatingEditModule", "Rating Editor Features", "This module demonstrates the features supported by a rating editor.")
                    { IsHighlighted = false, Sources = new string[] { "RatingEditViewModel.cs" } },
                new DemoModule("GeneralTextEditorModule", "Text Editor", "This module demonstrates the features supported by a text editor.")
                    { IsHighlighted = true, Sources = new string[] { "TextEditModule.xaml", "TextEditModule.xaml.cs", "ButtonEditModule.xaml", "ButtonEditModule.xaml.cs", "ButtonOnlyModule.xaml", "ButtonOnlyModule.xaml.cs" } },
                new DemoModule("MasksModule", "Mask", "This module demonstrates the masked input features, including full Regular Expression support.")
                    { IsHighlighted = true, UpdatedIn = DXReleaseVersion.V192, Sources = new string[] { "EditorsDemo/MaskModule/NumericMaskModule.xaml", "EditorsDemo/MaskModule/NumericMaskModule.xaml.cs", "EditorsDemo/MaskModule/DateTimeMaskModule.xaml", "EditorsDemo/MaskModule/DateTimeMaskModule.xaml.cs", "EditorsDemo/MaskModule/TimeSpanMaskModule.xaml", "EditorsDemo/MaskModule/TimeSpanMaskModule.xaml.cs", "EditorsDemo/MaskModule/RegExMaskModule.xaml", "EditorsDemo/MaskModule/RegExMaskModule.xaml.cs", "EditorsDemo/MaskModule/SimpleMaskModule.xaml", "SimpleMaskModule.xaml.cs", "EditorsDemo/MaskModule/RegularMaskModule.xaml", "EditorsDemo/MaskModule/RegularMaskModule.xaml.cs" } },
                new DemoModule("DateEditModule", "Date-Time Editor", "This modules demonstrates the features available in the DevExpress Date-Time Editor control."),
                new DemoModule("DateTimePickerModule", "Date-Time Picker", "This modules demonstrates the features available in the DevExpress Date-Time Picker control.")
                    { Sources = new string[] { "DatePickerModule.xaml", "DatePickerModule.xaml.cs", "TimePickerModule.xaml", "TimePickerModule.xaml.cs" } },
                new DemoModule("SpinEditModule", "Numeric Editor", "This demo shows the features available in the DevExpress SpinEdit control, including value increment/decrement and value range limitation."),
                new DemoModule("DateNavigatorModule", "DateNavigator", "This example demonstrates the features available in the DateNavigator control including range selection."),
                new DemoModule("DateNavigatorSpecialDatesModule", "DateNavigator: Cell Customization", "This demo shows how to use templates to customize date cell content and appearance.")
                    { Sources = new string[] { "EditorsDemo/Data/DateNavigatorSpecialDatesTemplateSelector.cs", "EditorsDemo/Data/DateNavigatorSpecialDatesViewModel.cs" } }
            });
            #endregion
            #region PDFVIewer
            var pdfViewerGroup = new DemoModuleGroup("PDFViewer", "PDF Viewer", "PdfViewer", new List<DemoModule>() {
                new DemoModule("SimplePdfViewerModule", "PDF Viewer", "This module demonstrates the PDF Viewer control that allows you to print a PDF document via the standard Windows Print dialog.")
                    { IsHighlighted = true, Sources = new string[] { "PdfViewerDemo/SimplePdfViewerViewModel.cs" } },
            });
            #endregion
            DemoModuleGroups.AddRange(new DemoModuleGroup[] { schedulerGroup, gridGroup, editorsGroup, ribbonGroup, layoutGroup, chartsGroup, gaugesGroup, /*mapsGroup,*/ pdfViewerGroup, controlsGroup });
        }
    }
}

