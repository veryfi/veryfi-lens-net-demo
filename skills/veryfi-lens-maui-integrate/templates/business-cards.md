# Business cards

Cloud extraction of `business_card` on the Full UI SDK this plugin links. Not a separate package. `"mobile"` is for `credit_card`, `code`, and `barcode` only, so this recipe stays on `"api"`.

The sample does not demonstrate `business_card`. If the camera does not detect cards, the model may be missing from the vendored binary. That is **Unconfirmed**. Ask support before adding another native framework.

## Permissions

Camera usage string. Gallery is off, backup is off, location is off. Photo-library usage is unnecessary unless you turn `galleryIsOn` on.

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
    ["documentTypes"] = new[] { "business_card" },
    ["showDocumentTypes"] = false,
    ["autoCaptureIsOn"] = false,
    ["galleryIsOn"] = false,
    ["stitchIsOn"] = false,
    ["autoRotateIsOn"] = true,
    ["autoDocDetectionAndCropIsOn"] = true,
    ["cleanBorderIsOn"] = true
};
```

One type, picker off, no stitch. `ShowCamera(page)` is the presentation. iOS waits for configure true. Android's true is not the native result.

`OnSuccess` with `status` `"done"` is the cloud extraction. That is not the credit-card local payload. Business card data does go to the API when the engine is `"api"`.

## Do not

- Set `"mobile"`
- Treat this as Any-Documents. A business card template id is a different solution; see [any-documents.md](../any-documents.md) only if support says the account uses `any_document`
- Set `showInfoButton` or `cameraInfoView`
