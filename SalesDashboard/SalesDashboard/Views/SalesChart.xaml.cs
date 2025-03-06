namespace SalesDashboard;

public partial class SalesChart : ContentView
{
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
}