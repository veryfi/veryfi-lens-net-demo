using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Org.Json;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace VeryfiLensAndroidNetDemo.fragments
{
    public class DataFragment : Fragment
    {
        private JSONObject dataToShow;

        public DataFragment()
        {
        }

        public void SetData(JSONObject data)
        {
            dataToShow = data;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_data, container, false);
            UpdateUI(view);
            return view;
        }

        private void UpdateUI(View view)
        {
            AssignData(view, Resource.Id.txt_updated_date, "updated_date");
            AssignNestedData(view, Resource.Id.txt_invoice_number, "invoice_number");
            AssignNestedData(view, Resource.Id.txt_currency_code, "currency_code");
            AssignNestedData(view, Resource.Id.txt_tax, "tax");
            AssignNestedData(view, Resource.Id.txt_category, "category");
            AssignData(view, Resource.Id.txt_img_file_name, "img_file_name");
            AssignData(view, Resource.Id.txt_reference_number, "reference_number");
            AssignData(view, Resource.Id.txt_created_date, "created_date");
            AssignData(view, Resource.Id.txt_id, "id");
            AssignData(view, Resource.Id.txt_img_url, "img_url");
            AssignNestedData(view, Resource.Id.txt_date, "date");
            AssignData(view, Resource.Id.txt_pdf_url, "pdf_url");
        }
        
        private void AssignNestedData(View view, int resourceId, string jsonKey)
        {
            if (dataToShow != null && dataToShow.Has("data"))
            {
                var jsonData = dataToShow.GetJSONObject("data");
                if (jsonData.Has(jsonKey))
                {
                    var keyData = jsonData.GetString(jsonKey);
                    var textView = view.FindViewById<TextView>(resourceId);
                    textView.Text = keyData;
                }
            }
        }

        private void AssignData(View view, int resourceId, string jsonKey)
        {
            if (dataToShow != null && dataToShow.Has("data"))
            {
                var jsonData = dataToShow.GetJSONObject("data");
                if (jsonData.Has(jsonKey))
                {
                    var keyData = jsonData.Get(jsonKey);
                    var textView = view.FindViewById<TextView>(resourceId);
                    textView.Text = keyData.ToString();
                }
            }
        }
    }
}