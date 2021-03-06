﻿using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Geo;
using LagoVista.Core.IOC;
using LagoVista.Uas.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class BaseViewModel : AppViewModelBase
    {
        public IGeoLocator LocationProvider
        {
            get => SLWIOC.Get<IGeoLocator>();
        }

        public override async Task InitAsync()
        {
            await LocationProvider.InitAsync();
            await base.InitAsync();
        }
    }
}
