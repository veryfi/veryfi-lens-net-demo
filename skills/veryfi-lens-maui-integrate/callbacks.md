# Callbacks, files, and entry points

## Delegate

Implement all four methods. The binding does not add an `event` field. Keys are whatever the native SDK put in the JSON.

```csharp
sealed class HostDelegate : IVeryfiLensDelegate
{
    public void OnClose(Dictionary<string, object> payload) { }
    public void OnError(Dictionary<string, object> payload) { }
    public void OnSuccess(Dictionary<string, object> payload) { }
    public void OnUpdate(Dictionary<string, object> payload) { }
}
```

iOS native comments and the reviewed delegate (the bridge forwards this JSON unchanged):

| Moment | Keys the iOS SDK uses |
|--------|------------------------|
| Close | `session_scan_count`, `queue_count`, `framework-version`. Close means the UI session ended. It is not success |
| Update image | `msg` such as `img_thumbnail_path`, `img_original_path`, or `img_stitched_pdf_path`; `data` is often the path; `package_id` |
| Update progress | `status` `inprogress` or `progress`; `msg` such as `uploading` or `extracting` |
| Success | Package success uses `status` `"done"`. `package_id`. `data` depends on the engine. Barcode and OCR successes can be the result JSON without that envelope. Do not require `status` on those two |
| Error | `status` `error` or `fail` or `exception`. `msg` such as `invalid_credentials`. `package_id` when the failure is a package |

`OnUpdate` fires many times. Do not treat it as finished extraction.

`dataExtractionEngine` `"api"` success `data` is the API payload. `"mobile"` success is the on-device result. Credit card data is only that local payload. `"none"` does not extract: local decoupling can include `local_package_path` when auto-delete is off; remote decoupling (`uploadWithoutProcessingIsOn` true) includes `bucket` and `package_path`.

Android forwards the SDK `JSONObject` with the same method names and does not rename keys. Whether every iOS key appears on Android is **Unconfirmed**.

Help taps are not forwarded. `onHelpButtonClicked` is not on `IVeryfiLensDelegate`.

## Analytics

`OnAnalyticsEvent(string eventName, string paramsJson, string value)`. iOS always reports the name `VeryfiLensAnalyticsEvent` and an empty value. Android reads extras `event`, `params`, and `value`. Details: [bridges.md](bridges.md).

## Who deletes files

`autoDeleteLocalResourcesAfterProcessing` defaults true in the SDK. After a package finishes, Lens deletes that package's local images, zip, thumbnails, generated PDFs, and check TIFF paths. Shared session copies such as `Documents/image.jpg` can be overwritten by the next capture.

| Host need | Setting |
|-----------|---------|
| One document, copy the path inside `OnUpdate` immediately | Default true is enough if the copy happens before the next capture |
| Read files after success, or several packages at once | `autoDeleteLocalResourcesAfterProcessing` false |

When false, the host owns deletion. `clearLensStorage()`, `discardPackage`, and `getZipPath` are **not** on `VeryfiLensBinding`. Delete a path from the payload with `File.Delete` / `File.Exists` after you have copied what you need. Do not invent a Lens delete method.

There is no `GetFileBase64` on this API. The path string is the handle.

Photos and iCloud backups are separate from these package files. `backupDocsToGallery` true (the SDK default) copies every scan into the user's Photos library. `backupDocsToiCloud` true copies every scan into iCloud. Recipes set both false. If `moreMenuIsOn` stays true, the user can turn those copies back on.

## Entry points

Use one primary entry.

| Goal | Call or keys |
|------|----------------|
| Lens camera | `ShowCamera(page)` after configure |
| Photo library without opening camera first | `ShowGallery(page)` |
| Files / document browser without camera | `ShowDocumentBrowser(page)`. No camera flag required |
| Gallery button inside the camera | `galleryIsOn` true |
| Browse inside the camera More menu | `moreMenuIsOn` true and `browseIsOn` true |
| Top-right Files button on the camera | `showBrowserButton` true. Does not require `browseIsOn` |
| Photos-versus-Files menu on the gallery control | `galleryIsOn` true and `galleryButtonWithFilesIsOn` true |
| Paths the host already has | `UploadImages`. No Lens UI. UI SDK, not Headless |
| Dismiss camera | `CloseCamera(page)` |

`browseIsOn` is only the More-menu row. It is not a global permission for `ShowDocumentBrowser` or `showBrowserButton`.

Do not call `ShowCamera` and `ShowGallery` for the same action. Do not call `UploadImages` to stand in for Headless `process`.

`deviceBlockListIsOn` can end the flow with an error (`msg` `device_blocked` on the iOS path). That is an error callback, not a second presentation API.

## Retry from an error

`Retry(packageId, metadataJson, imageSourceTag)` after a failed package. The package id comes from the payload. iOS sends metadata and tag. Android retry keeps the id and drops metadata and tag. Say that in the host comments so an Android retry is not expected to reattach the JSON. `RetryAll` retries the native queue on both platforms.
