using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace LagoVista.Uas.BaseStation.ControlApp
{
    public class UWPDeviceInfo : IDeviceInfo
    {
        public UWPDeviceInfo()
        {
            DeviceType = "PC or Tablet";

            var deviceInformation = new EasClientDeviceInformation();
            DeviceUniqueId = deviceInformation.Id.ToString();
        }

        public string DeviceUniqueId { get; }

        public string DeviceType { get; }
    }
}
