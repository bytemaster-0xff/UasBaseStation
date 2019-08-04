using System.Threading.Tasks;
using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Commanding;
using LagoVista.Uas.BaseStation.Core.ViewModels.Uas;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Controller;
using LagoVista.Uas.Core.FlightRecorder;
using LagoVista.Uas.Core.Interfaces;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class HudViewModel : AppViewModelBase, INavigationProvider
    {
        private readonly IConnectedUasManager _connectedUasManager;

        public HudViewModel(IConnectedUasManager connectedUasManager, INavigation navigation, IFlightRecorder flightRecorder)
        {
            Navigation = navigation;
            _connectedUasManager = connectedUasManager;
            Connections = _connectedUasManager;
            LandingGear = new LandingGearViewModel(connectedUasManager);
            EditMissionCommand = new RelayCommand(()=> ViewModelNavigation.NavigateAsync<Missions.MissionPlannerViewModel>(this));
            UasMgr = connectedUasManager;
            FlightRecorder = flightRecorder;
        }

        public async override Task InitAsync()
        {
            await Navigation.InitAsync();
            await base.InitAsync();
        }

        public LandingGearViewModel LandingGear { get; }

        public IConnectedUasManager Connections { get; }

        public INavigation Navigation { get; }
        public IFlightRecorder FlightRecorder { get; }

        public IConnectedUasManager UasMgr { get;  }

        public RelayCommand EditMissionCommand { get; }

        INiVekFlightStickState _flightStickState;
        public INiVekFlightStickState FlightStickState
        {
            get => _flightStickState;
            set => Set(ref _flightStickState, value);
        }
    }
}
