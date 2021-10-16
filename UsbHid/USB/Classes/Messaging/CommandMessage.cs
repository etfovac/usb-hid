using System;

namespace UsbHid.USB.Classes.Messaging
{
    public class CommandMessage : IMesage
    {
        private readonly byte _length;
        private byte[] _parameters;
        public byte[] MessageData { get { return GetMessageData(); } }

        private byte[] GetMessageData()
        {
            var result = new byte[_length + 1];
            result[0] = 0;
            result[1] = Command;
            if (Parameters != null)
            {
                Array.Copy(Parameters, 0 ,result, 2, Parameters.Length);
            }
            return result;
        }

        public byte Command { get; set; }
    
        public byte[] Parameters
        {
            get { return _parameters; }
            set
            {
                if (value == null) return;
                if (value.Length < 1) throw new ArgumentOutOfRangeException("value", "Parameter needs to be at least 1 byte long");
                if (value.Length > (_length -1) ) throw new ArgumentOutOfRangeException("value", string.Format("Parameter cannot be longer than {0} bytes", (_length - 1)));
                _parameters = value;
            }
        }

        //public CommandMessage( byte length) : this(length, 0x00 , null) { }

        public CommandMessage( byte length ,byte command ) : this(length, command, null){}

        public CommandMessage( byte length, byte command, byte[] parameters) 
        {
            _length = length;
            Command = command;
            Parameters = parameters;
        }

    }
}
