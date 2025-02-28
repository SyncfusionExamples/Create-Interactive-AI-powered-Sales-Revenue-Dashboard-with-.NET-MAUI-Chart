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
    }
}
