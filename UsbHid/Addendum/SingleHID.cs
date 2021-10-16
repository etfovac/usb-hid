using System.Text.RegularExpressions;

namespace UsbHid.Addendum
{
    public class SingleHID
	{
        public string DeviceID { get; private set; }
        public ushort Vid { get; private set; }
        public ushort Pid { get; private set; }
        public ushort MI { get; private set; }
        public string Caption { get; private set; }
        public string Manufacturer { get; private set; }

        public SingleHID(SingleDevice singleDevice)
		{
            this.DeviceID = singleDevice.DeviceID;
            this.Caption = singleDevice.Caption;
            this.Manufacturer = singleDevice.Manufacturer;

            Match match = Regex.Match(this.DeviceID, "VID_(.{4})", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string vid_string = match.Groups[1].Value;
                Vid = ushort.Parse(vid_string, System.Globalization.NumberStyles.HexNumber);
            }
            match = Regex.Match(this.DeviceID, "PID_(.{4})", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string pid_string = match.Groups[1].Value;
                Pid = ushort.Parse(pid_string, System.Globalization.NumberStyles.HexNumber);
            }
            match = Regex.Match(this.DeviceID, "MI_(.{2})", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string mi_string = match.Groups[1].Value;
                MI = ushort.Parse(mi_string, System.Globalization.NumberStyles.HexNumber);
            }
        }
	}
}
