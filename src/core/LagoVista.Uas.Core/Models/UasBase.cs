using LagoVista.Core.Models;
using LagoVista.Core.Models.Geo;
using LagoVista.Uas.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace LagoVista.Uas.Core
{
    public abstract class UasBase : ModelBase, IUas
    {
        LagoVista.IoT.DeviceManagement.Core.Models.Device _device;

        public UasBase(LagoVista.IoT.DeviceManagement.Core.Models.Device device)
        {
            _device = device;

            Acc = new ObservableCollection<DOF3Sensor>();
            Acc.Add(new DOF3Sensor());
            Gyro = new ObservableCollection<DOF3Sensor>();
            Gyro.Add(new DOF3Sensor());
            Magnometer = new ObservableCollection<DOF3Sensor>();
            Magnometer.Add(new DOF3Sensor());
            EKFStatus = new EKF();
            GPSs = new ObservableCollection<GPS>();
            GPSs.Add(new GPS());
            Batteries = new ObservableCollection<Battery>();
            Batteries.Add(new Battery());
            SystemStatus = new SystemStatus();
            Attitude = new Attitude();
            FlightController = new FlightController();
            PowerStatus = new PowerStatus();
            RangeFinder = new RangeFinder();
            Comms = new Comms();
            Camera = new Camera();
            Location = new GeoLocation();

            Channels = new ObservableCollection<RCChannel>();
            for (var idx = 0; idx < 16; ++idx) Channels.Add(new RCChannel());

            ServoOutputs = new ObservableCollection<ServoOutput>();
            for (var idx = 0; idx < 16; ++idx) ServoOutputs.Add(new ServoOutput());

            ComponentId = 1;
            SystemId = 1;

            Sensors = new SensorList();
        }

        public byte SystemId { get; }
        public byte ComponentId { get; }

        public ObservableCollection<DOF3Sensor> Acc { get; }

        public ObservableCollection<DOF3Sensor> Gyro { get; }

        public ObservableCollection<DOF3Sensor> Magnometer { get; }

        public EntityHeader DeviceType { get; }
        public EntityHeader DeviceConfiguration { get; }

        IUasMessageAdapter _adapter;
        public void SetAdapter(IUasMessageAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Update(UasMessage msg)
        {
            _adapter.UpdateUas(this, msg);
        }

        public EKF EKFStatus { get; }

        public Attitude Attitude { get; }
        public Comms Comms { get; }

        public Camera Camera { get; }

        public SensorList Sensors { get; }

        public Battery Battery
        {
            get => Batteries.First();
        }

        public ObservableCollection<Battery> Batteries { get; }

        public ObservableCollection<RCChannel> Channels { get; }
        public ObservableCollection<ServoOutput> ServoOutputs { get; }
        public ObservableCollection<ESC> ESCs { get; }

        public SystemStatus SystemStatus { get; }

        public RangeFinder RangeFinder { get; }

        GeoLocation _location;
        public GeoLocation Location
        {
            get { return _location; }
            set { Set(ref _location, value); }
        }

        public GPS GSP
        {
            get => GPSs.First();
        }

        public ObservableCollection<GPS> GPSs { get; }

        GeoLocation _homeLocation;
        public GeoLocation HomeLocation
        {
            get { return _homeLocation; }
            set { Set(ref _homeLocation, value); }
        }

        GeoLocation _currentLocation;
        public GeoLocation CurrentLocation
        {
            get { return _currentLocation; }
            set { Set(ref _currentLocation, value); }
        }

        float _distanceToHome;
        public float DistanceToHome
        {
            get { return _distanceToHome; }
            set { Set(ref _distanceToHome, value); }
        }


        private float _angleOfAttack;
        public float AngleOfAttack
        {
            get => _angleOfAttack;
            set => Set(ref _angleOfAttack, value);
        }

        private float _airSpeed;
        public float AirSpeed
        {
            get => _airSpeed;
            set => Set(ref _airSpeed, value);
        }

        private float _groundSpeed;
        public float GroundSpeed
        {
            get => _groundSpeed;
            set => Set(ref _groundSpeed, value);
        }

        GeoLocation _homeBaseLocation;
        public GeoLocation HomeBaseLocation
        {
            get => _homeBaseLocation;
            set => Set(ref _homeBaseLocation, value);
        }

        public string UasSerialNumber { get; set; }
        public string UasName { get; set; }
        public string UasType { get; set; }

        public FlightController FlightController { get; }

        public PowerStatus PowerStatus { get; }
    }
}
