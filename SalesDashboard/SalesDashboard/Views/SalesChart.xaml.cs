using Syncfusion.Maui.Toolkit.Charts;

namespace SalesDashboard;

public partial class SalesChart : ContentView
{
    int month = int.MaxValue;
    public SalesChart()
    {
        InitializeComponent();
    }

    private void periodSelection_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is DateRangeOption selectedOption)

            if (this.BindingContext is SalesTrendsViewModel salesTrendsViewModel)
            {
                salesTrendsViewModel.SelectedDateRange = selectedOption.Value;
                salesTrendsViewModel.Initialize();
                salesTrendsViewModel.LoadDashboardData();
            }
    }

    private void DateTimeAxis_LabelCreated(object sender, Syncfusion.Maui.Toolkit.Charts.ChartAxisLabelEventArgs e)
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