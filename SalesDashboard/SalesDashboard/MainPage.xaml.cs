using Syncfusion.Maui.AIAssistView;
using Syncfusion.Maui.Toolkit.TabView;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace SalesDashboard
{
    public partial class MainPage : ContentPage
    {
        private readonly SalesTrendsViewModel salesTrendsViewModel;
        private readonly SalesDataService salesDataService;
        private readonly AzureBaseService baseAIService;
        private readonly PredictionService predictionService;
        private readonly PredictionViewModel predictionViewModel;

        public MainPage()
        {
            InitializeComponent();
            salesDataService = new SalesDataService();
            baseAIService = new AzureBaseService();
            predictionService = new PredictionService(baseAIService, salesDataService);

            predictionViewModel = new PredictionViewModel(predictionService, salesDataService);
            salesTrendsViewModel = new SalesTrendsViewModel(salesDataService);

            SalesChart sales = new SalesChart() { BindingContext = salesTrendsViewModel };
            ProductDetails productDetails = new ProductDetails() { BindingContext = salesTrendsViewModel };
            OrderDetails orderDetails = new OrderDetails() { BindingContext = salesTrendsViewModel };
            Prediction prediction = new Prediction() { BindingContext = predictionViewModel };
            var tabItems = new TabItemCollection
            {
                new SfTabItem()
                {
                    Header = "Sales",
                    Content = sales,
                    ImageSource = "sales.png",
                    ImageSize = 35, FontSize = 18,
                    ImagePosition = TabImagePosition.Left,
                },
                new SfTabItem()
                {
                    Header = "Products",
                    Content = productDetails,
                    ImageSource = "product.png",
                    ImageSize = 35, FontSize = 18,
                    ImagePosition = TabImagePosition.Left,
                },
                new SfTabItem()
                {
                    Header = "Order",
                    Content = orderDetails,
                    ImageSource = "order.png",
                    ImageSize = 35, FontSize = 18,
                    ImagePosition = TabImagePosition.Left,
                },
                 new SfTabItem()
                {
                    Header = "Prediction",
                    Content = prediction,
                    ImageSource = "revenue.png",
                    ImageSize = 35, FontSize = 18,
                    ImagePosition = TabImagePosition.Left,
                 },

            };

            tabView.Items = tabItems;

            this.BindingContext = salesTrendsViewModel;
        }

        private async void aiAssistView_Request(object sender, RequestEventArgs e)
        {
            var request = e.RequestItem;
            await GetResults(request);
        }

        private async Task GetResults(object inputQuery)
        {
            await Task.Delay(1000).ConfigureAwait(true);
            AssistItem request = (AssistItem)inputQuery;

            if (request != null)
            {
                await Task.Run(() => salesTrendsViewModel.Initialize());

                var prompt = GeneratePrompt(request.Text, salesTrendsViewModel.SalesData);

                // Get AI response
                string aiResponse = await baseAIService.GetAnswerFromGPT(prompt);

                AssistItem assistItem = new AssistItem()
                {
                    Text = CleanAndFormatOutput(aiResponse),
                };

                salesTrendsViewModel.Messages.Add(assistItem);
            }
        }

        public string GeneratePrompt(string customerQuery, ObservableCollection<SalesData> salesData)
        {
            if (string.IsNullOrWhiteSpace(customerQuery))
                return "Please provide a valid query related to sales.";

            if (salesData == null || !salesData.Any())
                return $"Customer query: \"{customerQuery}\". Unfortunately, there is no sales data available.";

            // Group sales by product and calculate total sales and profit
            var summary = salesData
                .GroupBy(s => s.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalSales = g.Sum(s => s.Cost),
                    TotalProfit = g.Sum(s => s.Profit)
                })
                .ToList();

            // Format the summary data for the AI prompt
            string formattedSummary = string.Join("; ",
                summary.Select(s => $"{s.ProductName}: {s.TotalSales} total sales, {s.TotalProfit} total profit"));

            // Construct the AI prompt dynamically based on customer query
            return $"Customer query: \"{customerQuery}\". Based on historical sales data, here are the key insights: {formattedSummary}.";
        }

        private string CleanAndFormatOutput(string aiResponse)
        {
            if (string.IsNullOrWhiteSpace(aiResponse))
                return string.Empty;

            aiResponse = aiResponse.Replace("####", "")
                                   .Replace("###", "")
                                   .Replace("**", "");

            aiResponse = Regex.Replace(aiResponse, @"(\d+\.\s)([A-Za-z\s]+)", m =>
                $"{m.Groups[1].Value}{m.Groups[2].Value.Trim()}");

            return aiResponse.Trim();
        }

        private void clickToShowPopup_Clicked(object sender, EventArgs e)
        {
            salesTrendsViewModel.ShowAssistView = true;
            assistViewHeader.IsVisible = true;
            assistViewBorder.IsVisible = true;
            clickToShowPopup.IsVisible = false;
        }

        private void close_Clicked(object sender, EventArgs e)
        {
            salesTrendsViewModel.ShowAssistView = false;
            assistViewHeader.IsVisible = false;
            assistViewBorder.IsVisible = false;
            clickToShowPopup.IsVisible = true;
        }
    }
}