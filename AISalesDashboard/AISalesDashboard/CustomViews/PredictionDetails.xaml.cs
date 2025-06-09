
using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.Charts;

namespace AISalesDashboard;

public partial class PredictionDetails : ContentView
{
    int month = int.MaxValue;
    public PredictionDetails()
	{
		InitializeComponent();

        segmentedControl.ItemsSource = new List<SfSegmentItem>()
            {
                new SfSegmentItem(){ Text = "\ue26b", TextStyle = new SegmentTextStyle(){ FontFamily = "MaterialSymbolOutlined", FontSize = 16 } },
                new SfSegmentItem(){ Text = "\ue9b0", TextStyle = new SegmentTextStyle(){ FontFamily = "MaterialSymbolOutlined", FontSize = 16} }
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

    private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        switch (e.NewIndex)
        {
            case 0:
                segmentedControl.ItemsSource = new List<SfSegmentItem>()
                    {
                        new SfSegmentItem(){ Text = "\ue26b", TextStyle = new SegmentTextStyle(){ FontFamily = "MaterialSymbolOutlined", FontSize = 16 } },
                        new SfSegmentItem(){ Text = "\ue9b0", TextStyle = new SegmentTextStyle(){ FontFamily = "MaterialSymbolOutlined", FontSize = 16} }
                    };
                chart.IsVisible = true;
                dataGrid.IsVisible = false;
                break;
            case 1:
                segmentedControl.ItemsSource = new List<SfSegmentItem>()
                    {
                        new SfSegmentItem(){ Text = "\ue26b", TextStyle = new SegmentTextStyle(){ FontFamily = "MaterialSymbolOutlined", FontSize = 16 } },
                        new SfSegmentItem(){ Text = "\ue9b0", TextStyle = new SegmentTextStyle(){ FontFamily = "MaterialSymbolOutlined", FontSize = 16} }
                    };
                chart.IsVisible = false;
                dataGrid.IsVisible = true;
                break;
        }
    }
}