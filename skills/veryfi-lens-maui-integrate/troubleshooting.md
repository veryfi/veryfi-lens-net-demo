# Troubleshooting

Use the symptom. Do not add a method `VeryfiLensBinding` does not declare.

## Camera never opens

| Check | What the source does |
|-------|----------------------|
| Credentials | `ClientId`, `Username`, `ApiKey`, `Url`. The sample refuses to configure when the client id is empty |
| iOS completion | Present only when the bool is true |
| Android completion | `true` is hardcoded. Look at `OnError` and logcat tag `VeryfiLensBridge` |
| Page | iOS throws `Could not resolve a UIViewController to present Veryfi Lens` when the page has no handler yet. Call from a shown page, as the sample does from the button after `OnAppearing` configured Lens |
| Permissions | iOS needs `NSCameraUsageDescription`. Android CAMERA is on the bridge manifest; a merger that strips library manifests would drop it |
| `Library not loaded: VeryfiLens.framework` | The app must embed `VeryfiLens`, `AWSCore`, and `AWSS3`. `MAUI_PLAN.md` records this. In-repo, `VeryfiLens.Sdk.iOS.NativeReferences.targets` does that for `OutputType=Exe` |

## Configure looks successful on Android and extraction never starts

The completion bool is not the native result. Confirm `SetDelegate` ran before `Configure`, then read `OnError`. An exception inside Android settings JSON is logged and configure still completes with a default settings object and `true`.

## Events never arrive

`DispatchEvent` returns immediately when the stored delegate is null. `Configure` attaches the native listener; events that fire before `SetListener` are dropped. Set the delegate first.

Android's reflection proxy ignores a callback whose first argument is not a `JSONObject`.

Payloads have no `event` string. Branch on which method was called (`OnSuccess`, not a field named `event`).

## Wrong document or no extraction

This plugin does not switch binaries per document. `documentTypes` and `dataExtractionEngine` do.

- `"mobile"` only for `credit_card`, `code`, and `barcode`
- Credit card data stays on device
- More than one document type needs `showDocumentTypes` true
- OCR caps versus strips is `ocrType` `"caps"` or `"strips"` with `documentTypes` `code`
- `"custom"` will not map on iOS
- Checks do not gain a TIFF by setting `retrieveCheckTiffIsOn` here
- If a type never detects, the model may be absent from the vendored Full binary. That is **Unconfirmed** until support or a scan says so. Do not add a second package from this skill

## Gallery, files, or upload

`galleryIsOn` and `browseIsOn` are in-camera settings. `ShowGallery` and `ShowDocumentBrowser` are the host calls. `browseIsOn` does nothing the user can see while `moreMenuIsOn` is false.

`UploadImages` is the no-UI path. It is not Headless. Headless and Lite are unsupported.

iOS `UploadImages` ignores `isSameTransaction`. If Android groups images differently from iOS, that flag is why. Do not "fix" iOS by passing it again; the partial method drops it.

## Retry does nothing useful

Android `Retry` keeps the package id and drops `metadataJson` and `imageSourceTag`. An empty id is a no-op. iOS parses JSON and sends the tag; invalid JSON becomes an empty metadata dictionary, and the retry still runs.

There is no `DiscardPackage` to clear a stuck queue. Say it is unbridged.

## `showInfoButton` does nothing or shows a blank page

Expected. The value must already be a `VeryfiInfoView`. A bool does not construct one. Do not set the key. Details: [unbridged.md](unbridged.md).

## Local files disappear

Default native behavior deletes package files after processing. Set `autoDeleteLocalResourcesAfterProcessing` false when the host must read paths later. There is no bridged `clearLensStorage` or `GetFileBase64`. Use the path from `OnUpdate` and normal file IO.

## Photos fill up

`backupDocsToGallery` defaults true in the SDK. Recipes send false. If the More menu is on, the user can enable gallery and iCloud copies again.

## Install failures

| Symptom | What to check |
|---------|----------------|
| Android AAR missing | This commit vendors `VeryfiLens.Maui/android/libs/*2.7.0.3*`. `MAUI_PLAN.md` still says `2.6.0.3` |
| README Maven env | `MAVEN_VERYFI_USERNAME` and `MAVEN_VERYFI_PASSWORD`. The bridge Gradle file uses the local `libs` directory, not a Nexus URL. If a clean machine lacks the AARs, those env vars are what the README tells you to set. The exact Maven URL is not in the Gradle file that was read |
| iOS framework missing at runtime | Embed the three xcframeworks. See the plan note above |
| SPM auth errors | README asks for credentials to `https://repo.veryfi.com/shared/lens/veryfi-lens-spm.git`. The binding csproj points at on-disk xcframeworks. If the build still fetches SPM, that step is outside the csproj that was read (**Unconfirmed**) |
| Xcode version check | Projects set `ValidateXcodeVersion` false because of an Xcode/SDK mismatch recorded in `MAUI_PLAN.md` |
| Duplicate Compose classes | History in `MAUI_PLAN.md`. The props workaround is commented out. Apply it only if the build shows the duplicate |

Do not store Maven, SPM, or API secrets in the skill or in git.
