using System.ComponentModel;

namespace SalesPerformanceAnalysis
{
    public class ProductInfo : INotifyPropertyChanged
    {
        private string? product;
        private double buyingPrice;
        private ImageSource? image;
        private ImageSource? availabiltyImage;
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
        public double Revenue1 { get; set; }
        public double Sales { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public decimal Revenue { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit => Revenue - Cost;
        public decimal ProfitMargin => Revenue > 0 ? Profit / Revenue * 100 : 0;
    }

    public class SalesPrediction
    {
        public DateTime Date { get; set; }
        public string ProductId { get; set; }
        public string RegionId { get; set; }
        public decimal PredictedRevenue { get; set; }
        public decimal LowerBound { get; set; }
        public decimal UpperBound { get; set; }
        public decimal Confidence { get; set; }
        public string Explanation { get; set; }
        public bool IsAnomaly { get; set; }
        public string AnomalyExplanation { get; set; }
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
        public string DisplayText { get; set; }
        public DateRange Value { get; set; }
    }

}
