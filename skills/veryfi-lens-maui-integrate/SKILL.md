---
name: veryfi-lens-maui-integrate
description: >-
  Integrates Veryfi Lens into .NET MAUI through VeryfiLens.VeryfiLensBinding
  (NuGet VeryfiLens.Maui): credentials, settings dictionaries, delegates,
  analytics, and separate iOS and Android bridge behavior. Covers Full UI plus
  receipts, long receipts, checks, credit cards, business cards, barcodes, OCR
  strips and caps, W-2, and W-9, and the UploadImages no-UI path. Use when
  adding Veryfi Lens to a MAUI app, porting off legacy Xamarin bindings, or
  answering a MAUI capture, upload, or extraction question.
---

# Veryfi Lens MAUI integration

Walk a .NET MAUI host from install to a working first scan. Change the **fewest** settings. Do not invent methods, event names, or dictionary keys. If a fact is not in the pinned plugin or the iOS dictionary initializer this pack reviewed, mark it **Unconfirmed** and leave it out of host code.

Public API: `VeryfiLens.VeryfiLensBinding`. Settings are a `Dictionary<string, object>` of camelCase keys. There is no public `VeryfiLensSettings` type on this API. Do not tell the host to construct `VeryfiLensReceiptsSettings` or any `VeryfiLens*Settings` subclass.

This plugin links the **Full** UI SDK (Android `com.veryfi.lens:veryfi-lens-sdk:2.7.0.3` plus helpers/cpp AARs, iOS vendored `VeryfiLens.xcframework`). Receipts, checks, cards, barcodes, OCR, W-2, and W-9 are settings on that binary, not a second NuGet package.

## Workflow

**Guided** is the recommended path. **Quick** is a first scan when the host is already a MAUI app and the document is a receipt.

```
MAUI Lens integrate:
- [ ] Path: quick or guided
- [ ] UI SDK Full (Headless and Lite are unsupported)
- [ ] Install + plist / Android permissions + credentials
- [ ] Base hardening + one recipe
- [ ] Conflict check
- [ ] SetDelegate, then Configure
- [ ] One presentation, or UploadImages
- [ ] OnClose / OnError / OnSuccess / OnUpdate
```

Read one level deep:

- [checklist.md](checklist.md) — copy-paste progress list
- [install.md](install.md) — NuGet, native AARs, xcframeworks, toolchain
- [syntax.md](syntax.md) — exact `VeryfiLensBinding` calls
- [bridges.md](bridges.md) — iOS versus Android behavior
- [legacy.md](legacy.md) — `VeryfiLensiOS` / `VeryfiNetBinding`, not this API
- [recipes.md](recipes.md) — base plus focused dictionaries
- [settings-groups.md](settings-groups.md) — conceptual groups
- [settings-catalog.md](settings-catalog.md) — key meanings
- [callbacks.md](callbacks.md) — payloads, files, entry points
- [conflicts.md](conflicts.md) — incompatible flags and warnings
- [unbridged.md](unbridged.md) — missing native APIs, InfoViews, port candidates
- [any-documents.md](any-documents.md) — open-ended templates; escalate to support
- [clarifications.md](clarifications.md) — Unconfirmed semantics
- [troubleshooting.md](troubleshooting.md)
- [suggestions.md](suggestions.md)
- [sources.md](sources.md) — pinned commit
- Product templates:

| Product | Template |
|---------|----------|
| Full / widest UI on this binary | [templates/full.md](templates/full.md) |
| Receipts | [templates/receipts.md](templates/receipts.md) |
| Long receipts | [templates/long-receipts.md](templates/long-receipts.md) |
| Checks | [templates/checks.md](templates/checks.md) |
| Credit cards | [templates/credit-cards-ui.md](templates/credit-cards-ui.md) |
| Business cards | [templates/business-cards.md](templates/business-cards.md) |
| Barcodes | [templates/barcodes.md](templates/barcodes.md) |
| OCR code strips | [templates/ocr-code-strips.md](templates/ocr-code-strips.md) |
| OCR code caps | [templates/ocr-code-caps.md](templates/ocr-code-caps.md) |
| W-2 | [templates/w2.md](templates/w2.md) |
| W-9 | [templates/w9.md](templates/w9.md) |

