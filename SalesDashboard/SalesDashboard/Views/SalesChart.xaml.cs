using Azure;
using Syncfusion.Maui.AIAssistView;
using Syncfusion.Maui.Toolkit.Charts;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SalesDashboard;

public partial class SalesChart : ContentView
{
    SalesTrendsViewModel viewModel;
    AzureBaseService _baseAIService;
    public SalesChart(AzureBaseService baseAIService)
    {
        InitializeComponent();
        _baseAIService = baseAIService;
    }

    private void periodSelection_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is DateRangeOption selectedOption)
        {
            viewModel = BindingContext as SalesTrendsViewModel;
            if (viewModel != null)
            {
                viewModel.SelectedDateRange = selectedOption.Value;
                viewModel.Initialize();
                viewModel.LoadDashboardData();
            }
        }
    }

    #region expandable button view

    //private async void PointerGestureRecognizer_PointerMoved(object sender, PointerEventArgs e)
    //{
    //    await ExpandPopupAsync();
    //}

    //private async void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
    //{
    //    await CollapsePopupAsync();
    //}

    //private async void PointerGestureRecognizer_PointerReleased(object sender, PointerEventArgs e)
    //{
    //    popup.Show();
    //    viewModel.ShowAssistView = true;
    //}

    //private Task ExpandPopupAsync()
    //{
    //    clickToShowPopup.Text = "Syncfusion HelpBot";
    //    return clickToShowPopup.ScaleTo(1.1, 200, Easing.CubicOut);
    //}

    //private Task CollapsePopupAsync()
    //{
    //    clickToShowPopup.Text = string.Empty;
    //    return clickToShowPopup.ScaleTo(1, 200, Easing.CubicOut);
    //}

    //private void PointerGestureRecognizer_PointerPressed(object sender, PointerEventArgs e)
    //{
    //    popup.Show();
    //} 

    #endregion

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
            if (periodSelection.SelectedItem is DateRangeOption selectedOption)
            {
                viewModel.SelectedDateRange = selectedOption.Value;

                await Task.Run(() => viewModel.Initialize());

                var prompt = GeneratePrompt(request.Text, viewModel.SalesData, viewModel.SelectedDateRange);

                // Get AI response
                string aiResponse = await _baseAIService.GetAnswerFromGPT(prompt);

                AssistItem assistItem = new AssistItem()
                {
                    Text = CleanAndFormatOutput(aiResponse),
                };

                viewModel.Messages.Add(assistItem);
                assistItem.IsRequested = false;
            }
        }
    }

    public string GeneratePrompt(string customerQuery, ObservableCollection<SalesData> salesData, DateRange dateRange)
    {
        if (string.IsNullOrWhiteSpace(customerQuery))
            return "Please provide a valid query related to sales.";

        if (salesData == null || !salesData.Any())
            return $"Customer query: \"{customerQuery}\". Unfortunately, no sales data is available for the period {dateRange.StartDate:yyyy-MM-dd} to {dateRange.EndDate:yyyy-MM-dd}.";

        // Filter data based on the selected date range
        var filteredData = salesData
            .Where(s => s.Date >= dateRange.StartDate && s.Date <= dateRange.EndDate)
            .ToList();

        if (!filteredData.Any())
            return $"Customer query: \"{customerQuery}\". There is no sales data available for the selected period ({dateRange.StartDate:yyyy-MM-dd} to {dateRange.EndDate:yyyy-MM-dd}).";

        // Create a summary of sales data
        var summary = filteredData
            .GroupBy(s => s.ProductName)
            .Select(g => new
            {
                ProductName = g.Key,
                TotalSales = g.Sum(s => s.Cost),
                TotalProfit = g.Sum(s => s.Profit)
            })
            .ToList();

        // Format the summary data for the prompt
        string formattedSummary = string.Join(", ", summary.Select(s => $"{s.ProductName}: {s.TotalSales} total sales, {s.TotalProfit} total profit"));

        // Construct the AI prompt using the customer query and summary data
        return $"Customer query: \"{customerQuery}\". Sales data summary from {dateRange.StartDate:yyyy-MM-dd} to {dateRange.EndDate:yyyy-MM-dd}: {formattedSummary}.";
    }

    private string CleanAndFormatOutput(string aiResponse)
    {
        if (string.IsNullOrWhiteSpace(aiResponse))
            return string.Empty;

        // Remove unwanted Markdown characters
        aiResponse = aiResponse.Replace("####", "")
                               .Replace("###", "")
                               .Replace("[", "")
                               .Replace("]", "")
                                .Replace("/", "");



        aiResponse = Regex.Replace(aiResponse, @"(\d+\.\s)([A-Za-z\s]+)", m =>
            $"{m.Groups[1].Value}{m.Groups[2].Value.Trim()}");

        return aiResponse.Trim();
    }


    private void clickToShowPopup_Clicked(object sender, EventArgs e)
    {
        popup.Show();
        viewModel.ShowAssistView = true;
    }

    private void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
    {
        viewModel.Messages.Clear();
    }
}