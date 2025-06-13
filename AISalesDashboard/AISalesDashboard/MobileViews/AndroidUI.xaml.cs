using Syncfusion.Maui.AIAssistView;
using Syncfusion.Maui.TabView;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace AISalesDashboard;

public partial class AndroidUI : ContentView
{
    private readonly SalesTrendsViewModel salesTrendsViewModel;
    private readonly SalesDataService salesDataService;
    private readonly AzureBaseService baseAIService;
    private readonly PredictionService predictionService;
    private readonly PredictionViewModel predictionViewModel;
    public AndroidUI()
    {
        InitializeComponent();
        salesDataService = new SalesDataService();
        baseAIService = new AzureBaseService();
        predictionService = new PredictionService(baseAIService, salesDataService);

        predictionViewModel = new PredictionViewModel(predictionService, salesDataService);
        salesTrendsViewModel = new SalesTrendsViewModel(salesDataService);

        HomeAndroid sales = new HomeAndroid() { BindingContext = salesTrendsViewModel };
        ProductDetailsAndroid productDetails = new ProductDetailsAndroid() { BindingContext = salesTrendsViewModel };
        OrderDetailsAndroid orderDetails = new OrderDetailsAndroid() { BindingContext = salesTrendsViewModel };
        PredictionAndroid prediction = new PredictionAndroid() { BindingContext = predictionViewModel };

        homeView.Content = sales;
        productsView.Content = productDetails;
        ordersView.Content = orderDetails;
        predictionsView.Content = prediction;

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

            var prompt = GeneratePrompt(request.Text, salesTrendsViewModel.SalesData!);

            // Get AI response
            string aiResponse = await baseAIService.GetAnswerFromGPT(prompt);

            AssistItem assistItem = new AssistItem()
            {
                Text = CleanAndFormatOutput(aiResponse),
            };

            salesTrendsViewModel.Messages!.Add(assistItem);
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
                        .Replace("**", "")
                        .Replace("/", "")
                        .Replace("[", "")
                        .Replace("]", "");

        aiResponse = Regex.Replace(aiResponse, @"(\d+\.\s)([A-Za-z\s]+)", m =>
            $"{m.Groups[1].Value}{m.Groups[2].Value.Trim()}");

        return aiResponse.Trim();
    }

    private void clickToShowPopup_Clicked(object sender, EventArgs e)
    {
        salesTrendsViewModel.ShowAssistView = true;
        clickToShowPopup.IsVisible = false;
        assistviewContent.IsVisible = true;
    }

    private void SfButton_Clicked(object sender, EventArgs e)
    {
        assistviewContent.IsVisible = false;
        clickToShowPopup.IsVisible = true;
    }

    private void tabView_SelectionChanged(object sender, TabSelectionChangedEventArgs e)
    {
        switch (e.NewIndex)
        {
            case 0:
                homeView.TextColor = Color.FromArgb("#6750A4");
                productsView.TextColor = Color.FromArgb("#FFFFFF");
                ordersView.TextColor = Color.FromArgb("#FFFFFF");
                predictionsView.TextColor = Color.FromArgb("#FFFFFF");
                break;
            case 1:
                homeView.TextColor = Color.FromArgb("#FFFFFF");
                productsView.TextColor = Color.FromArgb("#6750A4");
                ordersView.TextColor = Color.FromArgb("#FFFFFF");
                predictionsView.TextColor = Color.FromArgb("#FFFFFF");
                break;
            case 2:
                homeView.TextColor = Color.FromArgb("#FFFFFF");
                productsView.TextColor = Color.FromArgb("#FFFFFF");
                ordersView.TextColor = Color.FromArgb("#6750A4");
                predictionsView.TextColor = Color.FromArgb("#FFFFFF");
                break;
            case 3:
                homeView.TextColor = Color.FromArgb("#FFFFFF");
                productsView.TextColor = Color.FromArgb("#FFFFFF");
                ordersView.TextColor = Color.FromArgb("#FFFFFF");
                predictionsView.TextColor = Color.FromArgb("#6750A4");
                break;
        }
    }
}