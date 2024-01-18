using Android.Views;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Tabs;
using Org.Json;
using VeryfiLensAndroidNetDemo.adapters;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace VeryfiLensAndroidNetDemo.fragments
{
    public class ResultsFragments : Fragment
    {
        private JSONObject json;

        public void SetJson(JSONObject json)
        {
            this.json = json;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_results, container, false);

            var viewPager = view.FindViewById<ViewPager2>(Resource.Id.view_pager);
            var tabs = view.FindViewById<TabLayout>(Resource.Id.tabs);

            var adapter = new TabsAdapter(this, json);
            viewPager.Adapter = adapter;

            tabs.AddTab(tabs.NewTab().SetText("Extracted data"));
            tabs.AddTab(tabs.NewTab().SetText("Json"));
            return view;
        }
    }
}