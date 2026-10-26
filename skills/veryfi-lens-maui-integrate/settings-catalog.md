# Settings catalog

Keys a MAUI dictionary can send. iOS applies them in `VeryfiLensSettings.init(with:)` using the camelCase name. Snake_case is a different initializer and this plugin does not call it.

Meanings below are that initializer plus the Lens runtime notes reviewed with this pack. **Unconfirmed** items are listed in [clarifications.md](clarifications.md). Android `initWithJsonObject` was not opened; treat Android acceptance of every row as **Unconfirmed**.

The vendored xcframework is commented as `3.0.4` in the binding project. Equality between that binary and the reviewed initializer is **Unconfirmed** for any key you cannot see applied.

Legend: **Critical** = a wrong value often breaks the integration.

## Document type

| Key | Meaning |
|-----|---------|
| `documentTypes` | **Critical.** String array. One type works with the picker hidden. More than one needs `showDocumentTypes` true so the user can switch. Unknown strings are treated as `other` on iOS |
| `showDocumentTypes` | Camera type picker. Default false |
| `defaultSelectedDocumentType` | Initial selection when the picker is on |
| `anyDocumentTemplate` | Template id for `any_document`. Not a closed list. Do not invent an id. See [any-documents.md](any-documents.md) |
| `anyDocumentType` | **Deprecated.** Local regex OCR. Do not set |
| `useLegacyModels` | Debug only. Do not set |
| `cameraProcessingMode` | `"document"`, `"card"`, or `"check"`. Intended with `any_document`. May be ignored otherwise |

Strings the iOS decoder and document-type handling recognize include `long_receipt`, `receipt`, `invoice`, `check`, `credit_card`, `business_card`, `other`, `code`, `w2`, `w9`, `barcode`, `bank_statement`, `any_document`. Which of those the vendored Full binary can actually detect was not unpacked.

## Extraction and upload

| Key | Meaning |
|-----|---------|
| `dataExtractionEngine` | **Critical.** `"api"` (cloud), `"mobile"` (on device; only `credit_card`, `code`, `barcode`), `"none"` (no extract, any type). `"custom"` is not accepted |
| `uploadWithoutProcessingIsOn` | With `"none"` only. True: Lens uploads and does not call Process a Document (`bucket`, `package_path`). False: local package |
| `webhookIsOn` | Async process. Host must handle a delayed success |
| `autoDeleteAfterProcessing` | Delete the **server** document after process. Not the local-files flag |
| `autoDeleteLocalResourcesAfterProcessing` | **Critical.** Default true deletes local package files after processing. Set false when the host reads paths later. `clearLensStorage()` is not bridged |
| `returnStitchedPDF` | Include a stitched PDF when stitch is on |
| `retrieveCheckTiffIsOn` | Bank-check TIFF. **Do not set.** Not in this plugin binary. Remote Models is planned |
| `rejectInvalidCheckTIFF` | Needs the TIFF flag. Same limit |
| `ignoreRemoteSettings` | **Critical.** False lets dashboard `lens_settings` overwrite the dictionary. Clients should send true |
| `sameFileUploadPreventionPolicy` | `"none"`, `"session"`, `"always"`. `"always"` needs `"api"` and `allowReuploadingFiles`, which is not bridged |
| `shareExtensionModeIsOn` | iOS reads it. With `UploadImages`, true skips thumbnails and posts the zip directly. `UploadImages` does not set this for you. Leave false in the main app |
| `externalId`, `tags`, `categories` | Pass-through hints for cloud extract |
| `autoTagDeviceId`, `autoTagLensVersion`, `autoTagPlatform`, `autoTagSource` | Extra tags. Off unless requested |
| `boostModeIsOn`, `computeIsOn`, `gpuIsOn` | Leave defaults unless Veryfi says otherwise. Who should change them is **Unconfirmed** |
| `parseAddressIsOn`, `boundingBoxesIsOn`, `confidenceDetailsIsOn`, `detectBlurResponseIsOn` | Cloud response options. `detectBlurResponseIsOn` adds blur information; it does not block capture |
| `ocrType` | `"strips"` or `"caps"` for `code` |
| `ocrRegex` | Host pattern for code capture. Do not ship a sample pattern as if it were universal |
| `barcodeExtractionIsOn` | Extra barcode extract alongside another flow. Leave false in the barcode recipe; that recipe uses document type `barcode` |

