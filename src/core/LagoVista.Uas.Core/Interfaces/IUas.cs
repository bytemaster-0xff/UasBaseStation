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
        DOF3Sensor Acc { get; }
        ObservableCollection<RCChannel> Channels { get; }
        EntityHeader DeviceConfiguration { get; }
        EntityHeader DeviceType { get; }
        DOF3Sensor Gyro { get; }
        GeoLocation Location { get; set; }
        DOF3Sensor Magnometer { get; }
        void Update(UasMessage msg);
        float Pitch { get; set; }
        float PitchSpeed { get; set; }
        float Roll { get; set; }
        float RollSpeed { get; set; }
        ObservableCollection<ServoOutput> ServoOutputs { get; }
        float Yaw { get; set; }
        float YawSpeed { get; set; }

        byte SystemId { get; }
        byte ComponentId { get; }

        SensorList Sensors { get; }
    }
}
