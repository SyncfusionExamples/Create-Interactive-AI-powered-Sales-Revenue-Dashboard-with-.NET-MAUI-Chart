using Syncfusion.Maui.Toolkit.Charts;
using Syncfusion.Maui.Toolkit.SegmentedControl;

namespace AISalesDashboard;

public partial class PredictionAndroid : ContentView
{
    int month = int.MaxValue;
    public PredictionAndroid()
    {
        InitializeComponent();

        segmentedControl.ItemsSource = new List<SfSegmentItem>()
            {
                new SfSegmentItem(){ ImageSource = "chart_light.png"},
                new SfSegmentItem(){ ImageSource = "grid_dark.png"}
            };
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

    private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        switch (e.NewIndex)
        {
            case 0:
                segmentedControl.ItemsSource = new List<SfSegmentItem>()
                    {
                        new SfSegmentItem(){ ImageSource = "chart_light.png"},
                        new SfSegmentItem(){ ImageSource = "grid_dark.png"}
                    };
                chart.IsVisible = true;
                dataGrid.IsVisible = false;
                break;

            case 1:
                segmentedControl.ItemsSource = new List<SfSegmentItem>()
                    {
                        new SfSegmentItem(){ ImageSource = "chart_dark.png"},
                        new SfSegmentItem(){ ImageSource = "grid_light.png"}
                    };
                chart.IsVisible = false;
                dataGrid.IsVisible = true;
                break;
        }
    }
}