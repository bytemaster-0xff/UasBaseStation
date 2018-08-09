using LagoVista.Uas.Core.Configuration;
using LagoVista.Uas.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Utils
{
    internal class Modes
    {
        public static List<KeyValuePair<int, string>> getModesList(Firmwares firmware)
        {

            if (firmware == Firmwares.PX4)
            {
                /*
union px4_custom_mode {
    struct {
        uint16_t reserved;
        uint8_t main_mode;
        uint8_t sub_mode;
    };
    uint32_t data;
    float data_float;
};
                 */


                var temp = new List<KeyValuePair<int, string>>()
                {
                    new KeyValuePair<int, string>((int) PX4CustomModes.Manual << 16, CoreResources.PX4Mode_Manual),
                    new KeyValuePair<int, string>((int) PX4CustomModes.Acro << 16, CoreResources.PX4Mode_Acro),
                    new KeyValuePair<int, string>((int) PX4CustomModes.Stablilized << 16, CoreResources.PX4Mode_Stabilized),
                    new KeyValuePair<int, string>((int) PX4CustomModes.Rattitude << 16, CoreResources.PX4Mode_Stabilized),
                    new KeyValuePair<int, string>((int) PX4CustomModes.AltCtl << 16, CoreResources.PX4Mode_AltCtl),
                    new KeyValuePair<int, string>((int) PX4CustomModes.PosCtl << 16, CoreResources.PX4Mode_PoCtl),
                    new KeyValuePair<int, string>((int) PX4CustomModes.OffBoard<< 16, CoreResources.PX4Mode_Offboard),
                    new KeyValuePair<int, string>(
                        ((int) PX4CustomModes.Auto << 16) +
                        (int) PX4CustomSubodes.Ready << 24, CoreResources.PX4Mode_AutoReady),
                    new KeyValuePair<int, string>(
                        ((int) PX4CustomModes.Auto << 16) +
                        (int) PX4CustomSubodes.Takeoff << 24, CoreResources.PX4Mode_AutoTakeoff),
                    new KeyValuePair<int, string>(
                        ((int) PX4CustomModes.Auto << 16) +
                        (int) PX4CustomSubodes.Loiter << 24, CoreResources.PX4Mode_Loiter),
                    new KeyValuePair<int, string>(
                        ((int) PX4CustomModes.Auto << 16) +
                        (int) PX4CustomSubodes.Mission << 24, CoreResources.PX4Mode_Mission),
                    new KeyValuePair<int, string>(
                        ((int) PX4CustomModes.Auto << 16) +
                        (int) PX4CustomSubodes.RTL << 24, CoreResources.PX4Mode_RTL),
                    new KeyValuePair<int, string>(
                        ((int) PX4CustomModes.Auto << 16) +
                        (int) PX4CustomSubodes.Land << 24, CoreResources.PX4Mode_Landing)
                };

                return temp;
            }
            else if (firmware == Firmwares.ArduPlane)
            {
                var flightModes = Parameters.GetParameterOptionsInt("FLTMODE1",
                    firmware.ToString());
                flightModes.Add(new KeyValuePair<int, string>(16, "INITIALISING"));

                flightModes.Add(new KeyValuePair<int, string>(17, "QStabilize"));
                flightModes.Add(new KeyValuePair<int, string>(18, "QHover"));
                flightModes.Add(new KeyValuePair<int, string>(19, "QLoiter"));
                flightModes.Add(new KeyValuePair<int, string>(20, "QLand"));
                flightModes.Add(new KeyValuePair<int, string>(21, "QRTL"));

                return flightModes;
            }
            else if (firmware == Firmwares.Ateryx)
            {
                var flightModes = Parameters.GetParameterOptionsInt("FLTMODE1", firmware.ToString()); //same as apm
                return flightModes;
            }
            else if (firmware == Firmwares.ArduCopter2)
            {
                var flightModes = Parameters.GetParameterOptionsInt("FLTMODE1", firmware.ToString());
                return flightModes;
            }
            else if (firmware == Firmwares.ArduRover)
            {
                var flightModes = Parameters.GetParameterOptionsInt("MODE1", firmware.ToString());
                return flightModes;
            }
            else if (firmware == Firmwares.ArduTracker)
            {
                var temp = new List<KeyValuePair<int, string>>();
                temp.Add(new KeyValuePair<int, string>(0, CoreResources.TrackerMode_Manual));
                temp.Add(new KeyValuePair<int, string>(1, CoreResources.TrackerMode_Stop));
                temp.Add(new KeyValuePair<int, string>(2, CoreResources.TrackerMode_Scan));
                temp.Add(new KeyValuePair<int, string>(3, CoreResources.TrackerMode_ServoTest));
                temp.Add(new KeyValuePair<int, string>(10, CoreResources.TrackerMode_Auto));
                temp.Add(new KeyValuePair<int, string>(16, CoreResources.TrackerMode_Initializing));

                return temp;
            }

            return null;
        }
    }
}
