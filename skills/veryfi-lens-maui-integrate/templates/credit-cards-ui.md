# Credit cards (Lens UI)

On-device capture of `credit_card` through the Lens camera. The engine must be `"mobile"`. Card data is extracted locally and is never sent to the backend. Do not add a cloud upload of the number, and do not read a Process a Document payload for the card.

This is the UI SDK, not Headless Credit Cards. Headless is **unsupported** on this plugin. `UploadImages` does not replace a host-owned camera.

## Permissions

Camera usage string only. Gallery is off. Backup and location are off. Do not add photo-library or location usage strings for this recipe.

Android still merges the bridge CAMERA permission.

## Settings

```csharp
var settings = new Dictionary<string, object>
{
    ["ignoreRemoteSettings"] = true,
    ["backupDocsToGallery"] = false,       // do not copy card images into Photos
    ["backupDocsToiCloud"] = false,        // do not copy card images into iCloud
    ["moreSettingsMenuIsOn"] = false,
    ["moreMenuIsOn"] = false,
    ["autoCaptureOtherIsOn"] = false,
    ["showNoDocumentDetectedWarning"] = false,
    ["dictateIsOn"] = false,
    ["locationServicesIsOn"] = false,
    ["enableScreenshots"] = true,

    ["dataExtractionEngine"] = "mobile",
    ["documentTypes"] = new[] { "credit_card" },
    ["showDocumentTypes"] = false,
    ["autoCaptureIsOn"] = true,
    ["galleryIsOn"] = false,
    ["stitchIsOn"] = false,
    ["autoLightDetectionIsOn"] = false,
    ["autoRotateIsOn"] = true,
    ["cropLayoutWithAutoDetection"] = true,
    ["cropLayoutWidth"] = 80,
    ["cropLayoutHeight"] = 25,
    ["cropLayoutCornerRadius"] = 15,
    ["cropLayoutStroke"] = 16,
    ["cropLayoutBorderColor"] = "#FFFFFFFF"
};
```

`cropLayoutWithAutoDetection` shows the card frame and the green detection box. The saved crop stays rectangular. `showDocumentTypes` is false because there is one type.

A credit-card-only More menu, if you turn `moreMenuIsOn` back on, is limited on iOS to close-after-submit. This recipe leaves the menu off so users cannot enable gallery backup of card images.

## Present and result

`ShowCamera(page)`. There is no `ShowGallery` in this recipe.

`OnSuccess` is the on-device extraction. Do not forward that dictionary to your server as a Veryfi document unless the host's own privacy review says so. This skill does not add that upload.

`fraudDetectionIsOn` is a separate paid, backend-gated feature. It is not required to read the card, and the key does not unlock it.

## Do not

- Set `dataExtractionEngine` to `"api"` or `"none"`
- Set `backupDocsToGallery` true
- Call `UploadImages` and describe it as headless card scan
- Set `showInfoButton`
