using Android.Content;
using Android.Util;

namespace VeryfiLensAndroidNetDemo.Analytics;

[BroadcastReceiver(Enabled = true, Exported = false)]
[IntentFilter(new[] { "com.veryfi.lens.VeryfiLensAnalyticsEvent" })]
public class AnalyticsEventReceiver : BroadcastReceiver
{
    private static IAnalyticsEventListener listener;

    public AnalyticsEventReceiver() { }
    
    public static void RegisterListener(IAnalyticsEventListener eventListener)
    {
        listener = eventListener;
    }

    public static void UnregisterListener()
    {
        listener = null;
    }

    public override void OnReceive(Context context, Intent intent)
    {
        string eventValue = intent.GetStringExtra("event");
        string paramsValue = intent.GetStringExtra("params");
        string value = intent.GetStringExtra("value");

        Log.Debug("Events", eventValue + paramsValue + value);
        
        listener?.OnAnalyticsEventReceived(eventValue, paramsValue, value);
    }
}