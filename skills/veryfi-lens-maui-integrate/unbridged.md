# Native APIs this plugin does not bridge

`VeryfiLensBinding` wraps a subset of the Full UI SDK. A missing wrapper is a plugin gap. The native method can still exist on iOS or Android and **could be ported** by changing the plugin. Do not invent a C# call, and do not tell the host to skip the binding and touch the native SDK unless they already maintain that bridge.

This skill does not make that port.

## Hard to bridge: InfoViews

`showInfoButton` and `cameraInfoView` are `VeryfiInfoView` instances, not colors or booleans. The iOS initializer assigns them only with `as? VeryfiInfoView`. A bool, number, or string in the MAUI dictionary becomes `NSNumber` or `NSString` and does not become a screen.

iOS pins `showInfoButton` over the camera. `cameraInfoView` is a top banner. The view subclass calls inherited `close()` to dismiss. JSON has no way to pass that object. `utils.updateCameraInfoView` is not bridged.

`showHelpButton` is a bool and can show the native control. The help callback `onHelpButtonClicked` is not on `IVeryfiLensDelegate` and not in the Android reflection switch. With `showDefaultScanningGuide` false, the tap has nowhere to go in a MAUI host. Do not set `showHelpButton` and `showInfoButton` together: on iOS they share one control and Help wins.

## Public native methods with no VeryfiLensBinding entry

Verified absent from `VeryfiLensMaui.cs`. iOS signatures were read on `VeryfiLens` in the Lens source (`uploadImages(paths:)`, `headlessUpload`, `shareOCRFeedback`, `allowReuploadingFiles`, `showCrop`, `lensVC`, `closeCamera`). Android equivalents beyond the methods the Java bridge already calls were not opened and are **Unconfirmed** as port targets until that SDK is read. A port still means adding a C# method that calls the native one.

| Native UI SDK | On VeryfiLensBinding | Port note |
|---------------|----------------------|-----------|
| `showCamera` | `ShowCamera` | |
| `showGallery` | `ShowGallery` | |
| `showDocumentBrowser` | `ShowDocumentBrowser` | |
| `closeCamera` | `CloseCamera` | |
| `uploadImages` | `UploadImages` | UI SDK. iOS has no `isSameTransaction`. Android native method takes the flag |
| `headlessUpload` (images or URL) | No | In-memory UI SDK helper, type `receipt`, up to 4 images. Not a Headless product. Unsupported |
| `shareOCRFeedback` | No | Could forward the existing native call |
| `retryPackage` / retry all | `Retry` / `RetryAll` | Android port would need to pass metadata and tag; today it does not |
| `discardPackage` / discard all | No | Queue only on the native SDK. Does not delete retained files |
| Base64 of a local file | No | Host can read the path |
| Analytics observe / stop | Yes | |
| Help click and a host help screen | No | Would need a new delegate method plus a way to present |
| `showCrop` | No | Manual crop of an image the host already has |
| `lensVC` | No | Embeddable camera controller |
| SwiftUI presentation modifiers | No | MAUI uses the page methods |

## Utils and other surface

Not on `VeryfiLensBinding`. Do not generate host calls:

- `clearLensStorage`
- `update(settings:)` after configure
- `updateCameraInfoView`
- `packagesInQueue`, zip path, image, metadata, and PDF lookups
- `allowReuploadingFiles`
- `shareExtensionModeIsOn` is a **settings key** the iOS initializer reads. `UploadImages` does not set it. The host may put the bool in the dictionary. Android acceptance is **Unconfirmed**
- Any `VeryfiLens*Settings` preset subclass

If `autoDeleteLocalResourcesAfterProcessing` is false, say the host owns deletion with normal file APIs.

## Headless products

Headless Receipts and Headless Credit Cards use `VeryfiLensHeadless` and `process` on frames from the host camera. This commit does not reference that module. The legacy iOS `ApiDefinition.cs` that was searched has no `headless` export either.

Mark both **unsupported**. Do not approximate them with `ShowCamera` or `UploadImages`.

`UploadImages` configures the UI SDK and uploads paths. The host does not own detection.

## Lite and Remote Models

Lite / App Clip (`VeryfiLensLite`) does not appear in the MAUI or legacy trees that were searched. **Unsupported.**

No Gradle coordinate or SPM URL in this commit selects Remote Models. **Planned** as a future plugin variant. Check TIFF (`retrieveCheckTiffIsOn`) belongs to that future binary. Do not add it to the checks recipe.

## What a port would mean

Accept the argument on `VeryfiLensBinding`, call the native method from `VeryfiLens.ios.cs` and `VeryfiLens.android.cs`, and return the delegate payload the SDK already produces. Until that change exists in the plugin, tell the host the method is unbridged and stop.
