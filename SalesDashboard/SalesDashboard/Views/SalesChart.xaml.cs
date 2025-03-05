using Azure;
using Syncfusion.Maui.AIAssistView;
using Syncfusion.Maui.Toolkit.Charts;
using System.Collections.ObjectModel;
using System.Globalization;

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
   
    private async void PointerGestureRecognizer_PointerMoved(object sender, PointerEventArgs e)
    {
        await ExpandPopupAsync();
    }

    private async void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
    {
        await CollapsePopupAsync();
    }

    private async void PointerGestureRecognizer_PointerReleased(object sender, PointerEventArgs e)
    {
        popup.Show();
        viewModel.ShowAssistView = true;
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
            if (periodSelection.SelectedItem is DateRangeOption selectedOption)
            {
                viewModel.SelectedDateRange = selectedOption.Value;

                await Task.Run(() => viewModel.Initialize());

                var prompt = GeneratePrompt(request.Text, viewModel.SalesData, viewModel.SelectedDateRange);

                // Get AI response
                string aiResponse = await _baseAIService.GetAnswerFromGPT(prompt);

                AssistItem assistItem = new AssistItem()
                {
                    Text = aiResponse.ToString(),
                };

                viewModel.Messages.Add(assistItem);
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

        // Format relevant sales data
        string formattedItems = string.Join(", ", filteredData.Select(s => $"[{s.Date:yyyy-MM-dd}] {s.ProductName}: {s.Cost} sales, {s.Profit} profit"));

        // Construct the AI prompt using the customer query
        return $"Customer query: \"{customerQuery}\". Based on sales data from {dateRange.StartDate:yyyy-MM-dd} to {dateRange.EndDate:yyyy-MM-dd}, here is the relevant information: {formattedItems}. Provide insights in response to the query.";
    }



    private Task ExpandPopupAsync()
    {
        clickToShowPopup.Text = "Syncfusion HelpBot";
        return clickToShowPopup.ScaleTo(1.1, 200, Easing.CubicOut);
    }

    private Task CollapsePopupAsync()
    {
        clickToShowPopup.Text = string.Empty;
        return clickToShowPopup.ScaleTo(1, 200, Easing.CubicOut);
    }

    private void PointerGestureRecognizer_PointerPressed(object sender, PointerEventArgs e)
    {
        popup.Show();
    }
}