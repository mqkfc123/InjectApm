using System;

namespace CInject.Engine.Data
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }

        public MessageType MessageType { get; set; }
    }
}