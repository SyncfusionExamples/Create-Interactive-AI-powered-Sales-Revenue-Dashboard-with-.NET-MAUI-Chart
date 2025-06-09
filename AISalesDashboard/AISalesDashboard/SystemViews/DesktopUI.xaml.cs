using Syncfusion.Maui.AIAssistView;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AISalesDashboard;

public partial class DesktopUI : ContentView
{
    private readonly SalesTrendsViewModel salesTrendsViewModel;
    private readonly SalesDataService salesDataService;
    private readonly AzureBaseService baseAIService;
    private readonly PredictionService predictionService;
    private readonly PredictionViewModel predictionViewModel;
    public DesktopUI()
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

        salesTrendsViewModel.OnPageSelected += (viewName) =>
        {
            LoadContentView(viewName, ref sales, ref productDetails, ref orderDetails, ref prediction);
        };

        main.Content = new SalesChart() { BindingContext = salesTrendsViewModel };

        this.BindingContext = salesTrendsViewModel;
    }

    private void LoadContentView(string viewName, ref SalesChart sales, ref ProductDetails productDetails, ref OrderDetails orderDetails, ref Prediction prediction)
    {
        switch (viewName)
        {
            case "Products":
                main.Content = productDetails;
                break;
            case "Orders":
                main.Content = orderDetails;
                break;
            case "Predictions":
                main.Content = prediction;
                break;
            default:
                main.Content = sales;
                break;
        }
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
        assistViewHeader.IsVisible = true;
        assistViewBorder.IsVisible = true;
        clickToShowPopup.IsVisible = false;
        close.IsVisible = true;
    }

    private void close_Clicked(object sender, EventArgs e)
    {
        salesTrendsViewModel.ShowAssistView = false;
        assistViewHeader.IsVisible = false;
        assistViewBorder.IsVisible = false;
        clickToShowPopup.IsVisible = true;
        close.IsVisible = false;
    }

    private void menuButton_Clicked(object sender, EventArgs e)
    {
        navigationDrawer.ToggleDrawer();
    }

    private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        switch(e.SelectedItemIndex)
        {
            case 0:
                headerLabel.Text = "Home";
                break;
            case 1:
                headerLabel.Text = "Products";
                break;
            case 2:
                headerLabel.Text = "Orders";
                break;
            case 3:
                headerLabel.Text = "Predictions";
                break;
        }
    }
}

public class BoolToDarkThemeColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isSelected = false;
        _ = bool.TryParse(value?.ToString(), out isSelected);

        return Colors.Transparent;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}