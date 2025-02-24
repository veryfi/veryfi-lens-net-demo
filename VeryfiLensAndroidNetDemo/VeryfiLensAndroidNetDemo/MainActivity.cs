﻿using Android.Content;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using Com.Veryfi.Lens;
using Com.Veryfi.Lens.Helpers;
using Org.Json;
using VeryfiLensAndroidNetDemo.Analytics;
using VeryfiLensAndroidNetDemo.fragments;
using VeryfiLensAndroidNetDemo.interfaces;

namespace VeryfiLensAndroidNetDemo;

[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
public class MainActivity : AppCompatActivity, IFragmentCommunication, IAnalyticsEventListener
{
    const string? CLIENT_ID = "YOUR_CLIENT_ID";
    const string? AUTH_USRNE = "YOUR_USERNAME";
    const string? AUTH_API_K = "YOUR_API_KEY";
    const string? API_URL = "YOUR_URL";
    private bool isSuccessHandled = false;
    
    private AnalyticsEventReceiver receiver;
    private List<(string EventValue, string ParamsValue, string Value)> analyticsEvents = new List<(string, string, string)>();


    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_main);

        var toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.topAppBar);
        SetSupportActionBar(toolbar);

        SetUpVeryfiLens();
        SetUpVeryfiLensDelegate();

        var transaction = SupportFragmentManager.BeginTransaction();
        transaction.Replace(Resource.Id.fragment_container,
            new MenuFragment(CLIENT_ID, AUTH_USRNE, AUTH_API_K, API_URL));
        transaction.CommitAllowingStateLoss();
    }
    
    protected override void OnResume()
    {
        base.OnResume();
        receiver = new AnalyticsEventReceiver();
        AnalyticsEventReceiver.RegisterListener(this);
        IntentFilter filter = new IntentFilter("com.veryfi.lens.VeryfiLensAnalyticsEvent");
        RegisterReceiver(receiver, filter, ReceiverFlags.Exported);
    }
    
    protected override void OnPause()
    {
        base.OnPause();
        UnregisterReceiver(receiver);
        AnalyticsEventReceiver.UnregisterListener();
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
            DocumentType.Receipt!
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
            SaveLogsIsOn = true,
            ShareLogsIsOn = true,
            GpuIsOn = true,
            DataExtractionEngine = VeryfiLensSettings.ExtractionEngine.VeryfiCloudAPI,
            DocumentTypes = documentTypes,
            AutoSubmitDocumentOnCapture = false,
            AutoRotateIsOn = false,
            ExternalId = "testExternalId1234",
            GalleryIsOn = false,
            AllowSubmitUndetectedDocsIsOn = true,
            BarcodeExtractionIsOn = true,
            AutoCaptureFrameConfidence = 20,
            AutoCaptureMarginRatio = 0.08
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
    

    public override bool OnCreateOptionsMenu(IMenu? menu)
    {
        return true;
    }

    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        if (item.ItemId == Android.Resource.Id.Home)
        {
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.fragment_container,
                new MenuFragment(CLIENT_ID, AUTH_USRNE, AUTH_API_K, API_URL));
            transaction.CommitAllowingStateLoss();
            return true;
        }

        return base.OnOptionsItemSelected(item);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
        [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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
            mainActivity.isSuccessHandled = false;
        }

        public void VeryfiLensError(JSONObject json)
        {
            mainActivity.isSuccessHandled = false;
        }

        public void VeryfiLensSuccess(JSONObject json)
        {
            mainActivity.isSuccessHandled = true;
            mainActivity.RunOnUiThread(() =>
            {
                var resultsFragment = new ResultsFragments();
                resultsFragment.SetJson(json);

                var transaction = mainActivity.SupportFragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.fragment_container, resultsFragment);
                transaction.CommitAllowingStateLoss();
            });
        }
        
        public void VeryfiLensUpdate(JSONObject json)
        {
            if (!mainActivity.isSuccessHandled)
            {
                mainActivity.ShowLoadingFragment();
            }
        }
    }

    private void ShowLoadingFragment()
    {
        if (IsActivityInForeground())
        {
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.fragment_container, new LoadingFragment());
            transaction.CommitAllowingStateLoss();
        }
    }

    private bool IsActivityInForeground()
    {
        return !IsFinishing && !IsDestroyed;
    }

    public void ResetSuccessHandled()
    {
        isSuccessHandled = false;
    }

    public void OnAnalyticsEventReceived(string eventValue, string paramsValue, string value)
    {
        analyticsEvents.Add((eventValue, paramsValue, value));
    }
    
    public List<(string EventValue, string ParamsValue, string Value)> GetAnalyticsEvents()
    {
        return analyticsEvents;
    }
}