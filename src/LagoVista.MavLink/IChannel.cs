using LagoVista.Core.Validation;
using System;
using System.Threading.Tasks;

namespace MavLinkNet
{
    public interface IChannel
    {
        event PacketReceivedDelegate OnPacketReceived;
        event EventHandler OnReceptionEnded;

        void Dispose();
        void Initialize();
        void SendMessage(UasMessage msg);

        Task<InvokeResult<TMavlinkPacket>> WaitForMessageAsync<TMavlinkPacket>(MAVLINK_MSG_ID messageId, TimeSpan timeout) where TMavlinkPacket : struct;

        Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(IDrone drone, MAVLINK_MSG_ID outgoingMessageId, Object req, MAVLINK_MSG_ID incomingMessageId, TimeSpan timeout) where TMavlinkPacket : struct;
    }
}