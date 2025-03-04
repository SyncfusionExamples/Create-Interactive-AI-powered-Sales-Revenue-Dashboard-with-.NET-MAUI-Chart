using Microsoft.Maui.Controls;
using Syncfusion.Maui.Inputs;
using Syncfusion.Maui.Toolkit.Buttons;
using System.ComponentModel;

namespace SalesPerformanceAnalysis
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private readonly ViewModel viewModel = new();
        private readonly ChartAIService aiService = new();
        private readonly SalesTrendsViewModel salesTrendsViewModel;
        private readonly SalesDataService salesDataService = new();

        public MainPage()
        {
            InitializeComponent();
            salesTrendsViewModel = new SalesTrendsViewModel(salesDataService);
            contentView.Content = new SalesChart(salesTrendsViewModel);
            sales.Background = new SolidColorBrush(Color.FromArgb("#F5F5F5"));

            RemoveChildIfExists(exportSelection);
            BindingContext = salesTrendsViewModel;
        }

        private void Revenue_Clicked(object sender, EventArgs e) => SetPageContent(new RevenueChart() { BindingContext = viewModel}, "AverageRevenue", revenue, null, false);

        private void Sales_Clicked(object sender, EventArgs e) => SetPageContent(new SalesChart(salesTrendsViewModel), "AverageSales", sales, aiButton, false);

        private void Order_Clicked(object sender, EventArgs e) => SetPageContent(new OrderDetails() { BindingContext = viewModel }, "AverageRevenue", orders, exportSelection, false);

        private void Product_Clicked(object sender, EventArgs e) => SetPageContent(new ProductDetails() { BindingContext = viewModel }, "AverageRevenue", product, exportSelection,  true);

        private void SetPageContent(View newContent, string bindingProperty, SfButton activeButton, View additionalChild, bool removePeriod )
        {
            contentView.Content = newContent;
            SetButtonStyles(activeButton);

            RemoveChildIfExists(exportSelection);
            RemoveChildIfExists(aiButton);
            if (removePeriod) RemoveChildIfExists(periodSelection);
            else AddChildIfNotExists(periodSelection, 0);

            AddChildIfNotExists(additionalChild, 1);
           // average.SetBinding(Label.TextProperty, new Binding(bindingProperty));
        }

        private void SetButtonStyles(SfButton activeButton)
        {
            revenue.Background = product.Background = orders.Background = sales.Background = new SolidColorBrush(Color.FromArgb("#FFFFFF"));
            activeButton.Background = new SolidColorBrush(Color.FromArgb("#F5F5F5"));
        }

        private void RemoveChildIfExists(View child)
        {
            if (child != null && middleLayout.Children.Contains(child))
            {
                middleLayout.Children.Remove(child);
            }
        }

        void AddChildIfNotExists(View child, int index)
        {
            if (child != null && !middleLayout.Children.Contains(child))
            {
                // Ensure index is within valid bounds
                if (index < 0) index = 0;
                if (index > middleLayout.Children.Count) index = middleLayout.Children.Count;

                middleLayout.Children.Insert(index, child);
            }
        }


        private async void SfButton_Clicked(object sender, EventArgs e)
        {
            var items = viewModel.RevenueData.Take(40).ToList();
            var prompt = aiService.GeneratePrompt(items, periodSelection.SelectedItem.ToString());
            viewModel.Text = await aiService.GetAnswerFromGPT(prompt);
            popUp.Show();
        }

        private void Export_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
        {
            if (contentView.Content is OrderDetails orderPage)
            {
                if (exportSelection.SelectedIndex == 1) orderPage.ExportPDF(sender, null);
                else if (exportSelection.SelectedIndex == 0) orderPage.ExportAsExcel(sender, null);
            }
            else if (contentView.Content is ProductDetails productPage)
            {
                if (exportSelection.SelectedIndex == 1) productPage.ExportPDF(sender, null);
                else if (exportSelection.SelectedIndex == 0) productPage.ExportAsExcel(sender, null);
            }
        }
    }
}
