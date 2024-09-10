namespace VeryfiLensAndroidNetDemo.settings;

public class SettingsManager
{
    private static SettingsManager _instance;

    public static SettingsManager Instance => _instance ?? (_instance = new SettingsManager());

    public bool switchCameraIsOn { get; set; } = false;
    public bool stitchIsOn { get; set; } = true;
    public bool MultipleDocumentsIsOn { get; set; } = true;
    public bool ZoomIsOn { get; set; } = true;

    private SettingsManager()
    {
    }
}
