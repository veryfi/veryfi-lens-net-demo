namespace VeryfiLensAndroidNetDemo.Analytics;

public interface IAnalyticsEventListener
{
    void OnAnalyticsEventReceived(string eventValue, string paramsValue, string value);

}