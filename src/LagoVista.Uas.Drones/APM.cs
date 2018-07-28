using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.Uas.Core;
using System;

namespace LagoVista.Uas.Drones
{
    public class APM : UasBase, IUas
    {
        public APM(Device device) : base(device)
        {
        }
    }
}
