﻿using LagoVista.Client.Core;
using LagoVista.DroneBaseStation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.BaseStation
{
    public class ClientAppInfo : IClientAppInfo
    {
        public Type MainViewModel => typeof(MainViewModel);
    }

}
