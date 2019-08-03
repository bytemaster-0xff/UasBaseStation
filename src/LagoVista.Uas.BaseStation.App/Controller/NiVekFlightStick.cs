using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using LagoVista.Uas.Core.Controller;
using System.Diagnostics;

namespace LagoVista.Uas.BaseStation.App.Controller
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
            public bool Start
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
            public bool A
            {
                get => _a;
                set => Set(ref _a, value);
            }


            private bool _b;
            public bool B
            {
                get => _b;
                set => Set(ref _b, value);
            }

            private bool _x;
            public bool X
            {
                get => _x;
                set => Set(ref _x, value);
            }

            private bool _y;
            public bool Y
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
            public short LeftX
            {
                get => _leftX;
                set => Set(ref _leftX, value);
            }

            private short _leftY;
            public short LeftY
            {
                get => _leftY;
                set => Set(ref _leftY, value);
            }

            private short _rightX;
            public short RightX
            {
                get => _rightX;
                set => Set(ref _rightX, value);
            }

            private short _rightY;
            public short RightY
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
        }

        public static NiVekFlightStick Empty
        {
            get
            {
                return new NiVekFlightStick()
                {
                    _state = new NiVekFlightStickState()
                    {
                        DPad = new NiVekFlightStickState.DPAD(),
                        IsConnected = false,

                    }
                };
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


        NiVekFlightStickState _state = new NiVekFlightStickState();

        public NiVekFlightStickState State
        {
            get => _state;
            set => Set(ref _state, value);
        }

        public event EventHandler<ButtonStateChangedEventArgs> ButtonStateChanged;

        public NiVekFlightStick()
        {
            // _tpTimer = ThreadPoolTimer.CreatePeriodicTimer(_timer_Tick, TimeSpan.FromMilliseconds(20));
        }

        void RaiseButtonChangedEvent(GamePadButtons button, ButtonState newState)
        {
            if (ButtonStateChanged != null)
                ButtonStateChanged(this, new ButtonStateChangedEventArgs() { Button = button, ButtonState = newState });
        }

        public void Refresh(Gamepad gamePad)
        {
            if (gamePad == null)
            {
                State = NiVekFlightStick.Empty.State;
                return;
            }

            try
            {
                var state = gamePad.GetCurrentReading();

                var currentState = new NiVekFlightStickState
                {
                    A = (state.Buttons & GamepadButtons.A) == GamepadButtons.A,
                    B = (state.Buttons & GamepadButtons.B) == GamepadButtons.B,
                    X = (state.Buttons & GamepadButtons.X) == GamepadButtons.X,
                    Y = (state.Buttons & GamepadButtons.Y) == GamepadButtons.Y,
                    LeftShoulder = (state.Buttons & GamepadButtons.LeftShoulder) == GamepadButtons.LeftShoulder,
                    RightShoulder = (state.Buttons & GamepadButtons.RightShoulder) == GamepadButtons.RightShoulder,
                    LeftTrigger = (state.LeftTrigger > -100 && state.LeftTrigger < 100) ? Convert.ToInt16(0) : Convert.ToInt16(state.LeftTrigger * 1000),
                    RightTrigger = (state.RightTrigger > -100 && state.RightTrigger < 100) ? Convert.ToInt16(0) : Convert.ToInt16(state.RightTrigger * 1000),
                    Start = (state.Buttons & GamepadButtons.View) == GamepadButtons.View,
                    Back = (state.Buttons & GamepadButtons.Menu) == GamepadButtons.Menu,
                    LeftX = (state.LeftThumbstickX > -.10 && state.LeftThumbstickX < .1) ? Convert.ToInt16(0) : Convert.ToInt16(state.LeftThumbstickX * 1000),
                    LeftY = (state.LeftThumbstickY > -.10 && state.LeftThumbstickY < .1) ? Convert.ToInt16(0) : Convert.ToInt16(state.LeftThumbstickY * 1000),
                    RightX = (state.RightThumbstickX > -.10 && state.RightThumbstickX < .1) ? Convert.ToInt16(0) : Convert.ToInt16(state.RightThumbstickX * 1000),
                    RightY = (state.RightThumbstickY > -.10 && state.RightThumbstickY < .1) ? Convert.ToInt16(0) : Convert.ToInt16(state.RightThumbstickY * 1000)
                };

                currentState.DPad.Down = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
                currentState.DPad.Up = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
                currentState.DPad.Left = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
                currentState.DPad.Right = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;


                if (_state != null)
                {
                    if (currentState.A != _state.A)
                        RaiseButtonChangedEvent(GamePadButtons.A, currentState.A ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.B != _state.B)
                        RaiseButtonChangedEvent(GamePadButtons.B, currentState.B ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.X != _state.X)
                        RaiseButtonChangedEvent(GamePadButtons.X, currentState.X ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.Y != _state.Y)
                        RaiseButtonChangedEvent(GamePadButtons.Y, currentState.Y ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.Back != _state.Back)
                        RaiseButtonChangedEvent(GamePadButtons.Back, currentState.Back ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.Start != _state.Start)
                        RaiseButtonChangedEvent(GamePadButtons.Start, currentState.Start ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.DPad.Down != _state.DPad.Down)
                        RaiseButtonChangedEvent(GamePadButtons.DPADDown, currentState.DPad.Down ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.DPad.Left != _state.DPad.Left)
                        RaiseButtonChangedEvent(GamePadButtons.DPADLeft, currentState.DPad.Left ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.DPad.Right != _state.DPad.Right)
                        RaiseButtonChangedEvent(GamePadButtons.DPADRight, currentState.DPad.Right ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.DPad.Up != _state.DPad.Up)
                        RaiseButtonChangedEvent(GamePadButtons.DPADUp, currentState.DPad.Up ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.LeftShoulder != _state.LeftShoulder)
                        RaiseButtonChangedEvent(GamePadButtons.LeftShoulder, currentState.LeftShoulder ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.RightShoulder != _state.RightShoulder)
                        RaiseButtonChangedEvent(GamePadButtons.RightShoulder, currentState.RightShoulder ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.LeftThumb != _state.LeftThumb)
                        RaiseButtonChangedEvent(GamePadButtons.LeftThumb, currentState.LeftThumb ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.RightThumb != _state.RightThumb)
                        RaiseButtonChangedEvent(GamePadButtons.RightThumb, currentState.RightThumb ? ButtonState.Pressed : ButtonState.Released);
                }

                _state = currentState;
                _state.IsConnected = true;

            }
            catch (Exception)
            {
                _state.IsConnected = false;
            }
        }
    }
}

