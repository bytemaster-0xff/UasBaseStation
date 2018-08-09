using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core
{
    public enum PX4CustomModes
    {
        Manual = 1,
        AltCtl,
        PosCtl,
        Auto,
        Acro,
        OffBoard,
        Stablilized,
        Rattitude
    }

    public enum PX4CustomSubodes
    {
        Ready = 1,
        Takeoff,
        Loiter,
        Mission,
        RTL,
        Land,
        RTGS
    }

    public enum Firmwares
    {
        ArduPlane,
        ArduCopter2,
        ArduRover,
        ArduSub,
        Ateryx,
        ArduTracker,
        Gymbal,
        PX4,
        Unknown
    }
    class Common
    {
    }
}
