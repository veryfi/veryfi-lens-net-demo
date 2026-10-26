# Conflicts and warnings

Run after the recipe. Prefer removing an extra key over adding another one.

## Platform warnings

| What you will see | Why | What to do |
|-------------------|-----|------------|
| Android `Configure` completion is true and the camera still fails | `VeryfiLens.android.cs` calls `completion(true)` itself | Treat Android true as "the call returned". Handle `OnError` |
| iOS `UploadImages` ignores `isSameTransaction` | The iOS partial method does not pass the bool | Do not branch iOS behavior on that flag |
| Android `Retry` drops metadata and tag | Java `retry(packageId)` only | Do not expect the tag to survive on Android |
| `ShowCamera` throws on iOS | No `UIViewController` | Pass the live `ContentPage` |
| Events never arrive | Delegate was null when they fired, or `Configure` never attached the listener | `SetDelegate` before `Configure` |
| README sample does not compile | README uses `VeryfiLens.Sdk.VeryfiLens` | Use `VeryfiLens.VeryfiLensBinding` |

## Hard failures

| Combination | What goes wrong | Fix |
|-------------|-----------------|-----|
| `"mobile"` except on `credit_card`, `code`, or `barcode` | No on-device extract for receipts and the other types | `"api"`, or `"none"` for capture-only |
| `credit_card` with `"api"` or `"none"` | Card data must stay on device | `"mobile"` |
| Present or `UploadImages` before iOS configure returns true | Native "not configured" | Wait on iOS. On Android, still wait until you have tried configure, and handle `OnError` |
| `autoDeleteLocalResourcesAfterProcessing` true and the host reads paths later | Empty or overwritten files | Set false, or copy inside `OnUpdate` immediately |
| `backupDocsToGallery` true without the photo-add usage string | Permission prompt or failure | Plist, or set the flag false |
| `locationServicesIsOn` without a location usage string | Permission failure | Plist, or leave the flag false |
| `sameFileUploadPreventionPolicy` `"always"` | Needs cloud confirmation and `allowReuploadingFiles`, which is not bridged | `"session"` or omit it |
| `retrieveCheckTiffIsOn` | TIFF model is not in this plugin binary | Leave it off. Remote Models is planned |
| `documentFieldsToDetect` or `fraudDetectionIsOn` without a paid entitlement | Local models stay off | Settings cannot unlock them. Field detection also needs a flavor this package does not ship |
| Quoting the XCFramework or AAR size as the app-size increase | Those files contain every architecture slice | Measure the final build with Lens linked and with it removed. [SDK binary sizes](https://docs.veryfi.com/lens/mobile/settings/sdk_binary_sizes/) |
| `browseIsOn` true while `moreMenuIsOn` is false, expecting a Browse row | The row lives in the More menu | `ShowDocumentBrowser`, `showBrowserButton`, or the gallery Files menu |

## Submit and detection

| Combination | What goes wrong | Fix |
|-------------|-----------------|-----|
| `autoSubmitDocumentOnCapture` and `batchUploadOnCamera` both true | Two policies for when the camera ends | Pick one capture flow |
| `showDocumentTypes` false with more than one `documentTypes` entry | User cannot switch | Turn the picker on, or set a single type |
| `other` in the list and `autoCaptureOtherIsOn` left at the SDK default true | `other` auto-captures even when `autoCaptureIsOn` is false | Set `autoCaptureOtherIsOn` false |
| `allowSubmitUndetectedDocsIsOn` true while trying to block blur | The undetected flag still allows blur | Set both false to block both, or undetected false and blurred true to allow blur only |
| `stitchIsOn` true on checks or cards | Wrong capture UX | false for those recipes |
| `multiplePagesCaptureIsOn` used as the "separate packages" switch | It only controls crops inside one image | Separate packages are `multipleDocumentsIsOn` |
| `closeCameraOnSubmit` and `submitWithoutClose` both true | Contradictory submit UX. Which one wins is **Unconfirmed** | Set only one |
| `enableScreenshots` used as the imported-file policy | Imported screenshots still submit, or the opposite | `allowSubmittingScreenshots` rejects imported images the detector calls screenshots. `enableScreenshots` blanks the camera in device screenshots and recordings |
| `showHelpButton` and `showInfoButton` both set | They share one control. Help wins. A dictionary cannot pass a `VeryfiInfoView` | Do not set `showInfoButton` from MAUI |
| `glareDetectionIsOn` turned on casually | Current detector is a weak algorithmic gate | Leave false unless the host asked for that gate |
| `cameraProcessingMode` without `any_document` | May be ignored. Card types that already use a card detector are **Unconfirmed** | Set it only for an Any-Documents flow support confirmed |
| `pdfPreviewIsOn` without deciding `pdfEditIsOn` | Preview images and the packaged file are different | Preview keeps the original PDF. Edit packages the image copy and drops the original |

## Privacy and menus

| Combination | What goes wrong | Fix |
|-------------|-----------------|-----|
| `ignoreRemoteSettings` false | Dashboard `lens_settings` can overwrite the recipe | true unless the host wants that |
| `moreMenuIsOn` true | Users persist auto capture, torch, gallery backup, iCloud backup, Superman mode, and close-after-submit | false unless those overrides are the product |
| `backupDocsToGallery` left at the SDK default true | Every scan is copied into Photos | false |
| `backupDocsToiCloud` true | Every scan is copied into iCloud | false |
| `dictateIsOn` true | Obsolete voice control, not partner available | false |

## Engine

| Combination | What goes wrong | Fix |
|-------------|-----------------|-----|
| `"none"` while expecting extracted JSON | Lens does not call Process a Document | `"api"`, or the host processes the package |
| `"none"` and auto-delete still true | No retained `local_package_path` | Set auto-delete false for the local-zip path |
| `uploadWithoutProcessingIsOn` true without `"none"` | The flag applies with `"none"` | Set the engine to `"none"` |
| `"custom"` | Not in the iOS decoder (`api`, `mobile`, `none` only) | Do not emit it |
| `anyDocumentType` set | Deprecated local regex OCR | Leave it unset. `anyDocumentTemplate` only after support confirms the id |
| `webhookIsOn` true and the host only reads the immediate success | Results can arrive later | Handle the delay or leave the flag false |
