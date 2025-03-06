namespace SalesDashboard
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
            DateRange futurePeriod)
        {
            // Get historical data for the model
            var historicalData = await _salesDataService.GetSalesDataAsync(historicalPeriod);

            if (!historicalData.Any())
            {
                return new List<SalesPrediction>();
            }

            // Get predictions from OpenAI
            var predictions = await _baseAIService.PredictSalesTrend(historicalData, futurePeriod);

            return predictions;
        }
    }
}
