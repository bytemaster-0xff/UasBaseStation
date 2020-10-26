using DJI.WindowsSDK;
using LagoVista.Core.Validation;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.ControlApp.Drones
{
    public class DJITransport : ITransport
    {
        private readonly DJIDrone _drone;

        public DJITransport(DJIDrone drone)
        {
            this._drone = drone;
            Messages = new ObservableCollection<MavLinkPacket>();
        }

        public bool IsConnected => true;

        public ObservableCollection<MavLinkPacket> Messages { get; }

        public int MessagesReceived => 0;

        public int Errors => 0;

        public long BytesReceived => 0;

        public event EventHandler<MavLinkPacket> OnPacketReceived;
        public event EventHandler<UasMessage> OnMessageReceived;
        public event EventHandler OnReceptionEnded;

        public void Dispose()
        {
        }

        public void Initialize()
        {
        }

        public async Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(UasMessage msg, UasMessages incomingMessageId, TimeSpan timeout) where TMavlinkPacket : class
        {
            await Task.Delay(1);
            return InvokeResult<TMavlinkPacket>.FromError("Uknown"); ;
        }

        public async Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(UasMessage msg, UasMessages incomingMessageId) where TMavlinkPacket : class
        {
            await Task.Delay(1);
            return InvokeResult<TMavlinkPacket>.FromError("Uknown");
        }

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
                                DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StartTakeoffAsync();
                                break;

                            case 23:
                                DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).StartAutoLandingAsync();
                                break;
                        }
                    }
                    break;
                case UasMessages.GimbalControl:
                    {
                        var cmd = msg as UasGimbalControl;
                        DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateByAngleAsync(new GimbalAngleRotation()
                        {

                        });
                    }

                    break;
                case UasMessages.ManualControl:
                    {
                        var cmd = msg as UasManualControl;
                        DJISDKManager.Instance.VirtualRemoteController.UpdateJoystickValue(cmd.Z / 1000.0f, cmd.Y / 1000.0f, cmd.X / 1000.0f, cmd.R / 1000.0f);
                    }
                    break;
            }
        }

        public async Task<InvokeResult<TMavlinkPacket>> WaitForMessageAsync<TMavlinkPacket>(UasMessages messageId, TimeSpan timeout) where TMavlinkPacket : class
        {
            await Task.Delay(1);
            return InvokeResult<TMavlinkPacket>.FromError("Uknown");
        }
    }
}
