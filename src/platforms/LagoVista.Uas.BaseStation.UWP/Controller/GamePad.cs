using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.System.Threading;

namespace LagoVista.Uas.BaseStation.UWP.Controller
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

            public sbyte LeftX { get; set; }
            public sbyte LeftY { get; set; }

            public sbyte RightX { get; set; }
            public sbyte RightY { get; set; }

            public bool LeftTrigger { get; set; }
            public bool RightTrigger { get; set; }
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

        ThreadPoolTimer _tpTimer;

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

        public void Refresh(GamePad gamePad)
        {
            lock (_tpTimer)
            {
                try
                {
                    var state = gamePad.GetState();

                    var currentState = new GamePadState();

                    currentState.A = state.A;
                    currentState.B = state.B;
                    currentState.X = state.X;
                    currentState.Y = state.Y;
                    currentState.DPad.Down = state.DPad.Down;
                    currentState.DPad.Up = state.DPad.Up;
                    currentState.DPad.Left = state.DPad.Left;
                    currentState.DPad.Right = state.DPad.Right;
                    currentState.LeftShoulder = state.LeftShoulder;
                    currentState.RightShoulder = state.RightShoulder;
                    currentState.LeftTrigger = state.LeftTrigger;
                    currentState.RightShoulder = state.RightShoulder;
                    currentState.Start = state.Start;
                    currentState.Back = state.Back;
                    currentState.LeftX = state.LeftX;
                    currentState.LeftY = state.LeftY;
                    currentState.RightX = state.RightX;
                    currentState.RightY = state.RightY;

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

                        if (currentState.LeftTrigger != _lastState.LeftTrigger)
                            RaiseButtonChangedEvent(GamePadButtons.LeftTrigger, currentState.LeftTrigger ? ButtonState.Pressed : ButtonState.Released);

                        if (currentState.RightTrigger != _lastState.RightTrigger)
                            RaiseButtonChangedEvent(GamePadButtons.RightTrigger, currentState.RightTrigger ? ButtonState.Pressed : ButtonState.Released);

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
}
