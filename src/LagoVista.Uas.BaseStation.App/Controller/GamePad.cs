using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace LagoVista.Uas.BaseStation.App.Controller
{
    public class GamePad
    {
        public class GamePadState
        {
            public class DPAD
            {
                public bool Up { get; set; }
                public bool Down { get; set; }
                public bool Left { get; set; }
                public bool Right { get; set; }
            }

            public bool Start { get; set; }
            public bool Back { get; set; }

            public bool LeftThumb { get; set; }
            public bool RightThumb { get; set; }

            public bool LeftShoulder { get; set; }
            public bool RightShoulder { get; set; }

            public bool A { get; set; }
            public bool B { get; set; }
            public bool X { get; set; }
            public bool Y { get; set; }

            public DPAD DPad { get; set; }

            public double LeftX { get; set; }
            public double LeftY { get; set; }

            public double RightX { get; set; }
            public double RightY { get; set; }

            public double LeftTrigger { get; set; }
            public double RightTrigger { get; set; }
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

        GamePadState _lastState = null;


        public class ButtonStateChangedEventArgs : EventArgs
        {
            public GamePadButtons Button { get; set; }
            public ButtonState ButtonState { get; set; }
        }

        public bool HasState
        {
            get
            {
                return _lastState != null;
            }
        }

        public GamePadState GetState()
        {
            return _lastState;
        }

        public event EventHandler<ButtonStateChangedEventArgs> ButtonStateChanged;

        public GamePad()
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
            try
            {
                var state = gamePad.GetCurrentReading();

                var currentState = new GamePadState();

                currentState.A = (state.Buttons & GamepadButtons.A) == GamepadButtons.A;
                currentState.B = (state.Buttons & GamepadButtons.B) == GamepadButtons.B;
                currentState.X = (state.Buttons & GamepadButtons.X) == GamepadButtons.X;
                currentState.Y = (state.Buttons & GamepadButtons.Y) == GamepadButtons.Y;
                currentState.DPad.Down = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
                currentState.DPad.Up = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
                currentState.DPad.Left = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
                currentState.DPad.Right = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
                currentState.LeftShoulder = (state.Buttons & GamepadButtons.LeftShoulder) == GamepadButtons.LeftShoulder; 
                currentState.RightShoulder = (state.Buttons & GamepadButtons.RightShoulder) == GamepadButtons.RightShoulder; 
                currentState.LeftTrigger = state.LeftTrigger;
                currentState.RightTrigger = state.RightTrigger;
                currentState.Start = (state.Buttons & GamepadButtons.View) == GamepadButtons.View;
                currentState.Back = (state.Buttons & GamepadButtons.Menu) == GamepadButtons.Menu;
                currentState.LeftX = state.LeftThumbstickX;
                currentState.LeftY = state.LeftThumbstickY;
                currentState.RightX = state.RightThumbstickX;
                currentState.RightY = state.RightThumbstickY;

                if (_lastState != null)
                {
                    if (currentState.A != _lastState.A)
                        RaiseButtonChangedEvent(GamePadButtons.A, currentState.A ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.B != _lastState.B)
                        RaiseButtonChangedEvent(GamePadButtons.B, currentState.B ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.X != _lastState.X)
                        RaiseButtonChangedEvent(GamePadButtons.X, currentState.X ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.Y != _lastState.Y)
                        RaiseButtonChangedEvent(GamePadButtons.Y, currentState.Y ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.Back != _lastState.Back)
                        RaiseButtonChangedEvent(GamePadButtons.Back, currentState.Back ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.Start != _lastState.Start)
                        RaiseButtonChangedEvent(GamePadButtons.Start, currentState.Start ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.DPad.Down != _lastState.DPad.Down)
                        RaiseButtonChangedEvent(GamePadButtons.DPADDown, currentState.DPad.Down ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.DPad.Left != _lastState.DPad.Left)
                        RaiseButtonChangedEvent(GamePadButtons.DPADLeft, currentState.DPad.Left ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.DPad.Right != _lastState.DPad.Right)
                        RaiseButtonChangedEvent(GamePadButtons.DPADRight, currentState.DPad.Right ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.DPad.Up != _lastState.DPad.Up)
                        RaiseButtonChangedEvent(GamePadButtons.DPADUp, currentState.DPad.Up ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.LeftShoulder != _lastState.LeftShoulder)
                        RaiseButtonChangedEvent(GamePadButtons.LeftShoulder, currentState.LeftShoulder ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.RightShoulder != _lastState.RightShoulder)
                        RaiseButtonChangedEvent(GamePadButtons.RightShoulder, currentState.RightShoulder ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.LeftThumb != _lastState.LeftThumb)
                        RaiseButtonChangedEvent(GamePadButtons.LeftThumb, currentState.LeftThumb ? ButtonState.Pressed : ButtonState.Released);

                    if (currentState.RightThumb != _lastState.RightThumb)
                        RaiseButtonChangedEvent(GamePadButtons.RightThumb, currentState.RightThumb ? ButtonState.Pressed : ButtonState.Released);
                }

                _lastState = currentState;

            }
            catch (Exception)
            {

            }

        }
    }
}

