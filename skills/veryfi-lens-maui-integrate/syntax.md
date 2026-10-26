# VeryfiLens.VeryfiLensBinding

Namespace `VeryfiLens`. Class `VeryfiLensBinding` in `VeryfiLensMaui.cs`. Every method below is `public static` on that class. Platform bodies are `partial` methods. There is no instance to construct.

```csharp
using VeryfiLens;

VeryfiLensBinding.SetDelegate(new HostDelegate());
VeryfiLensBinding.SetAnalyticsDelegate(new HostAnalytics());

var credentials = new VeryfiLensCredentials
{
    ClientId = clientId,
    Username = username,
    ApiKey = apiKey,
    Url = "https://api.veryfi.com/"
};

var settings = new Dictionary<string, object>
{
    ["ignoreRemoteSettings"] = true,
    ["backupDocsToGallery"] = false,
    ["backupDocsToiCloud"] = false,
    ["dataExtractionEngine"] = "api",
    ["documentTypes"] = new[] { "receipt" }
};

VeryfiLensBinding.Configure(credentials, settings, success =>
{
    if (!success) return;
    VeryfiLensBinding.ObserveAnalyticsEvents();
    VeryfiLensBinding.ShowCamera(this);
});
```

`SetDelegate` before `Configure`. `Configure` installs the native listener and then forwards into whatever `IVeryfiLensDelegate` is stored when the event arrives. Setting the delegate only after events have already fired drops those events.

## Credentials

`VeryfiLensCredentials` properties, all `string`, default `""`:

- `ClientId`
- `Username`
- `ApiKey`
- `Url`

iOS builds `VeryfiLensCredentials(clientId:username:apiKey:url:)`. Android calls `setClientId`, `setUsername`, `setApiKey`, `setUrl` on a new credentials object.

## Settings

`Dictionary<string, object>`. CamelCase keys. One recipe from [recipes.md](recipes.md).

iOS converts the map to `NSDictionary`: `string` → `NSString`, `bool` → `NSNumber`, `int` / `long` / `double` / `float` → `NSNumber`, nested `IDictionary<string, object>` → dictionary, `IEnumerable` (including `string[]`) → `NSArray`. Anything else goes through `NSObject.FromObject` or `NSNull`. The Swift bridge then calls `VeryfiLensSettings(with:)`.

Android converts the same shapes to `Java.Lang.String`, boxed primitives, `HashMap`, and `ArrayList`, then `JSONObject` and `initWithJsonObject`. A null value is omitted on Android. On iOS null becomes `NSNull`.

`documentTypes` in the sample is `new[] { "receipt" }`. Nested maps are how `customHeaders` and `customLensStrings` must be passed if used. Android acceptance of those nested objects is **Unconfirmed**.

## Delegates

```csharp
public interface IVeryfiLensDelegate
{
    void OnClose(Dictionary<string, object> payload);
    void OnError(Dictionary<string, object> payload);
    void OnSuccess(Dictionary<string, object> payload);
    void OnUpdate(Dictionary<string, object> payload);
}
```

`SetDelegate` accepts null and clears the stored delegate. Event names inside the binding are `"close"`, `"error"`, `"success"`, `"update"`. Unknown names are ignored. Payloads are unwrapped to plain dictionaries (nested maps and lists preserved). Null values are dropped. No `event` key is inserted.

Both platforms deliver delegate and analytics callbacks on the main thread (`MainThread.BeginInvokeOnMainThread`).

`IVeryfiLensAnalyticsDelegate`:

```csharp
void OnAnalyticsEvent(string eventName, string paramsJson, string value);
```

Call `ObserveAnalyticsEvents` after configure succeeds on iOS. On Android, configure's `true` does not prove native success; the sample still calls observe inside that callback. `StopObservingAnalyticsEvents` removes the observer. A second `ObserveAnalyticsEvents` while one is active returns without adding another observer.

What each platform puts in the three strings: [bridges.md](bridges.md).

## Public methods

| Method | Signature in source |
|--------|---------------------|
| `SetDelegate` | `void SetDelegate(IVeryfiLensDelegate? lensDelegate)` |
| `SetAnalyticsDelegate` | `void SetAnalyticsDelegate(IVeryfiLensAnalyticsDelegate? analyticsDelegate)` |
| `Configure` | `void Configure(VeryfiLensCredentials credentials, Dictionary<string, object> settings, Action<bool>? completion = null)` |
| `ObserveAnalyticsEvents` | `void ObserveAnalyticsEvents()` |
| `StopObservingAnalyticsEvents` | `void StopObservingAnalyticsEvents()` |
| `ShowCamera` | `void ShowCamera(Page? page = null)` |
| `ShowGallery` | `void ShowGallery(Page? page = null)` |
| `ShowDocumentBrowser` | `void ShowDocumentBrowser(Page? page = null)` |
| `CloseCamera` | `void CloseCamera(Page? page = null)` |
| `Retry` | `void Retry(string packageId, string metadataJson = "{}", string imageSourceTag = "")` |
| `RetryAll` | `void RetryAll()` |
| `UploadImages` | `void UploadImages(IReadOnlyList<string>? paths, bool isSameTransaction = false)` |

`Page` is `Microsoft.Maui.Controls.Page`. Pass the current `ContentPage` (`this` from a page handler). One presentation at a time.

### UploadImages

UI SDK upload of paths the host already has. No Lens camera. Not Headless, and not `headlessUpload`.

```csharp
VeryfiLensBinding.UploadImages(paths, isSameTransaction: false);
```

Call it after configure. The same `IVeryfiLensDelegate` reports progress, success, and errors. iOS ignores `isSameTransaction` and calls `uploadImages(paths:)`. Android passes both arguments to `VeryfiLens.uploadImages(paths, isSameTransaction)`.

`shareExtensionModeIsOn` is a settings key the iOS initializer reads. `UploadImages` does not set it. Set it in the dictionary only for a short-lived extension-style upload. Leave it false in the main app. Android acceptance of that key is **Unconfirmed**.

Native iOS `uploadImages` (reviewed in `VeryfiLens.swift`): requires configure; a bad file is reported on the error callback and skipped. `msg` can be `invalid_file`, `empty_file`, `invalid_pdf_file`, `image_too_small`, `empty_image`, `invalid_image`, or `blurred_image` when `nonInteractiveBlurCheckIsOn` is on. `allowSubmittingScreenshots` false stops the call on a detected screenshot. Null `paths` is forwarded; what the native coordinator does with null is the SDK's behavior (the method accepts `[String]?`).

### Retry

```csharp
VeryfiLensBinding.Retry(packageId, metadataJson, imageSourceTag);
VeryfiLensBinding.RetryAll();
```

`metadataJson` defaults to `"{}"`. iOS parses it as a JSON object and passes `imageSourceTag` into `utils.retryPackage`. Invalid JSON becomes an empty metadata dictionary. Android calls `VeryfiLens.retryUpload(packageId)` only, and only when the id is non-empty. Metadata and tag are dropped. `RetryAll` maps to `retryAllPackages` on iOS and `retryAll` on Android.

## What this class does not declare

`DiscardPackage`, discard-all, `ShareOCRFeedback`, file base64, `headlessUpload`, Headless `process`, Lite, help-button callbacks, `showCrop`, `lensVC`, `clearLensStorage`, `allowReuploadingFiles`, SwiftUI covers. Those stay on the native SDK until someone ports them. See [unbridged.md](unbridged.md).
