using Android.Views;
using AndroidX.AppCompat.App;
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

            TabLayoutMediator.ITabConfigurationStrategy tabConfigurationStrategy = new TabConfigurationStrategyImpl();
            new TabLayoutMediator(tabs, viewPager, tabConfigurationStrategy).Attach();

            if (Activity is AppCompatActivity activity)
            {
               
                activity.SupportActionBar?.Show();
                activity.SupportActionBar?.SetDisplayHomeAsUpEnabled(true);
                activity.SupportActionBar?.SetTitle(Resource.String.results_fragment_title);
            }


            return view;
        }


        private class TabConfigurationStrategyImpl : Java.Lang.Object, TabLayoutMediator.ITabConfigurationStrategy
        {
            public void OnConfigureTab(TabLayout.Tab tab, int position)
            {
                tab.SetText(position == 0 ? "Extracted data" : "Json");
            }
        }
    }
}