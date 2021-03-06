﻿using LagoVista.Core.IOC;
using LagoVista.Uas.BaseStation.UWP.Drone;
using LagoVista.Uas.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LagoVista.Uas.BaseStation.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new LagoVista.Uas.BaseStation.App());
            var mgr = SLWIOC.Get<IConnectedUasManager>();

            var _djiDrone = new DJIDrone(mgr);
        }
    }
}
