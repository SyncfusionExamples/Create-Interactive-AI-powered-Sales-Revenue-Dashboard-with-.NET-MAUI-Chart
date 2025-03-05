using System.Text.Json;

namespace SalesPerformanceAnalysis
{
    public class SettingsService
    {
        private readonly string _settingsKey = "AppSettings";

        public string AzureOpenAIEndpoint { get; private set; } = "https://mobilemaui.openai.azure.com/";
        public string AzureOpenAIKey { get; private set; } = "6673b6975f334c79bd0db8a1cd70aa49";
        public string AzureOpenAIDeploymentName { get; private set; } = "gpt-4o";

        public SettingsService()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (Preferences.ContainsKey(_settingsKey))
            {
                var json = Preferences.Get(_settingsKey, string.Empty);
                var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                if (settings != null)
                {
                    if (settings.TryGetValue(nameof(AzureOpenAIEndpoint), out var endpoint))
                        AzureOpenAIEndpoint = endpoint;

                    if (settings.TryGetValue(nameof(AzureOpenAIKey), out var key))
                        AzureOpenAIKey = key;

                    if (settings.TryGetValue(nameof(AzureOpenAIDeploymentName), out var deploymentName))
                        AzureOpenAIDeploymentName = deploymentName;
                }
            }
        }

        public void SaveSettings()
        {
            var settings = new Dictionary<string, string>
        {
            { nameof(AzureOpenAIEndpoint), AzureOpenAIEndpoint },
            { nameof(AzureOpenAIKey), AzureOpenAIKey },
            { nameof(AzureOpenAIDeploymentName), AzureOpenAIDeploymentName }
        };

            var json = JsonSerializer.Serialize(settings);
            Preferences.Set(_settingsKey, json);
        }

        public void UpdateOpenAISettings(string endpoint, string key, string deploymentName)
        {
            AzureOpenAIEndpoint = endpoint;
            AzureOpenAIKey = key;
            AzureOpenAIDeploymentName = deploymentName;
            SaveSettings();
        }
    }


}
