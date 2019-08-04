using LagoVista.Core.IOC;
using System.Linq;
using LagoVista.Uas.BaseStation.Core.ViewModels;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Models;
using LagoVista.Uas.Core.Services;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.Gaming.Input;
using System;
using System.Diagnostics;
using LagoVista.Uas.Core.FlightRecorder;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LagoVista.Uas.BaseStation.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        Timer _timer;
        IConnectedUasManager _uasMgr;
        INavigation _navigation;
        IFlightRecorder _flightRecorder;
        Gamepad _xboxController;
        Controller.NiVekFlightStick _flightStick;
        HudViewModel _hudViewModel;

        RawGameController _tmFlightStick;
        RawGameController _throttle;

        public MainPage()
        {
            _flightStick = new Controller.NiVekFlightStick(Dispatcher);
            
            this.InitializeComponent();
            _timer = new Timer(Timer_callBack, null, 100, 100);

            _flightRecorder = SLWIOC.Get<IFlightRecorder>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _uasMgr = SLWIOC.Get<IConnectedUasManager>();
            var missionPlanner = new MissionPlanner(_uasMgr);
            _navigation = new LagoVista.Uas.Core.Services.Navigation(_uasMgr, missionPlanner, _flightRecorder);

            AOAControl.GetDevices();

            _hudViewModel = new HudViewModel(_uasMgr, _navigation, _flightRecorder);
            DataContext = _hudViewModel;

            NotifyPropertyChanged(nameof(ViewModel));

            _flightStick.StartMission += (s, a) => _navigation.StartMission();
            _flightStick.TakeOff += (s, a) => _navigation.Takeoff();
            _flightStick.Land += (s, a) => _navigation.Land();
            _flightStick.ReturnToHome += (s, a) => _navigation.ReturnToHome();
            _hudViewModel.FlightStickState = _flightStick.State;

            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            RawGameController.RawGameControllerAdded += RawGameController_RawGameControllerAdded;
        }

        private void RawGameController_RawGameControllerAdded(object sender, RawGameController e)
        {
            switch (e.HardwareVendorId)
            {
                case 0x44f:
                    switch (e.HardwareProductId)
                    {
                        case 0xB10A: _tmFlightStick = e; break;
                        case 0xB687: _throttle = e; break;
                    }
                    break;

            }
        }

        private void Timer_callBack(object obj)
        {
            if (_flightStick != null)
            {
                if (_tmFlightStick != null)
                {
                    _flightStick.RefreshFromThrustMaster1600(_tmFlightStick);

                    if (_throttle != null)
                    {
                        _flightStick.RefreshFromThrustMasterThrottle(_throttle);
                    }
                }
                else if (_xboxController != null)
                {
                    _flightStick.RefreshFromXBox(_xboxController);
                }

                if (_flightStick.State.IsConnected)
                {
                    _navigation.SetVirtualJoystick(_flightStick.State.Throttle, _flightStick.State.Pitch, _flightStick.State.Rudder, _flightStick.State.Roll);
                    //RunOnUIThread(() => _hudViewModel.FlightStickState = _flightStick.State);
                }
            }
        }

        public async void RunOnUIThread(Action action)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                action();
            });
        }

        private void Gamepad_GamepadAdded(object sender, Windows.Gaming.Input.Gamepad e)
        {
            this._xboxController = e;
        }

        public HudViewModel ViewModel { get; private set; }
    }
}
