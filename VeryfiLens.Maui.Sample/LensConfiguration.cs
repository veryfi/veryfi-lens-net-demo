using System.Text.Json;

namespace VeryfiLens.Maui.Sample;

static class LensConfiguration
{
    public static (string ClientId, string Username, string ApiKey, string Url) Load()
    {
        var clientId = Environment.GetEnvironmentVariable("VERYFI_CLIENT_ID") ?? "";
        var username = Environment.GetEnvironmentVariable("VERYFI_USERNAME") ?? "";
        var apiKey = Environment.GetEnvironmentVariable("VERYFI_API_KEY") ?? "";
        var url = Environment.GetEnvironmentVariable("VERYFI_URL") ?? "";

        if (!string.IsNullOrEmpty(clientId))
        {
            return (clientId, username, apiKey, url);
        }

        try
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").GetAwaiter().GetResult();
            using var document = JsonDocument.Parse(stream);
            var root = document.RootElement.GetProperty("VeryfiLens");
            return (
                root.GetProperty("ClientId").GetString() ?? "",
                root.GetProperty("Username").GetString() ?? "",
                root.GetProperty("ApiKey").GetString() ?? "",
                root.GetProperty("Url").GetString() ?? "https://api.veryfi.com/");
        }
        catch
        {
            return ("", "", "", "https://api.veryfi.com/");
        }
    }
}
