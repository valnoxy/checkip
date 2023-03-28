# CheckIP - IP Informations

<img align="right" src="https://dl.exploitox.de/checkip/checkip.png" alt="CheckIP Logo" width="150">

[![Android](https://img.shields.io/badge/Android-Xamarin-brightgreen.svg)](https://github.com/valnoxy/checkip/tree/main/CheckIP.Mobile)
[![iOS](https://img.shields.io/badge/iOS-Xamarin-green)](https://github.com/valnoxy/checkip/tree/main/CheckIP.Mobile)
[![macOS](https://img.shields.io/badge/macOS-Swift-orange)](https://github.com/valnoxy/checkip/tree/main/CheckIP.MacOS)
[![Windows](https://img.shields.io/badge/Windows-WPF-blue)](https://github.com/valnoxy/checkip/tree/main/CheckIP.Windows)
[![License](https://img.shields.io/badge/license-GNU%20General%20Public%20License-purple)](/LICENSE)

<p align="center">
   <strong>Status: </strong>Maintained
   <br />
   <strong>WPF Version: </strong>2.2.1
   <br />
   <strong>Swift (macOS) Version: </strong>0.0.1-dev (WIP)
   <br />
   <strong>Xamarin Version: </strong>2.1.0-alpha2
   <br />
   <a href="https://github.com/valnoxy/checkip/issues">Report Bug</a>
   ·
   <a href="https://github.com/valnoxy/checkip/releases">Download</a>
  </p>
</p>
</br>

<img src="https://raw.githubusercontent.com/valnoxy/valnoxy/main/assets/bar.gif">

## About CheckIP
CheckIP is a GUI based application which can provide much information about an IP address.

## Screenshots
<img src="https://dl.exploitox.de/checkip/v2.2.0/CheckIP_FetchIP.png" width="350"> <img src="https://dl.exploitox.de/checkip/v2.2.0/CheckIP_MyIP.png" width="350">

## Download
[![GooglePlay](https://dl.exploitox.de/checkip/PlayStoreBadge.png)](https://play.google.com/store/apps/details?id=dev.valnoxy.checkip)
[![AppStore](https://dl.exploitox.de/checkip/AppStoreBadge.png)](https://apps.apple.com/us/app/checkip-ip-informations/id1618841457)
[![MicrosoftStore](https://dl.exploitox.de/checkip/MicrosoftStoreBadge.png)](https://www.microsoft.com/store/apps/9NFGS0SX9CP3)

<a href="https://testflight.apple.com/join/auUvXWJJ">Join iOS Beta via TestFlight (currently closed) »</a>

> Note: The mobile applications are currently in beta. The MAUI source base is outdated and and exists only for archival purposes. After MAUI is more stable, this project will move from Xamarin to MAUI.

## Localization
Want to add a new language or revise an existing one? Great! Follow these steps so you can translate CheckIP:

1. Clone this Repository
2. Copy ```Localization\ResourceDirectory.xaml``` and paste your country code into the new file name (e.g. ```ResourceDirectory.de-DE.xaml```).
3. Open ```Pages\App.xaml``` and add a reference to the new file under ```MergedDictionaries```.
   
   ```<ResourceDictionary Source="/CheckIP;component/Localization/ResourceDictionary.de-DE.xaml"/>``` 

4. Open ```Pages\Main.xaml.cs```, go to the function ```Main``` and add your country code in the switch/case function with reference to the new file.
   ```c#
    switch (language)
    {
      default:
      case "en-US":
          dict.Source = new Uri(@"/CheckIP;component/Localization/ResourceDictionary.xaml", UriKind.Relative);
          break;
      case "de-DE":
          dict.Source = new Uri(@"/CheckIP;component/Localization/ResourceDictionary.de-DE.xaml", UriKind.Relative);
          break;
    }
   ```

5. Modify your new file as you wish by translating the value of each variable.
6. When you are ready, you can open a pull request.

## License (GNU GPL)
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.


This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

---

<h6>

```Google Play and the Google Play logo are trademarks of Google LLC.```

```Apple, the Apple logo, Apple Pay, Apple Pencil, Apple TV, Apple Watch, iPad, iPhone, iPod, iPod touch, iTunes, the iTunes logo, Mac, iMac, MacBook, MacBook Pro, MacBook Air, macOS, QuickTime, Siri, watchOS, and 3D Touch are trademarks of Apple Inc., registered in the U.S. and other countries.```

</h6>

---

<h6 align="center">© 2018 - 2023 valnoxy. All Rights Reserved. 
<br>
By Jonas Günner &lt;jonas@exploitox.de&gt;</h6>
<p align="center">
	<a href="https://github.com/valnoxy/checkip/blob/main/LICENSE"><img src="https://img.shields.io/static/v1.svg?style=for-the-badge&label=License&message=GNU%20GENERAL%20PUBLIC%20%20LICENSE&logoColor=d9e0ee&colorA=363a4f&colorB=b7bdf8"/></a>
</p
