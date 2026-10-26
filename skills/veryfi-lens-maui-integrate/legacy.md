# Legacy Xamarin bindings

`VeryfiNetBinding/` is the older .NET for iOS and .NET for Android binding. The repo README still lists it for existing consumers. It is not `VeryfiLens.VeryfiLensBinding`.

## iOS legacy surface that was read

File: `VeryfiNetBinding/iOSVeryfiBinding/ApiDefinition.cs`.

| | Legacy | MAUI |
|--|--------|------|
| Namespace | `VeryfiLensiOS` | `VeryfiLens` |
| Type | `VeryfiLens` (native name `_TtC10VeryfiLens10VeryfiLens`) | `VeryfiLensBinding` |
| Entry | `VeryfiLens.Shared` | static methods |
| Configure | `ConfigureWith(credentials, settings, completion)` | `Configure(credentials, dictionary, completion)` |
| Camera | `ShowCameraIn(UIViewController)` | `ShowCamera(Page)` |
| Gallery | `ShowGalleryIn` | `ShowGallery` |
| Files | `ShowDocumentBrowserIn` | `ShowDocumentBrowser` |
| Upload | `UploadImagesWithPaths(string[])` — paths only, no `isSameTransaction` | `UploadImages(paths, isSameTransaction)` which iOS still ignores |
| Close | `CloseCameraIn` | `CloseCamera` |
| Also exported | `ShowCropIn`, `ShareOCRFeedback`, `AllowReuploadingFilesWithIdentifiers`, `Utils` | Not on `VeryfiLensBinding` |
| Settings | Generated `VeryfiLensSettings` properties (`InitWith`) | `Dictionary<string, object>` |
| Delegate | `VeryfiLensDelegate` with `VeryfiLensClose` and the other native names | `IVeryfiLensDelegate.OnClose` and the other `On*` methods |

No `headless` symbol was found in that `ApiDefinition.cs`.

## Android legacy sample

`VeryfiLensAndroidTestApp/MainActivity.cs` calls `VeryfiLens.Configure(Application, credentials, settings)` and `VeryfiLens.ShowCamera()`, and implements `IVeryfiLensDelegate`. That is the generated binding over the Android SDK, not the MAUI bridge in `VeryfiLens.android.cs`.

## How to choose

New MAUI hosts use `VeryfiLensBinding` only. Do not show both APIs in one walkthrough.

If the project already calls `VeryfiLensiOS.VeryfiLens` or the legacy Android `VeryfiLens` type, stay on that binding. Do not rename `ConfigureWith` to `Configure` or `ShowCameraIn` to `ShowCamera` without a port. This pack did not re-check every legacy property against the current native SDK.

`MAUI_PLAN.md` still says the public type is `VeryfiLens` in namespace `VeryfiLens.Sdk` and that settings are a typed class with JSON-string delegate payloads. That plan does not match `VeryfiLensMaui.cs`. Ignore it for new code.
