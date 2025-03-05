using Syncfusion.Maui.Toolkit.Charts;
using System.Globalization;

namespace SalesPerformanceAnalysis;

public partial class RevenueChart : ContentView
{
    private readonly PredictionViewModel _viewModel;
    public RevenueChart(PredictionViewModel viewModel)
	{
	   InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

}