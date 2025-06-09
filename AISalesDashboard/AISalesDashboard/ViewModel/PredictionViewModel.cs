using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace AISalesDashboard
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

        private DateRange? _predictionPeriod;
        public DateRange PredictionPeriod
        {
            get => _predictionPeriod!;
            set
            {
                if (_predictionPeriod != value)
                {
                    _predictionPeriod = value;
                    OnPropertyChanged(nameof(PredictionPeriod));
                }
            }
        }

        private string? _insightsExplanation;
        public string InsightsExplanation
        {
            get => _insightsExplanation!;
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
     
        private DateTime startDate = DateTime.Now.AddDays(1);
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateTime endDate = DateTime.Now.AddDays(30);
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

#if MACCATALYST
        private int PageSize = 10;
#else 
        private int PageSize = 5;
#endif
        private int currentpredictionPage;
        
        public ObservableCollection<SalesPrediction> PredictionPagedItems { get; } = new();

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public string PredictionPageInfo => $"Page {currentpredictionPage + 1} of {TotalPredictionPages}";
        private int TotalPredictionPages => (int)Math.Ceiling((double)Predictions!.Count / PageSize);

        public PredictionViewModel(PredictionService predictionService, SalesDataService salesDataService)
        {
            _predictionService = predictionService;
            _salesDataService = salesDataService;
            PredictionPeriod = new DateRange(startDate, endDate);
            NextPageCommand = new Command(NextPredictionPage);
            PreviousPageCommand = new Command(PreviousPredictionPage);
            _ = Initialize();
        }

        public async Task Initialize()
        {

            try
            {
                // Load products
                var products = await _salesDataService!.GetProductsAsync();
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
                
                // Load initial predictions
                await GeneratePredictions();

                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating predictions: {ex.Message}");
            }
        }

        public async Task GeneratePredictions()
        {
            try
            {
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
                ConfidenceAverage = predictions.Any() ? predictions.Average(p => p.Confidence) * 100 : 0;
                PredictedTotalRevenue = predictions.Sum(p => p.PredictedRevenue);

                var recentPredictions = predictions.OrderByDescending(d => d.Date).Take(5).ToList();
                if (predictions.Any())
                {
                    InsightsExplanation = await _predictionService.GetRandomExplanationAsync(recentPredictions);
                }

                //Load the data in datagrid
                LoadPredictionPage();

                // Prepare chart data
                PrepareChartData(predictions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating predictions: {ex.Message}");
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

        private void LoadPredictionPage()
        {
            var startIndex = currentpredictionPage * PageSize;
            var itemsToShow = Predictions!
                .Skip(startIndex)
                .Take(PageSize)
                .ToList();

            PredictionPagedItems.Clear();
            foreach (var item in itemsToShow)
                PredictionPagedItems.Add(item);

            OnPropertyChanged(nameof(PredictionPageInfo));
        }

        private void NextPredictionPage()
        {
            if (currentpredictionPage < TotalPredictionPages - 1)
            {
                currentpredictionPage++;
                LoadPredictionPage();
            }
        }

        private void PreviousPredictionPage()
        {
            if (currentpredictionPage > 0)
            {
                currentpredictionPage--;
                LoadPredictionPage();
            }
        }
    }
}