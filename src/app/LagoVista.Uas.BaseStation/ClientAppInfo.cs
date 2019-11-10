using LagoVista.Client.Core;
using LagoVista.Client.Core.ViewModels;
using LagoVista.Uas.BaseStation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.BaseStation
{
    public class ClientAppInfo : IClientAppInfo
    {
        public Type MainViewModel => typeof(DroneConnectViewModel);
    }

}
