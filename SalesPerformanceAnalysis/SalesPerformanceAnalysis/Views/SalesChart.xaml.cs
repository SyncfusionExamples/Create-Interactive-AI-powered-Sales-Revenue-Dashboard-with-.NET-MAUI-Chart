using Syncfusion.Maui.Toolkit.Charts;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SalesPerformanceAnalysis;

public partial class SalesChart : ContentView
{
    private readonly SalesTrendsViewModel _viewModel;

    public SalesChart(SalesTrendsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
 
}