using LagoVista.Core.Models;
using LagoVista.Core.Models.Geo;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace LagoVista.Uas.Core
{

    public enum LandingGearPositions
    {
        Up,
        Down
    }

    public interface IUas : INotifyPropertyChanged
    {
        EntityHeader DeviceConfiguration { get; }
        EntityHeader DeviceType { get; }

        ObservableCollection<DOF3Sensor> Acc { get; }
        ObservableCollection<DOF3Sensor> Gyro { get; }
        ObservableCollection<DOF3Sensor> Magnometer { get; }
        GPS GPS { get; }
        bool HasLocation { get; }

        ObservableCollection<GPS> GPSs { get; }
        Battery Battery { get; }
        ObservableCollection<Battery> Batteries { get; }

        GeoLocation HomeLocation { get; set; }
        GeoLocation CurrentLocation { get; set; }

        float DistanceToHome { get; set; }

        FlightController FlightController { get; }

        ObservableCollection<RCChannel> Channels { get; }
        ObservableCollection<ServoOutput> ServoOutputs { get; }
        ObservableCollection<ESC> ESCs { get; }
        SensorList Sensors { get; }
        PowerStatus PowerStatus { get; }

        ICameraManager Camera { get; }

        EKF EKFStatus { get; }

        void Update(UasMessage msg);

        float AngleOfAttack { get; set; }

        Attitude Attitude { get; }

        SystemStatus SystemStatus { get; }
        byte SystemId { get; }
        byte ComponentId { get; }

        RangeFinder RangeFinder { get; }

        float AirSpeed { get; set; }
        float GroundSpeed { get; set; }

        Comms Comms { get; }
        GeoLocation HomeBaseLocation { get; }
        GeoLocation Location { get; set; }

        String UasName { get; }
        String UasType { get; }
        String UasSerialNumber { get; }

        int TemperatureHigh { get; set; }
        int TemperatureLow { get; set; }
    }
}
