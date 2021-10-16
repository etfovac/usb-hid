using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UsbHid.USB.Classes.DllWrappers;
using UsbHid.USB.Structures;
using System.Linq;
using System.Threading.Tasks;
using UsbHid.Addendum;
using System.Windows.Interop;

namespace UsbHid.USB.Classes
{
    public class DeviceDiscovery
    {
        public delegate void DeviceAddedEventHandler(object sender, string devPath);
        public delegate void DeviceRemovedEventHandler(object sender, string devPath);
        public event DeviceAddedEventHandler RaiseDeviceAddedEvent;
        public event DeviceRemovedEventHandler RaiseDeviceRemovedEvent;
        private readonly IntPtr sourceHandle;

        public DeviceDiscovery()
        {
            sourceHandle = CreateMessageOnlyWindow();
            UsbNotification.RegisterUsbDeviceNotification(sourceHandle);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == UsbNotification.WmDevicechange)
            {
                switch ((int)wparam)
                {
                    case UsbNotification.DbtDeviceremovecomplete:
                        OnDeviceRemoved();
                        break;
                    case UsbNotification.DbtDevicearrival:
                        OnDeviceAdded();
                        break;
                }
            }
            return IntPtr.Zero;
        }

        private IntPtr CreateMessageOnlyWindow()
        {
            IntPtr HWND_MESSAGE = new IntPtr(-3);
            HwndSourceParameters sourceParam = new HwndSourceParameters() { ParentWindow = HWND_MESSAGE };
            HwndSource source = new HwndSource(sourceParam);
            source.AddHook(WndProc);
            return source.Handle;
        }
        public static bool FindHidDevices(ref List<string> DevicePathNames)
        {
            Debug.WriteLine("findHidDevices() -> Method called");
            
            // Initialize the internal variables required for performing the search
            var bufferSize = 0;
            var detailDataBuffer = IntPtr.Zero;
            bool deviceFound;
            var deviceInfoSet = new IntPtr();
            var lastDevice = false;
            int listIndex;
            var deviceInterfaceData = new SpDeviceInterfaceData();

            // Get the required GUID
            var systemHidGuid = new Guid();
            Hid.HidD_GetHidGuid(ref systemHidGuid);
            Debug.WriteLine(string.Format("findHidDevices() -> Fetched GUID for HID devices ({0})", systemHidGuid.ToString()));

            try
            {
                // Here we populate a list of plugged-in devices matching our class GUID (DIGCF_PRESENT specifies that the list
                // should only contain devices which are plugged in)
                Debug.WriteLine("findHidDevices() -> Using SetupDiGetClassDevs to get all devices with the correct GUID");
                deviceInfoSet = SetupApi.SetupDiGetClassDevs(ref systemHidGuid, IntPtr.Zero, IntPtr.Zero, Constants.DigcfPresent | Constants.DigcfDeviceinterface);

                // Reset the deviceFound flag and the memberIndex counter
                deviceFound = false;
                listIndex = 0;

                deviceInterfaceData.cbSize = Marshal.SizeOf(deviceInterfaceData);

                // Look through the retrieved list of class GUIDs looking for a match on our interface GUID
                do
                {
                    //Debug.WriteLine("usbGenericHidCommunication:findHidDevices() -> Enumerating devices");
                    var success = SetupApi.SetupDiEnumDeviceInterfaces(deviceInfoSet,IntPtr.Zero,ref systemHidGuid, listIndex, ref deviceInterfaceData);

                    if (!success)
                    {
                        //Debug.WriteLine("usbGenericHidCommunication:findHidDevices() -> No more devices left - giving up");
                        lastDevice = true;
                    }
                    else
                    {
                        // The target device has been found, now we need to retrieve the device path so we can open
                        // the read and write handles required for USB communication

                        // First call is just to get the required buffer size for the real request
                        SetupApi.SetupDiGetDeviceInterfaceDetail
                            (deviceInfoSet,
                             ref deviceInterfaceData,
                             IntPtr.Zero,
                             0,
                             ref bufferSize,
                             IntPtr.Zero);

                        // Allocate some memory for the buffer
                        detailDataBuffer = Marshal.AllocHGlobal(bufferSize);
                        Marshal.WriteInt32(detailDataBuffer, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);

                        // Second call gets the detailed data buffer
                        //Debug.WriteLine("usbGenericHidCommunication:findHidDevices() -> Getting details of the device");
                        SetupApi.SetupDiGetDeviceInterfaceDetail
                            (deviceInfoSet,
                             ref deviceInterfaceData,
                             detailDataBuffer,
                             bufferSize,
                             ref bufferSize,
                             IntPtr.Zero);

                        // Skip over cbsize (4 bytes) to get the address of the devicePathName.
                        var pDevicePathName = new IntPtr(detailDataBuffer.ToInt32() + 4);

                        // Get the String containing the devicePathName.
                        DevicePathNames.Add(Marshal.PtrToStringAuto(pDevicePathName));

                        //Debug.WriteLine(string.Format("usbGenericHidCommunication:findHidDevices() -> Found matching device (memberIndex {0})", memberIndex));
                        deviceFound = true;
                    }
                    listIndex += 1;
                }
                while (lastDevice != true);
            }
            catch (Exception)
            {
                // Something went badly wrong... output some debug and return false to indicated device discovery failure
                Debug.WriteLine("findHidDevices() -> EXCEPTION: Something went south whilst trying to get devices with matching GUIDs - giving up!");
                return false;
            }
            finally
            {
                // Clean up the unmanaged memory allocations
                if (detailDataBuffer != IntPtr.Zero)
                {
                    // Free the memory allocated previously by AllocHGlobal.
                    Marshal.FreeHGlobal(detailDataBuffer);
                }

                if (deviceInfoSet != IntPtr.Zero)
                {
                    SetupApi.SetupDiDestroyDeviceInfoList(deviceInfoSet);
                }
            }

            if (deviceFound)
            {
                Debug.WriteLine(string.Format("findHidDevices() -> Found {0} devices with matching GUID", (DevicePathNames.Count).ToString(CultureInfo.InvariantCulture)));
            }
            else Debug.WriteLine("findHidDevices() -> No matching devices found");

            return deviceFound;
        }

