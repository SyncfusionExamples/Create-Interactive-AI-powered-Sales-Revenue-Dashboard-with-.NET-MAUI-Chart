using Syncfusion.Maui.Toolkit.Charts;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SalesPerformanceAnalysis
{
    public class ViewModel : INotifyPropertyChanged
    {
        string text;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        private ObservableCollection<SalesData> revenueData;

        public ObservableCollection<SalesData> RevenueData
        {
            get => revenueData;
            set
            {
                revenueData = value;
                OnPropertyChanged(nameof(RevenueData));
            }
        }

        private ObservableCollection<SalesData> salesReport;

        public ObservableCollection<SalesData> SalesReport
        {
            get => salesReport;
            set
            {
                salesReport = value;
                OnPropertyChanged(nameof(SalesReport));
            }
        }

        double autoScrollingDelta = 1;

        public double AutoScrollingDelta
        {
            get
            {
                return autoScrollingDelta;
            }
            set
            {
                if (autoScrollingDelta != value)
                {
                    autoScrollingDelta = value;
                    OnPropertyChanged(nameof(AutoScrollingDelta));
                }
            }
        }

        private ObservableCollection<ProductInfo> productInfos;

        public ObservableCollection<ProductInfo> ProductInfos
        {
            get
            {
                return productInfos;
            }
            set
            {
                if (productInfos != value)
                {
                    productInfos = value;
                    OnPropertyChanged(nameof(ProductInfos));
                }
            }
        }

        private ObservableCollection<OrderInfo> orderInfos;

        public ObservableCollection<OrderInfo> OrderInfos
        {
            get
            {
                return orderInfos;
            }
            set
            {
                if (orderInfos != value)
                {
                    orderInfos = value;
                    OnPropertyChanged(nameof(OrderInfos));
                }
            }
        }

        string averageRevenue;

        public string AverageRevenue
        {
            get
            {
                return averageRevenue;
            }
            set
            {
                if (averageRevenue != null)
                {
                    averageRevenue = value;
                    OnPropertyChanged(nameof(AverageRevenue));
                }
            }
        }

        public ObservableCollection<string> ComboBoxData { get; set; }

    
        private string selectedItem = "Month";
        public string SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private Random random;

        public ViewModel()
        {
            random = new Random();
        
            AutoScrollingDelta = 1;

            averageRevenue = string.Empty;

            ProductInfos = new()
            {
                new ProductInfo { Product = "Samsung Galaxy S3", BuyingPrice = 149.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 14 },
                new ProductInfo { Product = "iPhone 4", BuyingPrice = 199.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 8 },
                new ProductInfo { Product = "Google Nexus 5", BuyingPrice = 129.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 2 },
                new ProductInfo { Product = "HTC One M7", BuyingPrice = 169.99, Image = ImageSource.FromFile("smart_tv.png"), ProductStockCount = 0 },
                new ProductInfo { Product = "BlackBerry Bold 9900", BuyingPrice = 119.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 0 },
                new ProductInfo { Product = "Sony Xperia Z1", BuyingPrice = 179.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 4 },
                new ProductInfo { Product = "LG G2", BuyingPrice = 149.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 16 },
                new ProductInfo { Product = "Nokia Lumia 920", BuyingPrice = 139.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 3 },
                new ProductInfo { Product = "Motorola Moto X (2013)", BuyingPrice = 129.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 8 },
                new ProductInfo { Product = "Huawei Ascend P6", BuyingPrice = 159.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 25 },
                new ProductInfo { Product = "Samsung Galaxy Note 4", BuyingPrice = 179.99, Image = ImageSource.FromFile("smart_tv.png"), ProductStockCount = 2 },
                new ProductInfo { Product = "iPhone 5S", BuyingPrice = 199.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 28 },
                new ProductInfo { Product = "Laptop", BuyingPrice = 899.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 10 },
                new ProductInfo { Product = "Smartphone", BuyingPrice = 699.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 25 },
                new ProductInfo { Product = "Wireless Earbuds", BuyingPrice = 129.99, Image = ImageSource.FromFile("smart_tv.png"), ProductStockCount = 15 },
                new ProductInfo { Product = "Smartwatch", BuyingPrice = 199.99, Image = ImageSource.FromFile("smart_tv.png"), ProductStockCount = 20 },
                new ProductInfo { Product = "Bluetooth Speaker", BuyingPrice = 79.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 5 },
                new ProductInfo { Product = "Gaming Console", BuyingPrice = 499.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 0 },
                new ProductInfo { Product = "Tablet", BuyingPrice = 349.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 12 },
                new ProductInfo { Product = "Monitor", BuyingPrice = 229.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 18 },
                new ProductInfo { Product = "Mechanical Keyboard", BuyingPrice = 129.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 22 },
                new ProductInfo { Product = "Gaming Mouse", BuyingPrice = 59.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 30 },
                new ProductInfo { Product = "VR Headset", BuyingPrice = 399.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 7 },
                new ProductInfo { Product = "External SSD", BuyingPrice = 149.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 16 },
                new ProductInfo { Product = "Router", BuyingPrice = 99.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 0 },
                new ProductInfo { Product = "Smart TV", BuyingPrice = 599.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 14 },
                new ProductInfo { Product = "Webcam", BuyingPrice = 89.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 3 },
                new ProductInfo { Product = "Microphone", BuyingPrice = 139.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 11 },
                new ProductInfo { Product = "Graphics Card", BuyingPrice = 699.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 4 },
                new ProductInfo { Product = "Power Bank", BuyingPrice = 49.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 21 },
                new ProductInfo { Product = "Smart Light Bulb", BuyingPrice = 19.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 35 },
                new ProductInfo { Product = "Portable Projector", BuyingPrice = 259.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 0 },
                new ProductInfo { Product = "Action Camera", BuyingPrice = 299.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 13 },
                new ProductInfo { Product = "Drone", BuyingPrice = 799.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 5 },
                new ProductInfo { Product = "E-Reader", BuyingPrice = 129.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 17 },
                new ProductInfo { Product = "3D Printer", BuyingPrice = 599.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 2 },
                new ProductInfo { Product = "Wireless Charger", BuyingPrice = 39.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 0 },
                new ProductInfo { Product = "Noise Cancelling Headphones", BuyingPrice = 249.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 9 },
                new ProductInfo { Product = "Electric Scooter", BuyingPrice = 499.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 7 },
                new ProductInfo { Product = "Security Camera", BuyingPrice = 179.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 12 },
                new ProductInfo { Product = "Graphics Tablet", BuyingPrice = 199.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 4 },
                new ProductInfo { Product = "Smart Thermostat", BuyingPrice = 149.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 10 },
                new ProductInfo { Product = "Dash Cam", BuyingPrice = 129.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 0 },
                new ProductInfo { Product = "Gaming Chair", BuyingPrice = 249.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 15 },
                new ProductInfo { Product = "PC Case", BuyingPrice = 89.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 5 },
                new ProductInfo { Product = "CPU Cooler", BuyingPrice = 79.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 8 },
                new ProductInfo { Product = "Motherboard", BuyingPrice = 179.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 9 },
                new ProductInfo { Product = "RAM (16GB)", BuyingPrice = 99.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 14 },
                new ProductInfo { Product = "External Hard Drive", BuyingPrice = 89.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 0 },
                new ProductInfo { Product = "Streaming Stick", BuyingPrice = 49.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 22 }
            };

            OrderInfos = MonthlyOrders(1);

            ComboBoxData = new ObservableCollection<string>
            {
                "Year",
                "Month",
                "Week"
            };

            SalesReport = GenerateMonthlyData(1);
        }

        public ObservableCollection<OrderInfo> YearlyOrders()
        {
            var yearlyOrders = new ObservableCollection<OrderInfo>();
            var random = new Random();

            for (int i = 0; i < 30; i++)
            {
                var product = ProductInfos[random.Next(ProductInfos.Count)];
                double marginValue = random.NextDouble() * 20; // Randomly generate a margin value
                yearlyOrders.Add(new OrderInfo
                {
                    Product = product.Product,
                    Image = product.Image,
                    ActualPrice = product.BuyingPrice,
                    MarginValue = marginValue,
                    SellingPrice = product.BuyingPrice + marginValue // Calculate the SellingPrice
                });
            }

            return yearlyOrders;
        }

        public ObservableCollection<OrderInfo> MonthlyOrders(int month)
        {
            var monthlyOrders = new ObservableCollection<OrderInfo>();
            var random = new Random();

            for (int i = 0; i < 15; i++)
            {
                var product = ProductInfos[random.Next(ProductInfos.Count)];
                double marginValue = random.NextDouble() * 20;
                monthlyOrders.Add(new OrderInfo
                {
                    Product = product.Product,
                    Image = product.Image,
                    ActualPrice = product.BuyingPrice,
                    MarginValue = marginValue,
                    SellingPrice = product.BuyingPrice + marginValue
                });
            }

            return monthlyOrders;
        }

        public ObservableCollection<OrderInfo> WeeklyOrders()
        {
            var weeklyOrders = new ObservableCollection<OrderInfo>();
            var random = new Random();

            for (int i = 0; i < 8; i++)
            {
                var product = ProductInfos[random.Next(ProductInfos.Count)];
                double marginValue = random.NextDouble() * 20;
                weeklyOrders.Add(new OrderInfo
                {
                    Product = product.Product,
                    Image = product.Image,
                    ActualPrice = product.BuyingPrice,
                    MarginValue = marginValue,
                    SellingPrice = product.BuyingPrice + marginValue
                });
            }

            return weeklyOrders;
        }

        public ObservableCollection<SalesData> GenerateYearlyData()
        {
            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var yearlyData = months.Select((month, index) => new SalesData
            {
                Category = month,
                Revenue = Math.Round(random.NextDouble() * 10000, 2)
            }).ToList();

            return new ObservableCollection<SalesData>(yearlyData);
        }

        public ObservableCollection<SalesData> GenerateMonthlyData(int month)
        {
            int daysInMonth = DateTime.DaysInMonth(2024, month);
            var monthlyData = Enumerable.Range(1, daysInMonth)
                .Select(day => new SalesData
                {
                    Category = $"Jan {day}",
                    Revenue = Math.Round(random.NextDouble() * 10000, 2)
                })
                .ToList();

            return new ObservableCollection<SalesData>(monthlyData);
        }

        public ObservableCollection<SalesData> GenerateWeeklyData()
        {
            var weeklyData = new List<SalesData>();

            for (int day = 1; day <= 7; day++) 
            {
                weeklyData.Add(new SalesData
                {
                    Category = $"Jan {day}",
                    Revenue = Math.Round(random.NextDouble() * 10000, 2)
                });
            }

            return new ObservableCollection<SalesData>(weeklyData);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
       
        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}