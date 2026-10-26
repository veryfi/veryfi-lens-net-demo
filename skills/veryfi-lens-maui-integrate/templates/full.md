# Full UI on the MAUI plugin

Widest dictionary on the Full binary this package already links: receipts, invoices, other, and long receipt. It is not a second NuGet package, and it is not a substitute for a Checks, Credit Cards, OCR, or Headless binary. Those products are other recipes, or unsupported.

Which extra models the vendored `VeryfiLens.xcframework` and the `2.7.0.3` AAR actually contain was not unpacked. If a type never detects, treat the missing model as **Unconfirmed** and ask support. Do not add another native artifact from this skill.

Client API: `VeryfiLens.VeryfiLensBinding` and a dictionary. Not `VeryfiLensSettings`.

## Permissions

| Need | When |
|------|------|
| iOS `NSCameraUsageDescription` | `ShowCamera` |
| iOS `NSPhotoLibraryUsageDescription` | `galleryIsOn` or `ShowGallery` |
| iOS photo-add usage | Only if `backupDocsToGallery` is true. This template sets it false |
| iOS location usage | Only if `locationServicesIsOn` is true. This template sets it false |
| Android `CAMERA` | Declared on the bridge manifest |

Do not add Contacts or Calendar usage strings from the vendored framework README.

## Settings

`showDocumentTypes` stays true because more than one type is set. `browseIsOn` does not show a row while `moreMenuIsOn` is false. Files without the More menu: `ShowDocumentBrowser(page)` or `showBrowserButton` true.

```csharp
var settings = new Dictionary<string, object>
{
    ["ignoreRemoteSettings"] = true,
    ["backupDocsToGallery"] = false,       // otherwise every scan is copied into Photos
    ["backupDocsToiCloud"] = false,        // otherwise every scan is copied into iCloud
    ["moreSettingsMenuIsOn"] = false,
    ["moreMenuIsOn"] = false,
    ["autoCaptureOtherIsOn"] = false,      // SDK default true still auto-captures "other"
    ["showNoDocumentDetectedWarning"] = false,
    ["dictateIsOn"] = false,
    ["locationServicesIsOn"] = false,
    ["enableScreenshots"] = true,

    ["dataExtractionEngine"] = "api",
    ["documentTypes"] = new[] { "receipt", "invoice", "other", "long_receipt" },
    ["showDocumentTypes"] = true,
    ["defaultSelectedDocumentType"] = "receipt",
    ["autoSubmitDocumentOnCapture"] = false,
    ["batchUploadOnCamera"] = false,
    ["imagePreviewIsOn"] = true,
    ["galleryIsOn"] = true,
    ["browseIsOn"] = true,
    ["showBrowserButton"] = false,
    ["autoCaptureIsOn"] = false,
    ["autoDocDetectionAndCropIsOn"] = true,
    ["autoRotateIsOn"] = true,
    ["autoSkewCorrectionIsOn"] = true,
    ["cleanBorderIsOn"] = true,
    ["dewarpingIsOn"] = false,
    ["stitchIsOn"] = true,
    ["stitchOtherIsOn"] = false,
    ["multipleDocumentsIsOn"] = true,      // submit can send each image as its own package
    ["multiplePagesCaptureIsOn"] = true,   // side-by-side pages in one photo stay separate crops
    ["singleScanPackages"] = false,
    ["returnStitchedPDF"] = true,
    ["packageMaxScans"] = 10,
    ["closeCameraOnSubmit"] = true,
    ["sameFileUploadPreventionPolicy"] = "session",
    ["allowSubmittingScreenshots"] = true, // imported files; device UI capture is enableScreenshots
    ["webhookIsOn"] = false,
    ["autoDeleteAfterProcessing"] = false
};
```

Paid field detection (`documentFieldsToDetect`) is not part of this template. The entitlement is backend-gated and this package does not ship the Field Detection or Remote Models flavor.

If the host reads files after callbacks, also set `autoDeleteLocalResourcesAfterProcessing` false. `clearLensStorage()` is not bridged.

## Present

`SetDelegate`, then `Configure`, then one of `ShowCamera(page)`, `ShowGallery(page)`, or `ShowDocumentBrowser(page)`. iOS: wait for completion true. Android: completion true is not native success. See [bridges.md](../bridges.md).

`UploadImages` is available when the host already has paths. It is the UI SDK no-UI utility, not a second screen and not Headless. iOS ignores `isSameTransaction`.

## Do not

- Turn `moreMenuIsOn` on unless end users should override capture, torch, backups, and close-on-submit
- Leave gallery backup at the SDK default true
- Set `dictateIsOn` true
- Add `retrieveCheckTiffIsOn`
- Promise Headless, Lite, or Remote Models
