using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core.Interfaces
{
    public interface IDeviceRepo
    {
        Task RegisterDeviceTypeAsync(EntityHeader deviceType);
        Task<EntityHeader> GetRegisteredDeviceTypesAsync();
        Task UnregisterDeviceTypeAsync(EntityHeader deviceType);

        Task<InvokeResult<Device>> GetDeviceAsync(string deviceId, string userId, string orgId);
        Task<InvokeResult<DeviceSummary>> GetDeviceSummariesAsync(string userId, string orgId);
    }
}
