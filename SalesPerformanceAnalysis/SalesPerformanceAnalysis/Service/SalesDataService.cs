using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPerformanceAnalysis
{
    public class SalesDataService
    {
        private List<SalesData> _cachedSalesData;
        private List<Product> _products;
        private List<Region> _regions;

        public SalesDataService()
        {
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            // In a real app, this would come from a database or API
            _products = new List<Product>
        {
            new Product { Id = "P001", Name = "Smartphone X", Category = "Electronics", BasePrice = 999.99m, IsActive = true },
            new Product { Id = "P002", Name = "Laptop Pro", Category = "Electronics", BasePrice = 1499.99m, IsActive = true },
            new Product { Id = "P003", Name = "Wireless Headphones", Category = "Audio", BasePrice = 199.99m, IsActive = true },
            new Product { Id = "P004", Name = "Smart Watch", Category = "Wearables", BasePrice = 249.99m, IsActive = true },
            new Product { Id = "P005", Name = "Tablet Ultra", Category = "Electronics", BasePrice = 599.99m, IsActive = true }
        };

            _regions = new List<Region>
        {
            new Region { Id = "R001", Name = "North America", Country = "USA", Latitude = 40.7128, Longitude = -74.0060 },
            new Region { Id = "R002", Name = "Europe", Country = "Germany", Latitude = 52.5200, Longitude = 13.4050 },
            new Region { Id = "R003", Name = "Asia Pacific", Country = "Japan", Latitude = 35.6762, Longitude = 139.6503 },
            new Region { Id = "R004", Name = "Latin America", Country = "Brazil", Latitude = -23.5505, Longitude = -46.6333 }
        };

            // Generate 2 years of sample data
            _cachedSalesData = GenerateSampleData();
        }

        private List<SalesData> GenerateSampleData()
        {
            var random = new Random(123); // Fixed seed for reproducibility
            var startDate = DateTime.Now.AddYears(-2);
            var endDate = DateTime.Now;

            var salesData = new List<SalesData>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                foreach (var product in _products)
                {
                    foreach (var region in _regions)
                    {
                        // Base quantity varies by product
                        var baseQuantity = product.Id switch
                        {
                            "P001" => 100,
                            "P002" => 50,
                            "P003" => 200,
                            "P004" => 75,
                            "P005" => 60,
                            _ => 50
                        };

                        // Seasonal effects
                        var month = date.Month;
                        var seasonalFactor = product.Category switch
                        {
                            "Electronics" => month is 11 or 12 ? 1.5 : 1.0, // Higher during holidays
                            "Wearables" => month is 1 or 6 ? 1.3 : 1.0,    // Higher in January and June
                            _ => 1.0
                        };

                        // Regional factor
                        var regionalFactor = region.Id switch
                        {
                            "R001" => 1.2,
                            "R002" => 1.1,
                            "R003" => 1.3,
                            "R004" => 0.9,
                            _ => 1.0
                        };

                        // Day of week factor
                        var dayOfWeekFactor = date.DayOfWeek switch
                        {
                            DayOfWeek.Saturday => 1.2,
                            DayOfWeek.Sunday => 0.8,
                            _ => 1.0
                        };

                        // Trending factor - products get more popular over time except for planned obsolescence
                        var daysSinceStart = (date - startDate).Days;
                        var trendFactor = product.Id switch
                        {
                            "P001" => Math.Min(1.5, 1.0 + daysSinceStart * 0.0005), // Growing trend
                            "P004" => Math.Min(1.8, 1.0 + daysSinceStart * 0.0008), // Strongly growing trend
                            "P002" => daysSinceStart > 365 ? 0.7 : 1.0,            // Decline after a year
                            _ => 1.0
                        };

                        // Random factor
                        var randomFactor = 0.8 + random.NextDouble() * 0.4; // 0.8 to 1.2

                        // Calculate quantity
                        var quantity = (int)(baseQuantity * seasonalFactor * regionalFactor * dayOfWeekFactor * trendFactor * randomFactor);

                        // Calculate unit price with small random variations
                        var unitPrice = product.BasePrice * (0.95m + (decimal)random.NextDouble() * 0.1m);

                        // Calculate total revenue
                        var revenue = quantity * unitPrice;

                        // Calculate cost (assume 60% of revenue is cost)
                        var cost = revenue * 0.6m;

                        // Create sales data entry
                        salesData.Add(new SalesData
                        {
                            Date = date,
                            ProductId = product.Id,
                            ProductName = product.Name,
                            RegionId = region.Id,
                            RegionName = region.Name,
                            Revenue = revenue,
                            Quantity = quantity,
                            UnitPrice = unitPrice,
                            Cost = cost
                        });

                        // Don't generate data for every combination to make it more realistic
                        if (random.NextDouble() > 0.8)
                            break;
                    }
                }
            }

            return salesData;
        }

        public async Task<List<SalesData>> GetSalesDataAsync(DateRange dateRange, string productId = null, string regionId = null)
        {
            // Simulate network delay
            await Task.Delay(200);

            return _cachedSalesData
                .Where(x => x.Date >= dateRange.StartDate && x.Date <= dateRange.EndDate)
                .Where(x => productId == null || x.ProductId == productId)
                .Where(x => regionId == null || x.RegionId == regionId)
                .ToList();
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            await Task.Delay(100);
            return _products.ToList();
        }

        public async Task<List<Region>> GetRegionsAsync()
        {
            await Task.Delay(100);
            return _regions.ToList();
        }

        public async Task<Dictionary<string, decimal>> GetSalesSummaryAsync(DateRange dateRange)
        {
            var salesData = await GetSalesDataAsync(dateRange);

            var totalRevenue = salesData.Sum(x => x.Revenue);
            var totalCost = salesData.Sum(x => x.Cost);
            var totalProfit = totalRevenue - totalCost;
            var avgProfitMargin = totalRevenue > 0 ? totalProfit / totalRevenue * 100 : 0;
            var totalQuantity = salesData.Sum(x => x.Quantity);

            var previousPeriodLength = (dateRange.EndDate - dateRange.StartDate).TotalDays;
            var previousPeriodStart = dateRange.StartDate.AddDays(-previousPeriodLength);
            var previousPeriodEnd = dateRange.StartDate.AddDays(-1);

            var previousPeriodData = await GetSalesDataAsync(new DateRange(previousPeriodStart, previousPeriodEnd));
            var previousRevenue = previousPeriodData.Sum(x => x.Revenue);
            var revenueGrowth = previousRevenue > 0 ? (totalRevenue - previousRevenue) / previousRevenue * 100 : 0;

            return new Dictionary<string, decimal>
        {
            { "TotalRevenue", totalRevenue },
            { "TotalCost", totalCost },
            { "TotalProfit", totalProfit },
            { "AverageProfitMargin", avgProfitMargin },
            { "TotalQuantity", totalQuantity },
            { "RevenueGrowth", revenueGrowth }
        };
        }
    }
}
