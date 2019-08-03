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
        Gamepad _xboxController;
        Controller.NiVekFlightStick _flightStick = new Controller.NiVekFlightStick();
        HudViewModel _hudViewModel;

        public MainPage()
        {
            this.InitializeComponent();
            _timer = new Timer(Timer_callBack, null, 50, 50);
            _flightStick.ButtonStateChanged += _gamePad_ButtonStateChanged;
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
            _navigation = new LagoVista.Uas.Core.Services.Navigation(_uasMgr, missionPlanner);

            AOAControl.GetDevices();

            _hudViewModel = new HudViewModel(_uasMgr, _navigation);
            DataContext = _hudViewModel;

            NotifyPropertyChanged(nameof(ViewModel));

            Windows.Gaming.Input.Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Windows.Gaming.Input.FlightStick.FlightStickAdded += FlightStick_FlightStickAdded;
        }


        private void _gamePad_ButtonStateChanged(object sender, Controller.NiVekFlightStick.ButtonStateChangedEventArgs e)
        {
            switch(e.Button)
            {
                case Controller.NiVekFlightStick.GamePadButtons.A:
                    _navigation.Takeoff();
                    break;
                case Controller.NiVekFlightStick.GamePadButtons.B:
                    _navigation.Land();
                    break;
            }
        }

        private void Timer_callBack(object obj)
        {
            if (_flightStick != null && _xboxController != null)
            {
                _flightStick.Refresh(_xboxController);
                if (_flightStick.State.IsConnected)
                {
                    _navigation.SetVirtualJoystick(_flightStick.State.LeftY, _flightStick.State.RightY, _flightStick.State.LeftX, _flightStick.State.RightX);
                    RunOnUIThread(() => _hudViewModel.FlightStickState = _flightStick.State);
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

        private void FlightStick_FlightStickAdded(object sender, Windows.Gaming.Input.FlightStick e)
        {
            var fs = e;
        }

        private void Gamepad_GamepadAdded(object sender, Windows.Gaming.Input.Gamepad e)
        {
            this._xboxController = e;
        }

        public HudViewModel ViewModel { get; private set; }
    }
}
