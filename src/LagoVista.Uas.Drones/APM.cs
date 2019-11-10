using LagoVista.Core.Models;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using System;

namespace LagoVista.Uas.Drones
{
    public class APM : UasBase
    {
        private readonly SerialPortTransport _traansport;

        public APM(SerialPortTransport transport, Device device) : base(device)
        {
            this.SetAdapter(new APMAdapter());

            this._traansport = transport ?? throw new ArgumentNullException(nameof(transport));

            this._traansport.OnMessageReceived += _serialPort_OnMessageReceived;
        }

        private void _serialPort_OnMessageReceived(object sender, Core.Models.UasMessage e)
        {
            this.Update(e);
        }
    }
}
   