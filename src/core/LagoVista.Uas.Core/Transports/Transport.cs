using LagoVista.Core.ServiceCommon;
using LagoVista.Core.Validation;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core.MavLink
{
    public abstract class Transport : ServiceBase, IDisposable, ITransport
    {
        public byte MavlinkSystemId = 200;
        public byte MavlinkComponentId = 1;
        public MavLinkState UavState = new MavLinkState();

        public event EventHandler OnReceptionEnded;
        public event EventHandler<UasMessage> OnPacketReceived;

        public abstract void Initialize();
        public abstract void Dispose();
        public abstract void SendMessage(UasMessage msg);

        protected abstract void HandlePacketReceived(object sender, MavLinkPacket e);
        protected abstract void HandleReceptionEnded(object sender);

        public Task<InvokeResult<TMavlinkPacket>> WaitForMessageAsync<TMavlinkPacket>(short messageId, TimeSpan timeout) where TMavlinkPacket : class
        {
            throw new NotImplementedException();
        }

        public Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(IUas drone, int outgoingMessageId, object req, int incomingMessageId, TimeSpan timeout) where TMavlinkPacket : class
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<UasMessage> Messages { get; } = new ObservableCollection<UasMessage>();


        private int _messagesReceived = 0;
        public int MessagesReceived
        {
            get { return _messagesReceived; }
            private set { Set(ref _messagesReceived, value); }
        }

        private int _errors;
        public int Errors
        {
            get { return _errors; }
            private set { Set(ref _errors, value); }
        }

        private long _bytesReceived = 0;
        public long BytesReceived
        {
            get { return _bytesReceived; }
            private set { Set(ref _bytesReceived, value); }
        }

        private bool _isConnected = false;
        public bool IsConected
        {
            get { return _isConnected; }
            set { Set(ref _isConnected, value); }
        }
    }
}
