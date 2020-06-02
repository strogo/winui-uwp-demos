using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

using PropertyChangedEventHandler = Microsoft.UI.Xaml.Data.PropertyChangedEventHandler;
using INotifyPropertyChanged = Microsoft.UI.Xaml.Data.INotifyPropertyChanged;
using PropertyChangedEventArgs = Microsoft.UI.Xaml.Data.PropertyChangedEventArgs;

namespace FeatureDemo.Data {
    [XmlRoot("StockItems")]
    public class StockItems : List<StockItem> {
    }
    [XmlRoot("StockItem")]
    public class StockItem : INotifyPropertyChanged {
        internal const int HistoryLength = 10;

        string companyName;
        public string CompanyName {
            get { return companyName; }
            set {
                companyName = value;
                OnPropertyChanged("CompanyName");
            }
        }
        double price;
        public double Price {
            get { return price; }
            set {
                price = value;
                OnPropertyChanged("Price");
                if(LowPrice > value)
                    LowPrice = value;
                if(HighPrice < value)
                    HighPrice = value;
            }
        }
        int volume;
        public int Volume {
            get { return volume; }
            set {
                volume = value;
                OnPropertyChanged("Volume");
            }
        }
        double lowPrice;
        public double LowPrice {
            get { return lowPrice; }
            set {
                lowPrice = value;
                OnPropertyChanged("LowPrice");
            }
        }
        double highPrice;
        public double HighPrice {
            get { return highPrice; }
            set {
                highPrice = value;
                OnPropertyChanged("HighPrice");
            }
        }

        double deltaPrice;
        public double DeltaPrice {
            get { return deltaPrice; }
            set {
                deltaPrice = value;
                Price += value;
                OnPropertyChanged("DeltaPrice");
            }
        }
        int deltaPriceSignChange;
        public int DeltaPriceSignChange {
            get { return deltaPriceSignChange; }
            set {
                deltaPriceSignChange = value;
                OnPropertyChanged("DeltaPriceSignChange");
            }
        }

        double deltaPricePercent;
        public double DeltaPricePercent {
            get { return deltaPricePercent; }
            set {
                deltaPricePercent = value;
                OnPropertyChanged("DeltaPricePercent");
            }
        }


        List<double> priceHistory = new List<double>(new double[HistoryLength]);
        public List<double> PriceHistory {
            get { return priceHistory; }
            set {
                priceHistory = value;
                OnPropertyChanged("PriceHistory");
            }
        }

        public void UpdatePrice(Random rnd) {
            var oldDeltaPrice = DeltaPrice;
            DeltaPrice = rnd.NextDouble() * 0.5 - 0.25;
            DeltaPricePercent = (Price + DeltaPrice) / Price * 100 - 100;
            DeltaPriceSignChange = Comparer<int>.Default.Compare(Math.Sign(DeltaPrice), Math.Sign(oldDeltaPrice));
            Volume += Convert.ToInt32(rnd.NextDouble() * Volume * 0.005 - 0.0025);
            UpdatePriceHistory();
        }

        void UpdatePriceHistory() {
            List<double> newPriceHistory = new List<double>(new double[HistoryLength]);
            for(int i = 1; i < HistoryLength; i++)
                newPriceHistory[i - 1] = PriceHistory[i];
            newPriceHistory[HistoryLength - 1] = Price;
            PriceHistory = newPriceHistory;
        }

        void OnPropertyChanged(string propName) {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class StockItemsData {
        public StockItems DataSource {
            get { return StockDataStorage.MyStockItems; }
        }
    }

    public static class StockDataStorage {
        static StockItems myStockItems;
        public static StockItems MyStockItems {
            get {
                if(myStockItems == null) {
                    StorageFile file = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/StockSource.xml")).AsTask().Result;
                    Stream stream = file.OpenStreamForReadAsync().Result;
                    XmlSerializer serializier = new XmlSerializer(typeof(StockItems));
                    myStockItems = serializier.Deserialize(stream) as StockItems;
                }
                return myStockItems;
            }
        }
    }
}
