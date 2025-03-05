using Azure;
using Azure.AI.OpenAI;
using SalesPerformanceAnalysis;
using System.Text.Json;

namespace SalesPerformanceAnalysis;

public class OpenAIService
{
    private readonly OpenAIClient _client;
    private readonly string _deploymentName;
    private readonly SettingsService _settingsService;

    public OpenAIService(SettingsService settingsService)
    {
        _settingsService = settingsService;
        var endpoint = _settingsService.AzureOpenAIEndpoint;
        var key = _settingsService.AzureOpenAIKey;
        _deploymentName = _settingsService.AzureOpenAIDeploymentName;

        _client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
    }

    public async Task<List<SalesPrediction>> PredictSalesTrend(
        List<SalesData> historicalData,
        DateRange predictionPeriod,
        string productId = null,
        string regionId = null)
    {
        // Prepare the historical data for the API
        var jsonData = JsonSerializer.Serialize(historicalData.Take(100).ToList());

        // Create the system message for the model
        var systemMessage = @"
        You are an expert sales forecasting AI. Your task is to analyze historical sales data and provide accurate predictions.
        Follow these guidelines:
        1. Analyze trends, seasonality, and patterns in the data
        2. Consider product-specific and region-specific patterns
        3. Output predictions in JSON format with confidence intervals
        4. Identify and explain potential anomalies
        5. Provide a brief explanation for each prediction
        ";

        // Create the user message with the data and request
        var startDate = predictionPeriod.StartDate.ToString("yyyy-MM-dd");
        var endDate = predictionPeriod.EndDate.ToString("yyyy-MM-dd");

        var userMessage = $@"
        Here is the historical sales data:
        {jsonData}
        
        Please predict sales from {startDate} to {endDate}.
        " + (productId != null ? $" Focus on product ID: {productId}." : "")
          + (regionId != null ? $" Focus on region ID: {regionId}." : "");

        // Create chat completion options
        var chatCompletionOptions = new ChatCompletionsOptions
        {
            DeploymentName = _deploymentName,
            Temperature = 0.1f,
            MaxTokens = 2000,
            ResponseFormat = ChatCompletionsResponseFormat.JsonObject
        };

        // Add messages
        chatCompletionOptions.Messages.Add(new ChatRequestSystemMessage(systemMessage));
        chatCompletionOptions.Messages.Add(new ChatRequestUserMessage(userMessage));

        try
        {
            // Get the prediction from Azure OpenAI
            var response = await _client.GetChatCompletionsAsync(chatCompletionOptions);
            var prediction = response.Value.Choices[0].Message.Content;

            // Parse the prediction response
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var predictions = JsonSerializer.Deserialize<List<SalesPrediction>>(prediction, options);

            return predictions ?? new List<SalesPrediction>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error predicting sales: {ex.Message}");
            return new List<SalesPrediction>();
        }
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

        var chatCompletionOptions = new ChatCompletionsOptions
        {
            DeploymentName = _deploymentName,
            Temperature = 0.5f,
            MaxTokens = 1000
        };

        chatCompletionOptions.Messages.Add(new ChatRequestSystemMessage(systemMessage));
        chatCompletionOptions.Messages.Add(new ChatRequestUserMessage(userMessage));

        try
        {
            var response = await _client.GetChatCompletionsAsync(chatCompletionOptions);
            return response.Value.Choices[0].Message.Content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error analyzing sales insights: {ex.Message}");
            return "Unable to analyze sales insights at this time.";
        }
    }

    public async Task<string> AnswerSalesQuery(string query, List<SalesData> contextData)
    {
        var systemMessage = @"
        You are a helpful sales analysis assistant. Answer questions about sales data accurately
        and concisely, based on the provided sales context data. If you cannot answer a question
        based on the provided data, state that clearly.
        ";

        var contextSummary = contextData
            .GroupBy(x => new { x.ProductName, x.RegionName })
            .Select(g => new
            {
                Product = g.Key.ProductName,
                Region = g.Key.RegionName,
                TotalRevenue = g.Sum(x => x.Revenue),
                TotalQuantity = g.Sum(x => x.Quantity),
                AveragePrice = g.Average(x => x.UnitPrice)
            })
            .Take(20)
            .ToList();

        var jsonContext = JsonSerializer.Serialize(contextSummary);

        var userMessage = $@"
        Here is a summary of recent sales data:
        {jsonContext}
        
        Query: {query}
        
        Please provide a concise and accurate answer based on this data.
        ";

        var chatCompletionOptions = new ChatCompletionsOptions
        {
            DeploymentName = _deploymentName,
            Temperature = 0.3f,
            MaxTokens = 800
        };

        chatCompletionOptions.Messages.Add(new ChatRequestSystemMessage(systemMessage));
        chatCompletionOptions.Messages.Add(new ChatRequestUserMessage(userMessage));

        try
        {
            var response = await _client.GetChatCompletionsAsync(chatCompletionOptions);
            return response.Value.Choices[0].Message.Content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error answering sales query: {ex.Message}");
            return "I'm unable to answer your query at this time due to a technical issue.";
        }
    }
}