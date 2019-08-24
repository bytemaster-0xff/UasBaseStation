using LagoVista.Core.Validation;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.ControlApp.Drones
{
    public class TelloTransport : ITransport
    {
        TelloDrone _drone;

        public TelloTransport(TelloDrone drone)
        {
            _drone = drone;
            Messages = new ObservableCollection<MavLinkPacket>();
        }

        public bool IsConnected => throw new NotImplementedException();

        public ObservableCollection<MavLinkPacket> Messages { get; }

        public int MessagesReceived { get; private set; }

        public int Errors { get; private set; }

        public long BytesReceived { get; private set; }

        public event EventHandler<MavLinkPacket> OnPacketReceived;
        public event EventHandler<UasMessage> OnMessageReceived;
        public event EventHandler OnReceptionEnded;

        public void Dispose()
        {

        }

        public void Initialize()
        {

        }

        private void StateObserver_StateChanged(object sender, Tello.Events.StateChangedArgs e)
        {

        }

        public Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(UasMessage msg, UasMessages incomingMessageId, TimeSpan timeout) where TMavlinkPacket : class
        {
            throw new NotImplementedException();
        }

        public Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(UasMessage msg, UasMessages incomingMessageId) where TMavlinkPacket : class
        {
            throw new NotImplementedException();
        }

        int _lastX;
        int _lastY;
        int _lastZ;
        int _lastR;
        
        public void SendMessage(UasMessage msg)
        {
            switch (msg.MessageId)
            {
                case UasMessages.CommandLong:
                    {
                        var cmd = msg as UasCommandLong;
                        switch (cmd.Command)
                        {
                            case 22:
                                _drone.Tello.Controller.TakeOff();
                                break;

                            case 23:
                                _drone.Tello.Controller.Land();
                                break;
                        }
                    }
                    break;
                case UasMessages.GimbalControl:

                    break;
                case UasMessages.ManualControl:
                    {
                        var cmd = msg as UasManualControl;

                        var x = cmd.R / 10;
                        var y = cmd.X / 10;
                        var r = cmd.Y / 10;
                        var z = cmd.Z / 10;
                        

                        if (x != _lastX || y != _lastY || z != _lastZ || r != _lastR)
                        {
                            _drone.Tello.Controller.Set4ChannelRC(x, y, z, r);
                            _lastX = x;
                            _lastY = y;
                            _lastZ = z;
                            _lastR = r;
                        }
                    }
                    break;
            }
        }

        public Task<InvokeResult<TMavlinkPacket>> WaitForMessageAsync<TMavlinkPacket>(UasMessages messageId, TimeSpan timeout) where TMavlinkPacket : class
        {
            throw new NotImplementedException();
        }
    }
}