        public static bool FindTargetDevices(ref DeviceInformationStructure deviceInformation, string Path = "")
        {
            Debug.WriteLine("findTargetDevice() -> Method called");
            List<string> listOfDevicePathNames = new List<string>(); // 128 is the maximum number of USB devices allowed on a single host
            bool isDeviceDetected = false;
            List<string> DevicePaths = new List<string>();
            try
            {
                bool deviceFoundByGuid = FindHidDevices(ref listOfDevicePathNames); // Get all the devices with the correct HID GUID

                if (deviceFoundByGuid)
                {
                    isDeviceDetected = FindHIDs(deviceInformation, listOfDevicePathNames, ref DevicePaths);
                    deviceInformation.Paths = DevicePaths;
                }
                if (Path == "") // take first
                {
                    // Store the device's pathname in the device information
                    deviceInformation.DevicePathName = DevicePaths[0];
                }
                else
                {
                    deviceInformation.DevicePathName = DevicePaths.Find(x => x.Equals(Path));
                }

                // If we found a matching device 
                // then open read and write handles to the device to allow communication
                if (isDeviceDetected)
                {
                    InitSelectedHID(ref deviceInformation);
                }

                return isDeviceDetected;
            }
            catch (Exception)
            {
                Debug.WriteLine("findTargetDevice() -> EXCEPTION: Unknown - device not found");
                
                return isDeviceDetected;
            }
        }

