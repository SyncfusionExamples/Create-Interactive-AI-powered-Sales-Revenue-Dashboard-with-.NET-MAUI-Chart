using Syncfusion.Maui.Toolkit.Charts;
using System.Globalization;

namespace SalesPerformanceAnalysis;

public partial class RevenueChart : ContentView
{
	public RevenueChart()
	{
	InitializeComponent();
	}

    private void CategoryAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
    {
        if (this.BindingContext is ViewModel viewModel && sender is CategoryAxis axis)
        {
            var selectedItem = viewModel.SelectedItem;
            string labelText = e.Label.ToString(); // Get the current label text

            switch (selectedItem)
            {
                case "Year":
                    // Show only month names: "Jan", "Feb", ..., "Dec"
                    if (int.TryParse(labelText, out int month) && month >= 1 && month <= 12)
                    {
                        e.Label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
                    }

                    break;

                case "Month":

                    if (axis.VisibleLabels.Count > 0)
                    {
                        string firstVisible = axis.VisibleLabels[0].Content.ToString();

                        // Extract numeric part (day) from "Jan 24"
                        string numericPart = new string(e.Label.Where(char.IsDigit).ToArray());

                        if (int.TryParse(numericPart, out int day) && day >= 1 && day <= 31)
                        {
                            // First visible label should include month
                            e.Label = e.Label == firstVisible ? $"Jan-{day:D2}" : day.ToString();
                        }
                    }
                    break;

                case "Week":
                    // Convert "1", "2", etc., to actual day names (Monday, Tuesday, etc.)
                    if (int.TryParse(labelText, out int weekDay) && weekDay >= 1 && weekDay <= 7)
                    {
                        string[] weekDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                        e.Label = weekDays[weekDay - 1];
                    }
                    break;
            }
        }
    }

}