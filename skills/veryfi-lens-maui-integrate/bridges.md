# iOS and Android bridges

`VeryfiLensBinding` is one C# signature. The two partial classes do not do the same work. Generate host code that still compiles on both, and say which platform a behavior belongs to.

## Configure

| | iOS | Android |
|--|-----|---------|
| Native call | `VeryfiLens.shared().configure(with:settings:completion:)` | `VeryfiLens.configure(application, credentials, settings)` with no completion |
| Completion | The native `Bool`, then `MainThread` | `completion?.Invoke(true)` in C# after the Java call returns |
| Meaning of `true` | Native validation reported success | The binding always passes true. It does not read a native result |
| Listener | `SetListener` and `SetAnalyticsListener` run after `Configure` is invoked | Same order, then the hardcoded `true` |
| Failure | Completion false, and `OnError` if the SDK emits one after the listener is attached | A native failure is not represented by the completion bool. Watch `OnError` |

Do not treat Android `success == true` as proof the credentials were accepted.

Settings init: iOS `VeryfiLensSettings(with:)` (camelCase). Android `initWithJsonObject`. An exception while building the Android JSON is logged (`VeryfiLensBridge`) and configure continues with a default `VeryfiLensSettings`. That path still ends in `completion(true)`.

## Presentation

| Call | iOS | Android |
|------|-----|---------|
| `ShowCamera` / `ShowGallery` / `ShowDocumentBrowser` | Resolves a `UIViewController` from the `Page` (handler, window, scene, then key window). Throws `InvalidOperationException` if none. Presents with `showCamera(in:)`, `showGallery(in:)`, or `showDocumentBrowser(in:)` | Ignores `page`. Calls `VeryfiLens.showCamera()`, `showGallery()`, or `showDocumentBrowser()` with no activity argument |
| `CloseCamera` | Calls `closeCamera(in:)` only when a view controller resolves. Null resolution is a silent return | Ignores `page`. Calls `VeryfiLens.closeCamera()` |

Pass the page anyway so iOS can present. Do not open two of these for one user action.

In-camera gallery and Files flags (`galleryIsOn`, `browseIsOn`, `showBrowserButton`, `galleryButtonWithFilesIsOn`) are settings. They are not substitutes for `ShowGallery` and `ShowDocumentBrowser`, and those host methods do not require the in-camera flags.

## UploadImages and isSameTransaction

```csharp
VeryfiLensBinding.UploadImages(paths, isSameTransaction);
```

| Platform | What the source does |
|----------|----------------------|
| iOS | `UploadImagesPlatform` calls `VeryfiLensBridge.UploadImages(paths?.ToArray())`. The bool is not a parameter. Swift `uploadImages(paths:)` has no `isSameTransaction` argument |
| Android | `Bridge.UploadImages(paths?.ToArray(), isSameTransaction)` → `VeryfiLens.uploadImages(paths, isSameTransaction)` |

Keep the argument in shared code so Android can use it. Do not document it as doing anything on iOS.

## Retry

| Platform | What the source does |
|----------|----------------------|
| iOS | `RetryPackage(packageId, metadataJson, imageSourceTag)`. JSON object → `[String: Any]`. Parse failure → `[:]`. Then `utils.retryPackage(with:metadata:imageSourceTag:)` |
| Android | `Retry(packageId)` → `VeryfiLens.retryUpload(packageId)`. `metadataJson` and `imageSourceTag` are not forwarded |

`RetryAll` exists on both. Android retry of an empty package id is a no-op in the Java bridge.

## Analytics

| | iOS | Android |
|--|-----|---------|
| Start | `NotificationCenter` observer for `VeryfiLensAnalyticsEvent`, main queue. A second call returns if an observer exists | `BroadcastReceiver` for action `com.veryfi.lens.VeryfiLensSecureAnalyticsEvent`, permission `{packageName}.permission.ANALYTICS_EVENT`, `RECEIVER_EXPORTED` on API 26+. No-op if already observing or the application is null |
| `eventName` | Always the literal `VeryfiLensAnalyticsEvent` | Intent extra `event`, or the action if that extra is missing |
| `paramsJson` | `String(describing: notification.object)`, or `"{}"` if object is nil. This is not run through `JSONSerialization` | Intent extra `params`, or `"{}"` if missing |
| `value` | Always `""` | Intent extra `value`, or `""` |
| Stop | Removes the observer | `unregisterReceiver`. `IllegalArgumentException` is ignored |

Android `ObserveAnalyticsEvents` throws `InvalidOperationException` if `MauiApplication.Current` is null. iOS observe does not touch the MAUI application object.

The bridge manifest requests the analytics permission. It does not contain a `<permission>` definition. Whether the Lens AAR declares `ANALYTICS_EVENT` is **Unconfirmed**.

## Delegate payload

Both bridges map native close, error, success, and update into `"close"`, `"error"`, `"success"`, `"update"` and unwrap JSON to a dictionary. They do not rename payload keys and they do not add `event`.

iOS unwrap drops nulls and turns unrecognized `NSObject` values into `ToString()`. Android's Java bridge keeps nested objects and arrays; the C# side maps `String`, boxed numbers, `IMap`, and `IList`, and `ToString()` for anything else.

Android's reflection proxy only forwards a method when the first argument is a `JSONObject`. Other signatures are ignored. Help-style callbacks are not in that switch.

## Dictionary typing

iOS `init(with:)` casts most flags with `as? Bool`, `as? Int`, or `as? Double`. The C# bridge sends `NSNumber` for those CLR types. Only `blurThreshold` and `maxZoom` in the reviewed initializer also read `NSNumber` explicitly. Whether the other casts succeed across the bridge is **Unconfirmed** (the sample does pass bools this way; this pack did not execute configure).

Integers that the initializer reads as `Double` (`alertButtonHeight`, crop overlay alpha, and similar) need a CLR `double` if the `as? Double` cast does not accept an int `NSNumber`. That cast is **Unconfirmed**. Prefer `double` for fractional keys (`0.08`, `15.0`, `0.3`).
