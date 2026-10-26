# Settings groups

Not public types. Use them to decide which keys belong together. Each group is entries in the `Dictionary<string, object>` passed to `Configure`.

Compose: **ExtractionMode** + **CaptureFlow** + **CaptureSources** + **QualityGate** + **MultiPage** + optional extras + **Chrome**. Branding last, and only if the host asked.

Android acceptance of a key is **Unconfirmed**. iOS reads the camelCase name in `VeryfiLensSettings.init(with:)`.

## ExtractionMode

| ID | Keys | Use |
|----|------|-----|
| `CloudProcess` | `dataExtractionEngine` `"api"`, `uploadWithoutProcessingIsOn` false | Veryfi API processing |
| `InApp` | `"mobile"` | Only `credit_card`, `code`, `barcode`. Those types extract on device. Credit card data is never sent to the backend |
| `DecoupledLocalPackage` | `"none"`, `uploadWithoutProcessingIsOn` false, `autoDeleteLocalResourcesAfterProcessing` false | Any document type. No extract. Host keeps `local_package_path` and calls Process a Document |
| `DecoupledRemotePackage` | `"none"`, `uploadWithoutProcessingIsOn` true | Lens uploads. Host processes later with `bucket` / `package_path` |
| `VisualOnly` | `"none"`, `uploadWithoutProcessingIsOn` false | Local package or PDF, no extract |

## CaptureFlow

| ID | Keys |
|----|------|
| `ReviewThenSubmit` | `autoSubmitDocumentOnCapture` false, `batchUploadOnCamera` false, `imagePreviewIsOn` true |
| `AutoSubmit` | `autoSubmitDocumentOnCapture` true, `batchUploadOnCamera` false |
| `BatchToMaxThenReview` | `batchUploadOnCamera` true, `autoSubmitDocumentOnCapture` false, `packageMaxScans` set |

Do not combine AutoSubmit with BatchToMaxThenReview.

## CaptureSources

| ID | Keys / calls |
|----|----------------|
| `CameraOnly` | `galleryIsOn` false, `browseIsOn` false, `showBrowserButton` false, `dictateIsOn` false. Present with `ShowCamera` |
| `CameraPlusGallery` | `galleryIsOn` true for the in-camera control, or call `ShowGallery` instead of opening camera |
| `CameraPlusFiles` | More menu (`moreMenuIsOn` true and `browseIsOn` true), top-right `showBrowserButton` true, or gallery menu `galleryIsOn` true and `galleryButtonWithFilesIsOn` true |
| `FilesFirst` | `ShowDocumentBrowser(page)` without camera. Camera flags are not required |

`galleryOtherIsOn` / `browseOtherIsOn` only if `other` is in `documentTypes` and those sources should differ.

`autoCropGalleryIsOn` / `autoCropBrowserIsOn` are best-effort for paper photos and scans and also need `autoDocDetectionAndCropIsOn`. They do not crop every import. Digital-copy checks can keep screenshots and digital copies. An original-file PDF is not page-cropped.

## QualityGate

| ID | Keys |
|----|------|
| `Lenient` | `allowSubmitUndetectedDocsIsOn` true. That legacy flag also permits blur. `allowSubmitBlurredDocsIsOn` is redundant while this is true |
| `Balanced` | `blurDetectionIsOn` true, `allowSubmitUndetectedDocsIsOn` false, `allowSubmitBlurredDocsIsOn` true |
| `StrictBanking` | undetected submit false, check side and size validators on, `captureMarginRatio` `0.08`. Leave `validateCheckCorners` false unless the host asked |

## MultiPage

| ID | Keys |
|----|------|
| `SingleFrame` | `stitchIsOn` false, `multiplePagesCaptureIsOn` false |
| `StitchToPdf` | `stitchIsOn` true, `multiplePagesCaptureIsOn` true, `singleScanPackages` false, `returnStitchedPDF` true |
| `SeparatePackages` | `multipleDocumentsIsOn` true, `stitchIsOn` true, `singleScanPackages` true |

`multiplePagesCaptureIsOn` keeps every crop from one image (false keeps the larger crop). `multipleDocumentsIsOn` asks, on submit preview when stitch is on, whether several images are separate packages.

## Compliance

| ID | Keys |
|----|------|
| `NoGalleryBackup` | `backupDocsToGallery` false, `backupDocsToiCloud` false |
| `KeepLocalResources` | `autoDeleteLocalResourcesAfterProcessing` false. Host deletes files. `clearLensStorage()` is not bridged |
| `NoScreenshotSubmit` | `allowSubmittingScreenshots` false. Device screenshots of the UI are `enableScreenshots` |
| `DeviceBlocklist` | `deviceBlockListIsOn` true |
| `SameFileSession` | `sameFileUploadPreventionPolicy` `"session"` |

Do not set `"always"` from this plugin. That policy needs cloud confirmation and `allowReuploadingFiles`, which is not bridged.

`fraudDetectionIsOn` and `documentFieldsToDetect` request paid features. The account/backend entitlement has to allow them. This package does not ship the Field Detection or Remote Models flavor, so do not turn field detection on here.

## Identity and checks

| ID | Keys |
|----|------|
| `CheckBack` | `checkSequenceMode` true, `checksBackIsOn`, `forceLandscapeImage` true |
| `CheckValidation` | `validateCheckSides`, `validateCheckSize`, and the allow-submit flags. `validateCheckCorners` stays off unless requested |
| `CheckTiff` | Not on this plugin. Remote Models is planned. Do not set `retrieveCheckTiffIsOn` |
| `BackOfAnyDocument` | Only after support confirms an `any_document` template. `anyDocumentBackIsOn` true and `packageMaxScans` exactly `2`. See [any-documents.md](any-documents.md) |

Checks, driver licenses, and insurance cards do not use `anyDocumentBackIsOn`. Driver-license and insurance back flags exist on the iOS initializer. This plugin's templates do not emit them, because those products are Any-Documents shortcuts or separate flavors whose models were not verified in the vendored binary.

## Chrome

| ID | Keys |
|----|------|
| `Minimal` | `moreMenuIsOn` false, `moreSettingsMenuIsOn` false, `shareLogsIsOn` false |
| `GuideAnimation` | `showGuideShortCounter` (receipt), `showGuideCounter` (long receipt), `showPrescriptionLabelGuideCounter`. `animationDisplayDuration` `0` waits for dismissal. `showHelpButton` true with `showDefaultScanningGuide` true replays the built-in guide. The help **callback** is not bridged, so the host cannot present its own screen from that tap |

`showInfoButton` and `cameraInfoView` are `VeryfiInfoView` instances. A bool in the dictionary does not become a screen. See [unbridged.md](unbridged.md).

## Mapping

| Recipe | Groups |
|--------|--------|
| `full` / `receipts` | CloudProcess, ReviewThenSubmit, CameraPlusGallery, Balanced, StitchToPdf, Minimal |
| `long-receipts` | CloudProcess, StitchToPdf, Minimal |
| `checks` | CloudProcess, ReviewThenSubmit, StrictBanking, SingleFrame, CheckBack, CheckValidation, Minimal |
| `credit-cards` / `barcodes` / `ocr-*` | InApp, CameraOnly, Minimal |
| `business-cards` / `w2` / `w9` | CloudProcess, Minimal |
| Any-Documents | No recipe in this pack. Support first |
