using Android.OS;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomSheet;
using AndroidX.Fragment.App;
using VeryfiLensAndroidNetDemo.settings; 

namespace VeryfiLensAndroidNetDemo.fragments;

public class SettingsBottomSheetFragment : BottomSheetDialogFragment
{
    private Switch switchCamera;
    private Switch switchStitch;
    private Switch switchMultipleDocuments;
    private Switch switchZoom;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var view = inflater.Inflate(Resource.Layout.bottom_sheet_settings, container, false);

        InitializeUI(view);
        SetInitialSwitchStates();
        SetupSwitchEventHandlers();

        return view;
    }

    private void InitializeUI(View view)
    {
        switchStitch = view.FindViewById<Switch>(Resource.Id.switch_stitch);
        switchMultipleDocuments = view.FindViewById<Switch>(Resource.Id.switch_multiple_documents);
        switchZoom = view.FindViewById<Switch>(Resource.Id.switch_zoom);
        switchCamera = view.FindViewById<Switch>(Resource.Id.switch_camera);
        
    }

    private void SetInitialSwitchStates()
    {
        var settings = SettingsManager.Instance;
        switchCamera.Checked = settings.switchCameraIsOn;
        switchStitch.Checked = settings.stitchIsOn;
        switchMultipleDocuments.Checked = settings.MultipleDocumentsIsOn;
        switchZoom.Checked = settings.ZoomIsOn;
    }

    private void SetupSwitchEventHandlers()
    {
        switchCamera.CheckedChange += (sender, e) =>
        {
            SettingsManager.Instance.switchCameraIsOn = e.IsChecked;
        };

        switchStitch.CheckedChange += (sender, e) =>
        {
            SettingsManager.Instance.stitchIsOn = e.IsChecked;
        };

        switchMultipleDocuments.CheckedChange += (sender, e) =>
        {
            SettingsManager.Instance.MultipleDocumentsIsOn = e.IsChecked;
        };

        switchZoom.CheckedChange += (sender, e) =>
        {
            SettingsManager.Instance.ZoomIsOn = e.IsChecked;
        };
    }
}
