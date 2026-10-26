# Long receipts

Vertical multi-frame stitch for `long_receipt` on the Full binary this plugin links. Not a separate package. Ordinary multi-page receipts stay on [receipts.md](receipts.md): those use `stitchIsOn` without changing the document type to `long_receipt`.

This recipe keeps both `documentTypes` `long_receipt` and `stitchIsOn` true. The type selects the long-receipt camera. Stitch still builds the combined image.

The vendored binary was not unpacked. If long-receipt capture does not start, the model may be absent. That gap is **Unconfirmed**. Ask support instead of adding a native flavor package.

## Permissions

Camera usage string. Photo library usage because `galleryIsOn` is true. Backup and location stay off, so do not add those usage strings.

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

    ["dataExtractionEngine"] = "api",
    ["documentTypes"] = new[] { "long_receipt" },
    ["showDocumentTypes"] = false,
    ["stitchIsOn"] = true,
    ["multiplePagesCaptureIsOn"] = true,
    ["returnStitchedPDF"] = true,
    ["autoCaptureIsOn"] = false,
    ["autoDocDetectionAndCropIsOn"] = true,
    ["galleryIsOn"] = true
};
```

One document type, so the picker stays off. `showGuideCounter` is the automatic long-receipt guide frequency. Leave it unset (SDK default 0, no automatic guide) unless the host asked for the animation. The help callback that would open a MAUI page is not bridged.

## Present

`ShowCamera(page)`. `ShowGallery(page)` only when the host wants the library without the camera. Do not open both for one action.

iOS configure must return true before present. Android configure's true is not native success.

## Do not

- Drop `stitchIsOn` because the document type is already `long_receipt`
- Set `retrieveCheckTiffIsOn` or a Remote Models package
- Set `"mobile"`
