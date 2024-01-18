using AndroidX.Fragment.App;
using AndroidX.ViewPager2.Adapter;
using VeryfiLensAndroidNetDemo.fragments;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace VeryfiLensAndroidNetDemo.adapters
{
    public class TabsAdapter : FragmentStateAdapter
    {
        private string json;

        public TabsAdapter(Fragment fragment, string json) : base(fragment)
        {
            this.json = json;
        }

        public override int ItemCount => 2;

        public override Fragment CreateFragment(int position)
        {
            switch (position)
            {
                case 0:
                    var dataFragment = new DataFragment();
                    dataFragment.SetData(json);
                    return dataFragment;
                case 1:
                    var jsonFragment = new JsonFragment();
                    jsonFragment.SetJson(json);
                    return jsonFragment;
                default:
                    return new DataFragment();
            }
        }
    }
}