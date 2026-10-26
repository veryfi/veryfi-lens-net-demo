# Barcodes

On-device `barcode` capture. `dataExtractionEngine` is `"mobile"`. The Full UI SDK this plugin links is the binary. There is no barcode NuGet package.

`continuousAutoFocus` is the iOS close-up focus flag for small codes. The dictionary is forwarded on both platforms. Android acceptance is **Unconfirmed**.

## Permissions

Camera usage string. Gallery is off. Backup and location are off.

## Settings

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
    ["documentTypes"] = new[] { "barcode" },
    ["showDocumentTypes"] = false,
    ["autoCaptureIsOn"] = true,
    ["galleryIsOn"] = false,
    ["stitchIsOn"] = false,
    ["autoLightDetectionIsOn"] = false,
    ["continuousAutoFocus"] = true,
    ["dewarpingIsOn"] = true,
    ["cleanBorderIsOn"] = true,
    ["autoRotateIsOn"] = true
};
```

## Result

`ShowCamera(page)` after configure.

Barcode success may be the result JSON itself, without a package envelope whose `status` is `"done"`. Read `OnSuccess` without requiring that envelope. Do not drop a payload that lacks `status`.

`barcodeExtractionIsOn` is a different flag (extra barcode data beside another document). This recipe does not set it. The document type is `barcode`.

## Do not

- Set `"api"` unless the host explicitly wants cloud processing instead of on-device barcode decode. This template is the on-device path
- Set `ocrType`. That key is for `code`, not `barcode`
- Open `ShowGallery` unless the host asked. This recipe leaves gallery off
