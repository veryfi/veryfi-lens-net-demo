using Android.OS;
using Android.Views;
using Android.Widget;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace VeryfiLensAndroidNetDemo.fragments
{
    public class LoadingFragment : Fragment
    {
        public override View? OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_loading, container, false);
        }
    }
}