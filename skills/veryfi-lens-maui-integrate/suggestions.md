# Suggestions

Apply these when writing the host. They are defaults for a first successful scan, not extra product scope.

1. Prefer the narrowest `documentTypes` list that matches the documents. Open one template. Do not start from Full when the host only scans receipts.
2. Keep base hardening: ignore remote settings, both backups off, both menus off, dictate off, `autoCaptureOtherIsOn` false.
3. On Android, do not celebrate `Configure`'s true. Log `OnError` and enable the scan button only after you have a plan for that error. On iOS, gate the button on the native bool, as the sample does.
4. Pass the `ContentPage` into `ShowCamera`, `ShowGallery`, and `ShowDocumentBrowser` even though Android ignores it.
5. If the host shows images after the callback, set `autoDeleteLocalResourcesAfterProcessing` false and delete with `File` APIs. Say that `clearLensStorage` is not available.
6. For paths the user already picked, call `UploadImages` once. Do not also open the camera. Mention that iOS drops `isSameTransaction`.
7. For checks, leave TIFF off and say Remote Models is planned, not shipped. Do not add a second native package.
8. For credit cards, `"mobile"` only, and tell the host the payload is local. Do not add an API upload of the card number.
9. For fraud or field detection, say the feature is paid and backend-gated. Do not flip the key to "turn it on."
10. For Any-Documents, stop and tell the host to confirm the template id with support. Do not guess `us_passport` or another id.
11. Measure size on a release build with the Lens reference and without it. Point at [SDK binary sizes](https://docs.veryfi.com/lens/mobile/settings/sdk_binary_sizes/). Do not paste the AAR or xcframework byte size as the install delta.
12. If the project is already on `VeryfiLensiOS`, stay there. Suggest a port only when the host asked to move to MAUI.
13. When a needed call is missing (`DiscardPackage`, help screen, base64, Headless), name it as a port candidate and stop. Do not stub a method that will not compile against `VeryfiLensBinding`.
