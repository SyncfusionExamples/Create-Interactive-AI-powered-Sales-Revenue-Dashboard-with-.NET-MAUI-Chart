using Microsoft.Maui.Controls;
using Syncfusion.Maui.Toolkit.TabView;

namespace SalesDashboard
{
    public partial class MainPage : ContentPage
    {
        private readonly SalesTrendsViewModel salesTrendsViewModel;
        private readonly SalesDataService salesDataService;
        private readonly AzureBaseService baseAIService;


        private readonly PredictionService predictionService;
        private readonly PredictionViewModel predictionViewModel;
        public MainPage()
        {
            InitializeComponent();
            salesDataService = new SalesDataService();
            baseAIService = new AzureBaseService();
            predictionService = new PredictionService(baseAIService, salesDataService);

            predictionViewModel = new PredictionViewModel(predictionService, salesDataService);
            salesTrendsViewModel = new SalesTrendsViewModel(salesDataService);

            TabViewLoad();
        }

        void TabViewLoad()
        {
            SfTabView tabView = new SfTabView()
            {
                TabWidthMode = TabWidthMode.Default,
                TabBarPlacement = TabBarPlacement.Bottom,
                SelectedIndex = 0,
                TabBarHeight = 75,
                Margin = 10,
                IndicatorPlacement = TabIndicatorPlacement.Fill,
                IndicatorBackground = new SolidColorBrush(Color.FromArgb("#F5F5F5")),
            };

            SalesChart sales = new SalesChart(baseAIService) { BindingContext = salesTrendsViewModel };
            ProductDetails productDetails = new ProductDetails() { BindingContext = salesTrendsViewModel };
            OrderDetails orderDetails = new OrderDetails() { BindingContext = salesTrendsViewModel };
            Prediction prediction = new Prediction() { BindingContext = predictionViewModel };

            var tabItems = new TabItemCollection
            {
                new SfTabItem()
                {
                    Header = "Sales",
                    Content = sales,
                    ImageSource = "sales.png",
                    ImageSize = 35, FontSize = 18,
                    ImagePosition = TabImagePosition.Left,
                },
                new SfTabItem()
                {
                    Header = "Products",
                    Content = productDetails,
                    ImageSource = "product.png",
                    ImageSize = 35, FontSize = 18,
                    ImagePosition = TabImagePosition.Left,
                },
                new SfTabItem()
                {
                    Header = "Order",
                    Content = orderDetails,
                    ImageSource = "order.png",
                    ImageSize = 35, FontSize = 18,
                    ImagePosition = TabImagePosition.Left,
                },
                 new SfTabItem()
                {
                    Header = "Prediction",
                    Content = prediction,
                    ImageSource = "revenue.png",
                    ImageSize = 35, FontSize = 18,
                    ImagePosition = TabImagePosition.Left,
                },

            };

            tabView.Items = tabItems;
            this.Content = tabView;
        }
    }
}