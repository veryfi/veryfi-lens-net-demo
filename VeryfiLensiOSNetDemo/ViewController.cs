using CoreFoundation;
using VeryfiLensiOS;
namespace iOSVeryfiTestApp;

public partial class ViewController : UIViewController
{
    const string CLIENT_ID = "XXXX"; //Replace with your clientId
    const string AUTH_USRNE = "XXXX"; //Replace with your username
    const string AUTH_API_K = "XXXX"; //Replace with your apiKey
    const string API_URL = "XXXX"; //Replace with your url

    VeryfiLensDelegateListener _veryfiLensDelegate;

    public ViewController() : base(nameof(ViewController), null)
    {
    }

    public ViewController(IntPtr handle) : base(handle)
    {
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        this.ShowLogs("Configuring Lens");
        SetUpVeryfiLens();
        SetUpVeryfiLensDelegate();
    }

    partial void OpenLens(NSObject sender)
    {
        VeryfiLens.Shared.ShowCameraIn(this);
    }

    private void SetUpVeryfiLensDelegate()
    {
        _veryfiLensDelegate = new VeryfiLensDelegateListener(this);
        VeryfiLens.Shared.Delegate = _veryfiLensDelegate;
    }

    private void SetUpVeryfiLens()
    {
        var categories = new string[]
        {
                "Meals",
                "Entertainment",
                "Supplies"
        };
        var documentTypes = new string[]
        {
                "long_receipt",
                "receipt",
                "bill",
                "other"
        };
        var tags = new string[]
        {
                "Tag 1",
                "Tag 2",
                "Tag 3"
        };
        VeryfiLensSettings veryfiLensSettings = new VeryfiLensSettings
        {
            ShowDocumentTypes = true,
            BlurDetectionIsOn = true,
            AutoLightDetectionIsOn = false,
            AutoCaptureIsOn = false,
            BackupDocsToGallery = true,
            AutoDocDetectionAndCropIsOn = true,
            AutoCropGalleryIsOn = false,
            EmailCCIsOn = false,
            ConfidenceDetailsIsOn = true,
            CloseCameraOnSubmit = true,
            RotateDocIsOn = true,
            ReturnStitchedPDF = true,
            StitchIsOn = true,
            MultipleDocumentsIsOn = true,
            LocationServicesIsOn = true,
            MoreMenuIsOn = true,
            DataExtractionEngine = DataExtractionEngine.VeryfiCloudAPI,
            Categories = categories,
            IsProduction = false,
            OriginalImageMaxSizeInMB = 2.5f,
            StitchedPDFPixelDensityMultiplier = 2.0f,
            SaveLogsIsOn = true,
            ShareLogsIsOn = true,
            DocumentTypes = documentTypes,
            AutoSubmitDocumentOnCapture = false,
            AutoRotateIsOn = true,
            ExternalId = "testExternalId1234",
            GalleryIsOn = true,
            Tags = tags,
            AllowSubmitUndetectedDocsIsOn = true
        };
        var veryfiLensCredentials = new VeryfiLensCredentials(CLIENT_ID, AUTH_USRNE, AUTH_API_K, API_URL);
        Action<bool> completion = (success) =>
        {
            if (success)
            {
                DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    this.ShowLogs("Lens configured");
                });
            }
        };

        VeryfiLens.Shared.ConfigureWith(veryfiLensCredentials, veryfiLensSettings, completion);
        SetCategories();
        SetCustomers();
        SetCostCodes();

    }

    private void SetCostCodes()
    {
        var costCodes = new VeryfiLensCostCode[10];
        for (int i = 0; i < 10; i++)
        {
            var job = new VeryfiLensCostCode
            {
                CostCodeId = i,
                Code = "00" + i,
                Name = "Cost " + i
            };
            costCodes[i] = job;
        }
        VeryfiLens.Shared.Settings.CostCodes = costCodes;
    }

    private void SetCustomers()
    {
        var customers = new VeryfiLensCPModel[10];
        for (int i = 0; i < 10; i++)
        {
            var customer = new VeryfiLensCPModel
            {
                ProjectId = i,
                CustomerId = i,
                CustomerName = "Customer " + i
            };
            customers[i] = customer;
        }
        VeryfiLens.Shared.Settings.Customers = customers;
    }

    private void SetCategories()
    {
        var categories = new VeryfiLensCategory[10];
        for (int i = 0; i < 10; i++)
        {
            VeryfiLensCategory category = new VeryfiLensCategory
            {
                CategoryId = i,
                Name = "Category Number " + i,
                Type = "mo_expense"
            };
            categories[i] = category;
        }
        VeryfiLens.Shared.Settings.CategoriesList = categories;
    }

    private void ShowLogs(string log)
    {
        var text = Logs.Text + log + "\n";
        Logs.Text = text;
    }

    private class VeryfiLensDelegateListener : VeryfiLensDelegate
    {
        private ViewController viewController;

        public VeryfiLensDelegateListener(ViewController viewController)
        {
            this.viewController = viewController;
        }

        public override void VeryfiLensClose(NSDictionary<NSString, NSObject> json)
        {
            viewController.ShowLogs(json.ToString());
        }

        public override void VeryfiLensError(NSDictionary<NSString, NSObject> json)
        {
            viewController.ShowLogs(json.ToString());
        }

        public override void VeryfiLensSuccess(NSDictionary<NSString, NSObject> json)
        {
            viewController.ShowLogs(json.ToString());
        }

        public override void VeryfiLensUpdate(NSDictionary<NSString, NSObject> json)
        {
            viewController.ShowLogs(json.ToString());
        }
    }
}

