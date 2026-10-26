# OCR code strips

On-device OCR of a code **strip** (`documentTypes` `code`, `ocrType` `"strips"`). Caps are the other mode on the same binary and the same document type: [ocr-code-caps.md](ocr-code-caps.md). Not a second package.

`"mobile"` is required. iOS reads `ocrType` `"strips"` and `"caps"`. Android acceptance of `ocrType` is **Unconfirmed**.

`continuousAutoFocus` helps small print near the lens. Android acceptance of that key is **Unconfirmed**.

## Permissions

Camera only. Gallery, backup, and location stay off.

## Settings

Replace `ocrRegex` with the host's pattern. The value below is a placeholder shape, not a universal code format.

```csharp
var settings = new Dictionary<string, object>
{
    ["ignoreRemoteSettings"] = true,
    ["backupDocsToGallery"] = false,
    ["backupDocsToiCloud"] = false,
    ["moreSettingsMenuIsOn"] = false,
    ["moreMenuIsOn"] = false,
    ["autoCaptureOtherIsOn"] = false,
    ["showNoDocumentDetectedWarning"] = false,
    ["dictateIsOn"] = false,
    ["locationServicesIsOn"] = false,
    ["enableScreenshots"] = true,

    ["dataExtractionEngine"] = "mobile",
    ["documentTypes"] = new[] { "code" },
    ["showDocumentTypes"] = false,
    ["ocrType"] = "strips",
    ["ocrRegex"] = "[A-Z0-9]{10}",       // replace with the host pattern
    ["autoCaptureIsOn"] = true,
    ["galleryIsOn"] = false,
    ["stitchIsOn"] = false,
    ["autoLightDetectionIsOn"] = false,
    ["continuousAutoFocus"] = true
};
```

## Result

`ShowCamera(page)`.

OCR success may be the result JSON without `status` `"done"`. Handle `OnSuccess` either way. `ShareOCRFeedback` exists on the native SDK and is **not** on `VeryfiLensBinding`. Do not call it. A port could add it later. See [unbridged.md](../unbridged.md).

## Do not

- Set `ocrType` to anything other than `"strips"` in this template
- Set `"api"` for this on-device recipe
- Leave the example regex in production code
