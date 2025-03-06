
using Syncfusion.Maui.AIAssistView;
using Syncfusion.Maui.Toolkit.TabView;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SalesDashboard
{
    public class SalesTrendsViewModel : BaseViewModel
    {
        #region Properties

        private readonly SalesDataService _salesDataService;

        private ObservableCollection<Product> _products = new();
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private ObservableCollection<Region> _regions = new();
        public ObservableCollection<Region> Regions
        {
            get => _regions;
            set
            {
                _regions = value;
                OnPropertyChanged(nameof(Regions));
            }
        }
   
        private ObservableCollection<SalesData> _salesData = new();
        public ObservableCollection<SalesData> SalesData
        {
            get => _salesData;
            set
            {
                _salesData = value;
                OnPropertyChanged(nameof(SalesData));
            }
        }

        private ObservableCollection<SalesData> _filteredSalesData = new();
        public ObservableCollection<SalesData> FilteredSalesData
        {
            get => _filteredSalesData;
            set
            {
                _filteredSalesData = value;
                OnPropertyChanged(nameof(FilteredSalesData));
            }
        }

        private List<DateRangeOption> dateRanges;
        public List<DateRangeOption> DateRanges
        {
            get => dateRanges;
            set
            {
                dateRanges = value;
                OnPropertyChanged(nameof(DateRanges));
            }
        }

        private decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set
            {
                _totalRevenue = value;
                OnPropertyChanged(nameof(TotalRevenue));
            }
        }

        private decimal _totalProfit;
        public decimal TotalProfit
        {
            get => _totalProfit;
            set
            {
                _totalProfit = value;
                OnPropertyChanged(nameof(TotalProfit));
            }
        }

        private double _profitMargin;
        public double ProfitMargin
        {
            get => _profitMargin;
            set
            {
                _profitMargin = value;
                OnPropertyChanged(nameof(ProfitMargin));
            }
        }

        private double _growthRate;
        public double GrowthRate
        {
            get => _growthRate;
            set
            {
                _growthRate = value;
                OnPropertyChanged(nameof(GrowthRate));
            }
        }

        private TabItemCollection items;
        public TabItemCollection Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private bool showAssistView;
        public bool ShowAssistView
        {
            get => showAssistView;
            set
            {
                showAssistView = value;
                OnPropertyChanged(nameof(ShowAssistView));
            }
        }

        private ObservableCollection<IAssistItem> messages = new();
        public ObservableCollection<IAssistItem> Messages
        {
            get => messages;
            set
            {
                messages = value;
                OnPropertyChanged(nameof(Messages));
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

        public ObservableCollection<Brush> PaletteBrushes { get; set; }

        private ObservableCollection<ExportOption> exportOptions;
        public ObservableCollection<ExportOption> ExportOptions
        {
            get { return exportOptions; }
            set
            {
                exportOptions = value;
                OnPropertyChanged(nameof(ExportOptions));
            }
        }

        private ObservableCollection<CustomMarker> mapMarker;
        public ObservableCollection<CustomMarker> MapMarkerCollection
        {
            get => mapMarker;
            set
            {
                mapMarker = value;
                OnPropertyChanged(nameof(MapMarkerCollection));
            }
        }

        #endregion

        #region Constructor

        public SalesTrendsViewModel(SalesDataService salesDataService)
        {

            ExportOptions = new ObservableCollection<ExportOption>
            {
                new ExportOption { Name = "Export Excel", Icon = "excel.png" },
                new ExportOption { Name = "Export Pdf", Icon = "pdf.png" }
            };

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
                new ProductInfo { Product = "Samsung Galaxy S3", BuyingPrice = 149.99, Image = ImageSource.FromFile("mobile.png"), ProductStockCount = 14, AvailabilityImage = ImageSource.FromFile("correct.png") },
                new ProductInfo { Product = "iPhone 4", BuyingPrice = 199.99, Image = ImageSource.FromFile("mobile.png"), ProductStockCount = 8, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Google Nexus 5", BuyingPrice = 129.99, Image = ImageSource.FromFile("mobile.png"), ProductStockCount = 2, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "HTC One M7", BuyingPrice = 169.99, Image = ImageSource.FromFile("smartlight.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "BlackBerry Bold 9900", BuyingPrice = 119.99, Image = ImageSource.FromFile("mobile.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "Sony Xperia Z1", BuyingPrice = 179.99, Image = ImageSource.FromFile("monitor.png"), ProductStockCount = 4, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "LG G2", BuyingPrice = 149.99, Image = ImageSource.FromFile("mobile.png"), ProductStockCount = 16 , AvailabilityImage = ImageSource.FromFile("correct.png") },
                new ProductInfo { Product = "Nokia Lumia 920", BuyingPrice = 139.99, Image = ImageSource.FromFile("headset.png"), ProductStockCount = 3 },
                new ProductInfo { Product = "Motorola Moto X (2013)", BuyingPrice = 129.99, Image = ImageSource.FromFile("gaming.png"), ProductStockCount = 8 , AvailabilityImage = ImageSource.FromFile("correct.png")},
                new ProductInfo { Product = "Huawei Ascend P6", BuyingPrice = 159.99, Image = ImageSource.FromFile("webcam.png"), ProductStockCount = 25, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Samsung Galaxy Note 4", BuyingPrice = 179.99, Image = ImageSource.FromFile("headset.png"), ProductStockCount = 2, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "iPhone 5S", BuyingPrice = 199.99, Image = ImageSource.FromFile("monitor.png"), ProductStockCount = 28, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Laptop", BuyingPrice = 899.99, Image = ImageSource.FromFile("monitor.png"), ProductStockCount = 10, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Smartphone", BuyingPrice = 699.99, Image = ImageSource.FromFile("mobile.png"), ProductStockCount = 25, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Wireless Earbuds", BuyingPrice = 129.99, Image = ImageSource.FromFile("headset.png"), ProductStockCount = 15, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Smartwatch", BuyingPrice = 199.99, Image = ImageSource.FromFile("watch.png"), ProductStockCount = 20, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Bluetooth Speaker", BuyingPrice = 79.99, Image = ImageSource.FromFile("gaming.png"), ProductStockCount = 5 , AvailabilityImage = ImageSource.FromFile("correct.png") },
                new ProductInfo { Product = "Gaming Console", BuyingPrice = 499.99, Image = ImageSource.FromFile("monitor.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "Tablet", BuyingPrice = 349.99, Image = ImageSource.FromFile("hard_drive.png"), ProductStockCount = 12, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Monitor", BuyingPrice = 229.99, Image = ImageSource.FromFile("webcam.png"), ProductStockCount = 18 , AvailabilityImage = ImageSource.FromFile("correct.png")},
                new ProductInfo { Product = "Mechanical Keyboard", BuyingPrice = 129.99, Image = ImageSource.FromFile("monitor.png"), ProductStockCount = 22, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Gaming Mouse", BuyingPrice = 59.99, Image = ImageSource.FromFile("gaming.png"), ProductStockCount = 30, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "VR Headset", BuyingPrice = 399.99, Image = ImageSource.FromFile("smartlight.png"), ProductStockCount = 7, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "External SSD", BuyingPrice = 149.99, Image = ImageSource.FromFile("hard_drive.png"), ProductStockCount = 16, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Router", BuyingPrice = 99.99, Image = ImageSource.FromFile("gaming.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png")  },
                new ProductInfo { Product = "Smart TV", BuyingPrice = 599.99, Image = ImageSource.FromFile("monitor.png"), ProductStockCount = 14, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Webcam", BuyingPrice = 89.99, Image = ImageSource.FromFile("webcam.png"), ProductStockCount = 3 , AvailabilityImage = ImageSource.FromFile("correct.png") },
                new ProductInfo { Product = "Microphone", BuyingPrice = 139.99, Image = ImageSource.FromFile("headset.png"), ProductStockCount = 11, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Graphics Card", BuyingPrice = 699.99, Image = ImageSource.FromFile("gaming.png"), ProductStockCount = 4, AvailabilityImage = ImageSource.FromFile("correct.png")  },
                new ProductInfo { Product = "Power Bank", BuyingPrice = 49.99, Image = ImageSource.FromFile("webcam.png"), ProductStockCount = 21 , AvailabilityImage = ImageSource.FromFile("correct.png")},

            };

            OrderInfos = Orders(1);
            MapMarkerCollection = new ObservableCollection<CustomMarker>();
            _salesDataService = salesDataService;
            Initialize();
            LoadDashboardData();

            DateRanges = new List<DateRangeOption>
            {
                new DateRangeOption { DisplayText = "Last 30 Days", Value = DateRange.Last30Days },
                new DateRangeOption { DisplayText = "Last Quarter", Value = DateRange.LastQuarter },
                new DateRangeOption { DisplayText = "Year to Date", Value = DateRange.YearToDate },
                new DateRangeOption { DisplayText = "Last Year", Value = DateRange.LastYear }
            };
        }

        #endregion

        #region Methods

        private ObservableCollection<OrderInfo> Orders(int month)
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

        public async Task LoadDashboardData()
        {
            // Load financial summary
            var summary = await _salesDataService.GetSalesSummaryAsync(SelectedDateRange);
            TotalRevenue = summary["TotalRevenue"];
            TotalProfit = summary["TotalProfit"];
            ProfitMargin = (double)summary["AverageProfitMargin"];
            GrowthRate = (double)summary["RevenueGrowth"];
        }

        public async Task Initialize()
        {
            try
            {
                var products = await _salesDataService.GetProductsAsync();
                Products = new ObservableCollection<Product>(products);

                var regions = await _salesDataService.GetRegionsAsync();
                Regions = new ObservableCollection<Region>(regions);

                // adding map marker collection

                foreach (var region in regions)
                {
                    MapMarkerCollection.Add(new CustomMarker()
                    {
                        Latitude = region.Latitude,
                        Longitude = region.Longitude,
                        Name = region.Country,
                    });
                }

                await LoadSalesData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating predictions: {ex.Message}");

            }
        }

        public async Task LoadSalesData()
        {
            try
            {
                var data = await LoadSalesDataAsync();

                SalesData = new ObservableCollection<SalesData>(data);
                await ApplyFilters();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating predictions: {ex.Message}");
            }
        }

        private async Task<List<SalesData>> LoadSalesDataAsync()
        {
            if (SelectedDateRange != null)
            {
                var data = await _salesDataService.GetSalesDataAsync(
                    SelectedDateRange);

                return data;
            }

            return new List<SalesData>();
        }

        public async Task ApplyFilters()
        {
            if (SalesData.Count == 0)
                return;

            var filtered = SalesData.ToList();

            var groupedByDate = filtered
                .GroupBy(x => x.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(x => x.Revenue),
                    Quantity = g.Sum(x => x.Quantity),
                    Cost = g.Sum(x => x.Cost)
                })
                .OrderBy(x => x.Date)
                .ToList();

            FilteredSalesData = new ObservableCollection<SalesData>(
                filtered.OrderByDescending(x => x.Date).Take(100));
        }

        #endregion
    }
}