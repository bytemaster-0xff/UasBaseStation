namespace LagoVista.Uas.Core.Controller
{
    public interface INiVekFlightStickState
    {
        bool A { get; set; }
        bool B { get; set; }
        bool Back { get; set; }
        INiVekDPad DPad { get; set; }
        bool IsConnected { get; set; }
        bool LeftShoulder { get; set; }
        bool LeftThumb { get; set; }
        short LeftTrigger { get; set; }
        short LeftX { get; set; }
        short LeftY { get; set; }
        bool RightShoulder { get; set; }
        bool RightThumb { get; set; }
        short RightTrigger { get; set; }
        short RightX { get; set; }
        short RightY { get; set; }
        bool Start { get; set; }
        bool X { get; set; }
        bool Y { get; set; }
    }

    public interface INiVekDPad
    {
        bool Left { get; set; }

        bool Up { get; set; }

        bool Right { get; set; }

        bool Down { get; set; }
    }
}