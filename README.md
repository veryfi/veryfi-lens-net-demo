<img src="https://user-images.githubusercontent.com/30441118/212185646-f96d2e4c-daf4-4286-8f1b-c92058224b87.png#gh-dark-mode-only" width="200">
<img src="https://user-images.githubusercontent.com/30441118/212185644-ab61c399-0f0c-4d22-a361-0191632d63d2.png#gh-light-mode-only" width="200">

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
# Veryfi Lens for .NET

<a href="https://apps.apple.com/co/app/veryfi-lens/id1498300628?l=en"><img src="https://upload.wikimedia.org/wikipedia/commons/thumb/3/3c/Download_on_the_App_Store_Badge.svg/2560px-Download_on_the_App_Store_Badge.svg.png" width="200"></a>&nbsp;&nbsp;
<a href="https://play.google.com/store/apps/details?id=com.veryfi.lensdemo"><img src="https://en.logodownload.org/wp-content/uploads/2019/06/get-it-on-google-play-badge-1.png" width="200"></a>

Veryfi Lens is code (a framework) with UI for your mobile app to give it document capture superpowers in minutes.

Let Veryfi handle the complexities of frame processing, asset preprocessing, edge routing, and machine vision challenges in document capture. We have been at this for a long time and understand the intricate nature of mobile capture. That’s why we built Lens. Veryfi Lens is built by developers for developers; making the whole process of integrating Lens into your app fast and easy with as few lines as possible.

Veryfi Lens is a Framework: a self-contained, reusable chunks of code and resources you can import into you app.

Lens is built in native code and optimized for fast performance, clean user experience and low memory usage.

You can read further about Lens in Veryfi's dedicated page: https://www.veryfi.com/lens/

You can watch our video:
[![Veryfi Lens](https://img.youtube.com/vi/TUV5SXpKN48/maxresdefault.jpg)](http://www.youtube.com/watch?v=TUV5SXpKN48 "Veryfi Lens Features")

## Table of content
1. [Veryfi Lens .NET Examples](#examples)
2. [How to add Veryfi Lens to your project](#cocoapods)
3. [How to run this project](#configuration)
4. [Other platforms](#other_platforms)
5. [Get in contact with our team](#contact)

## Veryfi Lens Receipts & Invoices .NET Example <a name="example"></a>
This is an example of how to use Veryfi Lens Receipts & Invoices in your app, you can find the developer documentation [here](https://app.veryfi.com/lens/docs/xamarin/).

![receipt-demo](https://user-images.githubusercontent.com/8684090/236651344-306b1f55-a690-4151-87bd-c08d4d9297de.gif)

## How to add Veryfi Lens Receipts to your project 



## How to run this project <a name="configuration"></a>

Running iOS project:

Add Veryfi Nuget repo configuration to Visual Studio(right-click on Packages and then Manage NuGet Packages).

<img width="372" alt="Screen Shot 2022-02-11 at 12 02 30 PM" src="https://github.com/veryfi/veryfi-lens-net-demo/assets/8684090/67056cee-22a0-41e6-9c8e-75af26119fe2">

<img width="889" alt="Screen Shot 2022-02-09 at 3 06 20 PM" src="https://github.com/veryfi/veryfi-lens-net-demo/assets/8684090/60813674-5b70-4ecc-9846-700bf586d4d6">

Click on package source

<img width="298" alt="Screen Shot 2022-02-09 at 3 06 37 PM" src="https://github.com/veryfi/veryfi-lens-net-demo/assets/8684090/c7cbf7b3-bce0-4e54-87d2-022cba62c5e1">

Now select Configure Sources…

<img width="298" alt="Screen Shot 2022-02-09 at 3 06 44 PM" src="https://github.com/veryfi/veryfi-lens-net-demo/assets/8684090/29b324ac-0b9d-4e4c-83a1-72b340c81dcb">

Click add source

<img width="741" alt="Screen Shot 2022-02-09 at 3 06 55 PM" src="https://github.com/veryfi/veryfi-lens-net-demo/assets/8684090/06c7f836-af1b-4e48-b71c-6098606ec577">

Set up the Veryfi Nuget repo URL and your username and password (the same that were created in the Hub in the Xamarin section)

<img width="493" alt="Screen Shot 2022-02-09 at 3 33 06 PM" src="https://github.com/veryfi/veryfi-lens-net-demo/assets/8684090/1625029a-a25f-4fa4-8b9e-73b9625f9a72">

Now click Add Source and now you have the Veryfi Nuget source on packages marketplace and the VeryfiLensiOSNetBinding plugin, add this package to your project.

Set up your project to use Lens SDK:

Add this to your Info.plist file:
```
<key>NSCameraUsageDescription</key>
<string>Scan Documents</string>
<key>NSPhotoLibraryAddUsageDescription</key>
<string>Access Photo Gallery for Document Backups</string>
<key>NSPhotoLibraryUsageDescription</key>
<string>Access Photo Gallery for Document Uploads</string>
```
Update your Entitlements.plist changing the menu in the top right section to `Entitlements`
Make sure that `Keychaing Sharing` switch is on

Set your credentials in ViewController.cs and you should be ready to run the project
```
const string CLIENT_ID = "XXXXX";
const string AUTH_USRNE = "XXXXX";
const string AUTH_API_K = "XXXXX";
const string API_URL = "XXXXX";

```


## Lens iOS Examples <a name="examples"></a>
You can find some example projects, which are the different versions of Lens that we currently offer:
- [Lens for Long Receipts](https://github.com/veryfi/veryfi-lens-long-receipts-ios-demo)
- [Lens for Receipts](https://github.com/veryfi/veryfi-lens-receipts-ios-demo)
- [Lens for Credit Cards](https://github.com/veryfi/veryfi-lens-credit-cards-ios-demo)
- [Lens for Business Cards](https://github.com/veryfi/veryfi-lens-business-cards-ios-demo)
- [Lens for Checks](https://github.com/veryfi/veryfi-lens-checks-ios-demo)
- [Lens for W-2](https://github.com/veryfi/veryfi-lens-w2-ios-demo)
- [Lens for W-9](https://github.com/veryfi/veryfi-lens-w9-ios-demo)

## Lens Android Examples <a name="other_platforms"></a>
You can find these examples for Lens Android 
- [Long Receipts](https://github.com/veryfi/veryfi-lens-long-receipts-android-demo)
- [Receipts](https://github.com/veryfi/veryfi-lens-receipts-android-demo)
- [Credit Cards](https://github.com/veryfi/veryfi-lens-credit-cards-android-demo)
- [Business Cards](https://github.com/veryfi/veryfi-lens-business-cards-android-demo)
- [Checks](https://github.com/veryfi/veryfi-lens-checks-android-demo)
- [W-2](https://github.com/veryfi/veryfi-lens-w2-android-demo)
- [W-9](https://github.com/veryfi/veryfi-lens-w9-android-demo)

We also support the following wrappers for hybrid frameworks:
- [Cordova](https://hub.veryfi.com/lens/docs/cordova/)
- [React Native](https://hub.veryfi.com/lens/docs/react-native/)
- [Flutter](https://hub.veryfi.com/lens/docs/flutter/)
- [Xamarin](https://hub.veryfi.com/lens/docs/xamarin/)

If you don't have access to our Hub, please contact our sales team, you can find the contact bellow.

## Get in contact with our sales team <a name="contact"></a>
Contact sales@veryfi.com to learn more about Veryfi's awesome products.
