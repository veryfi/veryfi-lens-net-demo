# Checks

Capture and cloud-extract a bank check on the Full UI SDK this plugin links. Not a Checks-only package.

Check TIFF is unavailable now. `retrieveCheckTiffIsOn` belongs to Remote Models, and a Remote Models plugin variant is planned, not shipped. Do not set the flag and do not promise a TIFF path on success.

## Permissions

Camera usage string. Photo library usage because gallery and browse are on. Backup is off, so no photo-add string. Location stays off.

`browseIsOn` is only the More-menu row, and this recipe hides that menu. To import a check from Files, call `ShowDocumentBrowser(page)` or set `showBrowserButton` true.

## Settings

`checksBackIsRequired` stays false. Set it true only when both sides are mandatory. `validateCheckCorners` stays false; the corner check is aggressive.

Fractional and delay values are doubles so the iOS `as? Double` cast has a chance. Whether an int would also work is **Unconfirmed**.

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
    ["documentTypes"] = new[] { "check" },
    ["showDocumentTypes"] = false,
    ["stitchIsOn"] = false,
    ["autoCaptureIsOn"] = false,
    ["galleryIsOn"] = true,
    ["browseIsOn"] = true,
    ["checksBackIsOn"] = true,
    ["checksBackIsRequired"] = false,
    ["checkSequenceMode"] = true,          // front, then back
    ["forceLandscapeImage"] = true,
    ["autoSelectManualCropIsOn"] = true,
    ["autoRotateIsOn"] = true,
    ["autoSkewCorrectionIsOn"] = false,
    ["captureMarginRatio"] = 0.08,
    ["checksCropMargin"] = 2,
    ["checksMinAspectRatio"] = 1.9,
    ["checksMaxAspectRatio"] = 2.85,
    ["forceCropLayoutOnRetake"] = true,
    ["cropLayoutShowDelay"] = 15.0,
    ["cropLayoutAspectRatio"] = 2.18,
    ["cropLayoutCornerRadius"] = 5,
    ["cropLayoutOverlayAlpha"] = 0.3,
    ["validateCheckSize"] = true,
    ["validateCheckSides"] = true,
    ["validateCheckCorners"] = false,
    ["allowSubmitCheckSideNotDetected"] = true,
    ["allowSubmitCheckCornersNotDetected"] = true,
    ["allowSubmitUndetectedDocsIsOn"] = false,
    ["returnStitchedPDF"] = true
};
```

`allowSubmitUndetectedDocsIsOn` false blocks a missing rectangle. The allow-submit check flags stay true so a failed side or corner warning can still continue. `validateCheckCorners` is off, so the corner allow flag is idle until someone turns the validator on.

## Present

`ShowCamera(page)` after configure. Success `status` `"done"` is the extracted check, not a TIFF. Copy image paths in `OnUpdate` if auto-delete stays at the SDK default.

## Do not

- Set `retrieveCheckTiffIsOn` or `rejectInvalidCheckTIFF`
- Set `"mobile"`
- Use `anyDocumentBackIsOn` for the check back. The back is `checksBackIsOn` and `checkSequenceMode`
- Quote the AAR or xcframework size as the installed size. Measure the final build. [SDK binary sizes](https://docs.veryfi.com/lens/mobile/settings/sdk_binary_sizes/)