        private static bool InitSelectedHID(ref DeviceInformationStructure deviceInformation)
        {
            deviceInformation.IsDeviceAttached = false;
            // Query the HID device's capabilities (primarily we are only really interested in the 
            // input and output report byte lengths as this allows us to validate information sent
            // to and from the device does not exceed the devices capabilities.
            //
            // We could determine the 'type' of HID device here too, but since this class is only
            // for generic HID communication we don't care...


            Debug.WriteLine("findTargetDevice() -> Performing CreateFile ");
            deviceInformation.HidHandle = Kernel32.CreateFile(deviceInformation.DevicePathName, 0, Constants.FileShareRead | Constants.FileShareWrite, IntPtr.Zero, Constants.OpenExisting, 0, 0);
            QueryDeviceCapabilities(ref deviceInformation);
            //deviceInformation.HidHandle.Close();

            // Open the readHandle to the device
            Debug.WriteLine("findTargetDevice() -> Opening a readHandle to the device");
            deviceInformation.ReadHandle = Kernel32.CreateFile(
                deviceInformation.DevicePathName,
                Constants.GenericRead,
                Constants.FileShareRead | Constants.FileShareWrite,
                IntPtr.Zero, Constants.OpenExisting,
                Constants.FileFlagOverlapped,
                0);

            // Did we open the readHandle successfully?
            if (deviceInformation.ReadHandle.IsInvalid)
            {
                Debug.WriteLine("findTargetDevice() -> Unable to open a readHandle to the device!");
                //
                deviceInformation.ReadHandle.Close();
                deviceInformation.IsDeviceAttached = false;
                return deviceInformation.IsDeviceAttached;
            }

            Debug.WriteLine("findTargetDevice() -> Opening a writeHandle to the device");
            deviceInformation.WriteHandle = Kernel32.CreateFile(
                deviceInformation.DevicePathName,
                Constants.GenericWrite,
                Constants.FileShareRead | Constants.FileShareWrite,
                IntPtr.Zero,
                Constants.OpenExisting, 0, 0);

            // Did we open the writeHandle successfully?
            if (deviceInformation.WriteHandle.IsInvalid)
            {
                Debug.WriteLine("findTargetDevice() -> Unable to open a writeHandle to the device!");

                // Attempt to close the writeHandle
                deviceInformation.WriteHandle.Close();
                deviceInformation.IsDeviceAttached = false;
                return deviceInformation.IsDeviceAttached;
            }

            // Device is now discovered and ready for use, update the status
            deviceInformation.IsDeviceAttached = true;
            return deviceInformation.IsDeviceAttached;

        }

