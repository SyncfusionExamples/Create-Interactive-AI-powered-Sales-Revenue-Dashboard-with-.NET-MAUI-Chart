using System.ComponentModel;

namespace SalesPerformanceAnalysis
{
    public class ProductInfo : INotifyPropertyChanged
    {
        private string? product;
        private double buyingPrice;
        private ImageSource? image;
        private double productStockCount;
        private bool availability;

        public string? Product
        {
            get => product;
            set { product = value; OnPropertyChanged(nameof(Product)); }
        }

        public double BuyingPrice
        {
            get => buyingPrice;
            set { buyingPrice = value; OnPropertyChanged(nameof(BuyingPrice)); }
        }

        public ImageSource? Image
        {
            get => image;
            set { image = value; OnPropertyChanged(nameof(Image)); }
        }

        public double ProductStockCount
        {
            get => productStockCount;
            set
            {
                productStockCount = value;
                Availability = productStockCount > 0;
                OnPropertyChanged(nameof(ProductStockCount));
            }
        }

        public bool Availability
        {
            get => availability;
            private set { availability = value; OnPropertyChanged(nameof(Availability)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OrderInfo
    {
        public string? Product { get; set; }
        public ImageSource? Image { get; set; }
        public double MarginValue { get; set; } 
        public double ActualPrice { get; set; }
        public double SellingPrice { get; set; }
    }

    public class SalesData
    {

        public string? Category { get; set; } // Instead of DateTime
        public double Revenue { get; set; }
        public double Sales { get; set; }
    }

    public class ExportOption
    {
        public string? Name { get; set; }
        public string? Icon { get; set; } // Path to the image
    }

    public class Product
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
    }

    public class Region
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Value { get; set; }
    }

}