### 1. Intake

**Quick.** Do not interview. Write the assumptions in the route card: UI SDK Full, camera, `documentTypes` `receipt`, `dataExtractionEngine` `"api"`, recipe `receipts`, backups and dictate and more menus off.

**Guided.** Ask once. Skip rows the project already answers.

| Topic | Options |
|-------|---------|
| Binding | `VeryfiLensBinding` (new). Legacy `VeryfiLensiOS` stays on [legacy.md](legacy.md); do not rename those calls |
| Documents | receipt, invoice, other, long receipt, check, credit card, business card, barcode, OCR code, W-2, W-9. Any-Documents: [any-documents.md](any-documents.md), do not invent a template id |
| Extract | `"api"`, `"mobile"` (only `credit_card`, `code`, `barcode`), `"none"` |
| UX | Review then submit, auto-submit, batch until max then review |
| Sources | Camera, gallery (`ShowGallery` or in-camera `galleryIsOn`), Files (`ShowDocumentBrowser` or in-camera browse flags) |
| Files after capture | Keep local package files, or let Lens delete them |
| No camera | `UploadImages` for paths the host already has. That is UI SDK, not Headless |

### 2. Product

Headless Receipts and Headless Credit Cards are separate binaries. This plugin does not export `process` or `headlessUpload`. Say **unsupported**. Do not imitate them with `ShowCamera` or `UploadImages`.

Lite / App Clip is not referenced. Say **unsupported**.

Remote Models is not a current artifact. A plugin variant is **planned**. Do not add a Remote Models package. Bank-check TIFF (`retrieveCheckTiffIsOn`) is unavailable on this link now.

Any-Documents is an open-ended solution. Nutrition facts, US passport, driver license, and prescription labels are shortcut flavors of that solution, not separate products. This pack does not emit an `any_document` recipe: the pinned demo does not show which templates are inside the linked Full binary. Contact support to confirm a template or arrange one that is not supported yet.

Local fraud detection (`fraudDetectionIsOn`) and local field detection (`documentFieldsToDetect`) are paid features gated by the validated account and backend entitlement. A dictionary key does not unlock them. Field detection also needs a flavor this plugin does not ship (Field Detection or Remote Models).

