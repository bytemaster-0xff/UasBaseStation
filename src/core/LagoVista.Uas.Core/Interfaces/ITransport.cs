using LagoVista.Core.Validation;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core
{
    public interface ITransport
    {
        event EventHandler<UasMessage> OnPacketReceived;
        event EventHandler OnReceptionEnded;

        void Dispose();
        void Initialize();
        void SendMessage(UasMessage msg);

        Task<InvokeResult<TMavlinkPacket>> WaitForMessageAsync<TMavlinkPacket>(short messageId, TimeSpan timeout) where TMavlinkPacket : class;

        Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(IUas drone, int outgoingMessageId, Object req, int incomingMessageId, TimeSpan timeout) where TMavlinkPacket : class;

        bool IsConected { get; }
        ObservableCollection<UasMessage> Messages { get; } 
        int MessagesReceived { get; }
        int Errors { get; }
        long BytesReceived { get; }
    }
}
