# Receipts

Cloud extraction of receipts, invoices, and `other` on the Full UI SDK this plugin already links. Not a receipts-only NuGet package. Long receipts are [long-receipts.md](long-receipts.md).

Whether this vendored binary's receipt models match a native Receipts flavor was not unpacked. The dictionary is still how the plugin selects the type.

## Permissions

Camera usage string for `ShowCamera`. Photo library usage if `galleryIsOn` or `ShowGallery`. No photo-add string: backup is off. No location string.

## Settings

```csharp
var settings = new Dictionary<string, object>
{
    ["ignoreRemoteSettings"] = true,
    ["backupDocsToGallery"] = false,
    ["backupDocsToiCloud"] = false,
    ["moreSettingsMenuIsOn"] = false,
    ["moreMenuIsOn"] = false,
    ["autoCaptureOtherIsOn"] = false,
    ["showNoDocumentDetectedWarning"] = false,
    ["dictateIsOn"] = false,
    ["locationServicesIsOn"] = false,
    ["enableScreenshots"] = true,

    ["dataExtractionEngine"] = "api",
    ["documentTypes"] = new[] { "receipt", "invoice", "other" },
    ["showDocumentTypes"] = true,
    ["defaultSelectedDocumentType"] = "receipt",
    ["autoSubmitDocumentOnCapture"] = false,
    ["imagePreviewIsOn"] = true,
    ["galleryIsOn"] = true,
    ["browseIsOn"] = true,                 // visible only if moreMenuIsOn is turned back on
    ["autoCaptureIsOn"] = false,
    ["autoDocDetectionAndCropIsOn"] = true,
    ["autoRotateIsOn"] = true,
    ["autoSkewCorrectionIsOn"] = true,
    ["cleanBorderIsOn"] = true,
    ["stitchIsOn"] = true,
    ["multiplePagesCaptureIsOn"] = true,
    ["multipleDocumentsIsOn"] = true,
    ["returnStitchedPDF"] = true,
    ["packageMaxScans"] = 10,
    ["closeCameraOnSubmit"] = true
};
```

`multiplePagesCaptureIsOn` keeps side-by-side crops from one photo. `multipleDocumentsIsOn` is the submit question about separate packages. They are not one switch.

`browseIsOn` alone does not open Files. Call `ShowDocumentBrowser(page)` or set `showBrowserButton` true.

Customer-owned Process a Document: set `dataExtractionEngine` to `"none"` and follow the decoupled rows in [recipes.md](../recipes.md). Do not use `"mobile"` for receipts.

## Present and callbacks

`ShowCamera(page)` after configure. iOS waits for true. Android's true is the binding, not the SDK. Handle `OnUpdate` paths before the next capture, or set `autoDeleteLocalResourcesAfterProcessing` false.

Gallery backup stays false so scans are not copied into Photos. iCloud backup stays false for the same reason.

## Do not

- Set `"mobile"`
- Set `autoSubmitDocumentOnCapture` and `batchUploadOnCamera` both true
- Set `showInfoButton`
- Treat `UploadImages` as Headless
