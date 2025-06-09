using Syncfusion.Maui.Charts;

namespace AISalesDashboard;

public partial class HomeAndroid : ContentView
{
    int month = int.MaxValue;
    public HomeAndroid()
    {
        InitializeComponent();
    }

    private void periodSelection_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (e.AddedItems!.Count > 0 && e.AddedItems[0] is DateRangeOption selectedOption)

            if (this.BindingContext is SalesTrendsViewModel salesTrendsViewModel)
            {
                salesTrendsViewModel.SelectedDateRange = selectedOption.Value!;
                _ = salesTrendsViewModel.Initialize();
                _ = salesTrendsViewModel.LoadDashboardData();
            }
    }

    private void DateTimeAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
    {
        DateTime baseDate = new(1899, 12, 30);
        var date = baseDate.AddDays(e.Position);
        if (date.Month != month)
        {
            ChartAxisLabelStyle labelStyle = new();
            labelStyle.LabelFormat = "MMM-dd";
            labelStyle.FontAttributes = FontAttributes.Bold;
            e.LabelStyle = labelStyle;
            month = date.Month;
        }
        else
        {
            ChartAxisLabelStyle labelStyle = new();
            labelStyle.LabelFormat = "dd";
            e.LabelStyle = labelStyle;
        }
    }
}