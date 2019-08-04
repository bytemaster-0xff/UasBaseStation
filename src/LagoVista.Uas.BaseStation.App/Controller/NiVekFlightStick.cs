using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using LagoVista.Uas.Core.Controller;
using System.Diagnostics;
using Windows.UI.Core;

namespace LagoVista.Uas.BaseStation.ControlApp.Controller
{
    public class NiVekFlightStick : ModelBase
    {
        public class NiVekFlightStickState : ModelBase, INiVekFlightStickState
        {
            public class DPAD : ModelBase, INiVekDPad
            {
                private bool _up;
                public bool Up
                {
                    get => _up;
                    set => Set(ref _up, value);
                }

                private bool _down;
                public bool Down
                {
                    get => _down;
                    set => Set(ref _down, value);
                }


                private bool _left;
                public bool Left
                {
                    get => _left;
                    set => Set(ref _left, value);
                }

                private bool _right;
                public bool Right
                {
                    get => _right;
                    set => Set(ref _right, value);
                }
            }

            private bool _start;
            public bool StartMission
            {
                get => _start;
                set => Set(ref _start, value);
            }

            private bool _back;
            public bool Back
            {
                get => _back;
                set => Set(ref _back, value);
            }

            private bool _leftThumb;
            public bool LeftThumb
            {
                get => _leftThumb;
                set => Set(ref _leftThumb, value);
            }

            private bool _rightThumb;
            public bool RightThumb
            {
                get => _rightThumb;
                set => Set(ref _rightThumb, value);
            }

            private bool _leftShoulder;
            public bool LeftShoulder
            {
                get => _leftShoulder;
                set => Set(ref _leftShoulder, value);
            }

            private bool _rightShoulder;
            public bool RightShoulder
            {
                get => _rightShoulder;
                set => Set(ref _rightShoulder, value);
            }

            private bool _a;
            public bool TakeOff
            {
                get => _a;
                set => Set(ref _a, value);
            }


            private bool _b;
            public bool Land
            {
                get => _b;
                set => Set(ref _b, value);
            }

            private bool _returnToHome;
            public bool ReturnToHome
            {
                get => _returnToHome;
                set => Set(ref _returnToHome, value);
            }

            private bool _x;
            public bool NextWaypoint
            {
                get => _x;
                set => Set(ref _x, value);
            }

            private bool _y;
            public bool PreviousWaypoint
            {
                get => _y;
                set => Set(ref _y, value);
            }

            private INiVekDPad _dpad = new DPAD();
            public INiVekDPad DPad
            {

                get => _dpad;
                set => Set(ref _dpad, value);
            }

            private short _leftX;
            public short Rudder
            {
                get => _leftX;
                set => Set(ref _leftX, value);
            }

            private short _leftY;
            public short Throttle
            {
                get => _leftY;
                set => Set(ref _leftY, value);
            }

            private short _rightX;
            public short Roll
            {
                get => _rightX;
                set => Set(ref _rightX, value);
            }

            private short _rightY;
            public short Pitch
            {
                get => _rightY;
                set => Set(ref _rightY, value);
            }

            private short _rightTrigger;
            public short RightTrigger
            {
                get => _rightTrigger;
                set => Set(ref _rightTrigger, value);
            }

            private short _leftTrigger;
            public short LeftTrigger
            {
                get => _leftTrigger;
                set => Set(ref _leftTrigger, value);
            }

            public bool IsConnected { get; set; }

            private short _cameraGimbleX;
            public short CameraGimbleX
            {
                get => _cameraGimbleX;
                set => Set(ref _cameraGimbleX, value);
            }

            private short _cameraGimbleY;
            public short CameraGimbleY
            {
                get => _cameraGimbleY;
                set => Set(ref _cameraGimbleY, value);
            }

            private bool _beginManualCameraControl;
            public bool BegingManualCameraControl
            {
                get => _beginManualCameraControl;
                set => Set(ref _beginManualCameraControl, value);
            }

            private bool _endManualCameraControl;
            public bool EndManualCameraControl
            {
                get => _endManualCameraControl;
                set => Set(ref _endManualCameraControl, value);
            }

            private bool _endMission;
            public bool EndMission
            {
                get => _endMission;
                set => Set(ref _endMission, value);
            }

            private bool _puaseMission;
            public bool PauseMission
            {
                get => _puaseMission;
                set => Set(ref _puaseMission, value);
            }

            private bool _altitudeHold;
            public bool AltitudeHold
            {
                get => _altitudeHold;
                set => Set(ref _altitudeHold, value);
            }


            private bool _continueMission;
            public bool ContinueMission
            {
                get => _continueMission;
                set => Set(ref _continueMission, value);
            }


            public static NiVekFlightStickState Empty
            {
                get
                {
                    return new NiVekFlightStickState()
                    {
                        AltitudeHold = true,
                        DPad = new NiVekFlightStickState.DPAD(),
                        IsConnected = false,
                    };
                }
            }
        }

