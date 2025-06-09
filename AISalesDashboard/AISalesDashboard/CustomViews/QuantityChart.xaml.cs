using Syncfusion.Maui.Charts;

namespace AISalesDashboard;

public partial class QuantityChart : ContentView
{
    int month = int.MaxValue;
    public QuantityChart()
	{
		InitializeComponent();
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

    private void NumericalAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
    {
        double value = Convert.ToDouble(e.Label);
        if (value >= 1000000000)
        {
            e.Label = $"{value / 1000000000:0.#}B";
        }
        else if (value >= 1000000)
        {
            e.Label = $"{value / 1000000:0.#}M";
        }
        else if (value >= 1000)
        {
            e.Label = $"{value / 1000:0.#}K";
        }
        else
        {
            e.Label = value.ToString("0.#");
        }
    }
}