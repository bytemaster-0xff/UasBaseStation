using LagoVista.Uas.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.UI.Core;

namespace LagoVista.Uas.BaseStation.ControlApp.Controller
{
    public class FlightStickService : IDisposable
    {
        Gamepad _xboxController;
        RawGameController _tmFlightStick;
        RawGameController _throttle;

        private readonly NiVekFlightStick _flightStick;
        private readonly CoreDispatcher _dispatcher;
        private readonly INavigation _navigation;
        private readonly Timer _timer;

        public FlightStickService(NiVekFlightStick stick, CoreDispatcher dispatcher, INavigation navigation)
        {
            this._flightStick = stick ?? throw new ArgumentNullException(nameof(stick));
            this._navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            this._dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));

            _timer = new Timer(Timer_callBack, null, 100, 100);

            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            RawGameController.RawGameControllerAdded += RawGameController_RawGameControllerAdded;
        }

        public async void RunOnUIThread(Action action)
        {
            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                action();
            });
        }

        private void Timer_callBack(object obj)
        {
            if (_flightStick != null)
            {
                if (_tmFlightStick != null)
                {
                    var changed = _flightStick.RefreshFromThrustMaster1600(_tmFlightStick);

                    if (_throttle != null)
                    {
                        changed |= _flightStick.RefreshFromThrustMasterThrottle(_throttle);
                    }

                    if (changed)
                    {
                        RunOnUIThread(() => _flightStick.InvokeStateUpdated());
                    }
                }
                else if (_xboxController != null)
                {
                    _flightStick.RefreshFromXBox(_xboxController);
                }

                if (_flightStick.State.IsConnected)
                {
                    _navigation.SetVirtualJoystick(_flightStick.State.Throttle, _flightStick.State.Pitch, Convert.ToInt16(_flightStick.State.Rudder), _flightStick.State.Roll);
                }
            }
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

        private void Gamepad_GamepadAdded(object sender, Windows.Gaming.Input.Gamepad e)
        {
            this._xboxController = e;
        }

        public void Dispose()
        {
            this._timer.Dispose();
        }
    }
}