        public enum GamePadButtons
        {
            DPADUp = 0,
            DPADDown = 1,
            DPADLeft = 2,
            DPADRight = 3,

            Start = 4,
            Back = 5,

            LeftThumb = 6,
            RightThumb = 7,

            LeftShoulder = 8,
            RightShoulder = 9,

            A = 10,
            B = 11,
            X = 12,
            Y = 13,

            LeftTrigger = 14,
            RightTrigger = 15
        }

        public enum ButtonState
        {
            Pressed,
            Released
        }


        public class ButtonStateChangedEventArgs : EventArgs
        {
            public GamePadButtons Button { get; set; }
            public ButtonState ButtonState { get; set; }
        }


        private readonly CoreDispatcher _dispatcher;
        private readonly NiVekFlightStickState _state = new NiVekFlightStickState();

        public NiVekFlightStickState State
        {
            get => _state;
        }

        public NiVekFlightStick(CoreDispatcher dispatcher)
        {
            this._state = NiVekFlightStickState.Empty;
            this._dispatcher = dispatcher;

            // _tpTimer = ThreadPoolTimer.CreatePeriodicTimer(_timer_Tick, TimeSpan.FromMilliseconds(20));
        }

        private double? _thottleOffset = null;

        public event EventHandler StartMission;
        public event EventHandler NextWaypoint;
        public event EventHandler PreviousWaypoint;
        public event EventHandler PauseMission;
        public event EventHandler ContinueMission;
        public event EventHandler EndMission;

        public event EventHandler TakeOff;
        public event EventHandler ReturnToHome;
        public event EventHandler Land;
        public event EventHandler BeginManualCameraControl;
        public event EventHandler EndManualCameraControl;

        private void TriggerEvents(NiVekFlightStickState currentState)
        {
            if (_state == null)
            {
                return;
            }

            if (currentState.BegingManualCameraControl && !_state.BegingManualCameraControl)
            {
                BeginManualCameraControl?.Invoke(this, null);
                currentState.CameraGimbleX = 0;
                currentState.CameraGimbleY = 0;
            }

            if (currentState.EndManualCameraControl && !_state.EndManualCameraControl)
            {
                EndManualCameraControl?.Invoke(this, null);
                currentState.CameraGimbleX = 0;
                currentState.CameraGimbleY = 0;
            }

            if (currentState.TakeOff && !_state.TakeOff)
            {
                TakeOff?.Invoke(this, null);
            }

            if (currentState.ReturnToHome && !_state.ReturnToHome) ReturnToHome?.Invoke(this, null);
            if (currentState.Land && !_state.Land) Land?.Invoke(this, null);
            if (currentState.NextWaypoint && !_state.NextWaypoint) NextWaypoint?.Invoke(this, null);
            if (currentState.PreviousWaypoint && !_state.PreviousWaypoint) PreviousWaypoint?.Invoke(this, null);
            if (currentState.StartMission && !_state.StartMission) StartMission?.Invoke(this, null);
            if (currentState.PauseMission && !_state.PauseMission) PauseMission?.Invoke(this, null);
            if (currentState.ContinueMission && !_state.ContinueMission) ContinueMission?.Invoke(this, null);
            if (currentState.EndMission && !_state.EndMission) EndMission?.Invoke(this, null);
        }

        public void RefreshFromThrustMaster1600(RawGameController rawGC)
        {
            var axis = new double[rawGC.AxisCount];
            var switches = new GameControllerSwitchPosition[rawGC.SwitchCount];
            var buttons = new bool[rawGC.ButtonCount];
            rawGC.GetCurrentReading(buttons, switches, axis);

            var currentState = new NiVekFlightStickState
            {
                TakeOff = buttons[1],
                Land = buttons[3],
                Roll = Normalize(axis[0]),
                Pitch = Convert.ToInt16(-Normalize(axis[1])),              
            };

            TriggerEvents(currentState);

            RunOnUIThread(() =>
            {
                _state.TakeOff = currentState.TakeOff;
                _state.Land = currentState.Land;
                _state.Roll = currentState.Roll;
                _state.Pitch = currentState.Pitch;

                _state.IsConnected = true;
            });
        }

        private short Normalize(double value, double center = 0.5, short min = -1000, short max = 1000)
        {
            value = value - center;

            var deadZoneClearedValue = (value > -.025 && value < .025) ? 0 : value;
            return Convert.ToInt16(deadZoneClearedValue * (max - min));
        }

        private async void RunOnUIThread(Action action)
        {
            await this._dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
           {
               action();
           });
        }


