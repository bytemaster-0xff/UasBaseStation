using LagoVista.Client.Core.ViewModels;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class HudViewModel : AppViewModelBase
    {
        IConnectedUasManager _connectedUasManager;
        public HudViewModel(IConnectedUasManager connectedUasManager, INavigation navigation)
        {
            Navigation = navigation;
            _connectedUasManager = connectedUasManager;
            Connections = _connectedUasManager;
        }

        public void Arm()
        {
          //  var cmd = UasCommands.ArmAuthorizationRequest(Connections.Active.Uas.SystemId, Connections.Active.Uas.ComponentId, 0);
            var cmd = UasCommands.ComponentArmDisarm(Connections.Active.Uas.SystemId, Connections.Active.Uas.ComponentId, 1, 1);
            _connectedUasManager.Active.Transport.SendMessage(cmd);
        }

        public void Disarm()
        {
            //  var cmd = UasCommands.ArmAuthorizationRequest(Connections.Active.Uas.SystemId, Connections.Active.Uas.ComponentId, 0);
            var cmd = UasCommands.ComponentArmDisarm(Connections.Active.Uas.SystemId, Connections.Active.Uas.ComponentId, 0, 1);
            _connectedUasManager.Active.Transport.SendMessage(cmd);
        }

        public IConnectedUasManager Connections { get; }

        public INavigation Navigation { get; }


    }
}
