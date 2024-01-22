using Android.Content;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using Com.Veryfi.Lens;
using Com.Veryfi.Lens.Helpers;
using VeryfiLensAndroidNetDemo.interfaces;
using VeryfiLensAndroidNetDemo.settings;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace VeryfiLensAndroidNetDemo.fragments;

public class MenuFragment : Fragment
{
    private String clientId;
    private String userName;
    private String authApi;
    private String url;
    private DocumentType documentType;


    public MenuFragment(String clientId, String userName, String authApi, String url)
    {
        this.clientId = clientId;
        this.userName = userName;
        this.authApi = authApi;
        this.url = url;
    }


    private IFragmentCommunication fragmentCommunication;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var view = inflater.Inflate(Resource.Layout.fragment_menu, container, false);


        if (Activity is AppCompatActivity activity)
        {
            activity.SupportActionBar?.Hide();
        }

        ListenerEvents(view);
        return view;
    }

    private void ListenerEvents(View view)
    {
        var btnReceipts = view.FindViewById<ConstraintLayout>(Resource.Id.btn_receipts);
        var btnReceiptsSettings = view.FindViewById<ImageView>(Resource.Id.btn_receipts_settings);
        if (btnReceipts != null)
        {
            documentType = DocumentType.Receipt;
            btnReceipts.Click += ReceiptsOnClick;
        }

        if (btnReceiptsSettings != null)
        {
            btnReceiptsSettings.Click += ShowSettings;
        }

        var btnLongReceipts = view.FindViewById<ConstraintLayout>(Resource.Id.btn_long_receipts);
        var btnLongReceiptsSettings = view.FindViewById<ImageView>(Resource.Id.btn_long_receipts_settings);
        if (btnLongReceipts != null)
        {
            documentType = DocumentType.LongReceipt;
            btnLongReceipts.Click += LongReceiptsOnClick;
        }

        if (btnLongReceiptsSettings != null)
        {
            btnLongReceiptsSettings.Click += ShowSettings;
        }

        var btnChecks = view.FindViewById<ConstraintLayout>(Resource.Id.btn_checks);
        var btnChecksSettings = view.FindViewById<ImageView>(Resource.Id.btn_checks_settings);
        if (btnChecks != null)
        {
            documentType = DocumentType.Check;
            btnChecks.Click += ChecksOnClick;
        }

        if (btnChecksSettings != null)
        {
            btnChecksSettings.Click += ShowSettings;
        }


        var btnCreditCards = view.FindViewById<ConstraintLayout>(Resource.Id.btn_credit_cards);
        var btnCardsSettings = view.FindViewById<ImageView>(Resource.Id.btn_credit_cards_settings);
        if (btnCreditCards != null)
        {
            documentType = DocumentType.CreditCard;
            btnCreditCards.Click += CreditCardsOnClick;
        }

        if (btnCardsSettings != null)
        {
            btnCardsSettings.Click += ShowSettings;
        }

        var btnBusinessCards = view.FindViewById<ConstraintLayout>(Resource.Id.btn_business_cards);
        var btnBusinessCardsSettings = view.FindViewById<ImageView>(Resource.Id.btn_business_cards_settings);
        if (btnBusinessCards != null)
        {
            documentType = DocumentType.BusinessCard;
            btnBusinessCards.Click += BusinessCardsOnClick;
        }

        if (btnBusinessCardsSettings != null)
        {
            btnBusinessCardsSettings.Click += ShowSettings;
        }

        var btnOcr = view.FindViewById<ConstraintLayout>(Resource.Id.btn_ocr);
        var btnOcrSettings = view.FindViewById<ImageView>(Resource.Id.btn_ocr_settings);
        if (btnOcr != null)
        {
            documentType = DocumentType.Other;
            btnOcr.Click += OcrOnClick;
        }

        if (btnOcrSettings != null)
        {
            btnOcrSettings.Click += ShowSettings;
        }

        var btnW2 = view.FindViewById<ConstraintLayout>(Resource.Id.btn_w2);
        var btnW2Settings = view.FindViewById<ImageView>(Resource.Id.btn_w2_settings);
        if (btnW2 != null)
        {
            documentType = DocumentType.W2;
            btnW2.Click += W2OnClick;
        }

        if (btnW2Settings != null)
        {
            btnW2Settings.Click += ShowSettings;
        }

        var btnW9 = view.FindViewById<ConstraintLayout>(Resource.Id.btn_w9);
        var btnW9Settings = view.FindViewById<ImageView>(Resource.Id.btn_w9_settings);
        if (btnW9 != null)
        {
            documentType = DocumentType.W9;
            btnW9.Click += W9OnClick;
        }

        if (btnW9Settings != null)
        {
            btnW9Settings.Click += ShowSettings;
        }

        var btnBankStatements = view.FindViewById<ConstraintLayout>(Resource.Id.btn_bank_statements);
        var btnBankStatementsSettings = view.FindViewById<ImageView>(Resource.Id.btn_bank_statements_settings);
        if (btnBankStatements != null)
        {
            documentType = DocumentType.BankStatements;
            btnBankStatements.Click += BankStatementsOnClick;
        }

        if (btnBankStatementsSettings != null)
        {
            btnBankStatementsSettings.Click += ShowSettings;
        }

        var btnBarcode = view.FindViewById<ConstraintLayout>(Resource.Id.btn_barcode);
        var btnBarCodeSettings = view.FindViewById<ImageView>(Resource.Id.btn_barcode_settings);
        if (btnBarcode != null)
        {
            btnBarcode.Click += BarcodeOnClick;
        }

        if (btnBarCodeSettings != null)
        {
            btnBarCodeSettings.Click += ShowSettings;
        }
    }

    private void FabOnClick(DocumentType documentType, object sender, EventArgs eventArgs)
    {
        this.documentType = documentType;
        fragmentCommunication.ResetSuccessHandled();
        SetUpVeryfiLens();
        VeryfiLens.ShowCamera();
    }

    private void ShowSettings(object sender, EventArgs e)
    {
        var settingsBottomSheet = new SettingsBottomSheetFragment();
        settingsBottomSheet.Show(FragmentManager, settingsBottomSheet.Tag);
    }


    public override void OnAttach(Context context)
    {
        base.OnAttach(context);
        if (context is IFragmentCommunication)
        {
            fragmentCommunication = (IFragmentCommunication)context;
        }
    }

    private void SetUpVeryfiLens()
    {
        var settings = SettingsManager.Instance;

        var documentType = new List<DocumentType>
        {
            this.documentType
        };

        VeryfiLensSettings veryfiLensSettings = new VeryfiLensSettings
        {
            SwitchCameraIsOn = settings.switchCameraIsOn,
            StitchIsOn = settings.stitchIsOn,
            MultipleDocumentsIsOn = settings.MultipleDocumentsIsOn,
            ZoomIsOn = settings.ZoomIsOn,
            DocumentTypes = documentType
        };

        var veryfiLensCredentials = new VeryfiLensCredentials
        {
            ApiKey = authApi,
            Username = userName,
            ClientId = clientId,
            Url = url
        };

        if (Activity != null && Activity.Application != null)
        {
            VeryfiLens.Configure(Activity.Application, veryfiLensCredentials, veryfiLensSettings);
        }
    }

    private void ReceiptsOnClick(object sender, EventArgs e) => FabOnClick(DocumentType.Receipt, sender, e);
    private void LongReceiptsOnClick(object sender, EventArgs e) => FabOnClick(DocumentType.LongReceipt, sender, e);
    private void ChecksOnClick(object sender, EventArgs e) => FabOnClick(DocumentType.Check, sender, e);
    private void CreditCardsOnClick(object sender, EventArgs e) => FabOnClick(DocumentType.CreditCard, sender, e);
    private void BusinessCardsOnClick(object sender, EventArgs e) => FabOnClick(DocumentType.BusinessCard, sender, e);
    private void OcrOnClick(object sender, EventArgs e) => FabOnClick(DocumentType.Other, sender, e);
    private void W2OnClick(object sender, EventArgs e) => FabOnClick(DocumentType.W2, sender, e);
    private void W9OnClick(object sender, EventArgs e) => FabOnClick(DocumentType.W9, sender, e);

    private void BankStatementsOnClick(object sender, EventArgs e) =>
        FabOnClick(DocumentType.BankStatements, sender, e);

    private void BarcodeOnClick(object sender, EventArgs e) => FabOnClick(DocumentType.Barcodes, sender, e);
}