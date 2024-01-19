using Android.Content;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using Com.Veryfi.Lens;
using VeryfiLensAndroidNetDemo.interfaces;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace VeryfiLensAndroidNetDemo.fragments;

public class MenuFragment : Fragment
{
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
        if (btnReceipts != null)
        {
            btnReceipts.Click += FabOnClick;
        }

        var btnLongReceipts = view.FindViewById<ConstraintLayout>(Resource.Id.btn_long_receipts);
        if (btnLongReceipts != null)
        {
            btnLongReceipts.Click += FabOnClick;
        }

        var btnChecks = view.FindViewById<ConstraintLayout>(Resource.Id.btn_checks);
        if (btnChecks != null)
        {
            btnChecks.Click += FabOnClick;
        }

        var btnCreditCards = view.FindViewById<ConstraintLayout>(Resource.Id.btn_credit_cards);
        if (btnCreditCards != null)
        {
            btnCreditCards.Click += FabOnClick;
        }

        var btnBusinessCards = view.FindViewById<ConstraintLayout>(Resource.Id.btn_business_cards);
        if (btnBusinessCards != null)
        {
            btnBusinessCards.Click += FabOnClick;
        }

        var btnOcr = view.FindViewById<ConstraintLayout>(Resource.Id.btn_ocr);
        if (btnOcr != null)
        {
            btnOcr.Click += FabOnClick;
        }

        var btnW2 = view.FindViewById<ConstraintLayout>(Resource.Id.btn_w2);
        if (btnW2 != null)
        {
            btnW2.Click += FabOnClick;
        }

        var btnW9 = view.FindViewById<ConstraintLayout>(Resource.Id.btn_w9);
        if (btnW9 != null)
        {
            btnW9.Click += FabOnClick;
        }

        var btnBankStatements = view.FindViewById<ConstraintLayout>(Resource.Id.btn_bank_statements);
        if (btnBankStatements != null)
        {
            btnBankStatements.Click += FabOnClick;
        }

        var btnBarcode = view.FindViewById<ConstraintLayout>(Resource.Id.btn_barcode);
        if (btnBarcode != null)
        {
            btnBarcode.Click += FabOnClick;
        }
    }

    private void FabOnClick(object sender, EventArgs eventArgs)
    {
        fragmentCommunication.ResetSuccessHandled();
        VeryfiLens.ShowCamera();
    }

    public override void OnAttach(Context context)
    {
        base.OnAttach(context);
        if (context is IFragmentCommunication)
        {
            fragmentCommunication = (IFragmentCommunication)context;
        }
    }
}