Credit card data with `"mobile"` stays on device. Do not point the host at a backend card payload.

## Capture quality

| Key | Meaning |
|-----|---------|
| `autoCaptureIsOn` | Best-frame shutter. Often true for cards, barcodes, and codes |
| `autoCaptureOtherIsOn` | **Critical.** SDK default true auto-captures `other` even when `autoCaptureIsOn` is false. Send false |
| `autoCaptureFrameConfidence` | Frames required before auto shutter |
| `autoCaptureMarginRatio`, `captureMarginRatio` | Edge margin. Checks use `0.08` as a double |
| `autoDocDetectionAndCropIsOn` | Live detect and crop |
| `manualCropIsOn` | Finger crop |
| `autoSelectManualCropIsOn` | Open crop UI automatically. Checks often true |
| `autoCropGalleryIsOn`, `autoCropBrowserIsOn` | Best-effort crop of imported paper photos. Not a guarantee. Needs `autoDocDetectionAndCropIsOn`. Original-file PDFs are not page-cropped |
| `autoRotateIsOn`, `autoSkewCorrectionIsOn`, `dewarpingIsOn`, `cleanBorderIsOn` | Geometry. Receipts often skew and clean border. Dewarp is for barcodes in the barcode recipe |
| `blurDetectionIsOn` | Capture-time blur gate |
| `blurThreshold` | 0…1, clamped. The initializer also accepts `NSNumber` |
| `blurDetectionInAutocaptureIsOn` | Blur check after auto shutter |
| `allowSubmitUndetectedDocsIsOn` | Legacy umbrella. True also allows continuing from a blur warning |
| `allowSubmitBlurredDocsIsOn` | Blur exception. Blur can continue when this or the undetected flag is true |
| `nonInteractiveBlurCheckIsOn` | `UploadImages` can drop a blurred file and report `blurred_image` |
| `glareDetectionIsOn` | Weak algorithmic glare gate. Leave false unless the host asked |
| `fraudDetectionIsOn` | Requests paid local fraud detection for camera captures. Backend entitlement required. Does not unlock by itself |
| `showLCDIsNotAllowed` | Blocks a camera frame the fraud detector marks as a screen. Needs `fraudDetectionIsOn` and the paid entitlement |
| `autoLightDetectionIsOn` | Ambient torch |
| `torchAlwaysVisibleIsOn` | Always show the torch control |
| `continuousAutoFocus` | Close-up focus for caps and small codes. Android acceptance **Unconfirmed** |
| `zoomIsOn`, `maxZoom` | Zoom UI. `maxZoom` clamped 1…10 and accepts `NSNumber` |
| `cropLayoutIsOn` | Fixed frame when automatic detection is off. Saved crop stays rectangular |
| `cropLayoutWithAutoDetection` | Frame plus the green detection box. Does not also require `cropLayoutIsOn` |
| `cropLayoutWidth`, `cropLayoutHeight`, `cropLayoutAspectRatio`, `cropLayoutCornerRadius` | Frame geometry. Corner radius is visual only |
| `cropLayoutBorderColor`, `cropLayoutStroke`, `cropLayoutOverlayAlpha` | Outline and dimming. Send alpha as a double |
| `cropLayoutShowDelay` | Seconds before a check guide appears. `0` does not use a timer. Send `15.0` as a double when the checks recipe uses it |
| `forceCropLayoutOnRetake` | After a no-document retake, show the guide immediately |
| `showNoDocumentDetectedWarning` | SDK default shows the alert. Usual host choice is false |
| `documentFieldsToDetect` | Paid local field detection. Not on this binary (needs Field Detection or Remote Models) and not unlocked by the key. Supported names on the native SDK: `vendor`, `date`, `time`, `total`, `tax` |

## Session and package

