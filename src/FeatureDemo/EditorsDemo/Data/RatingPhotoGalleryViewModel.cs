using DevExpress.Mvvm;
using FeatureDemo.Data;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace EditorsDemo {
    public class RatingPhotoGalleryViewModel : ViewModelBase {
        public List<ImageInfoViewModel> ImageInfos { get; } = new List<ImageInfoViewModel>();
        public double FilterRating {
            get { return GetProperty<double>(); }
            set { SetProperty(value, ApplyFilter); }
        }
        public RatingPhotoGalleryViewModel() {
            FillImageInfos();
            FilterRating = 1;
        }
        void FillImageInfos() {
            Random rand = new Random();
            foreach(string filePath in DataStorage.CitiesPhotoLibrary) {
                ImageInfos.Add(new ImageInfoViewModel() {
                    ImageSource = filePath,
                    FileName = string.Format("photo{0}.jpg", rand.Next(1000, 9999)),
                    Rating = (double)rand.Next(5) + 1,
                    IsVisible = true
                });
            }
        }
        bool IsVisible(ImageInfoViewModel p) {
            return double.IsNaN(FilterRating) || p.Rating == FilterRating;
        }
        private void ApplyFilter() {
            ImageInfos.ForEach(x => x.IsVisible = x.Rating >= FilterRating);
        }
    }
    public class ImageInfoViewModel : ViewModelBase {
        public string ImageSource { get; set; }
        public string FileName { get; set; }
        public bool IsVisible {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        public double Rating { get; set; }
    }
}