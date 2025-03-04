

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


        public string GeneratePrompt(List<SalesData> items, string selectedCategory)
        {
            StringBuilder prompt = new StringBuilder();
            prompt.AppendLine($"📊 **Sales Insights Based on {selectedCategory} Data for 2025**:");

            switch (selectedCategory)
            {
                case "Year":
                    prompt.AppendLine("\n📅 **Yearly Revenue1 Data (Jan - Dec, 2025):**");
                    for (int month = 1; month <= 12; month++)
                    {
                        string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
                        var data = items.FirstOrDefault(d => d.Category == monthName);
                        prompt.AppendLine($"{monthName} 2025: ${data?.Revenue ?? 0}");
                    }
                    break;

                case "Month":
                    prompt.AppendLine("\n **Daily Revenue1 Data for January 2025:**");
                    for (int day = 1; day <= 31; day++)
                    {
                        var data = items.FirstOrDefault(d => d.Category == $"Jan {day}");
                        prompt.AppendLine($"Jan {day}, 2025: ${data?.Revenue ?? 0}");
                    }
                    break;

                case "Week":
                    prompt.AppendLine("\n **Weekly Revenue1 Data (Jan 1 - 7, 2025):**");
                    string[] weekDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                    for (int day = 1; day <= 7; day++)
                    {
                        var data = items.FirstOrDefault(d => d.Category == $"Jan {day}");
                        prompt.AppendLine($"{weekDays[day - 1]} (Jan {day}, 2025): ${data?.Revenue ?? 0}");
                    }
                    break;

                default:
                    prompt.AppendLine("\n⚠️ Invalid selection. Please choose 'Year', 'Month', or 'Week'.");
                    break;
            }

            return prompt.ToString();
        }
        

        #endregion
    }
}