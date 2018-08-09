using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class FlightController : GaugeBase
    {
        float _boardVoltage;
        public float BoardVoltage
        {
            get { return _boardVoltage; }
            set { Set(ref _boardVoltage, value); }
        }

        int _i2cErrorCount;
        public int I2CErrorCount
        {
            get { return _i2cErrorCount; }
            set { Set(ref _i2cErrorCount, value); }
        }

        public Version Version { get; private set; }

        private DateTime _systemTime;
        public DateTime SystemTime
        {
            get { return _systemTime; }
            set { Set(ref _systemTime, value); }
        }

        private string _flightVersion;
        public String FlightVersion
        {
            get { return _flightVersion; }
            set { Set(ref _flightVersion, value); }
        }

        private string _middleWareVersion;
        public String MiddleWareVersion
        {
            get { return _middleWareVersion; }
            set { Set(ref _middleWareVersion, value); }
        }

        private string _customVersion;
        public String CustomVersion
        {
            get { return _customVersion; }
            set { Set(ref _customVersion, value); }
        }

        private string _boardVersion;
        public String BoardVersion
        {
            get { return _boardVersion; }
            set { Set(ref _boardVersion, value); }
        }

        private string _vendorId;
        public String VendorId
        {
            get { return _vendorId; }
            set { Set(ref _vendorId, value); }
        }

        private string _productId;
        public String ProductId
        {
            get { return _productId; }
            set { Set(ref _productId, value); }
        }

        
        private string _osVersion;
        public String OSVersion
        {
            get { return _osVersion; }
            set { Set(ref _osVersion, value); }
        }

        private bool _missionInt;
        public bool MissionInt
        {
            get { return _missionInt; }
        }

        private bool _compassCalibration;
        public bool CompassCalibration
        {
            get { return _compassCalibration; }
        }

        private bool _mavLink2;
        public bool MavLink2
        {
            get { return _mavLink2; }
        }

        private byte _mavLinkVersion;
        public byte MavlinkVersion
        {
            get { return _mavLinkVersion; }
            set { Set(ref _mavLinkVersion, value); }
        }

        public void Update(UasHeartbeat hb)
        {

            MavlinkVersion = hb.MavlinkVersion;
        }

        public void Update(UasSystemTime sysTime)
        {
            DateTime date1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            try
            {
                SystemTime = date1.AddMilliseconds(sysTime.TimeUnixUsec / 1000);
            }
            catch (Exception) {  /* NOP */}
          
      }

        public void Update(UasHwstatus hwStatus)
        {
            I2CErrorCount = hwStatus.I2cerr;
            BoardVoltage = hwStatus.Vcc / 1000.0f;

            TimeStamp = DateTime.Now;
            GaugeStatus = GaugeStatus.OK;
        }
  
   
        public void Update(UasAutopilotVersion version)
        {
            CustomVersion = version.MiddlewareCustomVersion.ToString();
            MiddleWareVersion = version.MiddlewareSwVersion.ToString();
            OSVersion = version.OsSwVersion.ToString();
            VendorId = version.VendorId.ToString();
            ProductId = version.ProductId.ToString();
            BoardVersion = version.BoardVersion.ToString();

            var main = (byte)(version.FlightSwVersion >> 24);
            var sub = (byte)((version.FlightSwVersion >> 16) & 0xff);
            var rev = (byte)((version.FlightSwVersion >> 8) & 0xff);
            var type = (FirmwareVersionType)(version.FlightSwVersion& 0xff);

            FlightVersion = $"{main}.{sub}.{rev}.{type}";

            var capability = (uint)(MavProtocolCapability)version.Capabilities;
            _missionInt = (capability & (uint)MavProtocolCapability.MissionInt) > 0;
            _mavLink2 = (capability & (uint)MavProtocolCapability.Mavlink2) > 0;
            _compassCalibration = (capability & (uint)MavProtocolCapability.CompassCalibration) > 0;

        }
    }
}
