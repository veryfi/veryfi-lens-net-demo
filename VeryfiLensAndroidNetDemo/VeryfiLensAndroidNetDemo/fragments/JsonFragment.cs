using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Text;
using AndroidX.Fragment.App;
using Org.Json;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace VeryfiLensAndroidNetDemo.fragments
{
    public class JsonFragment : Fragment
    {
        private JSONObject jsonToShow;
        private CustomJsonFormatter jsonFormatter;

        public JsonFragment()
        {
           
        }

        public void SetJson(JSONObject json)
        {
            jsonToShow = json;
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            jsonFormatter = new CustomJsonFormatter(context);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_json, container, false);
            var textView = view.FindViewById<TextView>(Resource.Id.textViewJson);

            if (jsonToShow != null)
            {
                SpannableString formattedJson = jsonFormatter.FormatJsonWithColors(jsonToShow.ToString());
                textView.SetText(formattedJson, TextView.BufferType.Spannable);
            }

            return view;
        }
    }
}