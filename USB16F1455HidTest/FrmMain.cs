using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UsbHid;
using UsbHid.USB.Classes;
using UsbHid.USB.Classes.Messaging;

namespace USB16F1455HidTest
{
    public partial class FrmMain : Form
    {
        private const int VendorId = 0x0483;
        private const int ProductId = 0x5712;
        private const byte CommandGetStatus = 0xAA;
        private const byte CommandToggleLed = 0x81;

        public UsbHidDevice Device; 

        public bool IsButtonDown
        {
            get { return PbDn.Visible; }
            set
            {
                if (PbDn.Visible == value) return;
                ThreadSafe(() => PbDn.Visible = value);
                ThreadSafe(() => PbUp.Visible = !value);
            }
        }

        public bool IsLedOn 
        {
            get { return PbOn.Visible; }
            set
            {
                if (PbOn.Visible == value) return;
                ThreadSafe(() => PbOn.Visible = value);
                ThreadSafe(() => PbOff.Visible = !value);
            }
        }

        public FrmMain()
        {
            InitializeComponent();
            IsButtonDown = IsLedOn = BtnToggleLed.Enabled = TbxReseived.Enabled = TbxSent.Enabled = false;
        }

        private void FrmMainLoad(object sender, EventArgs e)
        {
            Device = new UsbHidDevice(VendorId, ProductId, 0x00, "HID-compliant device");//0x01, "HID-compliant game controller");//0x00, "HID-compliant device");
            Device.OnConnected += DeviceOnConnected;
            Device.OnDisconnected += DeviceOnDisConnected;
            Device.DataReceived += DeviceDataReceived;
            Device.Connect();
        }

        private void DeviceDataReceived(object sender, EventArgs<byte[]> e)
        {
            AppendText(ByteArrayToString(e.Value), true);
            ProcessReceivedData(e.Value);
        }

        private void ProcessReceivedData(IList<byte> value)
        {
            // Incoming Data cannot be Null, if it is the we are in WTF stage.
            if(value == null) return;

            // Incoming Data Packet cannot be shorter than 8 bytes, See Usb Documentation for details
            if (value.Count < 8) return;

            // Use Second Byte to see What the device wants
            // Byte[0] is always 0 -> Ignore
            if (value[1] == 0x80) 
            {
                ProcessNewStatus(value);
            }
        }

        private void ProcessNewStatus(IList<byte> value)
        {
            // Use Third Byte for Button Status
            IsButtonDown = value[2] > 0;

            // Use Fourth Byte For Led Status
            IsLedOn = value[3] > 0;
        }

        private void DeviceOnDisConnected(object sender, EventArgs e)
        {
            IsLedOn = false;
            IsButtonDown = false;
            SetFormEnabled(false);
        }

        private void DeviceOnConnected(object sender, EventArgs e)
        {
            SetFormEnabled(true);
            //GetStatus();
        }

        private void SetFormEnabled(bool isEnabled)
        {
            ThreadSafe(() => BtnToggleLed.Enabled = isEnabled);
            ThreadSafe(() => BtnGetstatus.Enabled = isEnabled);
            ThreadSafe(() => BtnClear.Enabled = isEnabled);
            ThreadSafe(() => TbxSent.Enabled = isEnabled);
            ThreadSafe(() => TbxReseived.Enabled = isEnabled);
        }


        private void ThreadSafe(MethodInvoker method)
        {
            if (InvokeRequired)
                Invoke(method);
            else
                method();
        }

        private void AppendText(string p, bool received)
        {
            if(received)
                ThreadSafe(() => TbxReseived.AppendText(p + Environment.NewLine));
            else
                ThreadSafe(() => TbxSent.AppendText(p + Environment.NewLine));
        }

        private static string ByteArrayToString(ICollection<byte> input)
        {
            var result = string.Empty;

            if (input == null || input.Count <= 0) return result;
            var isFirst = true;
            foreach (var b in input)
            {
                result += isFirst ? string.Empty : " ";
                result += b.ToString("X2");
                isFirst = false;
            }
            return result;
        }

        private void BtnGetstatusClick(object sender, EventArgs e)
        {
            GetStatus();
        }

        private void BtnToggleLedClick(object sender, EventArgs e)
        {
            ToggleLed();
        }

        private void GetStatus()
        {
            if (!Device.IsDeviceConnected) return;
            //var command = new CommandMessage(Device.InputReportSize, CommandGetStatus, null);
            byte[] enable = { 0x01 };
            var command = new CommandMessage(Device.InputReportSize, CommandGetStatus, enable);
            Device.SendMessage(command);
            AppendText(ByteArrayToString(command.MessageData), false);
        }

        private void ToggleLed()
        {
            if (!Device.IsDeviceConnected) return;
            var command = new CommandMessage(Device.InputReportSize, CommandToggleLed, null);
            Device.SendMessage(command);
            AppendText(ByteArrayToString(command.MessageData), false);
        }

        private void BtnClearClick(object sender, EventArgs e)
        {
            TbxSent.Text = string.Empty;
            TbxReseived.Text = string.Empty;
        }

    }
}
