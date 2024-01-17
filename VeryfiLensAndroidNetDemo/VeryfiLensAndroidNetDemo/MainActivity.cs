using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using Com.Veryfi.Lens;
using Com.Veryfi.Lens.Helpers;
using Org.Json;

namespace VeryfiLensAndroidNetDemo;

[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
public class MainActivity : AppCompatActivity
{
    const string CLIENT_ID = "YOUR_CLIENT_ID";
    const string AUTH_USRNE = "YOUR_USERNAME";
    const string AUTH_API_K = "YOUR_API_KEY";
    const string API_URL = "YOUR_URL";

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_main);

        SetUpVeryfiLens();
        SetUpVeryfiLensDelegate();

        ListenerEvents();

    }

    private void ListenerEvents()
    {
        var btnReceipts = FindViewById<ConstraintLayout>(Resource.Id.btn_receipts);
        if (btnReceipts != null)
        {
            btnReceipts.Click += FabOnClick;
        }

        var btnLongReceipts = FindViewById<ConstraintLayout>(Resource.Id.btn_long_receipts);
        if (btnLongReceipts != null)
        {
            btnLongReceipts.Click += FabOnClick;
        }

        var btnChecks = FindViewById<ConstraintLayout>(Resource.Id.btn_checks);
        if (btnChecks != null)
        {
            btnChecks.Click += FabOnClick;
        }

        var btnCreditCards = FindViewById<ConstraintLayout>(Resource.Id.btn_credit_cards);
        if (btnCreditCards != null)
        {
            btnCreditCards.Click += FabOnClick;
        }

        var btnBusinessCards = FindViewById<ConstraintLayout>(Resource.Id.btn_business_cards);
        if (btnBusinessCards != null)
        {
            btnBusinessCards.Click += FabOnClick;
        }

        var btnOcr = FindViewById<ConstraintLayout>(Resource.Id.btn_ocr);
        if (btnOcr != null)
        {
            btnOcr.Click += FabOnClick;
        }

        var btnW2 = FindViewById<ConstraintLayout>(Resource.Id.btn_w2);
        if (btnW2 != null)
        {
            btnW2.Click += FabOnClick;
        }

        var btnW9 = FindViewById<ConstraintLayout>(Resource.Id.btn_w9);
        if (btnW9 != null)
        {
            btnW9.Click += FabOnClick;
        }

        var btnBankStatements = FindViewById<ConstraintLayout>(Resource.Id.btn_bank_statements);
        if (btnBankStatements != null)
        {
            btnBankStatements.Click += FabOnClick;
        }

        var btnBarcode = FindViewById<ConstraintLayout>(Resource.Id.btn_barcode);
        if (btnBarcode != null)
        {
            btnBarcode.Click += FabOnClick;
        }
    }
    
    private void FabOnClick(object sender, EventArgs eventArgs)
    {
        VeryfiLens.ShowCamera();
    }

    private void SetUpVeryfiLensDelegate()
    {
        VeryfiLens.SetDelegate(new VeryfiLensDelegateListener(this));
    }

    private void SetUpVeryfiLens()
    {
        var categories = new List<string>
            {
                "Meals",
                "Entertainment",
                "Supplies"
            };
        var documentTypes = new List<DocumentType>
            {
                DocumentType.Receipt
            };
        VeryfiLensSettings veryfiLensSettings = new VeryfiLensSettings
        {
            ShowDocumentTypes = true,
            BlurDetectionIsOn = true,
            AutoLightDetectionIsOn = false,
            ConfidenceDetailsIsOn = true,
            AutoCaptureIsOn = false,
            BackupDocsToGallery = true,
            AutoDocDetectionAndCropIsOn = true,
            AutoCropGalleryIsOn = false,
            EmailCCIsOn = false,
            CloseCameraOnSubmit = true,
            RotateDocIsOn = true,
            StitchIsOn = true,
            MultipleDocumentsIsOn = true,
            LocationServicesIsOn = true,
            MoreMenuIsOn = true,
            FloatMenu = true,
            Categories = categories,
            PrimaryColor = "#53BF8A",
            AccentColor = "#8B229D",
            Production = false,
            OriginalImageMaxSizeInMB = new Java.Lang.Float(2.5),
            StitchedPDFPixelDensityMultiplier = new Java.Lang.Float(2.0),
            SaveLogsIsOn = true,
            ShareLogsIsOn = true,
            GpuIsOn = true,
            DataExtractionEngine = VeryfiLensSettings.ExtractionEngine.VeryfiCloudAPI,
            DocumentTypes = documentTypes,
            AutoSubmitDocumentOnCapture = false,
            AutoRotateIsOn = false,
            ExternalId = "testExternalId1234",
            BrandImage = new Java.Lang.Integer(Resource.Drawable.ic_veryfi_lens_logo),
            GalleryIsOn = false,
            AllowSubmitUndetectedDocsIsOn = true
        };
        var veryfiLensCredentials = new VeryfiLensCredentials
        {
            ApiKey = AUTH_API_K,
            Username = AUTH_USRNE,
            ClientId = CLIENT_ID,
            Url = API_URL
        };
        VeryfiLens.Configure(Application, veryfiLensCredentials, veryfiLensSettings);
    }

    public override bool OnCreateOptionsMenu(IMenu menu)
    {
        MenuInflater.Inflate(Resource.Menu.menu_main, menu);
        return true;
    }

    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        int id = item.ItemId;
        if (id == Resource.Id.action_settings)
        {
            return true;
        }

        return base.OnOptionsItemSelected(item);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    private void ShowLogs(string log)
    {
        var json = new JSONObject(log);
    }

    private class VeryfiLensDelegateListener : Java.Lang.Object, IVeryfiLensDelegate
    {
        private readonly MainActivity mainActivity;

        public VeryfiLensDelegateListener(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public void VeryfiLensClose(JSONObject json)
        {
            mainActivity.ShowLogs(json.ToString());
        }

        public void VeryfiLensError(JSONObject json)
        {
            mainActivity.ShowLogs(json.ToString());
        }

        public void VeryfiLensSuccess(JSONObject json)
        {
            mainActivity.ShowLogs(json.ToString());
        }

        public void VeryfiLensUpdate(JSONObject json)
        {
            mainActivity.ShowLogs(json.ToString());
        }
    }
}