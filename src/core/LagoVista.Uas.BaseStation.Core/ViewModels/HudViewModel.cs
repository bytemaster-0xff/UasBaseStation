using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Commanding;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class HudViewModel : AppViewModelBase
    {
        IConnectedUasManager _connectedUasManager;
        public HudViewModel(IConnectedUasManager connectedUasManager)
        {
            ArmCommand = new RelayCommand(Arm);
            _connectedUasManager = connectedUasManager;
            Connections = _connectedUasManager;
        }

        public void Arm()
        {
          //  var cmd = UasCommands.ArmAuthorizationRequest(Connections.Active.Uas.SystemId, Connections.Active.Uas.ComponentId, 0);
            var cmd = UasCommands.ComponentArmDisarm(Connections.Active.Uas.SystemId, Connections.Active.Uas.ComponentId, 1, 1);
            _connectedUasManager.Active.Transport.SendMessage(cmd);
        }

        public IConnectedUasManager Connections { get; }

        public RelayCommand ArmCommand { get; }

    }
}
