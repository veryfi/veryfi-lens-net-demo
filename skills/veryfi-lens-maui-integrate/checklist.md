# Checklist

Copy and tick while integrating. Details live in the linked files. Do not skip a row by inventing an API.

```
MAUI Lens:
- [ ] Confirm VeryfiLens.VeryfiLensBinding, not VeryfiLensiOS and not VeryfiLens.Sdk.VeryfiLens
- [ ] Headless and Lite called unsupported if the host asked for them
- [ ] Remote Models and check TIFF left off (planned / unavailable)
- [ ] Install: project reference or NuGet VeryfiLens.Maui 2.0.60, native embed, plist
- [ ] Credentials from env or secure config, not git
- [ ] Quick assumptions written, or guided answers recorded
- [ ] Base dictionary + one recipe (or support escalation for Any-Documents)
- [ ] backupDocsToGallery and backupDocsToiCloud false unless the host wants copies
- [ ] Conflicts read
- [ ] SetDelegate before Configure
- [ ] iOS present only after completion true; Android true is not native success
- [ ] One of ShowCamera, ShowGallery, ShowDocumentBrowser, or UploadImages
- [ ] UploadImages: iOS ignores isSameTransaction; Android passes it
- [ ] Retry: Android drops metadata and imageSourceTag
- [ ] All four delegate methods
- [ ] autoDelete plan (copy immediately, or false + host deletes)
- [ ] InfoViews not set from the dictionary
- [ ] No DiscardPackage, GetFileBase64, headlessUpload, or clearLensStorage call
- [ ] Size note points at final builds, not the AAR/XCFramework file size
```

Route card to leave in the answer:

```
MAUI route:
- Path:
- Binding: VeryfiLens.VeryfiLensBinding
- Product: UI SDK Full
- Presentation:
- Documents:
- Recipe:
- Assumptions:
- Platform asymmetry:
- Not bridged:
```
