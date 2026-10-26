# OCR code caps

Same plugin binary and same document type as strips (`code`, engine `"mobile"`). The only mode change is `ocrType` `"caps"`. iOS `OCRType.caps` is that string. Android acceptance is **Unconfirmed**.

Use [ocr-code-strips.md](ocr-code-strips.md) when the code is a strip rather than a cap. Do not ship both document types.

## Permissions

Camera usage string. No gallery, backup, or location usage strings.

## Settings

Replace the regex. `continuousAutoFocus` is on because caps are small and close to the lens.

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
    ["ocrType"] = "caps",
    ["ocrRegex"] = "[A-Z0-9]{10}",       // replace with the host pattern
    ["autoCaptureIsOn"] = true,
    ["galleryIsOn"] = false,
    ["stitchIsOn"] = false,
    ["autoLightDetectionIsOn"] = false,
    ["continuousAutoFocus"] = true
};
```

## Present

`ShowCamera(page)` after `SetDelegate` and `Configure`. iOS: present only when completion is true. Android: that bool is always true from the binding; watch `OnError`.

Success might not include `status` `"done"`. `ShareOCRFeedback` is not bridged.

## Do not

- Install a separate OCR package
- Set `showInfoButton`
- Describe this as Headless or Lite
