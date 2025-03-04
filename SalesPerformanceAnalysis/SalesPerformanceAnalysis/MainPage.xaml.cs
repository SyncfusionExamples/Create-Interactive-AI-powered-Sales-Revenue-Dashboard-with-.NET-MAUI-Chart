using Microsoft.Maui.Controls;
using Syncfusion.XlsIO.Implementation.PivotAnalysis;
using System.ComponentModel;

namespace SalesPerformanceAnalysis
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        ViewModel viewModel;
        ChartAIService aiService = new ChartAIService();
        private ProductDetails productDetailsPage;
        private OrderDetails orderDetailsPage;

        public MainPage()
        {
            InitializeComponent();

            viewModel = new ViewModel(); // Ensure ViewModel is initialized and set
            BindingContext = viewModel; 

            contentView.Content = new RevenueChart() { BindingContext = viewModel };
            revenue.Background = new SolidColorBrush(Color.FromArgb("#F5F5F5"));

            if (exportSelection != null && middleLayout.Children.Contains(exportSelection))
            {
                middleLayout.Children.Remove(exportSelection);
            }
        }

        private void Revenue_Clicked(object sender, EventArgs e)
        {
            contentView.Content = new RevenueChart() { BindingContext = this.BindingContext };
            revenue.Background = new SolidColorBrush(Color.FromArgb("#F5F5F5"));
            product.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));
            orders.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));
            sales.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));

            if (exportSelection != null && middleLayout.Children.Contains(exportSelection))
            {
                middleLayout.Children.Remove(exportSelection);
                exportSelection.SelectedIndex = -1;
            }
           
            average.SetBinding(Label.TextProperty, new Binding("AverageRevenue"));
        }

        private void Sales_Clicked(object sender, EventArgs e)
        {
            contentView.Content = new SalesChart() { BindingContext = this.BindingContext };
            revenue.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));
            product.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));
            orders.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));
            sales.Background = new SolidColorBrush(Color.FromArgb("#F5F5F5"));

            if (exportSelection != null && middleLayout.Children.Contains(exportSelection))
            {
                middleLayout.Children.Remove(exportSelection);
                exportSelection.SelectedIndex = -1;
            }

            average.SetBinding(Label.TextProperty, new Binding("AverageSales"));
        }

        private void Order_Clicked(object sender, EventArgs e)
        {
            orderDetailsPage = new OrderDetails() { BindingContext = this.BindingContext };
            contentView.Content = orderDetailsPage;
            revenue.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));
            product.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));
            orders.Background = new SolidColorBrush(Color.FromArgb("#F5F5F5"));
            sales.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));

            if (!middleLayout.Children.Contains(exportSelection))
            {
                middleLayout.Children.Insert(1, exportSelection);
                exportSelection.SelectedIndex = -1;
            }

            average.SetBinding(Label.TextProperty, new Binding("AverageRevenue"));
        }

        private void Product_Clicked(object sender, EventArgs e)
        {
            productDetailsPage = new ProductDetails() { BindingContext = this.BindingContext };
            contentView.Content = productDetailsPage;
            revenue.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));
            product.Background = new SolidColorBrush(Color.FromArgb("#F5F5F5"));
            orders.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));
            sales.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));

            if (!middleLayout.Children.Contains(exportSelection))
            {
                middleLayout.Children.Insert(1, exportSelection);
                exportSelection.SelectedIndex = -1;
            }

            average.SetBinding(Label.TextProperty, new Binding("AverageRevenue"));
        }

        private void SfComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
        {
            double averageRevenue;
            double averageSales;
            switch (e.AddedItems?[0].ToString())
            {
                case "Year":
                    viewModel.SalesReport = viewModel.GenerateYearlyData();
                    viewModel.RevenueData = viewModel.GenerateYearlyRevenueData();
                    viewModel.OrderInfos = viewModel.YearlyOrders();
                    viewModel.AutoScrollingDelta = 6;
                    viewModel.SelectedItem = "Year";
                    averageRevenue = viewModel.RevenueData.Average(x => x.Revenue);
                    averageSales = viewModel.SalesReport.Average(x => x.Sales);
                    viewModel.AverageRevenue = $"${averageRevenue:F2} / Month";
                    viewModel.AverageSales = $"{averageSales:F2} / Month";
                    break;

                case "Month":
                    viewModel.SalesReport = viewModel.GenerateMonthlyData(1); // Defaulting to January
                    viewModel.RevenueData = viewModel.GenerateMonthlyRevenueData(1);
                    viewModel.OrderInfos = viewModel.MonthlyOrders(1);
                    viewModel.AutoScrollingDelta = 8;
                    viewModel.SelectedItem = "Month";
                    averageRevenue = viewModel.RevenueData.Average(x => x.Revenue);
                    averageSales = viewModel.SalesReport.Average(x => x.Sales);
                    viewModel.AverageRevenue = $"${averageRevenue:F2} / Day";
                    viewModel.AverageSales = $"{averageSales:F2} / Day";
                    break;

                case "Week":
                    viewModel.SalesReport = viewModel.GenerateWeeklyData();
                    viewModel.RevenueData = viewModel.GenerateWeeklyRevenueData();
                    viewModel.OrderInfos = viewModel.WeeklyOrders();
                    viewModel.AutoScrollingDelta = 5;
                    viewModel.SelectedItem = "Week";
                    averageRevenue = viewModel.RevenueData.Average(x => x.Revenue);
                    averageSales = viewModel.SalesReport.Average(x => x.Sales);
                    viewModel.AverageRevenue = $"${averageRevenue:F2} / Week";
                    viewModel.AverageSales = $"{averageSales:F2} / Week";
                    break;
            }
        }

        private async void SfButton_Clicked(object sender, EventArgs e)
        {
            var data = viewModel.RevenueData;
            var items = data.Take(40).ToList();
            var prompt = ChartAIService.GeneratePrompt(items, periodSelection.SelectedItem.ToString());


            viewModel.Text = await aiService.GetAnswerFromGPT(prompt);
            // viewModel.Text = prompt;
            popUp.Show();
            // popup
            // busy 
            // content - prompt
            // stop busy
        }

        private void ForecaseData_Clicked(object sender, EventArgs e)
        {

        }

        private void Export_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
        {
            if (exportSelection.SelectedIndex == 1)
            {
                if (contentView.Content == orderDetailsPage)
                {
                    orderDetailsPage.ExportPDF(sender, null);
                }
                else
                {
                    productDetailsPage.ExportPDF(sender, null);
                }
            }
            else if(exportSelection.SelectedIndex == 0)
            {
                if (contentView.Content == orderDetailsPage)
                {
                    orderDetailsPage.ExportAsExcel(sender, null);
                }
                else
                {
                    productDetailsPage.ExportAsExcel(sender, null);
                }
            }
        }
    }
}