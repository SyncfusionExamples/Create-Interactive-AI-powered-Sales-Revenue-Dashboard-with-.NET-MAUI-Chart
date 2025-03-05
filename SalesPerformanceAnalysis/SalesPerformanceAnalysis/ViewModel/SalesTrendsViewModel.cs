
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalesPerformanceAnalysis
{
    public class SalesTrendsViewModel : BaseViewModel
    {
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

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                    LoadSalesData();
                }
            }
        }

        private Region _selectedRegion;
        public Region SelectedRegion
        {
            get => _selectedRegion;
            set
            {
                if (_selectedRegion != value)
                {
                    _selectedRegion = value;
                    OnPropertyChanged(nameof(SelectedRegion));
                    LoadSalesData();
                }
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

        private ObservableCollection<KeyValuePair<string, List<decimal>>> _revenueChartData = new();
        public ObservableCollection<KeyValuePair<string, List<decimal>>> RevenueChartData
        {
            get => _revenueChartData;
            set
            {
                _revenueChartData = value;
                OnPropertyChanged(nameof(RevenueChartData));
            }
        }

        private ObservableCollection<KeyValuePair<string, List<decimal>>> _quantityChartData = new();
        public ObservableCollection<KeyValuePair<string, List<decimal>>> QuantityChartData
        {
            get => _quantityChartData;
            set
            {
                _quantityChartData = value;
                OnPropertyChanged(nameof(QuantityChartData));
            }
        }

        private ObservableCollection<KeyValuePair<string, List<decimal>>> _profitMarginChartData = new();
        public ObservableCollection<KeyValuePair<string, List<decimal>>> ProfitMarginChartData
        {
            get => _profitMarginChartData;
            set
            {
                _profitMarginChartData = value;
                OnPropertyChanged(nameof(ProfitMarginChartData));
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


        private async Task<List<SalesData>> LoadSalesDataAsync()
        {
            if (SelectedDateRange != null)
            {
                var data = await _salesDataService.GetSalesDataAsync(
                    SelectedDateRange,
                    SelectedProduct?.Id,
                    SelectedRegion?.Id);

                return data; 
            }

            return new List<SalesData>(); 
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

        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public SalesTrendsViewModel(SalesDataService salesDataService)
        {
            _salesDataService = salesDataService;
            Title = "Sales Trends";
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
            if (IsBusy)
                return;

            try
            {
                SetBusy(true, "Loading sales data...");

                var products = await _salesDataService.GetProductsAsync();
                Products = new ObservableCollection<Product>(products);
                Products.Insert(0, new Product { Id = null, Name = "All Products" });

                var regions = await _salesDataService.GetRegionsAsync();
                Regions = new ObservableCollection<Region>(regions);
                Regions.Insert(0, new Region { Id = null, Name = "All Regions" });

                SelectedProduct = Products.FirstOrDefault();
                SelectedRegion = Regions.FirstOrDefault();

                await LoadSalesData();
            }
            catch (Exception ex)
            {
                ShowError($"Error initializing: {ex.Message}");
            }
            finally
            {
                SetBusy(false);
            }
        }

        public async Task LoadSalesData()
        {
            try
            {
                SetBusy(true, "Loading sales data...");

                var data = await LoadSalesDataAsync();

                SalesData = new ObservableCollection<SalesData>(data);
                await ApplyFilters();
            }
            catch (Exception ex)
            {
                ShowError($"Error loading sales data: {ex.Message}");
            }
            finally
            {
                SetBusy(false);
                IsRefreshing = false;
            }
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

            RevenueChartData = new ObservableCollection<KeyValuePair<string, List<decimal>>>
            {
                new("Revenue", groupedByDate.Select(x => x.Revenue).ToList())
            };

            QuantityChartData = new ObservableCollection<KeyValuePair<string, List<decimal>>>
            {
                new("Quantity", groupedByDate.Select(x => decimal.Parse(x.Quantity.ToString())).ToList())
            };

            ProfitMarginChartData = new ObservableCollection<KeyValuePair<string, List<decimal>>>
            {
                new("Profit Margin %",
                    groupedByDate.Select(x => x.Revenue > 0 ? (x.Revenue - x.Cost) / x.Revenue * 100 : 0).ToList())
            };

            FilteredSalesData = new ObservableCollection<SalesData>(
                filtered.OrderByDescending(x => x.Date).Take(100));
        }

       

        public async Task RefreshData()
        {
            IsRefreshing = true;
            await LoadSalesData();
        }
    }
}
