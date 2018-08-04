using LagoVista.Core.Models;
using LagoVista.Core.Models.Geo;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Uas.Core
{
    public interface IUas
    {
        EntityHeader DeviceConfiguration { get; }
        EntityHeader DeviceType { get; }

        bool Armed { get; }

        List<DOF3Sensor> Acc { get; }
        List<DOF3Sensor> Gyro { get; }
        List<DOF3Sensor> Magnometer { get; }

        List<GPS> GPSs { get; set; }

        GeoLocation HomeLocation { get; set; }
        GeoLocation CurrentLocation { get; set; }

        float DistanceToHome { get; set; }

        ObservableCollection<RCChannel> Channels { get; }
        ObservableCollection<ServoOutput> ServoOutputs { get; }
        ObservableCollection<ESC> ESCs { get; }
        SensorList Sensors { get; }

        void Update(UasMessage msg);


        float AngleOfAttack { get; set; }
        float Pitch { get; set; }
        float PitchSpeed { get; set; }
        float Roll { get; set; }
        float RollSpeed { get; set; }

        
        float Yaw { get; set; }
        float YawSpeed { get; set; }

        byte SystemId { get; }
        byte ComponentId { get; }


        float AirSpeed { get; set; }
        float GroundSpeed { get; set; }

    }
}
