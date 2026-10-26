# Source record

Read this tree. Do not bump the pin from this skill. Do not edit the plugin repo or the native SDKs.

| Item | Value |
|------|--------|
| Plugin | `/Users/veryfij/plugins/veryfi-lens-xamarin-plugin` |
| Commit | `ef01d6b2fe32fcb2db0a10c6b198d8f1597e868a` (`ef01d6b` Merge branch `2.0.60-xcode27` into `master`) |
| Package | `PackageId` `VeryfiLens.Maui`, `<Version>2.0.60</Version>` in `VeryfiLens.Maui/src/VeryfiLens.Sdk/VeryfiLens.Sdk.csproj` |
| Target frameworks | `net10.0-android`, `net10.0-ios27.0` |
| Min OS | iOS 15.0, Android 23, both on that csproj |
| Android artifacts in git | `com.veryfi.lens:veryfi-lens-sdk:2.7.0.3`, `com.veryfi.lens.helpers:veryfi-helpers-sdk:2.7.0.3`, `com.veryfi.lens.cpp:veryfi-cpp-sdk:2.7.0.3` as local AARs under `VeryfiLens.Maui/android/libs/` |
| iOS native | `VeryfiLens.xcframework`, `AWSCore.xcframework`, `AWSS3.xcframework` under `VeryfiLens.Maui/ios/native/VeryfiLensBridge/Frameworks/`. Binding comment says the same xcframeworks as the split plugin, noted `3.0.4` |

## Files read for the public API

| Role | Path |
|------|------|
| C# surface | `VeryfiLens.Maui/src/VeryfiLens.Sdk/VeryfiLensMaui.cs` |
| Delegate | `IVeryfiLensDelegate.cs`, `IVeryfiLensAnalyticsDelegate.cs` |
| Credentials | `VeryfiLensCredentials.cs` |
| iOS partial | `Platforms/iOS/VeryfiLens.ios.cs` |
| Android partial | `Platforms/Android/VeryfiLens.android.cs` |
| iOS Swift bridge | `ios/native/VeryfiLensBridge/NewBinding/VeryfiLensBridge.swift` |
| Android Java bridge | `android/native/veryfilensbridge/src/main/java/com/veryfi/lens/bridge/VeryfiLensBridge.java` |
| Sample | `sample/VeryfiLens.Maui.Sample/MainPage.xaml.cs`, `LensConfiguration.cs`, iOS `Info.plist`, Android `AndroidManifest.xml` |
| Repo docs | `README.md`, `MAUI_PLAN.md` (both stale against the C# surface; see [clarifications.md](clarifications.md)) |
| Legacy iOS binding | `VeryfiNetBinding/iOSVeryfiBinding/ApiDefinition.cs` namespace `VeryfiLensiOS` |
| Legacy Android sample | `VeryfiNetBinding/VeryfiLensAndroidTestApp/MainActivity.cs` calls `VeryfiLens.Configure` / `ShowCamera` |

`VeryfiLens.Maui/DISTRIBUTION.md` is linked from the README and is **not in this commit**.

## Settings keys

iOS applies the dictionary with `VeryfiLensSettings(with:)`, the camelCase initializer, in `VeryfiLensBridge.swift`. Key names and meanings in [settings-catalog.md](settings-catalog.md) were checked against `init(with:)` in `veryfi-lens-ios` `Veryfi-Lens/Public/Model/VeryfiLensSettings.swift`. That Swift file is the Lens source reviewed with this pack. It is not proven to be the same build as the vendored xcframework (binding comment: `3.0.4`). A key present in the reviewed initializer and absent from that binary is **Unconfirmed**.

The bridge does **not** call `init(snake:)`. Snake_case keys are not applied on iOS by this plugin.

Android calls `settings.initWithJsonObject` on a `JSONObject` built from the map. The Android SDK source was not opened. Whether each catalog key is accepted on Android is **Unconfirmed**.

## Not verified

- Models embedded inside the vendored `VeryfiLens.xcframework` (which document types actually detect)
- A public NuGet feed URL. `IsPackable` and `PackageId` are set; no feed is named in the files read
- Runtime of `Configure` with empty credentials (the completion path is in source; the `invalid_credentials` payload is the native SDK)
- `initWithJsonObject` for every recipe key
- That `String(describing:)` of the iOS analytics notification object is JSON
- Legacy `VeryfiNetBinding` method-by-method parity with `VeryfiLensBinding`. New hosts use `VeryfiLensBinding`
