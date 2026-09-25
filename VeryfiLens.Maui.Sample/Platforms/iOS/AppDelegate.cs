using Foundation;
using UIKit;

namespace VeryfiLens.Maui.Sample;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override UISceneConfiguration GetConfiguration(
		UIApplication application,
		UISceneSession connectingSceneSession,
		UISceneConnectionOptions options)
	{
		return new UISceneConfiguration(
			"__MAUI_DEFAULT_SCENE_CONFIGURATION__",
			connectingSceneSession.Role)
		{
			DelegateType = typeof(SceneDelegate)
		};
	}
}
