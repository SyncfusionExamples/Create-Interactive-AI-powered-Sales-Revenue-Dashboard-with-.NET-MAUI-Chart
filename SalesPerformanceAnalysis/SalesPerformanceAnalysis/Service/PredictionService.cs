namespace SalesPerformanceAnalysis
{
    public class PredictionService
    {
        private readonly AzureBaseService _baseAIService;
        private readonly SalesDataService _salesDataService;

        public PredictionService(AzureBaseService baseAIService, SalesDataService salesDataService)
        {
            _baseAIService = baseAIService;
            _salesDataService = salesDataService;
        }

        public async Task<List<SalesPrediction>> GetSalesPredictionsAsync(
            DateRange historicalPeriod,
            DateRange futurePeriod,
            string productId = null,
            string regionId = null)
        {
            // Get historical data for the model
            var historicalData = await _salesDataService.GetSalesDataAsync(historicalPeriod, productId, regionId);

            if (!historicalData.Any())
            {
                return new List<SalesPrediction>();
            }

            // Get predictions from OpenAI
            var predictions = await _baseAIService.PredictSalesTrend(historicalData, futurePeriod, productId, regionId);

            return predictions;
        }

        public async Task<List<KeyValuePair<string, decimal>>> GetProductRevenueRankingAsync(DateRange period)
        {
            var salesData = await _salesDataService.GetSalesDataAsync(period);

            var productRevenue = salesData
                .GroupBy(x => x.ProductId)
                .Select(g => new KeyValuePair<string, decimal>(
                    g.First().ProductName,
                    g.Sum(x => x.Revenue)
                ))
                .OrderByDescending(x => x.Value)
                .Take(5)
                .ToList();

            return productRevenue;
        }

        public async Task<List<KeyValuePair<string, decimal>>> GetRegionRevenueRankingAsync(DateRange period)
        {
            var salesData = await _salesDataService.GetSalesDataAsync(period);

            var regionRevenue = salesData
                .GroupBy(x => x.RegionId)
                .Select(g => new KeyValuePair<string, decimal>(
                    g.First().RegionName,
                    g.Sum(x => x.Revenue)
                ))
                .OrderByDescending(x => x.Value)
                .Take(5)
                .ToList();

            return regionRevenue;
        }

        public async Task<string> GetSalesInsightsAsync(DateRange period)
        {
            var salesData = await _salesDataService.GetSalesDataAsync(period);

            if (!salesData.Any())
            {
                return "No sales data available for analysis.";
            }

            return await _baseAIService.AnalyzeSalesInsights(salesData, period);
        }
    }
}
