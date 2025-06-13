
using Syncfusion.Maui.AIAssistView;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace AISalesDashboard
{
    public class SalesTrendsViewModel : BaseViewModel
    {
        #region Properties

        private readonly SalesDataService? _salesDataService;

        private ObservableCollection<Product>? _products = new();
        public ObservableCollection<Product>? Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private ObservableCollection<Region>? _regions = new();
        public ObservableCollection<Region>? Regions
        {
            get => _regions;
            set
            {
                _regions = value;
                OnPropertyChanged(nameof(Regions));
            }
        }
   
        private ObservableCollection<SalesData>? _salesData = new();
        public ObservableCollection<SalesData>? SalesData
        {
            get => _salesData;
            set
            {
                _salesData = value;
                OnPropertyChanged(nameof(SalesData));
            }
        }

        private ObservableCollection<SalesData>? _filteredSalesData = new();
        public ObservableCollection<SalesData>? FilteredSalesData
        {
            get => _filteredSalesData;
            set
            {
                _filteredSalesData = value;
                OnPropertyChanged(nameof(FilteredSalesData));
            }
        }

        private List<DateRangeOption>? dateRanges;
        public List<DateRangeOption>? DateRanges
        {
            get => dateRanges;
            set
            {
                dateRanges = value;
                OnPropertyChanged(nameof(DateRanges));
            }
        }

        private decimal? _totalRevenue;
        public decimal? TotalRevenue
        {
            get => _totalRevenue;
            set
            {
                _totalRevenue = value;
                OnPropertyChanged(nameof(TotalRevenue));
                UpdateDashboardLabelCards();
            }
        }

        private decimal? _totalProfit;
        public decimal? TotalProfit
        {
            get => _totalProfit;
            set
            {
                _totalProfit = value;
                OnPropertyChanged(nameof(TotalProfit));
                UpdateDashboardLabelCards();
            }
        }

        private double? _profitMargin;
        public double? ProfitMargin
        {
            get => _profitMargin;
            set
            {
                _profitMargin = value;
                OnPropertyChanged(nameof(ProfitMargin));
                UpdateDashboardLabelCards();
            }
        }

        private double? _growthRate;
        public double? GrowthRate
        {
            get => _growthRate;
            set
            {
                _growthRate = value;
                OnPropertyChanged(nameof(GrowthRate));
                UpdateDashboardLabelCards();
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

        private ObservableCollection<IAssistItem>? messages = new();
        public ObservableCollection<IAssistItem>? Messages
        {
            get => messages;
            set
            {
                messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }

        private ObservableCollection<ProductInfo>? productInfos;
        public ObservableCollection<ProductInfo>? ProductInfos
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

        private ObservableCollection<OrderInfo>? orderInfos;
        public ObservableCollection<OrderInfo>? OrderInfos
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

        public ObservableCollection<Brush>? PaletteBrushes1 { get; set; }

        private ObservableCollection<ExportOption>? exportOptions;
        public ObservableCollection<ExportOption>? ExportOptions
        {
            get { return exportOptions; }
            set
            {
                exportOptions = value;
                OnPropertyChanged(nameof(ExportOptions));
            }
        }

        private ObservableCollection<CustomMarker>? mapMarker;
        public ObservableCollection<CustomMarker>? MapMarkerCollection
        {
            get => mapMarker;
            set
            {
                mapMarker = value;
                OnPropertyChanged(nameof(MapMarkerCollection));
            }
        }

        private List<PageInfo>? pages;
        public List<PageInfo>? Pages
        {
            get => pages;
            set
            {
                if (pages != value)
                {
                    pages = value;
                    OnPropertyChanged(nameof(Pages));
                }
            }
        }

        public event Action<string>? OnPageSelected;

        private PageInfo? _selectedPage;
        public PageInfo? SelectedPage
        {
            get => _selectedPage;
            set
            {
                if (_selectedPage != value)
                {
                    if (_selectedPage != null)
                        _selectedPage.IsSelected = false;

                    _selectedPage = value;

                    if (_selectedPage != null)
                        _selectedPage.IsSelected = true;

                    OnPropertyChanged(nameof(SelectedPage));
                    OnPageSelected?.Invoke(_selectedPage?.Title ?? string.Empty);
                }
            }
        }

        private ObservableCollection<DashboardLabelCardModel>? homepagecardLabels;
        public ObservableCollection<DashboardLabelCardModel>? HomePageCardLabels
        {
            get => homepagecardLabels;
            set
            {
                if(homepagecardLabels != value)
                {
                    homepagecardLabels = value;
                    OnPropertyChanged(nameof(HomePageCardLabels));
                }
            }
        }

        private ObservableCollection<ISuggestion> _suggestions;
        public ObservableCollection<ISuggestion> Suggestions
        {
            get => _suggestions;
            set
            {
                if (_suggestions != value)
                {
                    _suggestions = value;
                    OnPropertyChanged(nameof(Suggestions));
                }
            }
        }

        private Border orderList;
        public Border OrderList
        {
            get => orderList;
            set
            {
                if(orderList != value)
                {
                    orderList = value;
                    OnPropertyChanged(nameof(OrderList));
                }
            }
        }

        private const int SalesPageSize =5;

#if MACCATALYST
        private int ProductPageSize = 6;
#else
        private int ProductPageSize = 5;
#endif
        private int currentproductPage;
        private int currentsalesPage;

        public ObservableCollection<ProductInfo> ProductPagedItems { get; } = new();

        public ObservableCollection<SalesData> SalesPagedItems { get; } = new();

        public ICommand NextProductPageCommand { get; }
        public ICommand PreviousProductPageCommand { get; }

        public ICommand NextSalesPageCommand { get; }
        public ICommand PreviousSalesPageCommand { get; }

        public string ProductPageInfo => $"Page {currentproductPage + 1} of {TotalProductPages}";
        private int TotalProductPages => (int)Math.Ceiling((double)ProductInfos!.Count / ProductPageSize);

        public string SalesPageInfo => $"Page {currentsalesPage + 1} of {TotalSalesPages}";
        private int TotalSalesPages => (int)Math.Ceiling((double)FilteredSalesData!.Count / SalesPageSize);
#endregion

        #region Constructor

        public SalesTrendsViewModel(SalesDataService salesDataService)
        {
            Pages = new List<PageInfo>()
                {
                    new PageInfo(){ Title = "Home", PageIcon="\ue88a"},
                    new PageInfo(){ Title = "Products", PageIcon="\uf569"},
                    new PageInfo(){ Title = "Orders", PageIcon="\ue8cc"},
                    new PageInfo(){ Title = "Predictions", PageIcon="\ue26b"}
                };

            this._suggestions = new ObservableCollection<ISuggestion>()
            {
                new AssistSuggestion() {Text = "Top performing products?"},
                new AssistSuggestion() {Text = "Sales trend last month?"},
                new AssistSuggestion() { Text = "Region with highest growth?"},
            };

            PaletteBrushes1 = new ObservableCollection<Brush>()
                {
                     new SolidColorBrush(Color.FromArgb("#CAC4D0")),
                     new SolidColorBrush(Color.FromArgb("#116DF9")),
                     new SolidColorBrush(Color.FromArgb("#E2227E")),
                     new SolidColorBrush(Color.FromArgb("#25E739")),
                     new SolidColorBrush(Color.FromArgb("#F4890B")),
                     new SolidColorBrush(Color.FromArgb("#00E190")),
                     new SolidColorBrush(Color.FromArgb("#FF4E4E")),
                };

            SelectedPage = Pages.FirstOrDefault(p => p.Title == "Home");

            ExportOptions = new ObservableCollection<ExportOption>
            {
                new ExportOption { Name = "Export Excel", Icon = "excel.png" },
                new ExportOption { Name = "Export Pdf", Icon = "pdf.png" }
            };

            ProductInfos = new ObservableCollection<ProductInfo>(LoadProductData());

            OrderInfos = Orders(1);
            MapMarkerCollection = new ObservableCollection<CustomMarker>();
            _salesDataService = salesDataService;
            _ = Initialize();
            _ = LoadDashboardData();

            DateRanges = new List<DateRangeOption>
            {
                new DateRangeOption { DisplayText = "Last 30 Days", Value = DateRange.Last30Days },
                new DateRangeOption { DisplayText = "Last Quarter", Value = DateRange.LastQuarter },
                new DateRangeOption { DisplayText = "Year to Date", Value = DateRange.YearToDate },
                new DateRangeOption { DisplayText = "Last Year", Value = DateRange.LastYear }
            };

            HomePageCardLabels = new ObservableCollection<DashboardLabelCardModel>();
            UpdateDashboardLabelCards();
            
            NextProductPageCommand = new Command(NextProductPage);
            PreviousProductPageCommand = new Command(PreviousProductPage);
            NextSalesPageCommand = new Command(NextSalesPage);
            PreviousSalesPageCommand = new Command(PreviousSalesPage);

            LoadProductsPage();
        }

        #endregion

        #region Methods

        private ObservableCollection<OrderInfo> Orders(int month)
        {
            var monthlyOrders = new ObservableCollection<OrderInfo>();
            var random = new Random();
            string[] customers = new string[] { "Alen", "James", "Reena", "Mark", "Teena", "Thomas", "John", "Lawrence", "Samwell", "Arya", "Jennifer", "Robert", "Lilly",
            "Jessica", "Grace", "Elizabeth", "Melana", "Arthur", "Michael", "George" };
            DateTime yesterday = DateTime.Now.Date.AddDays(-1);
            Status[] statuses = (Status[])Enum.GetValues(typeof(Status));
            var numbers = Enumerable.Range(100001, 16).ToList();
            var count = 0;
#if WINDOWS || ANDROID || IOS
            count = 10;

#elif MACCATALYST
            count = 15;

#endif
            for (int i = 0; i < count; i++)
            {
                var product = ProductInfos![random.Next(ProductInfos.Count)];
                double marginValue = random.NextDouble() * 20;
                Status status = statuses[random.Next(statuses.Length)];
                int minutesToAdd = random.Next(0, 48 * 60);
                DateTime randomDateTime = yesterday.AddMinutes(minutesToAdd);
                monthlyOrders.Add(new OrderInfo
                {
                    Id = numbers[random.Next(numbers.Count)],
                    Product = product.Product,
                    Image = product.Image,
                    OrderStatus = status.ToString(),
                    Brush = GetColorForStatus(status),
                    Customer = customers[random.Next(customers.Length)],
                    OrderDate = randomDateTime.ToString("MMM d,yyyy\nh:mm tt"),
                    ActualPrice = product.BuyingPrice,
                    MarginValue = marginValue,
                    SellingPrice = product.BuyingPrice + marginValue
                });
            }

            return monthlyOrders;
        }

        private void UpdateDashboardLabelCards()
        {
            HomePageCardLabels!.Clear();

            HomePageCardLabels.Add(new DashboardLabelCardModel
            {
                Title = "Total Revenue",
                Value = TotalRevenue.HasValue ? $"${TotalRevenue.Value:N0}" : "$0",
                Icon = "\ue227",
                IconColor = Color.FromArgb("#116DF9"),
                Background = Color.FromArgb("#CFE2FF"),
                ValueColor = Color.FromArgb("#1C1B1F")
            });

            HomePageCardLabels.Add(new DashboardLabelCardModel
            {
                Title = "Total Profit",
                Value = TotalProfit.HasValue ? $"${TotalProfit.Value:N0}" : "$0",
                Icon = "\uf3ee",
                IconColor = Color.FromArgb("#E2227E"),
                Background = Color.FromArgb("#FFDAEC"),
                ValueColor = Color.FromArgb("#1C1B1F")
            });

            HomePageCardLabels.Add(new DashboardLabelCardModel
            {
                Title = "Profit Margin",
                Value = ProfitMargin.HasValue ? $"+{ProfitMargin.Value:N1}%" : "0%",
                Icon = "\uef92",
                IconColor = Color.FromArgb("#F4890B"),
                Background = Color.FromArgb("#FFE4C4"),
                ValueColor = Color.FromArgb("#15A03D")
            });

            HomePageCardLabels.Add(new DashboardLabelCardModel
            {
                Title = "Growth Rate",
                Value = GrowthRate.HasValue ? $"{GrowthRate.Value:N1}%" : "0%",
                Icon = "\ue8e5",
                IconColor = Color.FromArgb("#1BC92D"),
                Background = Color.FromArgb("#E4FFE7"),
                ValueColor = Color.FromArgb("#DC2626")
            });
        }

        private Color GetColorForStatus(Status status)
        {
            return status switch
            {
                Status.Delivered => Color.FromArgb("#25E739"),
                Status.Shipped => Color.FromArgb("#116DF9"),
                Status.Cancelled => Color.FromArgb("#FF4E4E"),
                Status.Refunded => Color.FromArgb("#F4890B"),
                _ => Colors.Gray,
            };
        }

        public async Task LoadDashboardData()
        {
            // Load financial summary
            var summary = await _salesDataService!.GetSalesSummaryAsync(SelectedDateRange);
            TotalRevenue = summary["TotalRevenue"];
            TotalProfit = summary["TotalProfit"];
            ProfitMargin = (double)summary["AverageProfitMargin"];
            GrowthRate = (double)summary["RevenueGrowth"];
        }

        public async Task Initialize()
        {
            try
            {
                var products = await _salesDataService!.GetProductsAsync();
                
                Products = new ObservableCollection<Product>(products.Select(x=> new Product { Name = x.Name, BasePrice = Math.Round(x.BasePrice, 0)}).OrderByDescending(p => p.BasePrice).Take(5));

                var regions = await _salesDataService.GetRegionsAsync();
                Regions = new ObservableCollection<Region>(regions);

                // Adding map marker collection using LINQ
                MapMarkerCollection = new ObservableCollection<CustomMarker>(
                    regions.Select(region => new CustomMarker
                    {
                        Latitude = region.Latitude,
                        Longitude = region.Longitude,
                        Name = region.Country
                    }).ToList()
                );

                await LoadSalesData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating predictions: {ex.Message}");
            }
        }

        private List<ProductInfo> LoadProductData()
        {
            var random = new Random();
            return new List<ProductInfo>()
            {
                new ProductInfo { Product = "Samsung Galaxy S3", BuyingPrice = 149.99, Image = ImageSource.FromFile("samsung.png"), ProductStockCount = 14, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5), ProductID = random.Next(01,30), TotalSales = random.Next(100, 300) },
                new ProductInfo { Product = "iPhone 4", BuyingPrice = 199.99, Image = ImageSource.FromFile("iphone_4.png"), ProductStockCount = 8, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5), ProductID = random.Next(01,30), TotalSales = random.Next(100, 300) },
                new ProductInfo { Product = "Google Nexus 5", BuyingPrice = 129.99, Image = ImageSource.FromFile("googlenexus_5.png"), ProductStockCount = 2, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5), ProductID = random.Next(01,30), TotalSales = random.Next(100, 300) },
                new ProductInfo { Product = "HTC One M7", BuyingPrice = 169.99, Image = ImageSource.FromFile("htcone_m7.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png"), ProductRating = random.Next(3,5), ProductID = random.Next(01,30), TotalSales = random.Next(100, 300) },
                new ProductInfo { Product = "BlackBerry Bold 9900", BuyingPrice = 119.99, Image = ImageSource.FromFile("blackberry_bold.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png"), ProductRating = random.Next(3,5), ProductID = random.Next(01,30), TotalSales = random.Next(100, 300) },
                new ProductInfo { Product = "Sony Xperia Z1", BuyingPrice = 179.99, Image = ImageSource.FromFile("sonyxperia_z1.png"), ProductStockCount = 4, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5), ProductID = random.Next(01,30), TotalSales = random.Next(100, 300) },
                new ProductInfo { Product = "LG G2", BuyingPrice = 149.99, Image = ImageSource.FromFile("lg_g2.png"), ProductStockCount = 16 , AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300) },
                new ProductInfo { Product = "Nokia Lumia 920", BuyingPrice = 139.99, Image = ImageSource.FromFile("nokia_lumia.png"), ProductStockCount = 3, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5), ProductID = random.Next(01,30), TotalSales = random.Next(100, 300) },
                new ProductInfo { Product = "Motorola Moto X (2013)", BuyingPrice = 129.99, Image = ImageSource.FromFile("motorola_moto.png"), ProductStockCount = 8 , AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3, 5), ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Huawei Ascend P6", BuyingPrice = 159.99, Image = ImageSource.FromFile("huawei_ascend.png"), ProductStockCount = 25, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3, 5), ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Samsung Galaxy Note 4", BuyingPrice = 179.99, Image = ImageSource.FromFile("samsung_galaxy_n4.png"), ProductStockCount = 2, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3, 5), ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "iPhone 5S", BuyingPrice = 199.99, Image = ImageSource.FromFile("iphone_5s.png"), ProductStockCount = 28, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3, 5), ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Laptop", BuyingPrice = 899.99, Image = ImageSource.FromFile("laptop.png"), ProductStockCount = 10, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3, 5), ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Smartphone", BuyingPrice = 699.99, Image = ImageSource.FromFile("smartphone.png"), ProductStockCount = 25, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3, 5), ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Wireless Earbuds", BuyingPrice = 129.99, Image = ImageSource.FromFile("wireless_earbuds.png"), ProductStockCount = 15, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Smartwatch", BuyingPrice = 199.99, Image = ImageSource.FromFile("smartwatch.png"), ProductStockCount = 20, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Bluetooth Speaker", BuyingPrice = 79.99, Image = ImageSource.FromFile("bluetooth_speaker.png"), ProductStockCount = 5 , AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Gaming Console", BuyingPrice = 499.99, Image = ImageSource.FromFile("gaming_console.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png"), ProductRating = random.Next(3,5), ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Tablet", BuyingPrice = 349.99, Image = ImageSource.FromFile("tablet.png"), ProductStockCount = 12, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Monitor", BuyingPrice = 229.99, Image = ImageSource.FromFile("monitor.png"), ProductStockCount = 18 , AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3, 5), ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Mechanical Keyboard", BuyingPrice = 129.99, Image = ImageSource.FromFile("mech_keyboard.png"), ProductStockCount = 22, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Gaming Mouse", BuyingPrice = 59.99, Image = ImageSource.FromFile("gaming_mouse.png"), ProductStockCount = 30, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "VR Headset", BuyingPrice = 399.99, Image = ImageSource.FromFile("vr_headset.png"), ProductStockCount = 7, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "External SSD", BuyingPrice = 149.99, Image = ImageSource.FromFile("external_ssd.png"), ProductStockCount = 16, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Router", BuyingPrice = 99.99, Image = ImageSource.FromFile("router.png"), ProductStockCount = 0, AvailabilityImage = ImageSource.FromFile("wrong.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Smart TV", BuyingPrice = 599.99, Image = ImageSource.FromFile("smart_tv.png"), ProductStockCount = 14, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Webcam", BuyingPrice = 89.99, Image = ImageSource.FromFile("webcam.png"), ProductStockCount = 3 , AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Microphone", BuyingPrice = 139.99, Image = ImageSource.FromFile("microphone.png"), ProductStockCount = 11, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Graphics Card", BuyingPrice = 699.99, Image = ImageSource.FromFile("graphics_card.png"), ProductStockCount = 4, AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3,5) , ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
                new ProductInfo { Product = "Power Bank", BuyingPrice = 49.99, Image = ImageSource.FromFile("power_bank.png"), ProductStockCount = 21 , AvailabilityImage = ImageSource.FromFile("correct.png"), ProductRating = random.Next(3, 5), ProductID = random.Next(01, 30), TotalSales = random.Next(100, 300)},
            };
        }

        private void LoadProductsPage()
        {
            var startIndex = currentproductPage * ProductPageSize;
            var itemsToShow = ProductInfos!
                .Skip(startIndex)
                .Take(ProductPageSize)
                .ToList();
            
            ProductPagedItems.Clear();
            foreach (var item in itemsToShow)
                ProductPagedItems.Add(item);

            OnPropertyChanged(nameof(ProductPageInfo));
        }

        private void LoadSalesPage()
        {
            var startIndex = currentsalesPage * SalesPageSize;
            var itemsToShow = FilteredSalesData!
                .Skip(startIndex)
                .Take(SalesPageSize)
                .ToList();

            SalesPagedItems.Clear();
            foreach (var item in itemsToShow)
                SalesPagedItems.Add(item);

            OnPropertyChanged(nameof(SalesPageInfo));
        }

        private void NextProductPage()
        {
            if (currentproductPage < TotalProductPages - 1)
            {
                currentproductPage++;
                LoadProductsPage();
            }
        }

        private void PreviousProductPage()
        {
            if (currentproductPage > 0)
            {
                currentproductPage--;
                LoadProductsPage();
            }
        }

        private void NextSalesPage()
        {
            if (currentsalesPage < TotalSalesPages - 1)
            {
                currentsalesPage++;
                LoadSalesPage();
            }
        }

        private void PreviousSalesPage()
        {
            if (currentsalesPage > 0)
            {
                currentsalesPage--;
                LoadSalesPage();
            }
        }

        public async Task LoadSalesData()
        {
            try
            {
                var data = await LoadSalesDataAsync();

                var random = new Random();

                var chartData = new List<SalesData>();

                DateTime startDate = SelectedDateRange.StartDate;
                DateTime endDate = SelectedDateRange.EndDate;
                int totalDays = 365;

                double midPoint = totalDays / 2.0;
                double spread = totalDays / 6.0;

                for (int i = 0; i < totalDays; i++)
                {
                    DateTime datePoint = startDate.AddDays(i);

                    double gaussianFactor = Math.Exp(-Math.Pow(i - midPoint, 2) / (2 * Math.Pow(spread, 2)));

                    int baseRevenue = 500000;
                    int peakRevenue = 1700000;
                    int revenue = (int)(baseRevenue + gaussianFactor * (peakRevenue - baseRevenue));

                    int quantity = random.Next(200, 1000);
                    int cost = random.Next(200000, 300000);

                    chartData.Add(new SalesData
                    {
                        Date = datePoint,
                        Revenue = revenue,
                        Quantity = quantity,
                        Cost = cost,
                        ProductName = data[i].ProductName
                    });
                }

                var filteredAndSortedData = chartData
                .Where(data => data.Date >= startDate && data.Date <= endDate)
                .OrderBy(data => data.Date)
                .ToList();

                SalesData = new ObservableCollection<SalesData>(filteredAndSortedData);

                ApplyFilters(data);
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
                var data = await _salesDataService!.GetSalesDataAsync(
                    SelectedDateRange);

                return data;
            }

            return new List<SalesData>();
        }

        public void ApplyFilters(List<SalesData> salesData)
        {
            if (salesData!.Count == 0)
                return;

            var filtered = salesData.ToList();

            FilteredSalesData = new ObservableCollection<SalesData>(
                filtered.OrderByDescending(x => x.Date).Take(50));

            LoadSalesPage();
        }

#endregion
    }
}