using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UsbHid.Addendum
{
    public class SingleDevice
    {
        public ManagementObject MngObj { get; private set; }
        public readonly int Availability;
        public readonly string Caption;
        public readonly string ClassGuid;
        public readonly string[] CompatibleID;
        public readonly int ConfigManagerErrorCode;
        public readonly bool ConfigManagerUserConfig;
        public readonly string CreationClassName;
        public readonly string Description;
        public readonly string DeviceID;
        public readonly bool ErrorCleared;
        public readonly string ErrorDescription;
        public readonly string[] HardwareID;
        public readonly DateTime InstallDate;
        public readonly int LastErrorCode;
        public readonly string Manufacturer;
        public readonly string Name;
        public readonly string PNPClass;
        public readonly string PNPDeviceID;
        public readonly int[] PowerManagementCapabilities;
        public readonly bool PowerManagementSupported;
        public readonly bool Present;
        public readonly string Service;
        public readonly string Status;
        public readonly int StatusInfo;
        public readonly string SystemCreationClassName;
        public readonly string SystemName;

        public SingleDevice(ManagementObject property)
        {
            MngObj = property;
            Availability = property.GetPropertyValue("Availability") as int? ?? 0;
            Caption = property.GetPropertyValue("Caption") as string ?? string.Empty;
            ClassGuid = property.GetPropertyValue("ClassGuid") as string ?? string.Empty;
            CompatibleID = property.GetPropertyValue("CompatibleID") as string[] ?? new string[] { };
            ConfigManagerErrorCode = property.GetPropertyValue("ConfigManagerErrorCode") as int? ?? 0;
            ConfigManagerUserConfig = property.GetPropertyValue("ConfigManagerUserConfig") as bool? ?? false;
            CreationClassName = property.GetPropertyValue("CreationClassName") as string ?? string.Empty;
            Description = property.GetPropertyValue("Description") as string ?? string.Empty;
            DeviceID = property.GetPropertyValue("DeviceID") as string ?? string.Empty;
            ErrorCleared = property.GetPropertyValue("ErrorCleared") as bool? ?? false;
            ErrorDescription = property.GetPropertyValue("ErrorDescription") as string ?? string.Empty;
            HardwareID = property.GetPropertyValue("HardwareID") as string[] ?? new string[] { };
            InstallDate = property.GetPropertyValue("InstallDate") as DateTime? ?? DateTime.MinValue;
            LastErrorCode = property.GetPropertyValue("LastErrorCode") as int? ?? 0;
            Manufacturer = property.GetPropertyValue("Manufacturer") as string ?? string.Empty;
            Name = property.GetPropertyValue("Name") as string ?? string.Empty;
            PNPClass = property.GetPropertyValue("PNPClass") as string ?? string.Empty;
            PNPDeviceID = property.GetPropertyValue("PNPDeviceID") as string ?? string.Empty;
            PowerManagementCapabilities = property.GetPropertyValue("PowerManagementCapabilities") as int[] ?? new int[] { };
            PowerManagementSupported = property.GetPropertyValue("PowerManagementSupported") as bool? ?? false;
            Present = property.GetPropertyValue("Present") as bool? ?? false;
            Service = property.GetPropertyValue("Service") as string ?? string.Empty;
            Status = property.GetPropertyValue("Status") as string ?? string.Empty;
            StatusInfo = property.GetPropertyValue("StatusInfo") as int? ?? 0;
            SystemCreationClassName = property.GetPropertyValue("SystemCreationClassName") as string ?? string.Empty;
            SystemName = property.GetPropertyValue("SystemName") as string ?? string.Empty;
        }
    }
}
