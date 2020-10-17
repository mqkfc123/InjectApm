using System;
using CInject.Engine.Data;

namespace CInject.Engine.Resolvers
{
    public abstract class BaseAssemblyResolver
    {
        private string _path;
        
        public string Path
        {
            get { return _path; }
            protected set { _path = value; }
        }

        public BaseAssemblyResolver(string path)
        {
            _path = path;
        }

        public event EventHandler<MessageEventArgs> OnMessageReceived;

        public void SendMessage(string message, MessageType messageType)
        {
            if (OnMessageReceived != null)
            {
                OnMessageReceived(this, new MessageEventArgs
                                            {
                                                MessageType = messageType,
                                                Message = message
                                            });
            }
        }
    }
}