When size matters, native Remote Models downloads heavy resources after install. This plugin does not expose that flavor yet. Measure the **final** release build once with the Lens dependency and once without it. The XCFramework and AAR sizes are fat artifacts, not the installed-app delta. [SDK binary sizes](https://docs.veryfi.com/lens/mobile/settings/sdk_binary_sizes/).

### 3. Install and recipe

See [install.md](install.md). Then apply **base** plus **one** id from [recipes.md](recipes.md), or open the matching template. Run [conflicts.md](conflicts.md).

Conservative defaults, unless the host wants the opposite:

- `ignoreRemoteSettings` true, or the dashboard can override the recipe
- `backupDocsToGallery` and `backupDocsToiCloud` false, or every scan is copied into Photos or iCloud
- `moreMenuIsOn` and `moreSettingsMenuIsOn` false
- `autoCaptureOtherIsOn` false (`other` still auto-captures when this stays at the SDK default true)
- `showNoDocumentDetectedWarning` false
- `dictateIsOn` false (obsolete; not partner available)
- `locationServicesIsOn` false
- `enableScreenshots` true (device UI capture). Imported screenshot files are `allowSubmittingScreenshots`

If the host reads files after callbacks, or keeps several packages, set `autoDeleteLocalResourcesAfterProcessing` false. `clearLensStorage()` is **not bridged**. The host deletes with normal file APIs. See [callbacks.md](callbacks.md).

### 4. Configure, present, callbacks

Order the source implements:

1. `SetDelegate` and, if needed, `SetAnalyticsDelegate` (the C# objects are read when events arrive; the native listener is attached inside `Configure`)
2. `Configure(credentials, settings, completion)`
3. On iOS, present only after the completion bool is true. On Android the binding calls `completion(true)` itself; that true is not the native SDK result. See [bridges.md](bridges.md)
4. One of `ShowCamera`, `ShowGallery`, `ShowDocumentBrowser`, or `UploadImages`

Pass the MAUI `ContentPage` into the show methods. iOS needs it to resolve a `UIViewController` and throws if it cannot. Android ignores the page.

Implement all four `IVeryfiLensDelegate` methods. The payload is a dictionary. No `event` field is added.

`UploadImages` is the UI SDK path for image or PDF paths the host already has. It is not a Headless product. iOS drops `isSameTransaction`. Android passes it.

## Do not

- Edit the plugin repo or the native SDK from this skill
- Use the README type `VeryfiLens.Sdk.VeryfiLens` or a typed `VeryfiLensSettings` class. The class is `VeryfiLens.VeryfiLensBinding`; settings are a dictionary
- Mix `VeryfiLensBinding` with legacy `VeryfiLensiOS.VeryfiLens` (`ConfigureWith`, `ShowCameraIn`, `UploadImagesWithPaths`)
- Set `dataExtractionEngine` to `"custom"` (some plugin READMEs mention it; the iOS decoder accepts `api`, `mobile`, and `none`)
- Set `"mobile"` except for `credit_card`, `code`, or `barcode`. Credit card data stays on device and is never sent to the backend
- Promise Headless, Lite, Remote Models, or check TIFF on this package
- Present an unverified Any-Documents template as included
- Set `showInfoButton` or `cameraInfoView` from JSON. Those values are native `VeryfiInfoView` objects. See [unbridged.md](unbridged.md)
- Set `anyDocumentType` (deprecated local regex OCR). Use `anyDocumentTemplate` only after support confirms the id
- Set `useLegacyModels` (debug only)
- Leave `dictateIsOn` true, or `ignoreRemoteSettings` false, unless the host wants dashboard overrides
- Leave `backupDocsToGallery` or `backupDocsToiCloud` on unless the host wants users' Photos or iCloud filled with document copies. Gallery still defaults true. Hide the More menu so users cannot turn the copies back on
- Treat `allowSubmittingScreenshots` and `enableScreenshots` as one switch
- Treat `multiplePagesCaptureIsOn` and `multipleDocumentsIsOn` as one switch
- Treat `browseIsOn` as a global Files gate. It only adds Browse to the camera More menu. `showBrowserButton`, `galleryButtonWithFilesIsOn`, and `ShowDocumentBrowser` are independent
- Call `UploadImages` and a camera presentation for the same user action
- Quote an XCFramework or AAR file size as the size added to the app
- Promise that `fraudDetectionIsOn` or `documentFieldsToDetect` unlocks a local model. Both are paid and backend-gated
- Invent `DiscardPackage`, `ShareOCRFeedback`, `GetFileBase64`, help-button events, or `clearLensStorage` on `VeryfiLensBinding`. They are port candidates in [unbridged.md](unbridged.md)

## Output

When generating host code, produce:

1. The plist keys and Android permissions actually needed
2. `SetDelegate` + `Configure` with the platform completion warning
3. One presentation path, or `UploadImages`
4. The recipe dictionary with a one-line comment per non-obvious key
5. A short conflict note if any residual risk remains
6. What is unbridged, including who deletes local files

```
MAUI route:
- Path: quick | guided
- Binding: VeryfiLens.VeryfiLensBinding
- Product: UI SDK Full
- Presentation:
- Documents:
- Recipe:
- Assumptions:
- Platform asymmetry:
- Not bridged:
```
