# Install

Package identity in source: NuGet `PackageId` `VeryfiLens.Maui`, version `2.0.60`. The sample does not restore that package from a feed. It project-references `VeryfiLens.Maui/src/VeryfiLens.Sdk/VeryfiLens.Sdk.csproj`. A public feed URL is **Unconfirmed**. `VeryfiLens.Maui/DISTRIBUTION.md`, which the README links, is not in this commit.

Do not add a second Lens package for receipts, checks, cards, OCR, W-2, W-9, Lite, Headless, or Remote Models. Those are not separate artifacts here.

## Toolchain the README names

- .NET 10 SDK and the MAUI workload. `global.json` pins SDK `10.0.401` with `rollForward: latestFeature` and workload `10.0.401`
- JDK 17
- Android SDK API 36 (platform and build-tools). `MAUI_PLAN.md` also mentions API 35 for the sample emulator
- Xcode 16.4 or newer. iOS projects set `ValidateXcodeVersion` false

Supported OS on the SDK csproj: iOS 15.0, Android API 23.

## What the host references

```xml
<ProjectReference Include="path/to/VeryfiLens.Sdk/VeryfiLens.Sdk.csproj" />
```

`UseMaui` is true on that project. Target frameworks: `net10.0-android` and `net10.0-ios27.0`.

In-repo executables also import `VeryfiLens.Maui/Directory.Build.targets`, which pulls native references when `OutputType` is `Exe`. The sample csproj imports `VeryfiLens.Maui.props`, and that file imports `VeryfiLens.Sdk.props`.

`VeryfiLensMaui.cs` says a packaged props alias lets you call `VeryfiLens.ShowCamera()`. The props files that were read only import Android native-reference targets. No `using` alias was found. Call `VeryfiLens.VeryfiLensBinding`.

## Android native

The binding project does not generate C# for the full Lens SDK (`Bind=false` on the AARs). Apps call `VeryfiLensBinding`. The thin Java bridge is the bound library (`namespace` `com.veryfi.lens.bridge`, `compileSdk` 36, `minSdk` 23).

Checked-in runtime AARs:

| Coordinate | File |
|------------|------|
| `com.veryfi.lens:veryfi-lens-sdk:2.7.0.3` | `VeryfiLens.Maui/android/libs/veryfi-lens-sdk-2.7.0.3.aar` |
| `com.veryfi.lens.helpers:veryfi-helpers-sdk:2.7.0.3` | `veryfi-helpers-sdk-2.7.0.3.aar` |
| `com.veryfi.lens.cpp:veryfi-cpp-sdk:2.7.0.3` | `veryfi-cpp-sdk-2.7.0.3.aar` |

The bridge Gradle file compiles against that local `libs` directory. It does not declare a Nexus URL. The README still tells developers to set `MAVEN_VERYFI_USERNAME` and `MAVEN_VERYFI_PASSWORD` before a build. `MAUI_PLAN.md` pins Android `2.6.0.3`; the csproj and the files in git are `2.7.0.3`. Follow the csproj.

The SDK csproj also package-references AndroidX Camera, ML Kit barcode scanning, Room, Play Services Location, Material, Volley, and Kotlin coroutines. Those versions are in `VeryfiLens.Sdk.csproj`. Do not invent a different set.

Bridge manifest permissions (library merge):

- `android.permission.INTERNET`
- `android.permission.CAMERA`
- `android.permission.ACCESS_NETWORK_STATE`
- `${applicationId}.permission.ANALYTICS_EVENT` as `uses-permission`

The bridge manifest does not define that custom permission and does not request location. If `locationServicesIsOn` is true, whether the AAR adds a location permission is **Unconfirmed**. The sample app manifest only sets `allowBackup` and does not repeat CAMERA; library merge is what the sample relies on.

`MAUI_PLAN.md` records a Compose duplicate-class workaround (`Xamarin.AndroidX.Compose.Runtime.Annotation.Jvm` with `ExcludeAssets=all`). The props snippet that would apply it is commented out. If D8 reports a duplicate `androidx.compose.runtime.Immutable`, that note is the in-repo history. Do not uncomment it unless the build shows the duplicate.

## iOS native

`VeryfiLens.iOS.Binding.csproj` builds the Swift bridge from `NewBinding.xcodeproj` and embeds:

- `VeryfiLens.xcframework` (linked frameworks listed on the `NativeReference`: AVFoundation, Foundation, CoreImage, CoreLocation, CoreMedia, CoreServices, CoreVideo, ImageIO, MetalKit, Photos, QuartzCore, SystemConfiguration, UIKit, Vision)
- `AWSCore.xcframework`
- `AWSS3.xcframework`

`VeryfiLens.Sdk.iOS.NativeReferences.targets` embeds the same three frameworks from `VeryfiNetBinding/iOSVeryfiBinding/` for executables, so the app actually contains `VeryfiLens.framework`. `MAUI_PLAN.md` describes the crash `Library not loaded: VeryfiLens.framework` when that embed is missing. SPM links the SDK into the bridge and does not embed it in the `.app` by itself.

The README and `MAUI_PLAN.md` tell the developer to configure git credentials for `https://repo.veryfi.com/shared/lens/veryfi-lens-spm.git`. No `Package.swift` dependency on that URL was found in the binding project that was read. This commit ships the xcframeworks in git. Whether an external consumer still needs those git credentials is **Unconfirmed** because `DISTRIBUTION.md` is absent.

## Info.plist

The sample sets:

| Key | Sample string |
|-----|----------------|
| `NSCameraUsageDescription` | Scan documents with Veryfi Lens |
| `NSPhotoLibraryUsageDescription` | Choose document images from your photo gallery |
| `NSPhotoLibraryAddUsageDescription` | Back up document images in your photo gallery |

Add Photo Library only when the host uses gallery or `backupDocsToGallery`. Add the Add usage string only when gallery backup stays on. This pack's recipes turn backup off, so the Add key is unnecessary for those recipes.

Add `NSLocationWhenInUseUsageDescription` only when `locationServicesIsOn` is true. The sample does not set it. The vendored framework README also lists Always-location, Contacts, and Calendar. Do not add those unless the host already uses those features. Contacts and Calendar from that README are **Unconfirmed** / likely stale.

## App credentials

Four fields, all required for a normal configure. Store them outside git.

| `VeryfiLensCredentials` | Sample source |
|-------------------------|----------------|
| `ClientId` | `VERYFI_CLIENT_ID`, else `Resources/Raw/appsettings.json` → `VeryfiLens.ClientId` |
| `Username` | `VERYFI_USERNAME` / `Username` |
| `ApiKey` | `VERYFI_API_KEY` / `ApiKey` |
| `Url` | `VERYFI_URL` / `Url`, sample fallback `https://api.veryfi.com/` |

The sample returns early when `ClientId` is empty and does not call `Configure`. Empty fields that do reach native configure surface on `OnError`. The exact `msg` `invalid_credentials` is the native iOS path reviewed with this pack, not a string the C# binding invents.

`MAUI_PLAN.md` shows a dev URL `https://devapi.veryfi.com` as an optional override. Use the URL Veryfi gave the customer. Do not hard-code the dev host into a production app.
