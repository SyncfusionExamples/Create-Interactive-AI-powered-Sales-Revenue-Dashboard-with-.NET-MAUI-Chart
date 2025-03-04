

using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using System.Globalization;
using System.Text;

namespace SalesPerformanceAnalysis
{
    internal class ChartAIService
    {
        #region Fields

        /// <summary>
        /// The EndPoint
        /// </summary>
        internal const string endpoint = "https://mobilemaui.openai.azure.com/";

        /// <summary>
        /// The Deployment name
        /// </summary>
        internal const string deploymentName = "gpt-4o";

        /// <summary>
        /// The Image Deployment name
        /// </summary>
        internal const string imageDeploymentName = "IMAGE_MODEL_NAME";

        /// <summary>
        /// The API key
        /// </summary>
        internal const string key = "6673b6975f334c79bd0db8a1cd70aa49";

        /// <summary>
        /// The already credential validated field
        /// </summary>
        private static bool isAlreadyValidated;

        /// <summary>
        /// The uri result field
        /// </summary>
        private Uri? uriResult;

        #endregion

        public ChartAIService()
        {
            ValidateCredential();
        }

        #region Properties

        internal IChatClient? Client { get; set; }

        internal string? ChatHistory { get; set; }

        internal static bool IsCredentialValid { get; set; }

        #endregion

        #region Private Methods

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
                    await Client.GetResponseAsync("Hello, Test Check");
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

        #region Azure OpenAI
        /// <summary>
        /// To get the Azure open ai kernal method
        /// </summary>
        private void GetAzureOpenAIKernal()
        {
            try
            {
                var client = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key)).AsChatClient(modelId: deploymentName);
                this.Client = client;
            }
            catch (Exception)
            {
            }
        }
        #endregion

        /// <summary>
        /// Retrieves an answer from the deployment name model using the provided user prompt.
        /// </summary>
        /// <param name="userPrompt">The user prompt.</param>
        /// <returns>The AI response.</returns>
        internal async Task<string> GetAnswerFromGPT(string userPrompt)
        {
            try
            {
                if (IsCredentialValid && ChatHistory != null && Client != null)
                {
                    ChatHistory = string.Empty;
                    // Add the system message and user message to the options
                    ChatHistory = ChatHistory + userPrompt;
                    var response = await Client.GetResponseAsync(ChatHistory);
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


        public static string GeneratePrompt(List<SalesData> items, string selectedCategory)
        {
            string prompt = $"Predicted revenue values for {selectedCategory} data in 2026:\n";

            switch (selectedCategory)
            {
                case "Year":
                    prompt += "Yearly Revenue Data for 2026:\n";
                    for (int month = 1; month <= 12; month++)
                    {
                        string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
                        var data = items.FirstOrDefault(d => d.Category == $"{monthName} 2026");
                        prompt += $"{monthName} 2026: {data?.Revenue ?? 0}\n";
                    }
                    break;

                case "Month":
                    int selectedMonth = 1; // January
                    string monthNameForMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(selectedMonth);
                    int daysInMonth = DateTime.DaysInMonth(2026, selectedMonth);

                    prompt += $"Daily Revenue Data for {monthNameForMonth} 2026:\n";
                    for (int day = 1; day <= daysInMonth; day++)
                    {
                        string dateKey = $"{monthNameForMonth} {day}, 2026";
                        var data = items.FirstOrDefault(d => d.Category == dateKey);
                        prompt += $"{dateKey}: {data?.Revenue ?? 0}\n";
                    }
                    break;

                case "Week":
                    DateTime startOfWeek = new DateTime(2025, 1, 1);

                    prompt += "Weekly Revenue Data (Jan 1 - Jan 7, 2026):\n";
                    for (int i = 0; i < 7; i++)
                    {
                        DateTime date = startOfWeek.AddDays(i);
                        string dateLabel = date.ToString("MMM d, yyyy", CultureInfo.InvariantCulture);

                        var data = items.FirstOrDefault(d => d.Category == dateLabel);
                        prompt += $"{dateLabel} ({date.DayOfWeek}): {data?.Revenue ?? 0}\n";
                    }
                    break;
            }

            return prompt;
        }

        #endregion
    }
}