using LagoVista.IoT.DeviceManagement.Core.Models;
using System;

namespace LagoVista.Uas.Core
{
    public interface IDroneFactory
    {
        void RegisterDroneType(String droneDeviceTypeId, Type droneType);
        IUas CreateDrone(Device device);
        IUas GetDrone(string droneDeviceId);
    }
}
