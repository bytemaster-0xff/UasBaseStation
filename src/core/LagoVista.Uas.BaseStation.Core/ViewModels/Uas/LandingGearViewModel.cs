using LagoVista.Core.Commanding;
using LagoVista.Core.ViewModels;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.BaseStation.Core.ViewModels.Uas
{
    public class LandingGearViewModel : ViewModelBase
    {
        IConnectedUasManager _connectedUasManager;
        public LandingGearViewModel(IConnectedUasManager connectedUasManager)
        {
            _connectedUasManager = connectedUasManager;
            RaiseLandingGearCommand = new RelayCommand(RaiseLandingGear);
            LowerLandingGearCommand = new RelayCommand(LowerLandingGear);
        }


        public void RaiseLandingGear()
        {
            var msg = UasCommands.AirframeConfiguration(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, -1 /* All */, 1 /* Up */, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN);
            _connectedUasManager.Active.Transport.SendMessage(msg);
        }

        public void LowerLandingGear()
        {
            var msg = UasCommands.AirframeConfiguration(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, -1, 0 /* down */, float.NaN, float.NaN, float.NaN, float.NaN, float.NaN);
            _connectedUasManager.Active.Transport.SendMessage(msg);
        }



        public RelayCommand RaiseLandingGearCommand { get; }

        public RelayCommand LowerLandingGearCommand { get; }

    }
}
