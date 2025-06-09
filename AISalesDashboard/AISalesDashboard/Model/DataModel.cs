using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AISalesDashboard
{
    public enum Status
    {
        Delivered, Shipped, Cancelled, Refunded
    }

    public class ExportOption
    {
        public string? Name { get; set; }
        public string? Icon { get; set; }
    }

    public class ProductInfo : INotifyPropertyChanged
    {
        private string? product;
        private double buyingPrice;
        private ImageSource? image;
        private ImageSource? availabiltyImage;
        private double productStockCount;
        private bool availability;
        private int productRating;
        private int productID;
        private int totalSales;
        
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

        public ImageSource? AvailabilityImage
        {
            get => availabiltyImage;
            set { availabiltyImage = value; OnPropertyChanged(nameof(AvailabilityImage)); }
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

        public int ProductRating
        {
            get => productRating;
            set
            {
                productRating = value;
                OnPropertyChanged(nameof(ProductRating));
            }
        }

        public int ProductID
        {
            get => productID;
            set
            {
                productID = value;
                OnPropertyChanged(nameof(ProductID));
            }
        }

        public int TotalSales
        {
            get => totalSales;
            set
            {
                totalSales = value;
                OnPropertyChanged(nameof(TotalSales));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OrderInfo
    {
        public int Id { get; set; }
        public string? Product { get; set; }
        public ImageSource? Image { get; set; }
        public Brush? Brush { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderDate { get; set; }
        public string? Customer { get; set; }
        public double MarginValue { get; set; }
        public double ActualPrice { get; set; }
        public double SellingPrice { get; set; }
    }

    public class SalesData
    {
        public DateTime Date { get; set; }
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? RegionId { get; set; }
        public string? RegionName { get; set; }
        public decimal Revenue { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit => Revenue - Cost;
        public decimal ProfitMargin => Revenue > 0 ? Profit / Revenue * 100 : 0;
        public decimal ProfitPercent => Revenue > 0 ? Profit / Revenue : 0;
        public string? Icon { get; set; }
    }

    public class SalesPrediction
    {
        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("ProductId")]
        public string? ProductId { get; set; }

        [JsonPropertyName("RegionId")]
        public string? RegionId { get; set; }

        [JsonPropertyName("PredictedRevenue")]
        public decimal PredictedRevenue { get; set; }

        [JsonPropertyName("LowerBound")]
        public decimal LowerBound { get; set; }

        [JsonPropertyName("UpperBound")]
        public decimal UpperBound { get; set; }

        [JsonPropertyName("Confidence")]
        public decimal Confidence { get; set; }

        [JsonPropertyName("Explanation")]
        public string? Explanation { get; set; }

        [JsonPropertyName("IsAnomaly")]
        public bool IsAnomaly { get; set; }

        [JsonPropertyName("AnomalyExplanation")]
        public string? AnomalyExplanation { get; set; }
    }


    public class Product
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public string? Icon { get; set; }
    }

    public class Region
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }

    public class DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public static DateRange Last30Days => new(DateTime.Now.AddDays(-30), DateTime.Now);
        public static DateRange LastQuarter => new(DateTime.Now.AddMonths(-3), DateTime.Now);
        public static DateRange LastYear => new(DateTime.Now.AddYears(-1), DateTime.Now);
        public static DateRange YearToDate
        {
            get
            {
                var now = DateTime.Now;
                return new DateRange(new DateTime(now.Year, 1, 1), now);
            }
        }
    }

    public class DateRangeOption
    {
        public string? DisplayText { get; set; }
        public DateRange? Value { get; set; }
    }

    public class PageInfo : INotifyPropertyChanged
    {
        public string? Title { get; set; }
        public string? PageIcon { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SegmentChangedInfo
    {
        public string? ImageChange { get; set; } 
    }

    public class DashboardLabelCardModel
    {
        public string? Title { get; set; }
        public string? Value { get; set; }
        public string? Icon { get; set; }
        public Color? IconColor { get; set; }
        public Color? ValueColor { get; set; }
        public Color? Background { get; set; }
    }
}