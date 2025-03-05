using Syncfusion.Maui.Toolkit.Buttons;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using System;


namespace SalesPerformanceAnalysis
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {

        private bool tapped;
        Animation animation;

        private readonly ViewModel viewModel = new();
        private readonly SalesTrendsViewModel salesTrendsViewModel;
        private readonly SalesDataService salesDataService;
        private readonly AzureBaseService baseAIService;


        private readonly PredictionService predictionService;
        private readonly PredictionViewModel predictionViewModel;

        public MainPage()
        {
            InitializeComponent();



            animation = new Animation();
            tapped = false;

            salesDataService = new SalesDataService();
            baseAIService = new AzureBaseService();
            predictionService = new PredictionService(baseAIService, salesDataService);


            predictionViewModel = new PredictionViewModel(predictionService, salesDataService);
            salesTrendsViewModel = new SalesTrendsViewModel(salesDataService);
            contentView.Content = new SalesChart(salesTrendsViewModel);
            sales.Background = new SolidColorBrush(Color.FromArgb("#F5F5F5"));

            RemoveChildIfExists(exportSelection);
            BindingContext = salesTrendsViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StartBubbleAnimation();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (this.AnimationIsRunning("BubbleEffect"))
            {
                this.AbortAnimation("BubbleEffect");
            }

            if (this.animation != null)
            {
                animation.Dispose();
            }
        }

        private void Revenue_Clicked(object sender, EventArgs e) => SetPageContent(new RevenueChart(predictionViewModel), "AverageRevenue", revenue, null, false);

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
            tapped = true;
            StopBubbleAnimation();

            if (viewModel == null || baseAIService == null)
                return;

            // Ensure period selection is valid
            if (periodSelection.SelectedItem is DateRangeOption selectedOption)
            {
                salesTrendsViewModel.SelectedDateRange = selectedOption.Value;

                await Task.Run(() => salesTrendsViewModel.Initialize());

                var prompt = GeneratePrompt(salesTrendsViewModel.SalesData, salesTrendsViewModel.SelectedDateRange);

                // Get AI response
                string aiResponse = await baseAIService.GetAnswerFromGPT(prompt);

                // Clean and format the AI response
                salesTrendsViewModel.Text = CleanAndFormatOutput(aiResponse);

                popUp.Show();
            }
        }

        public string GeneratePrompt(ObservableCollection<SalesData> salesData, DateRange dateRange)
        {
            if (salesData == null || !salesData.Any())
                return $"Generate a summary for {dateRange}, but no data is available.";

            // Filter sales data based on the selected date range
            var filteredData = salesData
                .Where(s => s.Date >= dateRange.StartDate && s.Date <= dateRange.EndDate)
                .ToList();

            if (!filteredData.Any())
                return $"Analyze the sales trend for {dateRange}, but no data is available within this period.";

            // Format the filtered sales data
            string formattedItems = string.Join(", ", filteredData.Select(s => $"[{s.Date:yyyy-MM-dd}] {s.ProductName}: {s.Profit}"));

            // Construct the prompt dynamically
            return $"Analyze the following sales data for {dateRange.StartDate:yyyy-MM-dd} to {dateRange.EndDate:yyyy-MM-dd}: {formattedItems}. Provide insights.";
        }

        private string CleanAndFormatOutput(string aiResponse)
        {
            if (string.IsNullOrWhiteSpace(aiResponse))
                return string.Empty;

            // Remove unwanted Markdown characters
            aiResponse = aiResponse.Replace("####", "")  // Remove section headers
                                   .Replace("###", "")   // Remove extra headers
                                   .Replace("**", "");
            // Ensure product titles remain bold
            aiResponse = Regex.Replace(aiResponse, @"(\d+\.\s)([A-Za-z\s]+)", m =>
                $"{m.Groups[1].Value}{m.Groups[2].Value.Trim()}");

            return aiResponse.Trim();
        }

        private void StartBubbleAnimation()
        {
            if (!tapped)
            {
                var bubbleEffect = new Animation(v => aiButton.Scale = v, 1, 1.15, Easing.CubicInOut);
                var fadeEffect = new Animation(v => aiButton.Opacity = v, 1, 0.5, Easing.CubicInOut);

                animation.Add(0, 0.5, bubbleEffect);
                animation.Add(0, 0.5, fadeEffect);
                animation.Add(0.5, 1, new Animation(v => aiButton.Scale = v, 1.15, 1, Easing.CubicInOut));
                animation.Add(0.5, 1, new Animation(v => aiButton.Opacity = v, 0.5, 1, Easing.CubicInOut));

                animation.Commit(this, "BubbleEffect", length: 1500, easing: Easing.CubicInOut, repeat: () => true);
                //animation.Commit(this, "BubbleEffect", length: 1000, easing: Easing.CubicInOut);
                //finished: (v, c) => StartBubbleAnimation()); // Loop the animation
            }
        }

        private void StopBubbleAnimation()
        {
            this.AbortAnimation("BubbleEffect");
            tapped = false;
            aiButton.Scale = 1;
            aiButton.Opacity = 1;
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

        private void periodSelection_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is DateRangeOption selectedOption)
            {
                var viewModel = BindingContext as SalesTrendsViewModel;
                if (viewModel != null)
                {
                    viewModel.SelectedDateRange = selectedOption.Value; 
                    viewModel.Initialize();
                    viewModel.LoadDashboardData();
                }
            }
        }
    }
}