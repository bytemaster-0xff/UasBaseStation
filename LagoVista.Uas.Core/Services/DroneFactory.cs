using LagoVista.IoT.DeviceManagement.Core.Models;
using System;

namespace LagoVista.Uas.Core
{
    public class DroneFactory : IDroneFactory
    {
        public IUas CreateDrone(Device device)
        {
            throw new NotImplementedException();
        }

        public void RegisterDroneType(string droneDeviceTypeId, Type droneType)
        {
            throw new NotImplementedException();
        }
    }
}
