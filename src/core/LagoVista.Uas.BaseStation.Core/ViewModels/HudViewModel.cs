using System.Threading.Tasks;
using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Commanding;
using LagoVista.Uas.BaseStation.Core.ViewModels.Uas;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Interfaces;
using LagoVista.Uas.Core.MavLink;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class HudViewModel : AppViewModelBase, INavigationProvider
    {
        IConnectedUasManager _connectedUasManager;
        public HudViewModel(IConnectedUasManager connectedUasManager, INavigation navigation)
        {
            Navigation = navigation;
            _connectedUasManager = connectedUasManager;
            Connections = _connectedUasManager;
            LandingGear = new LandingGearViewModel(connectedUasManager);
            EditMissionCommand = new RelayCommand(()=> ViewModelNavigation.NavigateAsync<Missions.MissionPlannerViewModel>(this));
        }

        public async override Task InitAsync()
        {
            await Navigation.InitAsync();
            await base.InitAsync();
        }

        public LandingGearViewModel LandingGear { get; }

        public IConnectedUasManager Connections { get; }

        public INavigation Navigation { get; }

        public RelayCommand EditMissionCommand { get; }
    }
}
