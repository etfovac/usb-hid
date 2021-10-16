using System;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UsbHid.USB.Classes;

namespace UsbHid.USB.Structures
{
    public struct DeviceInformationStructure
    {
        public event EventHandler<EventArgs<bool>>  ConnectedChanged;

        public int TargetVendorId;                  // Our target device's VID
        public int TargetProductId;                // Our target device's PID
        public int TargetMultiInterface;          // Our target device's MI
        public string TargetCaption;             // Our target device's Caption
        //public HiddAttributes Attributes;       // HID Attributes
        public HidpCaps Capabilities;          // HID Capabilities
        public SafeFileHandle ReadHandle;       // Read handle from the device
        public SafeFileHandle WriteHandle;      // Write handle to the device
        public SafeFileHandle HidHandle;        // Handle used for communicating via hid.dll
        public IntPtr DeviceNotificationHandle; // The device's notification handle


        // The device's path name
        private string _devicePathName;
        public string DevicePathName
        {
            get { return _devicePathName; }
            set
            {
                _devicePathName = value;
                if (string.IsNullOrEmpty(_devicePathName))
                {
                    DeviceChangeNotifier.Stop();
                }
                else
                {
                    DeviceChangeNotifier.Start(_devicePathName);
                }
            }
        }

        // Device attachment state flag
        private bool _isDeviceAttached;
        public bool IsDeviceAttached
        {
            get { return _isDeviceAttached; }
            set {
                if (_isDeviceAttached == value) return;
                _isDeviceAttached = value;
                if(ConnectedChanged == null) return;
                ConnectedChanged(this, new EventArgs<bool>(value));
            }
        }
        
        // All devices with VID,PID,MI
        internal List<string> Paths;
    }
}
