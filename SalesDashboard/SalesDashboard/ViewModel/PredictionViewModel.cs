using System.Collections.ObjectModel;

namespace SalesDashboard
{
    public class PredictionViewModel : BaseViewModel
    {
        private readonly PredictionService _predictionService;
        private readonly SalesDataService _salesDataService;

        private ObservableCollection<Product> _products = new();
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        private ObservableCollection<Region> _regions = new();
        public ObservableCollection<Region> Regions
        {
            get => _regions;
            set
            {
                if (_regions != value)
                {
                    _regions = value;
                    OnPropertyChanged(nameof(Regions));
                }
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
                    OnSelectedProductChanged(value);
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
                    OnSelectedRegionChanged(value);
                }
            }
        }

        private ObservableCollection<SalesPrediction> _predictions = new();
        public ObservableCollection<SalesPrediction> Predictions
        {
            get => _predictions;
            set
            {
                if (_predictions != value)
                {
                    _predictions = value;
                    OnPropertyChanged(nameof(Predictions));
                }
            }
        }

        private ObservableCollection<KeyValuePair<string, List<decimal>>> _predictionChartData = new();
        public ObservableCollection<KeyValuePair<string, List<decimal>>> PredictionChartData
        {
            get => _predictionChartData;
            set
            {
                if (_predictionChartData != value)
                {
                    _predictionChartData = value;
                    OnPropertyChanged(nameof(PredictionChartData));
                }
            }
        }

        private DateRange _predictionPeriod;
        public DateRange PredictionPeriod
        {
            get => _predictionPeriod;
            set
            {
                if (_predictionPeriod != value)
                {
                    _predictionPeriod = value;
                    OnPropertyChanged(nameof(PredictionPeriod));
                }
            }
        }

        private string _insightsExplanation;
        public string InsightsExplanation
        {
            get => _insightsExplanation;
            set
            {
                if (_insightsExplanation != value)
                {
                    _insightsExplanation = value;
                    OnPropertyChanged(nameof(InsightsExplanation));
                }
            }
        }

        private decimal _confidenceAverage;
        public decimal ConfidenceAverage
        {
            get => _confidenceAverage;
            set
            {
                if (_confidenceAverage != value)
                {
                    _confidenceAverage = value;
                    OnPropertyChanged(nameof(ConfidenceAverage));
                }
            }
        }

        private decimal _predictedTotalRevenue;
        public decimal PredictedTotalRevenue
        {
            get => _predictedTotalRevenue;
            set
            {
                if (_predictedTotalRevenue != value)
                {
                    _predictedTotalRevenue = value;
                    OnPropertyChanged(nameof(PredictedTotalRevenue));
                }
            }
        }

        public PredictionViewModel(PredictionService predictionService, SalesDataService salesDataService)
        {
            _predictionService = predictionService;
            _salesDataService = salesDataService;
            Title = "Sales Prediction";
            PredictionPeriod = new DateRange(DateTime.Now.AddDays(1), DateTime.Now.AddDays(30));
            Initialize();
        }

        public async Task Initialize()
        {
            if (IsBusy)
                return;

            try
            {
                SetBusy(true, "Initializing...");
                // Load products
                var products = await _salesDataService.GetProductsAsync();
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
                // Load regions
                var regions = await _salesDataService.GetRegionsAsync();
                Regions.Clear();
                foreach (var region in regions)
                {
                    Regions.Add(region);
                }
                // Add "All" options
                Products.Insert(0, new Product { Id = null, Name = "All Products" });
                Regions.Insert(0, new Region { Id = null, Name = "All Regions" });
                // Set default selections
                SelectedProduct = Products[0];
                SelectedRegion = Regions[0];
                // Load initial predictions
                await GeneratePredictions();
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

        public async Task GeneratePredictions()
        {
            try
            {
                SetBusy(true, "Generating AI predictions...");
                // Historical period for model training
                var historicalPeriod = new DateRange(
                    DateTime.Now.AddMonths(-6),
                    DateTime.Now
                );
                // Get predictions
                var predictions = await _predictionService.GetSalesPredictionsAsync(
                    historicalPeriod,
                    PredictionPeriod);
                Predictions.Clear();
                foreach (var prediction in predictions)
                {
                    Predictions.Add(prediction);
                }
                // Calculate metrics
                ConfidenceAverage = predictions.Any() ? predictions.Average(p => p.Confidence) : 0;
                PredictedTotalRevenue = predictions.Sum(p => p.PredictedRevenue);

                // Get a random explanation from the predictions for display
                if (predictions.Any())
                {
                    var randomPrediction = predictions[new Random().Next(predictions.Count)];
                    InsightsExplanation = randomPrediction.Explanation;
                }
                // Prepare chart data
                PrepareChartData(predictions);
            }
            catch (Exception ex)
            {
                ShowError($"Error generating predictions: {ex.Message}");
            }
            finally
            {
                SetBusy(false);
                IsRefreshing = false;
            }
        }

        private void PrepareChartData(List<SalesPrediction> predictions)
        {
            if (!predictions.Any())
                return;

            var predictedValues = new List<decimal>();
            var lowerBoundValues = new List<decimal>();
            var upperBoundValues = new List<decimal>();

            foreach (var prediction in predictions.OrderBy(p => p.Date))
            {
                predictedValues.Add(prediction.PredictedRevenue);
                lowerBoundValues.Add(prediction.LowerBound);
                upperBoundValues.Add(prediction.UpperBound);
            }

            PredictionChartData.Clear();
            PredictionChartData.Add(new KeyValuePair<string, List<decimal>>("Predicted", predictedValues));
            PredictionChartData.Add(new KeyValuePair<string, List<decimal>>("Lower Bound", lowerBoundValues));
            PredictionChartData.Add(new KeyValuePair<string, List<decimal>>("Upper Bound", upperBoundValues));
        }

        private void OnSelectedProductChanged(Product value)
        {
            if (!IsBusy && Predictions.Any())
            {
                _ = GeneratePredictions();
            }
        }

        private void OnSelectedRegionChanged(Region value)
        {
            if (!IsBusy && Predictions.Any())
            {
                _ = GeneratePredictions();
            }
        }

        public async Task RefreshPredictions()
        {
            IsRefreshing = true;
            await GeneratePredictions();
        }
    }
}