        public void RefreshFromThrustMasterThrottle(RawGameController rawGC)
        {
            var axis = new double[rawGC.AxisCount];
            var switches = new GameControllerSwitchPosition[rawGC.SwitchCount];
            var buttons = new bool[rawGC.ButtonCount];
            rawGC.GetCurrentReading(buttons, switches, axis);

            var leftRudder = axis[3];
            var rightRidder = axis[4];

            var cameraX = axis[0];
            var cameraY = axis[1];

            var currentState = new NiVekFlightStickState
            {
                BegingManualCameraControl = buttons[7],
                EndManualCameraControl = buttons[9],
                CameraGimbleX = Normalize(axis[0]),
                CameraGimbleY = Normalize(axis[1]),
                Rudder = Convert.ToInt16(-Normalize(axis[6])),

            };

            if (buttons[1] && !_state.AltitudeHold)
                currentState.AltitudeHold = true;
            else if (buttons[2] && _state.AltitudeHold)
                currentState.AltitudeHold = false;
            else
                currentState.AltitudeHold = _state.AltitudeHold;

            currentState.Throttle = currentState.AltitudeHold ? Normalize(axis[5]) : Normalize(axis[2], 0, 0, 1000);

            TriggerEvents(currentState);

            RunOnUIThread(() =>
            {
                _state.AltitudeHold = currentState.AltitudeHold;
                _state.Throttle = currentState.Throttle;
                _state.Rudder = currentState.Rudder;
                _state.CameraGimbleX = currentState.CameraGimbleX;
                _state.CameraGimbleY = currentState.CameraGimbleY;
                _state.BegingManualCameraControl = currentState.BegingManualCameraControl;
                _state.EndManualCameraControl = currentState.EndManualCameraControl;

                _state.IsConnected = true;
            });
        }

        public void RefreshFromXBox(Gamepad gamePad)
        {
            if (gamePad == null)
            {
                return;
            }

            var state = gamePad.GetCurrentReading();

            var currentState = new NiVekFlightStickState
            {
                TakeOff = (state.Buttons & GamepadButtons.A) == GamepadButtons.A,
                StartMission = (state.Buttons & GamepadButtons.View) == GamepadButtons.View,
                Land = (state.Buttons & GamepadButtons.B) == GamepadButtons.B,
                NextWaypoint = (state.Buttons & GamepadButtons.X) == GamepadButtons.X,
                PreviousWaypoint = (state.Buttons & GamepadButtons.Y) == GamepadButtons.Y,
                EndMission = (state.Buttons & GamepadButtons.Menu) == GamepadButtons.Menu,

                Rudder = (state.LeftThumbstickX > -.10 && state.LeftThumbstickX < .1) ? Convert.ToInt16(0) : Convert.ToInt16(state.LeftThumbstickX * 1000),
                Throttle = (state.LeftThumbstickY > -.10 && state.LeftThumbstickY < .1) ? Convert.ToInt16(0) : Convert.ToInt16(state.LeftThumbstickY * 1000),
                Roll = (state.RightThumbstickX > -.10 && state.RightThumbstickX < .1) ? Convert.ToInt16(0) : Convert.ToInt16(state.RightThumbstickX * 1000),
                Pitch = (state.RightThumbstickY > -.10 && state.RightThumbstickY < .1) ? Convert.ToInt16(0) : Convert.ToInt16(state.RightThumbstickY * 1000),

                LeftTrigger = (state.LeftTrigger > -100 && state.LeftTrigger < 100) ? Convert.ToInt16(0) : Convert.ToInt16(state.LeftTrigger * 1000),
                RightTrigger = (state.RightTrigger > -100 && state.RightTrigger < 100) ? Convert.ToInt16(0) : Convert.ToInt16(state.RightTrigger * 1000),
                LeftShoulder = (state.Buttons & GamepadButtons.LeftShoulder) == GamepadButtons.LeftShoulder,
                RightShoulder = (state.Buttons & GamepadButtons.RightShoulder) == GamepadButtons.RightShoulder,
            };

            currentState.DPad.Down = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
            currentState.DPad.Up = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
            currentState.DPad.Left = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
            currentState.DPad.Right = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;

            RunOnUIThread(() =>
            {
                TriggerEvents(currentState);

                _state.TakeOff = currentState.TakeOff;
                _state.Land = currentState.Land;
                _state.Roll = currentState.Roll;
                _state.Pitch = currentState.Pitch;
                _state.Rudder = currentState.Rudder;
                _state.Throttle = currentState.Throttle;

                _state.IsConnected = true;
            });
        }



        private void WriteGC(String name, RawGameController gc)
        {
            Debug.WriteLine(name);

            var axis = new double[gc.AxisCount];
            var switches = new GameControllerSwitchPosition[gc.SwitchCount];
            var buttons = new bool[gc.ButtonCount];
            gc.GetCurrentReading(buttons, switches, axis);
            for (var idx = 0; idx < buttons.Length; ++idx)
            {
                var btn = buttons[idx];
                Debug.Write($"{idx}. {btn} ");
            }

            Debug.WriteLine(String.Empty);

            for (var idx = 0; idx < axis.Length; ++idx)
            {
                var axs = axis[idx];
                Debug.Write($"{idx}. {axs} ");
            }

            Debug.WriteLine(String.Empty);

            for (var idx = 0; idx < switches.Length; ++idx)
            {
                var sw = switches[idx];
                Debug.Write($"{idx}. {sw}");
            }

            Debug.WriteLine(String.Empty);
            Debug.WriteLine(String.Empty);
        }
    }
}