        private static bool FindHIDs(DeviceInformationStructure deviceInformation, List<string> InputList, ref List<string> FoundList)
        {
            List<SingleDevice> devices = FindDevices(deviceInformation);
            if (devices.Count > 0)
            {
                foreach (SingleDevice sdev in devices)
                {
                    SingleHID singleHID = new SingleHID(sdev);

                    foreach (string path in InputList)
                    {
                        Match match = Regex.Match(path, "VID_(.{4})", RegexOptions.IgnoreCase);
                        ushort Vid;
                        if (match.Success)
                        {
                            string vid_string = match.Groups[1].Value;
                            Vid = ushort.Parse(vid_string, System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            Vid = 0;
                        }
                        match = Regex.Match(path, "PID_(.{4})", RegexOptions.IgnoreCase);
                        ushort Pid;
                        if (match.Success)
                        {
                            string pid_string = match.Groups[1].Value;
                            Pid = ushort.Parse(pid_string, System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            Pid = 0;
                        }
                        match = Regex.Match(path, "MI_(.{2})", RegexOptions.IgnoreCase);
                        ushort MI;
                        if (match.Success)
                        {
                            string mi_string = match.Groups[1].Value;
                            MI = ushort.Parse(mi_string, System.Globalization.NumberStyles.HexNumber);
                        }
                        else
                        {
                            MI = 0;
                        }

                        bool Detected = (Vid == singleHID.Vid) && (Pid == singleHID.Pid) && (MI == singleHID.MI);

                        if (Detected)
                        {
                            FoundList.Add(path);
                            //break; 
                            // Matching device found
                            Debug.WriteLine("findTargetDevice() -> Device with matching VID and PID found!");
                        }
                    }
                }


            }
            return FoundList.Count > 0;
        }

        // Returns a list with the device IDs of all HID devices
        // Filters may be removed if a complete list of USB devices is desired
        public static List<string> GetDeviceIdList()
        {
            List<string> deviceIDs = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice");
            ManagementObjectCollection objs = searcher.Get();
            foreach (ManagementObject wmi_HD in objs)
            {
                string dep = wmi_HD["Dependent"].ToString();
                Match match = Regex.Match(dep, "\"(.+VID.+PID.+)\"$", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string devId = match.Groups[1].Value;
                    devId = devId.Replace(@"\\", @"\");
                    devId = devId.ToUpper();
                    if (devId.Substring(0, 3) == "HID")
                    {
                        deviceIDs.Add(devId);
                    }
                }
            }
            return deviceIDs;
        }

        // Returns a list of Device object representing all devices returned by getDeviceIdList()
        public static List<SingleDevice> GetDeviceList()
        {
            List<SingleDevice> devices = new List<SingleDevice>();
            List<string> deviceIDs = GetDeviceIdList();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
            ManagementObjectCollection objs = searcher.Get();

            foreach (ManagementObject wmi_HD in objs)
            {
                string deviceId = wmi_HD["DeviceID"].ToString();
                if (deviceIDs.Contains(deviceId))
                {
                    SingleDevice dev = new SingleDevice(wmi_HD);
                    devices.Add(dev);
                }
            }
            return devices;
        }

        private static List<SingleDevice> FindDevices(DeviceInformationStructure dev1)
        {
            List<SingleDevice> singleDevices = GetDeviceList();
            List<SingleDevice> FoundDevices = new List<SingleDevice>();
            foreach (SingleDevice dev2 in singleDevices)
            {
                if (CheckDevice(dev1, new SingleHID(dev2)))
                {
                    FoundDevices.Add(dev2);
                }
            }
            return FoundDevices;
        }

        private static bool CheckDevice(DeviceInformationStructure dev1, SingleHID dev2)
        {
            bool Found = false;
            if (dev1.TargetCaption == dev2.Caption && dev1.TargetProductId == dev2.Pid && dev1.TargetVendorId == dev2.Vid && dev1.TargetMultiInterface == dev2.MI)
            {
                Found = true;
            }
            return Found;
        }

        async void OnDeviceRemoved()
        {
            //Return immediately and do all the work asynchronously
            await Task.Yield();
            //Raise event if there are any subscribers
            RaiseDeviceRemovedEvent?.Invoke(this, null);
        }

        async void OnDeviceAdded()
        {
            // Return immediately and do all the work asynchronously
            await Task.Yield();
            // Raise event if there are any subscribers
            RaiseDeviceAddedEvent?.Invoke(this, null);


        }

        public static void QueryDeviceCapabilities(ref DeviceInformationStructure deviceInformation)
        {
            var preparsedData = new IntPtr();

            try
            {
                // Get the preparsed data from the HID driver
                Hid.HidD_GetPreparsedData(deviceInformation.HidHandle, ref preparsedData);

                // Get the HID device's capabilities
                var result = Hid.HidP_GetCaps(preparsedData, ref deviceInformation.Capabilities);
                if ((result == 0)) return;
            
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() -> Device query results:");
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Usage: {0}",
                                              Convert.ToString(deviceInformation.Capabilities.Usage, 16));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Usage Page: {0}",
                                              Convert.ToString(deviceInformation.Capabilities.UsagePage, 16));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Input Report Byte Length: {0}",
                                              deviceInformation.Capabilities.InputReportByteLength.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Output Report Byte Length: {0}",
                                              deviceInformation.Capabilities.OutputReportByteLength.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Feature Report Byte Length: {0}",
                                              deviceInformation.Capabilities.FeatureReportByteLength.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Number of Link Collection Nodes: {0}",
                                              deviceInformation.Capabilities.NumberLinkCollectionNodes.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Number of Input Button Caps: {0}",
                                              deviceInformation.Capabilities.NumberInputButtonCaps.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Number of Input Value Caps: {0}",
                                              deviceInformation.Capabilities.NumberInputValueCaps.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Number of Input Data Indices: {0}",
                                              deviceInformation.Capabilities.NumberInputDataIndices.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Number of Output Button Caps: {0}",
                                              deviceInformation.Capabilities.NumberOutputButtonCaps.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Number of Output Value Caps: {0}",
                                              deviceInformation.Capabilities.NumberOutputValueCaps.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Number of Output Data Indices: {0}",
                                              deviceInformation.Capabilities.NumberOutputDataIndices.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Number of Feature Button Caps: {0}",
                                              deviceInformation.Capabilities.NumberFeatureButtonCaps.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Number of Feature Value Caps: {0}",
                                              deviceInformation.Capabilities.NumberFeatureValueCaps.ToString(CultureInfo.InvariantCulture));
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() ->     Number of Feature Data Indices: {0}",
                                              deviceInformation.Capabilities.NumberFeatureDataIndices.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception)
            {
                // Something went badly wrong... this shouldn't happen, so we throw an exception
                Debug.WriteLine("usbGenericHidCommunication:queryDeviceCapabilities() -> EXECEPTION: An unrecoverable error has occurred!");
                throw;
            }
            finally
            {
                // Free up the memory before finishing
                if (preparsedData != IntPtr.Zero)
                {
                    Hid.HidD_FreePreparsedData(preparsedData);
                }
            }
        }      
    }
}
