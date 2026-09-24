namespace VeryfiLensiOSNetDemo;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        return true;
    }

    public override UISceneConfiguration GetConfiguration(
        UIApplication application,
        UISceneSession connectingSceneSession,
        UISceneConnectionOptions options)
    {
        return new UISceneConfiguration(
            "Default Configuration",
            connectingSceneSession.Role)
        {
            DelegateType = typeof(SceneDelegate),
            Storyboard = UIStoryboard.FromName("Main", null)
        };
    }
}
