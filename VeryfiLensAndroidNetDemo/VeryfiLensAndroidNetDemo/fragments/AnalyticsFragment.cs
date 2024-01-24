using Android.Views;
using Android.Content;
using AndroidX.RecyclerView.Widget;
using VeryfiLensAndroidNetDemo.adapters;
using VeryfiLensAndroidNetDemo.Analytics;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace VeryfiLensAndroidNetDemo.fragments
{
    public class AnalyticsFragment : Fragment
    {
        private RecyclerView recyclerView;
        private AnalyticsAdapter adapter;
        private List<(string EventValue, string ParamsValue, string Value)> analyticsEvents = new List<(string, string, string)>();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_analytics, container, false);
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewAnalytics);
            
            if (Activity is MainActivity mainActivity)
            {
                analyticsEvents = mainActivity.GetAnalyticsEvents();
                adapter = new AnalyticsAdapter(analyticsEvents);
                recyclerView.SetAdapter(adapter);
                recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            }
            
            
            return view;
        }
    }

}