| Key | Meaning |
|-----|---------|
| `autoSubmitDocumentOnCapture` | Skip submit preview. Do not combine with `batchUploadOnCamera` |
| `batchUploadOnCamera` | Stay on camera until `packageMaxScans` |
| `imagePreviewIsOn` | Preview for gallery and browse imports |
| `packageMaxScans` | Max images per package |
| `multiplePagesCaptureIsOn` | Crops inside one image. False keeps the larger crop |
| `multipleDocumentsIsOn` | With stitch and more than one image, ask if they are the same transaction. No sends each image as its own package |
| `stitchIsOn`, `stitchOtherIsOn`, `stitchTitleIsOn` | Combine into a PDF. `stitchOtherIsOn` only matters when `other` is a document type |
| `singleScanPackages` | True skips the same-transaction alert and forces one package per image |
| `closeCameraOnSubmit` | True ends the session after submit. False returns to the camera |
| `afterScanCloseCameraIsOn` | Shows or hides the More menu row that overrides `closeCameraOnSubmit`. It is not an auto-capture switch |
| `submitWithoutClose` | Keep the submit screen after submit. Both this and close-on-submit true is **Unconfirmed** |
| `clearSessionOnBack` | Back clears the session |
| `pdfPreviewIsOn` | Preview page images and still send the original PDF |
| `pdfEditIsOn` | Package the preview image copy and drop the original PDF. Applies when preview is on |
| `pdfFormatIsOn`, `pdfMaxSizeInMB` | Generated PDF. Do not raise the max above what the SDK clamps (catalog note: not above 100) |
| `originalImageMaxSizeInMB` | Output image cap |
| `noCompress`, `noCompressOnWifi` | Skip compression |
| `galleryMultipleSelectionIsOn`, `browserMultipleSelectionIsOn` | Multi-select, capped by `packageMaxScans` |
| `isSameTransaction` | **Not a settings key.** It is the second argument of `UploadImages`, and only Android forwards it |

## Sources and chrome

| Key | Meaning |
|-----|---------|
| `galleryIsOn` | In-camera gallery control. Independent of `ShowGallery` |
| `galleryButtonWithFilesIsOn` | That control becomes a Photos-versus-Files menu |
| `browseIsOn` | Browse under Import in the More menu. Needs `moreMenuIsOn`. Not a global Files gate |
| `showBrowserButton` | Top-right Files button. Independent of `browseIsOn` |
| `dictateIsOn` | Obsolete. Not partner available. Send false. SDK default is still true |
| `moreMenuIsOn` | More button. While true, stored user overrides win for auto capture, torch, gallery backup, iCloud backup, Superman (`batchUploadOnCamera`), and close-after-submit |
| `moreSettingsMenuIsOn` | More > Settings. Does not reset those overrides |
| `showiCloudBackupOption` | Hides Backup to iCloud and drops that stored override |
| `showSupermanModeIsOn` | Hides Superman mode and drops that stored override |
| `backupDocsToGallery` | SDK default true copies every scan into Photos. Send false unless the host wants that |
| `backupDocsToiCloud` | Copies every scan into iCloud. Send false unless the host wants that |
| `allowSubmittingScreenshots` | Imported files. False rejects images classified as screenshots |
| `enableScreenshots` | Device UI capture. False blanks camera and submit screens in screenshots and recordings |
| `locationServicesIsOn` | Sends location. Needs a location usage string |
| `showHelpButton`, `showDefaultScanningGuide` | Native help control. The host callback is not bridged |
| `showInfoButton`, `cameraInfoView` | `VeryfiInfoView` only. Not settable from this dictionary. See [unbridged.md](unbridged.md) |
| `deviceBlockListIsOn` | Server blocklist can close the flow |
| `customLensStrings` | Nested dictionary: language code → English source string → replacement. Key list: https://docs.veryfi.com/lens/mobile/settings/customizable_strings/ |
| `customHeaders` | Nested string dictionary of extra HTTP headers |
| `appVersion` | Host version string for analytics |

## Checks

| Key | Meaning |
|-----|---------|
| `checksBackIsOn`, `checksBackIsRequired` | Reverse side. Required only when both sides are mandatory |
| `checkSequenceMode` | Front-then-back UI. Recommended when the back is captured |
| `validateCheckSides`, `validateCheckSize` | Alerts. Pair with `allowSubmitCheckSideNotDetected` and `allowSubmitCheckCornersNotDetected` (those allow flags default true) |
| `validateCheckCorners` | Aggressive. Leave false unless requested |
| `checksCropMargin`, `checksMinAspectRatio`, `checksMaxAspectRatio` | Geometry. `0` on a ratio means the SDK default |
| `forceLandscapeImage` | Horizontal check image |
| `allowSubmitUndetectedDocsIsOn` | Keep a way out of a stuck check warning, or the user cannot submit |

`checksBackIsOn` is not `anyDocumentBackIsOn`.
