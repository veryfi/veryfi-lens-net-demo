# Recipes

One settings dictionary. Pass it to `VeryfiLensBinding.Configure`. Do not construct `VeryfiLens*Settings`.

These recipes run on the **Full** UI SDK this plugin links. They are not separate packages. Android `initWithJsonObject` acceptance of a key is **Unconfirmed** unless a later note says the Android SDK was opened. It was not. Keys are the camelCase names `VeryfiLensSettings.init(with:)` reads.

**Lite / App Clip:** unsupported. No recipe.

**Headless receipts and Headless credit cards:** unsupported. No recipe. `UploadImages` is not a substitute.

**Remote Models:** not in this commit. A plugin variant is planned. Do not set `retrieveCheckTiffIsOn` and promise a TIFF download. Check TIFF is unavailable on this binary now.

Start from **base**, then add the keys in **one** focused recipe. The templates already include base. Skip flags the host did not ask for.

## Base

```csharp
var settings = new Dictionary<string, object>
{
    ["ignoreRemoteSettings"] = true,          // dashboard lens_settings must not override this recipe
    ["backupDocsToGallery"] = false,          // true copies every scan into Photos
    ["backupDocsToiCloud"] = false,           // true copies every scan into iCloud
    ["moreSettingsMenuIsOn"] = false,
    ["moreMenuIsOn"] = false,                 // hides user overrides for capture, torch, backups, Superman, close-on-submit
    ["autoCaptureOtherIsOn"] = false,         // SDK default true still auto-captures type "other"
    ["showNoDocumentDetectedWarning"] = false,
    ["dictateIsOn"] = false,                  // obsolete; not partner available
    ["locationServicesIsOn"] = false,
    ["enableScreenshots"] = true             // device screenshots of the camera UI
};
```

If the host must read files after the callback, or keep more than one package:

```csharp
settings["autoDeleteLocalResourcesAfterProcessing"] = false;
```

Who deletes those files: native `clearLensStorage()` is not bridged. See [callbacks.md](callbacks.md) and [unbridged.md](unbridged.md).

## Full (`full`)

Widest UI set on the linked binary: receipts, invoices, other, and long receipt. Use a narrower recipe when the host only needs one type. Which of these models the vendored xcframework actually contains was not unpacked. If detection fails for a type, that binary gap is **Unconfirmed** until support confirms the models. Template: [templates/full.md](templates/full.md).

```csharp
settings["dataExtractionEngine"] = "api";
settings["documentTypes"] = new[] { "receipt", "invoice", "other", "long_receipt" };
settings["showDocumentTypes"] = true;        // required so the user can switch
settings["defaultSelectedDocumentType"] = "receipt";
settings["autoSubmitDocumentOnCapture"] = false;
settings["batchUploadOnCamera"] = false;
settings["imagePreviewIsOn"] = true;
settings["galleryIsOn"] = true;
settings["browseIsOn"] = true;               // More-menu Browse only; moreMenuIsOn is false in base
settings["showBrowserButton"] = false;
settings["autoCaptureIsOn"] = false;
settings["autoDocDetectionAndCropIsOn"] = true;
settings["autoRotateIsOn"] = true;
settings["autoSkewCorrectionIsOn"] = true;
settings["cleanBorderIsOn"] = true;
settings["dewarpingIsOn"] = false;
settings["stitchIsOn"] = true;
settings["stitchOtherIsOn"] = false;
settings["multipleDocumentsIsOn"] = true;
settings["multiplePagesCaptureIsOn"] = true;
settings["singleScanPackages"] = false;
settings["returnStitchedPDF"] = true;
settings["packageMaxScans"] = 10;
settings["closeCameraOnSubmit"] = true;
settings["sameFileUploadPreventionPolicy"] = "session";
settings["allowSubmittingScreenshots"] = true;
settings["webhookIsOn"] = false;
settings["autoDeleteAfterProcessing"] = false; // server-side delete; product decision
```

`browseIsOn` does nothing visible while `moreMenuIsOn` is false. Use `ShowDocumentBrowser`, `showBrowserButton`, or turn the More menu on if Files must be reachable. `galleryIsOn` shows the in-camera gallery control. `ShowGallery` is a separate host call.

## Receipts (`receipts`)

[templates/receipts.md](templates/receipts.md). Same as Full without `long_receipt`.

## Long receipts (`long-receipts`)

[templates/long-receipts.md](templates/long-receipts.md). `long_receipt` is a vertical multi-frame stitch. It is not the same switch as `stitchIsOn` on ordinary receipts. Keep both for this recipe.

## Checks (`checks`)

Capture and cloud extract. Do not set `retrieveCheckTiffIsOn`. [templates/checks.md](templates/checks.md).

## Credit cards (`credit-cards`)

On-device only. `"mobile"` is required. Card data is not sent to the backend. [templates/credit-cards-ui.md](templates/credit-cards-ui.md).

## Business cards (`business-cards`)

`"api"`. The reviewed engine list does not put `business_card` on `"mobile"`. [templates/business-cards.md](templates/business-cards.md).

## Barcodes (`barcodes`)

`"mobile"` with `barcode`. [templates/barcodes.md](templates/barcodes.md).

## OCR strips (`ocr-strips`) and caps (`ocr-caps`)

Same document type `code`. `ocrType` is `"strips"` or `"caps"`. Replace `ocrRegex` with the host pattern. [templates/ocr-code-strips.md](templates/ocr-code-strips.md), [templates/ocr-code-caps.md](templates/ocr-code-caps.md).

## W-2 (`w2`) and W-9 (`w9`)

`"api"`. [templates/w2.md](templates/w2.md), [templates/w9.md](templates/w9.md).

## Decoupled extract (`none`)

When the customer owns Process a Document, set `dataExtractionEngine` to `"none"` on top of the capture recipe. Do not use `"mobile"` for this.

| Path | Keys | Success data the host uses |
|------|------|----------------------------|
| Local zip | `uploadWithoutProcessingIsOn` false, and `autoDeleteLocalResourcesAfterProcessing` false | `local_package_path` while the file still exists |
| Remote package | `uploadWithoutProcessingIsOn` true | `bucket` and `package_path`. Lens uploads and does not call Process a Document |

`uploadWithoutProcessingIsOn` applies with `"none"`. Other engines still process. `.always` duplicate prevention needs `"api"`, and `allowReuploadingFiles` is not bridged, so do not set `sameFileUploadPreventionPolicy` to `"always"` from this plugin.

## Do not combine

- `"mobile"` with `receipt`, `invoice`, `other`, `long_receipt`, `check`, `business_card`, `w2`, or `w9`
- `"api"` or `"none"` with `credit_card` when the host expects Lens to return card data
- `autoSubmitDocumentOnCapture` true with `batchUploadOnCamera` true
- `showHelpButton` and `showInfoButton` as two camera buttons. Help wins, and `showInfoButton` is not a JSON view
- `retrieveCheckTiffIsOn` on this package
- A Headless or Lite recipe
