using System;

namespace UsbHid.USB.Classes
{
    public class EventArgs<T> : EventArgs
    {
        public T Value { get; set; }
        public EventArgs(T value)
        {
            Value = value;
        }
    }
}
