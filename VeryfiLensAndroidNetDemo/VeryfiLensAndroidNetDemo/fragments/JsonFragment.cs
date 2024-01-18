using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Org.Json;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace VeryfiLensAndroidNetDemo.fragments
{
    public class JsonFragment : Fragment
    {
        private JSONObject jsonToShow;

        public JsonFragment()
        {
            
        }

        public void SetJson(JSONObject json)
        {
            jsonToShow = json;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            var view = inflater.Inflate(Resource.Layout.fragment_json, container, false);

            var textView = view.FindViewById<TextView>(Resource.Id.textViewJson);

            return view;
        }
    }
}