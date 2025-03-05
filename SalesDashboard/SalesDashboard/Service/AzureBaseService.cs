using Azure.AI.OpenAI;
using Azure;
using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SalesDashboard
{
    public  class AzureBaseService
    {
        #region Fields

        /// <summary>
        /// The EndPoint
        /// </summary>
     //   internal const string Endpoint = "YOUR_END_POINT_NAME";
        internal const string Endpoint = "https://mobilemaui.openai.azure.com/";

        /// <summary>
        /// The Deployment name
        /// </summary>
       // internal const string DeploymentName = "DEPLOYMENT_NAME";
        internal const string DeploymentName = "gpt-4o";

        /// <summary>
        /// The Image Deployment name
        /// </summary>
        internal const string ImageDeploymentName = "IMAGE_DEPOLYMENT_NAME";

        /// <summary>
        /// The API key
        /// </summary>
       // internal const string Key = "API_KEY";
        internal const string Key = "6673b6975f334c79bd0db8a1cd70aa49";

        /// <summary>
        /// The already credential validated field
        /// </summary>
        private static bool isAlreadyValidated = false;

        #endregion

        public AzureBaseService()
        {
            ValidateCredential();
        }

        #region Properties

        /// <summary>
        /// The kernal
        /// </summary>
        internal IChatClient? Client { get; set; }

        /// <summary>
        /// The chat histroy
        /// </summary>
        internal string? ChatHistory { get; set; }

        /// <summary>
        /// Gets or Set a value indicating whether an credentials are valid or not.
        /// </summary>
        internal static bool IsCredentialValid { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Validate Azure Credentials
        /// </summary>
        private async void ValidateCredential()
        {
            this.GetAzureOpenAIKernal();

            if (isAlreadyValidated)
            {
                return;
            }

            try
            {
                if (Client != null)
                {
                    await Client!.CompleteAsync("Hello, Test Check");
                    ChatHistory = string.Empty;
                    IsCredentialValid = true;
                    isAlreadyValidated = true;
                }
                else
                {
                    ShowAlertAsync();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// To get the Azure open ai kernal method
        /// </summary>
        private void GetAzureOpenAIKernal()
        {
            try
            {
                var client = new AzureOpenAIClient(new Uri(Endpoint), new AzureKeyCredential(Key)).AsChatClient(modelId: DeploymentName);
                this.Client = client;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Show Alert Popup
        /// </summary>
        private async void ShowAlertAsync()
        {
            var page = Application.Current?.Windows[0].Page;
            if (page != null && !IsCredentialValid)
            {
                isAlreadyValidated = true;
                await page.DisplayAlert("Alert", "The Azure API key or endpoint is missing or incorrect. Please verify your credentials. You can also continue with the offline data.", "OK");
            }
        }

        internal async Task<string> GetAnswerFromGPT(string userPrompt)
        {
            try
            {
                if (IsCredentialValid && Client != null)
                {
                    ChatHistory = string.Empty;
                    // Add the system message and user message to the options
                    ChatHistory = ChatHistory + userPrompt;
                    var response = await Client.CompleteAsync(ChatHistory);
                    return response.ToString();
                }
            }
            catch
            {
                // If an exception occurs (e.g., network issues, API errors), return an empty string.
                return "";
            }

            return "";
        }

        public async Task<string> AnalyzeSalesInsights(List<SalesData> salesData, DateRange period)
        {
            var systemMessage = @"
        You are an expert sales analyst AI. Provide concise insights about the sales data, focusing on:
        1. Key trends and patterns
        2. Top and bottom performing products and regions
        3. Anomalies or unusual patterns
        4. Actionable recommendations
        Keep your analysis concise, business-focused and actionable.
        ";

            var salesSummary = salesData
                .GroupBy(x => x.ProductName)
                .Select(g => new
                {
                    Product = g.Key,
                    TotalRevenue = g.Sum(x => x.Revenue),
                    TotalQuantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(10)
                .ToList();

            var jsonSummary = JsonSerializer.Serialize(salesSummary);

            var userMessage = $@"
        Here is a summary of the sales data from {period.StartDate:yyyy-MM-dd} to {period.EndDate:yyyy-MM-dd}:
        {jsonSummary}
        
        Please provide a concise business analysis with actionable insights.
        ";

            return await GetAnswerFromGPT(systemMessage + "\n\n" + userMessage);
        }

        public async Task<List<SalesPrediction>> PredictSalesTrend(
            List<SalesData> historicalData,
            DateRange predictionPeriod,
            string productId = null,
            string regionId = null)
        {
            var systemMessage = @"
        You are an expert sales forecasting AI. Your task is to analyze historical sales data and provide accurate predictions.
        Follow these guidelines:
        1. Analyze trends, seasonality, and patterns in the data
        2. Consider product-specific and region-specific patterns
        3. Output predictions in JSON format with confidence intervals
        4. Identify and explain potential anomalies
        5. Provide a brief explanation for each prediction
        ";

            var jsonData = JsonSerializer.Serialize(historicalData.Take(10).ToList());

            var startDate = predictionPeriod.StartDate.ToString("yyyy-MM-dd");
            var endDate = predictionPeriod.EndDate.ToString("yyyy-MM-dd");

            var userMessage = $@"
        Here is the historical sales data:
        {jsonData}
        
        Please predict sales from {startDate} to {endDate}.
        " + (productId != null ? $" Focus on product ID: {productId}." : "")
              + (regionId != null ? $" Focus on region ID: {regionId}." : "");

            var response = await GetAnswerFromGPT(systemMessage + "\n\n" + userMessage);

            string extractedJson = JsonExtractor.ExtractJson(response);


            try
            {
                return JsonSerializer.Deserialize<List<SalesPrediction>>(extractedJson) ?? new List<SalesPrediction>();
            }
            catch (Exception ex)
            {
                return new List<SalesPrediction>();
            }

        }

    }

        #endregion



public class JsonExtractor
    {
        public static string ExtractJson(string response)
        {
            try
            {
                // Use Regex to extract JSON content from the response
                Match match = Regex.Match(response, @"\[\s*\{[\s\S]*?\}\s*\]", RegexOptions.Singleline);
                if (match.Success)
                {
                    string json = match.Value;

                    // Validate JSON format to ensure it's correct
                    using (JsonDocument.Parse(json))
                    {
                        return json; // Return extracted JSON
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting JSON: {ex.Message}");
            }

            return "Invalid or No JSON Found";
        }
}



        


    }
