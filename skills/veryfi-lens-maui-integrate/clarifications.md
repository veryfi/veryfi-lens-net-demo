# Clarifications

Until product or engineering answers, keep the conservative default and the **Unconfirmed** mark. Do not invent the missing behavior in host code.

## Stale repo docs

| Doc says | Source says |
|----------|-------------|
| README `using VeryfiLens.Sdk` and `VeryfiLens.Sdk.VeryfiLens` | Class is `VeryfiLens.VeryfiLensBinding` |
| Class comment: props alias so you can call `VeryfiLens.ShowCamera()` | `VeryfiLens.Sdk.props` only imports the Android native-reference targets. No alias was in the props files read |
| `MAUI_PLAN.md` Android `2.6.0.3`, SPM `master`, target `net10.0-ios` | csproj is AAR `2.7.0.3`, vendored xcframeworks, `net10.0-ios27.0` |
| `MAUI_PLAN.md` typed `VeryfiLensSettings` and JSON-string delegate methods | Dictionary settings and `Dictionary<string, object>` payloads |
| README links `VeryfiLens.Maui/DISTRIBUTION.md` | File is not in commit `ef01d6b2` |
| Plugin READMEs elsewhere mention `dataExtractionEngine` `"custom"` | iOS decoder maps `api`, `mobile`, `none` only |

## Unconfirmed

1. **Vendored xcframework versus the reviewed `init(with:)`.** The binding comment says the frameworks match the split plugin at `3.0.4`. The catalog was checked against the Lens `VeryfiLensSettings` initializer in the tree reviewed with this pack. A key in that initializer might be newer than the binary. If a flag appears to do nothing, say the binary match is Unconfirmed instead of adding another key.

2. **Android `initWithJsonObject`.** The AAR was not decompiled. Every catalog key on Android is Unconfirmed. The Java bridge does forward the map.

3. **NSNumber casts.** C# `bool` and `int` become `NSNumber`. The initializer uses `as? Bool` / `as? Int` / `as? Double` for most keys, and special-cases `NSNumber` only for `blurThreshold` and `maxZoom`. The sample sends bools this way. This pack did not execute configure, so a silent ignore of a flag is Unconfirmed rather than a documented bug.

4. **Int versus Double.** Keys read as `Double` (`cropLayoutOverlayAlpha`, `cropLayoutShowDelay`, `captureMarginRatio`, alert spacing) should be sent as CLR `double`. Whether an int `NSNumber` satisfies `as? Double` was not executed.

5. **iOS analytics `paramsJson`.** It is `String(describing: notification.object)`, not `JSONSerialization`. Do not parse it as JSON unless a payload from a real event is in hand.

6. **Android analytics permission.** The bridge uses `${applicationId}.permission.ANALYTICS_EVENT` and does not define the permission. Whether the Lens AAR's manifest defines it was not checked.

7. **Android location permission** when `locationServicesIsOn` is true. Not in the bridge manifest.

8. **NuGet feed URL** for `VeryfiLens.Maui` `2.0.60`. Packable id is set. No feed URL was found. In-repo hosts use a project reference.

9. **SPM credentials.** README and `MAUI_PLAN.md` require git credentials for `veryfi-lens-spm.git`. The binding project embeds xcframeworks from disk. No `Package.swift` URL was found in the files read. External consumers may still need those credentials. Unconfirmed without `DISTRIBUTION.md`.

10. **Models inside the xcframework.** Do not claim `long_receipt`, `check`, `w2`, or `w9` detection works until a scan or support confirms the binary contains that model. The recipes still set the document type because that is how this Full plugin selects a product.

11. **`closeCameraOnSubmit` and `submitWithoutClose` both true.** Winner was not determined.

12. **`cameraProcessingMode` without `any_document`.** Header text limits it to any-document. Card types that already use a card detector may ignore it. Unconfirmed.

13. **Contacts and Calendar plist keys** in the vendored framework README. Not in the sample plist. Treat as stale unless the host already uses those features.

14. **Empty-credential `msg`.** Native iOS reports `invalid_credentials`. The C# layer does not build that dictionary. Android's completion is still true. Whether Android emits the same `msg` was not opened in the AAR.

15. **Null `UploadImages` paths.** The signature accepts null and iOS forwards it. The native method accepts `[String]?`. What a null array captures was not re-run here.

16. **Legacy binding parity.** `ConfigureWith` / `ShowCameraIn` / `UploadImagesWithPaths` were read. Every other legacy property was not diffed against `VeryfiLensBinding`.
