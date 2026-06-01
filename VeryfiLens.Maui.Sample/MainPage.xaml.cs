using VeryfiLens;

namespace VeryfiLens.Maui.Sample;

public partial class MainPage : ContentPage
{
    bool _configured;
    bool _setupStarted;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_setupStarted)
        {
            return;
        }

        _setupStarted = true;
        SetupVeryfiLens();
    }

    void SetupVeryfiLens()
    {
        AppendLog("Configuring Lens…");

        var (clientId, username, apiKey, url) = LensConfiguration.Load();
        if (string.IsNullOrWhiteSpace(clientId))
        {
            AppendLog("Missing credentials. Set VERYFI_* env vars or edit Resources/Raw/appsettings.json.");
            return;
        }

        VeryfiLensBinding.SetDelegate(new SampleDelegate(AppendLog));
        VeryfiLensBinding.SetAnalyticsDelegate(new SampleAnalyticsDelegate(AppendLog));

        var credentials = new VeryfiLensCredentials
        {
            ClientId = clientId,
            Username = username,
            ApiKey = apiKey,
            Url = url
        };

        var settings = new Dictionary<string, object>
        {
            ["blurDetectionIsOn"] = true,
            ["autoLightDetectionIsOn"] = false,
            ["documentTypes"] = new[] { "receipt" },
            ["defaultSelectedDocumentType"] = "receipt",
            ["showDocumentTypes"] = true,
            ["dataExtractionEngine"] = "api",
            ["moreMenuIsOn"] = true,
            ["autoRotateIsOn"] = true,
            ["galleryIsOn"] = true,
            ["autoSubmitDocumentOnCapture"] = false,
        };

        VeryfiLensBinding.Configure(credentials, settings, success =>
        {
            _configured = success;
            OpenCameraBtn.IsEnabled = success;
            OpenGalleryBtn.IsEnabled = success;
            OpenBrowserBtn.IsEnabled = success;
            AppendLog(success ? "Lens configured." : "Lens configuration failed.");

            if (!success)
            {
                return;
            }

            try
            {
                VeryfiLensBinding.ObserveAnalyticsEvents();
                AppendLog("Analytics events observed.");
            }
            catch (Exception ex)
            {
                AppendLog($"ObserveAnalyticsEvents failed: {ex.Message}. Analytics events will not be observed.");
            }
        });
    }   

    void OnOpenCameraClicked(object? sender, EventArgs e) => OpenLens(VeryfiLensBinding.ShowCamera);

    void OnOpenGalleryClicked(object? sender, EventArgs e) => OpenLens(VeryfiLensBinding.ShowGallery);

    void OnOpenDocumentBrowserClicked(object? sender, EventArgs e) => OpenLens(VeryfiLensBinding.ShowDocumentBrowser);

    void OpenLens(Action<Page> open)
    {
        if (!_configured)
        {
            AppendLog("Lens is not configured yet.");
            return;
        }

        AppendLog("Opening Lens flow…");
        open(this);
    }

    void AppendLog(string message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LogLabel.Text = $"{DateTime.Now:HH:mm:ss} {message}\n{LogLabel.Text}";
        });
    }

    sealed class SampleDelegate(Action<string> log) : IVeryfiLensDelegate
    {
        public void OnClose(Dictionary<string, object> payload) => log($"[close] {Summarize(payload)}");
        public void OnError(Dictionary<string, object> payload) => log($"[error] {Summarize(payload)}");
        public void OnSuccess(Dictionary<string, object> payload) => log($"[success] {Summarize(payload)}");
        public void OnUpdate(Dictionary<string, object> payload) => log($"[update] {Summarize(payload)}");

        static string Summarize(Dictionary<string, object> payload) =>
            $"keys=[{string.Join(", ", payload.Keys)}] count={payload.Count}";
    }

    sealed class SampleAnalyticsDelegate(Action<string> log) : IVeryfiLensAnalyticsDelegate
    {
        public void OnAnalyticsEvent(string eventName, string paramsJson, string value) =>
            log($"[analytics] {eventName} params={paramsJson} value={value}");
    }
}
