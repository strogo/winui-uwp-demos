using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace NavigationDemo {
    public class TilesModuleViewModel : DevExpress.Mvvm.BindableBase {
        public List<Listing> Listings { get { return NavigationDemo.Listings.DataSource; } }
        public FeatureDemo.Data.Employees Employees { get { return FeatureDemo.Data.DataStorage.Employees; } }
    }
    public class Listing {
        public string Address { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public ImageSource ImageSource {
            get { return string.IsNullOrEmpty(Image) ? null : new BitmapImage(new Uri(Image, UriKind.RelativeOrAbsolute)); }
        }
        public string Description { get; set; }
        public int Bedrooms { get; set; }
        public double Baths { get; set; }
        public int Built { get; set; }
        public int SqFootage { get; set; }
        public int PricePerSqFoot { get { return (int)(Price / SqFootage); } }
        public int LotSize { get; set; }
        public DateTime ListDate { get; set; }
        public string MLS { get; set; }
        //
        public string Kitchen { get; set; }
        public string DiningRoom { get; set; }
        public string Bedroom { get; set; }
        public bool LivingRoom { get; set; }
        public bool FamilyRoom { get; set; }
        public string FloorCoverings { get; set; }
        public bool Laundry { get; set; }
        public string Fireplace { get; set; }
        //
        public int Stories { get; set; }
        public string RoofType { get; set; }
        public string Parking { get; set; }
        public string Yard { get; set; }
        public string PoolType { get; set; }
        public string Spa { get; set; }
        //
        public string Amenities { get; set; }
        public int HOAFee { get; set; }
        public string HOAFeeIncludes { get; set; }
    }
    public static class Listings {
        public static readonly List<Listing> DataSource =
            new List<Listing> {
                new Listing {
                    Address = "6127 Kona Crest Ave, Pasadena, CA 91203", Price = 345999,
                    Image = "ms-appx:///NavigationDemo/Images/House1.jpg",
                    Description = "This wonderful condo in the Playhouse district of Pasadena features over 1,560 sq. ft. of spacious & luxurious " +
                        "living with 3 bdrm, 2 bath, secured entry, brand new carpet, recessed lighting throughout, window blinds, " +
                        "walk-in mirrored closets, 2 parking stalls, guest parking, front & back private patio. Minutes from paseo shopping center. " +
                        "This is an ideal condo in a great neighborhood. Come take a look.", 
                    Bedrooms = 3, Baths = 2, Built = 1997, SqFootage = 1567, ListDate = new DateTime(2009, 12, 25), MLS = "C09678945",
                    Kitchen = "BUILT-IN RANGE/OVEN, GAS RANGE/OVEN, DISHWASHER, MICROWAVE", DiningRoom = "DINING ELL", 
                    Bedroom = "MASTER BEDROOM, WALK-IN CLOSET", LivingRoom = true, FamilyRoom = true, FloorCoverings = "WALL TO WALL CARPET", Laundry = true, 
                    Stories = 1, RoofType = "CONCRETE TILE", Parking = "2 PARKING SPACES", Yard = "BALCONY", Spa = "COMMUNITY SAUNA",
                    Amenities = "ASSOCIATION BARBECUE", HOAFee = 250
                },
                new Listing {
                    Address = "108 Brand Blvd, Seattle, WA 98052", Price = 589000,
                    Image = "ms-appx:///NavigationDemo/Images/House2.jpg",
                    Description = "Gorgeous gated Seattle beauty with the best upgrades!! Open kitchen/family room with granite counters, " +
                        "tile backsplash. Wood floors, tile in baths - all very neutral. Plantation shutters throughout, large master with fireplace, " +
                        "marble countertops in master bath & deck off master with views. Private yard with built-in bbq. Nice!", 
                    Bedrooms = 4, Baths = 3.75, Built = 2007, SqFootage = 2590, LotSize = 7812, ListDate = new DateTime(2008, 1, 30), MLS = "W67340912",
                    Kitchen = "BUILT-IN BARBEQUE, BUILT-IN GAS RANGE, DISHWASHER, GARBAGE DISPOSAL, GAS OVEN, MICROWAVE", DiningRoom = "EATING AREA",
                    Bedroom = "MASTER BEDROOM", LivingRoom = true, FloorCoverings = "CARPET - PARTIAL, CERAMIC TILE, WOOD", Laundry = true,
                    Fireplace = "GAS, IN LIVING ROOM, IN MASTER BEDROOM", Stories = 2, RoofType = "CONCRETE TILE",
                    Parking = "DIRECT GARAGE ACCESS, ATTACHED GARAGE", Yard = "BALCONY, CONCRETE SLAB PATIO; YARD ENCLOSED WITH : BLOCK WALL",
                    PoolType = "ASSOCIATION POOL", Spa = "ASSOCIATION SPA", Amenities = "GATED COMMUNITY", HOAFee = 170, 
                    HOAFeeIncludes = "CLUB HOUSE RECREATION FACILITIES"
                },
                new Listing {
                    Address = "777 Las Vegas Blvd, Las Vegas, NV 89120", Price = 777333,
                    Image = "ms-appx:///NavigationDemo/Images/House3.jpg",
                    Description = "Loft condominium. New construction. 2-story open floorplan with spiral staircase, high ceilings, " +
                        "large windows and 2 balconies (5 units left). Unit feature fine italian kitchens with caesar stone counter tops, " +
                        "limited stainless steel appliances, custom designed bathroom sink/cabinets with tile floor and bathtub surround. " +
                        "2 side by side parking spaces. Security system, video intercom system, keyfob entry to building, HD DirecTV and cable pre-wired.", 
                    Bedrooms = 2, Baths = 2.5, Built = 2009, SqFootage = 3100, ListDate = new DateTime(2009, 4, 5), MLS = "N55200987",
                    Kitchen = "DISHWASHER, GARBAGE DISPOSAL, MICROWAVE, RANGE/OVEN", DiningRoom = "BREAKFAST AREA",
                    LivingRoom = true, FloorCoverings = "HARDWOOD, TILE", Laundry = false, Fireplace = "GAS, IN DINING ROOM", Stories = 2,
                    Parking = "COMMUNITY GARAGE, SIDE BY SIDE, SUBTERRANEAN", Yard = "OPEN PATIO", Spa = "ROOFTOP SPA",
                    Amenities = "ALARM SYSTEM, CABLE, INTERCOM, NETWORK WIRE, SATELLITE, GATED SECURITY", HOAFee = 314
                }
            };
    }
}
