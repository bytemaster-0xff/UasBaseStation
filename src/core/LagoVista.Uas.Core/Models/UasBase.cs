using LagoVista.Core.Models;
using LagoVista.Core.Models.Geo;
using LagoVista.Uas.Core.Models;
using System.Collections.ObjectModel;

namespace LagoVista.Uas.Core
{
    public abstract class UasBase : ModelBase, IUas
    {
        LagoVista.IoT.DeviceManagement.Core.Models.Device _device;

        public UasBase(LagoVista.IoT.DeviceManagement.Core.Models.Device device)
        {
            _device = device;

            Acc = new DOF3Sensor();
            Gyro = new DOF3Sensor();
            Magnometer = new DOF3Sensor();

            Channels = new ObservableCollection<RCChannel>();
            for (var idx = 0; idx < 8; ++idx) Channels.Add(new RCChannel());
            
            ServoOutputs = new ObservableCollection<ServoOutput>();
            for (var idx = 0; idx < 16; ++idx) ServoOutputs.Add(new ServoOutput());

            ComponentId = 1;
            SystemId = 1;

            Sensors = new SensorList();
        }

        public byte SystemId { get; }
        public byte ComponentId { get; }

        public DOF3Sensor Acc { get; }

        public DOF3Sensor Gyro { get; }

        public DOF3Sensor Magnometer { get; }

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

        public float _pitch;
        public float Pitch
        {
            get => _pitch;
            set => Set(ref _pitch, value);
        }

        public float _yaw;
        public float Yaw
        {
            get => _yaw;
            set => Set(ref _yaw, value);
        }

        public float _roll;
        public float Roll
        {
            get => _roll;
            set => Set(ref _roll, value);
        }

        public float _pitchSpeed;
        public float PitchSpeed
        {
            get => _pitchSpeed;
            set => Set(ref _pitchSpeed, value);
        }

        public float _yawSpeed;
        public float YawSpeed
        {
            get => _yawSpeed;
            set => Set(ref _yawSpeed, value);
        }

        public float _rollSpeed;
        public float RollSpeed
        {
            get => _rollSpeed;
            set => Set(ref _rollSpeed, value);
        }

        public SensorList Sensors { get; }

        public ObservableCollection<RCChannel> Channels { get; }
        public ObservableCollection<ServoOutput> ServoOutputs { get; }

        GeoLocation _location;
        public GeoLocation Location
        {
            get { return _location; }
            set { Set(ref _location, value); }
        }

    }
}
