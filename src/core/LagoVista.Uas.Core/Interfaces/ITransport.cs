using LagoVista.Core.Validation;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core
{
    public interface ITransport
    {
        event EventHandler<MavLinkPacket> OnPacketReceived;
        event EventHandler OnReceptionEnded;

        void Dispose();
        void Initialize();
        void SendMessage(UasMessage msg);

        Task<InvokeResult<TMavlinkPacket>> WaitForMessageAsync<TMavlinkPacket>(UasMessages messageId, TimeSpan timeout) where TMavlinkPacket : class;

        Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(UasMessage msg, UasMessages incomingMessageId, TimeSpan timeout) where TMavlinkPacket : class;
        Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(UasMessage msg, UasMessages incomingMessageId) where TMavlinkPacket : class;

        bool IsConnected { get; }
        ObservableCollection<MavLinkPacket> Messages { get; } 
        int MessagesReceived { get; }
        int Errors { get; }
        long BytesReceived { get; }
    }
}
