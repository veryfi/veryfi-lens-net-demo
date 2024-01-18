using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace VeryfiLensAndroidNetDemo.fragments
{
    public class DataFragment : Fragment
    {
        private string dataToShow;

        public DataFragment()
        {
            
        }

        public void SetData(string data)
        {
            dataToShow = data;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_data, container, false);

            var textView = view.FindViewById<TextView>(Resource.Id.textViewData);
            textView.Text = dataToShow;

            return view;
        }
    }
}