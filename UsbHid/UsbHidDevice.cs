using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using UsbHid.USB.Classes;
using UsbHid.USB.Classes.Messaging;
using UsbHid.USB.Structures;

namespace UsbHid
{
    public class UsbHidDevice : IDisposable
    {
        #region Variables

        private DeviceInformationStructure _deviceInformation;
        public int VID { get { return _deviceInformation.TargetVendorId; } }
        public int PID { get { return _deviceInformation.TargetProductId; } }
        public int MI { get { return _deviceInformation.TargetMultiInterface; } }
        public List<string> DevicePaths { get { return _deviceInformation.Paths; } } // in case more same devices are attached
        public string DevicePath { get { return _deviceInformation.DevicePathName; } }
        public bool IsDeviceConnected { get { return _deviceInformation.IsDeviceAttached; } }
        public byte InputReportSize { get { return (byte) (_deviceInformation.Capabilities.InputReportByteLength -1 ); } }
        public byte OutputReportSize { get { return (byte)(_deviceInformation.Capabilities.OutputReportByteLength - 1); } }
        private readonly BackgroundWorker _worker;
        private FileStream _fsDeviceRead;

        #endregion

        #region Events

        public event EventHandler<EventArgs<byte[]>> DataReceived;
        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;

        #endregion

        #region Constructor

        public UsbHidDevice(int vendorId, int productId)
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += WorkerDoWork;
            _deviceInformation.TargetVendorId = vendorId;
            _deviceInformation.TargetProductId = productId;
            _deviceInformation.ConnectedChanged += DeviceConnectedChanged;
            DeviceChangeNotifier.DeviceAttached += DeviceChangeNotifierDeviceAttached;
            DeviceChangeNotifier.DeviceDetached += DeviceChangeNotifierDeviceDetached;
        }

        public UsbHidDevice(int vendorId, int productId, int mInterface, string caption)
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += WorkerDoWork;
            _deviceInformation.TargetVendorId = vendorId;
            _deviceInformation.TargetProductId = productId;
            _deviceInformation.TargetMultiInterface = mInterface;
            _deviceInformation.TargetCaption = caption;
            _deviceInformation.ConnectedChanged += DeviceConnectedChanged;
            DeviceChangeNotifier.DeviceAttached += DeviceChangeNotifierDeviceAttached;
            DeviceChangeNotifier.DeviceDetached += DeviceChangeNotifierDeviceDetached;
        }

        ~UsbHidDevice()
        {
            Disconnect();
        }

        #endregion

        #region Event Handlers

        private void ReadCompleted(IAsyncResult iResult)
        {
            // Retrieve the stream and read buffer.
            var syncObj = (SyncObjT)iResult.AsyncState;
            try
            {
                // call end read : this throws any exceptions that happened during the read
                syncObj.Fs.EndRead(iResult);
                try
                {
                    if (DataReceived != null) DataReceived(this, new EventArgs<byte[]>(syncObj.Buf));
                }
                finally
                {
                    // when all that is done, kick off another read for the next report
                    BeginAsyncRead(ref syncObj.Fs, syncObj.Buf.Length);
                }
            }
            catch (IOException ex)	// if we got an IO exception, the device was removed
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void DeviceConnectedChanged(object sender, EventArgs<bool> e)
        {
            if (e.Value)
            {
                ReportConnected();
                _worker.RunWorkerAsync();
            }
            else
            {
                ReportDisConnected();
            }
        }

        private void DeviceChangeNotifierDeviceDetached(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void DeviceChangeNotifierDeviceAttached(object sender, EventArgs e)
        {
            if (IsDeviceConnected) Disconnect();
        }

        #endregion

        #region Methods

        #region Public 

        public bool Connect(string Path = "")
        {
            DeviceDiscovery.FindTargetDevices(ref _deviceInformation, Path);
            Debug.WriteLine("Connect() -> Method called");
            return IsDeviceConnected;
        }

        public void Disconnect()
        {
            Debug.WriteLine("usbGenericHidCommunication:detachUsbDevice() -> Method called");

            if (_fsDeviceRead != null)
            {
                _fsDeviceRead.Close();
            }
            
            // Is a device currently attached?
            if (IsDeviceConnected)
            {
                Debug.WriteLine("usbGenericHidCommunication:detachUsbDevice() -> Detaching device and closing file handles");
                // Close the readHandle, writeHandle and hidHandle
                if (!_deviceInformation.HidHandle.IsInvalid) _deviceInformation.HidHandle.Close();
                if (!_deviceInformation.ReadHandle.IsInvalid) _deviceInformation.ReadHandle.Close();
                if (!_deviceInformation.WriteHandle.IsInvalid) _deviceInformation.WriteHandle.Close();

                // Set the device status to detached;
                _deviceInformation.IsDeviceAttached = false;
            }
            else Debug.WriteLine("usbGenericHidCommunication:detachUsbDevice() -> No device attached");
        }

        public bool SendMessage(IMesage message)
        {
            return DeviceCommunication.WriteRawReportToDevice(message.MessageData, ref _deviceInformation);
        }

        public bool SendCommandMessage(byte command)
        {
            var message = new CommandMessage((byte)_deviceInformation.Capabilities.InputReportByteLength, command);
            return DeviceCommunication.WriteRawReportToDevice(message.MessageData, ref _deviceInformation);
        }

        #endregion

        #region Private

        private void BeginAsyncRead(ref FileStream fs, int iBufLen)
        {
            var syncObj = new SyncObjT { Fs = fs, Buf = new Byte[iBufLen] };
            try
            {
                fs.BeginRead(syncObj.Buf, 0, iBufLen, ReadCompleted, syncObj);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            _fsDeviceRead = new FileStream(_deviceInformation.ReadHandle, FileAccess.Read, 0x1000, true);
            BeginAsyncRead(ref _fsDeviceRead, _deviceInformation.Capabilities.InputReportByteLength);
        }

        private void ReportConnected()
        {
            OnConnected?.Invoke(this, null);
        }

        private void ReportDisConnected()
        {
            OnDisconnected?.Invoke(this, null);
        }
        
        #endregion

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
