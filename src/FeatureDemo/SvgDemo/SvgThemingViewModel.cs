using System;
using System.Collections.Generic;
using DevExpress.Core;
using DevExpress.Mvvm;
using DevExpress.Themes.Internal;
using Microsoft.UI;
using Color = Windows.UI.Color;

namespace SvgDemo {
    public class SvgThemingViewModel : NavigationViewModelBase {
        public List<SvgDemoIconViewModel> Icons { get; }
        public List<SvgDemoIconViewModel> IconsWithCustomPalette { get; }
        public List<SvgPaletteViewModel> CustomPalettes { get; }
        public List<SvgPaletteViewModel> DefaultPalettes { get; }

        public SvgPaletteViewModel SelectedCustomPalette {
            get { return GetProperty<SvgPaletteViewModel>(); }
            set { SetProperty(value, OnCustomPaletteChanged); }
        }
        public SvgPaletteViewModel SelectedDefaultPalette {
            get { return GetProperty<SvgPaletteViewModel>(); }
            set { SetProperty(value, OnDefaultPaletteChanged); }
        }

        public SvgThemingViewModel() {
            Icons = SvgDemoIconViewModel.CreateDefaultIconSet(true);
            IconsWithCustomPalette = SvgDemoIconViewModel.CreateDefaultIconSet(true);
            DefaultPalettes = CreateDefaultPalettes();
            CustomPalettes = CreateDefaultPalettes();
            SelectedDefaultPalette = DefaultPalettes[0];
            SelectedCustomPalette = CustomPalettes[0];
        }

        void OnDefaultPaletteChanged() {
            SvgPalette.DefaultPalette = SelectedDefaultPalette?.Palette;
        }
        void OnCustomPaletteChanged() {
            var palette = SelectedCustomPalette?.Palette;
            IconsWithCustomPalette.ForEach(x => x.Palette = palette);
        }

        static List<SvgPaletteViewModel> CreateDefaultPalettes() {
            var result = new List<SvgPaletteViewModel>();
            result.Add(new SvgPaletteViewModel(null, "None"));

            if(IsInDesignMode)
                return result;

            var inversePalette = new SvgPalette();
            var grayScale = new SvgPalette();
            for(int i = 0; i< colorNames.Count; ++i) {
                var inverseColor = Inverse(colors[i]);
                var luGrayscale = Grayscale(colors[i]);
                var invertGrayscale = Inverse(luGrayscale);
                //ColorParser.TryGetKnownColor
                inversePalette[colorNames[i]] = new SvgColorSet() { Default = inverseColor, Light = inverseColor };
                grayScale[colorNames[i]] = new SvgColorSet() { Default = invertGrayscale, Light = luGrayscale };
            }
            result.Add(new SvgPaletteViewModel(inversePalette, "Inverse"));
            result.Add(new SvgPaletteViewModel(grayScale, "Grayscale"));
            return result;
        }
        static Color Inverse(Color color) {
            return Color.FromArgb(255, (byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));
        }
        static Color Grayscale(Color color) {
            var gray = (byte)(0.21 * color.R + 0.72 * color.G + 0.07 * color.B);
            return Color.FromArgb(255, gray, gray, gray);
        }
        static List<string> colorNames = new List<string> { "white", "cadetblue", "green", "royalblue", "red", "gold" };
        static List<Color> colors = new List<Color> { Colors.White, Colors.CadetBlue, Colors.Green, Colors.RoyalBlue, Colors.Red, Colors.Gold };
    }
    public class SvgPaletteViewModel : BindableBase {
        public SvgPalette Palette { get; }
        public string Name {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public SvgPaletteViewModel(SvgPalette palette, string name) {
            Palette = palette;
            Name = name;
        }
    }
}
