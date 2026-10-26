# Any-Documents

Any-Documents is an open-ended capture solution: `documentTypes` contains `any_document`, and `anyDocumentTemplate` names the document. It is not a KYC-only or medical-only product, and it is not a closed menu of templates.

Nutrition facts, US passport, driver license, and prescription labels are shortcut flavors of Any-Documents. A shortcut is the same solution aimed at one document, not a separate MAUI package and not a separate `VeryfiLensBinding` method.

## What this pack will not emit

The pinned sample sets `documentTypes` to `receipt` only. The vendored `VeryfiLens.xcframework` was not unpacked to see which templates or models it contains. Do not generate an `any_document` dictionary with a guessed template id (`us_passport`, a driver-license id, or anything similar).

Contact Veryfi support (`support@veryfi.com`, from the framework README's support section) to:

1. Confirm the template id already enabled for the account, or
2. Arrange support for a document type that is not supported yet

After support confirms an id, the host still uses this same Full plugin and a dictionary. There is still no second NuGet package. Sketch only, and only with the confirmed string:

```csharp
settings["dataExtractionEngine"] = "api";
settings["documentTypes"] = new[] { "any_document" };
settings["anyDocumentTemplate"] = "<id support confirmed>";
settings["showDocumentTypes"] = false;
settings["stitchIsOn"] = false;
settings["moreMenuIsOn"] = false;
```

Do not set `anyDocumentType`. That key is deprecated local regex OCR (`ine`, `passport`, and similar matchers in older docs). It is not `anyDocumentTemplate`.

`cameraProcessingMode` (`"document"`, `"card"`, `"check"`) is for `any_document`. Setting it for other document types may be ignored. Card-like types that already select a card detector are **Unconfirmed**.

A back side, when support says the template has one, needs `anyDocumentBackIsOn` true and `packageMaxScans` exactly `2`. `anyDocumentBackIsRequired` true forces that back. Checks do not use these keys; they use `checksBackIsOn` and `checkSequenceMode`.

`identityVerificationIsOn` is an optional face sequence on the native SDK. Leave it off unless the host asked and support confirmed the account can use it.

## Flavor and size

This plugin cannot switch to a nutrition, passport, driver-license, or prescription xcframework. If support says the template needs a binary this Full link does not contain, stop and say the MAUI package does not ship that flavor. Remote Models, which can download heavy models after install, is a planned plugin variant and is not selectable here.

Do not quote the vendored xcframework size as the app impact. Measure a final build with the dependency and without it. [SDK binary sizes](https://docs.veryfi.com/lens/mobile/settings/sdk_binary_sizes/).
