using LagoVista.Core.Commanding;
using LagoVista.Uas.BaseStation.Core.ViewModels.Uas;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Controller;
using LagoVista.Uas.Core.FlightRecorder;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels
{
    public class FlightViewModel : BaseViewModel
    {
        private readonly IConnectedUasManager _connectedUasManager;

        public FlightViewModel(IConnectedUasManager connectedUasManager, INavigation navigation, IFlightRecorder flightRecorder, INiVekFlightStick flightStick)
        {
            Navigation = navigation;
            _connectedUasManager = connectedUasManager;
            Connections = _connectedUasManager;
            LandingGear = new LandingGearViewModel(connectedUasManager);
            EditMissionCommand = new RelayCommand(() => ViewModelNavigation.NavigateAsync<Missions.MissionPlannerViewModel>(this));
            UasMgr = connectedUasManager;
            FlightRecorder = flightRecorder;
            FlightStickState = flightStick.State;
            flightStick.StateUpdated += FlightStick_StateUpdated;
        }

        private void FlightStick_StateUpdated(object sender, INiVekFlightStickState e)
        {
            RaisePropertyChanged(nameof(FlightStickState));
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

        public IConnectedUasManager UasMgr { get; }

        public RelayCommand EditMissionCommand { get; }


        public INiVekFlightStickState FlightStickState { get; }

        public void RaiseIt()
        {
            this.RaisePropertyChanged(nameof(FlightStickState));
        }
    }
}
