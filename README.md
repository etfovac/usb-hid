# USB HID communication in C#
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/etfovac/hid/blob/main/LICENSE) 

Based on Szymon Roslowski's 3 fantastic articles on www.codeproject.com:  
📎 [HID USB Stack part 1](https://www.codeproject.com/Articles/830856/Microchip-PIC-F-USB-Stack)  
📎 [HID USB Stack part 2](https://www.codeproject.com/Articles/832135/Microchip-PIC-F-USB-Stack-Part)  
📎 [C# USB HID Interface](https://www.codeproject.com/Tips/530836/Csharp-USB-HID-Interface)

My contribution/patch for UsbHid class is contained in the Addendum folder: there is a distinction between SingleDevice and SingleHID because for a composite HID, VID and PID are not enough to differentiate and select a HID, and Interface number MI is also checked. 
Also DeviceDiscovery class uses [UsbNotification](https://stackoverflow.com/questions/16245706/check-for-device-change-add-remove-events) for detecting dis/connecting of the USB device.  

### Table of Contents (Wiki)
[Wiki Home](https://github.com/etfovac/hid/wiki)  
[Overview](https://github.com/etfovac/hid/wiki/Overview)  
[Notes](https://github.com/etfovac/hid/wiki/Notes)  
[Examples](https://github.com/etfovac/hid/wiki/Examples)  
[References](https://github.com/etfovac/hid/wiki/References) 


[hid](https://github.com/etfovac/hid) is maintained by [etfovac](https://github.com/etfovac).
