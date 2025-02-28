using System.ComponentModel;

namespace SalesPerformanceAnalysis
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        ViewModel viewModel;
        ChartAIService aiService = new ChartAIService();
        public MainPage()
        {
            InitializeComponent();

             viewModel = new ViewModel(); // Ensure ViewModel is initialized and set
            BindingContext = viewModel;

            contentView.Content = new RevenueChart(){BindingContext = viewModel};
            revenue.Background = new SolidColorBrush(Color.FromArgb("#16D1DE"));
        }

        private void Revenue_Clicked(object sender, EventArgs e)
        {
            contentView.Content = new RevenueChart() { BindingContext = this.BindingContext };
            revenue.Background = new SolidColorBrush(Color.FromArgb("#16D1DE"));
            product.Background = new SolidColorBrush(Color.FromArgb("#353666"));
            orders.Background = new SolidColorBrush(Color.FromArgb("#353666"));
            sales.Background = new SolidColorBrush(Color.FromArgb("#353666"));
        }

        private void Sales_Clicked(object sender, EventArgs e)
        {
            contentView.Content = new SalesChart() { BindingContext = this.BindingContext };
            revenue.Background = new SolidColorBrush(Color.FromArgb("#353666"));
            product.Background = new SolidColorBrush(Color.FromArgb("#353666"));
            orders.Background = new SolidColorBrush(Color.FromArgb("#353666"));
            sales.Background = new SolidColorBrush(Color.FromArgb("#16D1DE"));
        }

        private void Order_Clicked(object sender, EventArgs e)
        {
            contentView.Content = new OrderDetails() { BindingContext = this.BindingContext };
            revenue.Background = new SolidColorBrush(Color.FromArgb("#353666"));
            product.Background = new SolidColorBrush(Color.FromArgb("#353666"));
            orders.Background = new SolidColorBrush(Color.FromArgb("#16D1DE"));
            sales.Background = new SolidColorBrush(Color.FromArgb("#353666"));
        }

        private void Product_Clicked(object sender, EventArgs e)
        {
            contentView.Content = new ProductDetails() { BindingContext = this.BindingContext };
            revenue.Background = new SolidColorBrush(Color.FromArgb("#353666"));
            product.Background = new SolidColorBrush(Color.FromArgb("#16D1DE"));
            orders.Background = new SolidColorBrush(Color.FromArgb("#353666"));
            sales.Background = new SolidColorBrush(Color.FromArgb("#353666"));
        }

        private void SfComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
        {
            double average;
            switch (e.AddedItems?[0].ToString())
            {
                case "Year":
                    viewModel.SalesReport = viewModel.GenerateYearlyData();
                    viewModel.OrderInfos = viewModel.YearlyOrders();
                    viewModel.AutoScrollingDelta = 6;
                    viewModel.SelectedItem = "Year";
                    average = viewModel.SalesReport.Average(x => x.Revenue);
                    viewModel.AverageRevenue = $"${average:F2} / Month";
                    break;

                case "Month":
                    viewModel.SalesReport = viewModel.GenerateMonthlyData(1); // Defaulting to January
                    viewModel.OrderInfos = viewModel.MonthlyOrders(1);
                    viewModel.AutoScrollingDelta = 8;
                    viewModel.SelectedItem = "Month";
                    average = viewModel.SalesReport.Average(x => x.Revenue);
                    viewModel.AverageRevenue = $"${average:F2} / Day";
                    break;

                case "Week":
                    viewModel.SalesReport = viewModel.GenerateWeeklyData();
                    viewModel.OrderInfos = viewModel.WeeklyOrders();
                    viewModel.AutoScrollingDelta = 5;
                    viewModel.SelectedItem = "Week";
                    average = viewModel.SalesReport.Average(x => x.Revenue);
                    viewModel.AverageRevenue = $"${average:F2} / Week";
                    break;
            }
        }

        private  async void SfButton_Clicked(object sender, EventArgs e)
        {
            var prompt = "Provide the insights of Electrical and Electronics products Sales and Revenue in 2024";
            //viewModel.Text = await aiService.GetAnswerFromGPT(prompt);
            viewModel.Text = prompt;
            popUp.Show();
        }
    }
}