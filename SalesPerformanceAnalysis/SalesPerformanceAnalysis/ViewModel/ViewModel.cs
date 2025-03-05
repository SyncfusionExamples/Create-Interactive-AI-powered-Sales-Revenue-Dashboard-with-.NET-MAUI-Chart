using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SalesPerformanceAnalysis
{
    public class ViewModel : INotifyPropertyChanged
    {

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

        public ObservableCollection<Product> TopProducts { get; set; }

        private List<Region> _regions;
        public List<Region> Regions
        {
            get => _regions;
            set
            {
                _regions = value;
                OnPropertyChanged(nameof(Regions));
            }
        }

        public ObservableCollection<Brush> PaletteBrushes { get; set; }

        public ViewModel()
        {

            PaletteBrushes = new ObservableCollection<Brush>()
            {
               new SolidColorBrush(Color.FromArgb("#314A6E")),
                 new SolidColorBrush(Color.FromArgb("#48988B")),
                 new SolidColorBrush(Color.FromArgb("#5E498C")),
                 new SolidColorBrush(Color.FromArgb("#74BD6F")),
                 new SolidColorBrush(Color.FromArgb("#597FCA"))
            };


            ProductInfos = new()
            {
                new ProductInfo { Product = "Samsung Galaxy S3", BuyingPrice = 149.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 14, AvailabilityImage = ImageSource.FromFile("correct.png") },
                new ProductInfo { Product = "iPhone 4", BuyingPrice = 199.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 8, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Google Nexus 5", BuyingPrice = 129.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 2, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "HTC One M7", BuyingPrice = 169.99, Image = ImageSource.FromFile("smart_tv.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "BlackBerry Bold 9900", BuyingPrice = 119.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "Sony Xperia Z1", BuyingPrice = 179.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 4, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "LG G2", BuyingPrice = 149.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 16 , AvailabilityImage = ImageSource.FromFile("correct.png") },
                new ProductInfo { Product = "Nokia Lumia 920", BuyingPrice = 139.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 3 },
                new ProductInfo { Product = "Motorola Moto X (2013)", BuyingPrice = 129.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 8 , AvailabilityImage = ImageSource.FromFile("correct.png")},
                new ProductInfo { Product = "Huawei Ascend P6", BuyingPrice = 159.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 25, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Samsung Galaxy Note 4", BuyingPrice = 179.99, Image = ImageSource.FromFile("smart_tv.png"), ProductStockCount = 2, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "iPhone 5S", BuyingPrice = 199.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 28, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Laptop", BuyingPrice = 899.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 10, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Smartphone", BuyingPrice = 699.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 25, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Wireless Earbuds", BuyingPrice = 129.99, Image = ImageSource.FromFile("smart_tv.png"), ProductStockCount = 15, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Smartwatch", BuyingPrice = 199.99, Image = ImageSource.FromFile("smart_tv.png"), ProductStockCount = 20, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Bluetooth Speaker", BuyingPrice = 79.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 5 , AvailabilityImage = ImageSource.FromFile("correct.png") },
                new ProductInfo { Product = "Gaming Console", BuyingPrice = 499.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "Tablet", BuyingPrice = 349.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 12, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Monitor", BuyingPrice = 229.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 18 , AvailabilityImage = ImageSource.FromFile("correct.png")},
                new ProductInfo { Product = "Mechanical Keyboard", BuyingPrice = 129.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 22, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Gaming Mouse", BuyingPrice = 59.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 30, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "VR Headset", BuyingPrice = 399.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 7, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "External SSD", BuyingPrice = 149.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 16, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Router", BuyingPrice = 99.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "Smart TV", BuyingPrice = 599.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 14, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Webcam", BuyingPrice = 89.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 3 , AvailabilityImage = ImageSource.FromFile("correct.png") },
                new ProductInfo { Product = "Microphone", BuyingPrice = 139.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 11, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Graphics Card", BuyingPrice = 699.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 4, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Power Bank", BuyingPrice = 49.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 21 , AvailabilityImage = ImageSource.FromFile("correct.png")},
                new ProductInfo { Product = "Smart Light Bulb", BuyingPrice = 19.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 35, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Portable Projector", BuyingPrice = 259.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "Action Camera", BuyingPrice = 299.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 13, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Drone", BuyingPrice = 799.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 5 , AvailabilityImage = ImageSource.FromFile("correct.png")},
                new ProductInfo { Product = "E-Reader", BuyingPrice = 129.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 17, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "3D Printer", BuyingPrice = 599.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 2, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Wireless Charger", BuyingPrice = 39.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "Noise Cancelling Headphones", BuyingPrice = 249.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 9, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Electric Scooter", BuyingPrice = 499.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 7 , AvailabilityImage = ImageSource.FromFile("correct.png")},
                new ProductInfo { Product = "Security Camera", BuyingPrice = 179.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 12, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Graphics Tablet", BuyingPrice = 199.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 4, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Smart Thermostat", BuyingPrice = 149.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 10, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Dash Cam", BuyingPrice = 129.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "Gaming Chair", BuyingPrice = 249.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 15, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "PC Case", BuyingPrice = 89.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 5, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "CPU Cooler", BuyingPrice = 79.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 8, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Motherboard", BuyingPrice = 179.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 9, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "RAM (16GB)", BuyingPrice = 99.99, Image = ImageSource.FromFile("apple.png"), ProductStockCount = 14, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "External Hard Drive", BuyingPrice = 89.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "Streaming Stick", BuyingPrice = 49.99, Image = ImageSource.FromFile("tv_app.png"), ProductStockCount = 22, AvailabilityImage = ImageSource.FromFile("correct.png")  }
            };

            OrderInfos = MonthlyOrders(1);


            TopProducts = new ObservableCollection<Product>()
                {
                    new Product { Id = "P001", Name = "Smartphone X", Category = "Electronics", BasePrice = 999.99m, IsActive = true },
                    new Product { Id = "P002", Name = "Laptop Pro", Category = "Electronics", BasePrice = 1499.99m, IsActive = true },
                    new Product { Id = "P003", Name = "Wireless Headphones", Category = "Audio", BasePrice = 199.99m, IsActive = true },
                    new Product { Id = "P004", Name = "Smart Watch", Category = "Wearables", BasePrice = 249.99m, IsActive = true },
                    new Product { Id = "P005", Name = "Tablet Ultra", Category = "Electronics", BasePrice = 599.99m, IsActive = true }
            };

            //Regions = new List<Region>
            //{
            //    new Region { Id = "R001", Name = "North America", Country = "USA", Latitude = 40.7128, Longitude = -74.0060 , Value= 21},
            //    new Region { Id = "R002", Name = "Europe", Country = "Germany", Latitude = 52.5200, Longitude = 13.4050, Value= 31 },
            //    new Region { Id = "R003", Name = "Asia Pacific", Country = "Japan", Latitude = 35.6762, Longitude = 139.6503, Value= 21 },
            //    new Region { Id = "R004", Name = "Latin America", Country = "Brazil", Latitude = -23.5505, Longitude = -46.6333, Value= 41 }
            //};
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

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}