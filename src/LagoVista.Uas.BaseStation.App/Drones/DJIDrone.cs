using DJI.WindowsSDK;
using DJI.WindowsSDK.Components;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace LagoVista.Uas.BaseStation.ControlApp.Drones
{
    public class DJIDrone : UasBase
    {
        ComponentManager _componentMgr;
        IConnectedUasManager _mgr;

        private readonly CoreDispatcher _dispatcher;

        public DJIDrone(IConnectedUasManager mgr, CoreDispatcher dispatcher) : base(null)
        {
            Camera = new DJIServices.CameraManager();

            this._dispatcher = dispatcher;
            this._mgr = mgr ?? throw new ArgumentNullException(nameof(mgr));
            Task.Run(() =>
            {
                this.Init();
            });
        }

        public void Init()
        {
            DJISDKManager.Instance.EnableDebugLogSystem();
            DJISDKManager.Instance.SDKRegistrationStateChanged += Instance_SDKRegistrationStateChanged;
            DJISDKManager.Instance.AircraftBindingStateChanged += Instance_AircraftBindingStateChanged;

            DJISDKManager.Instance.RegisterApp("96ddcaa937503c0d37e9cdd9");
        }

        private void Instance_SDKRegistrationStateChanged(SDKRegistrationState state, SDKError errorCode)
        {
            if (errorCode == SDKError.NO_ERROR)
            {
                if (_componentMgr == null)
                {
                    this._componentMgr = DJISDKManager.Instance.ComponentManager;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).VelocityChanged += DJIDrone_VelocityChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AreMotorsOnChanged += DJIDrone_AreMotorsOnChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).CompassHasErrorChanged += DJIDrone_CompassHasErrorChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).FlightTimeInSecondsChanged += DJIDrone_FlightTimeInSecondsChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsLowBatteryWarningChanged += DJIDrone_IsLowBatteryWarningChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AttitudeChanged += DJIDrone_AttitudeChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsSeriousLowBatteryWarningChanged += DJIDrone_IsSeriousLowBatteryWarningChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AttitudeChanged += DJIDrone_AttitudeChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsFlyingChanged += DJIDrone_IsFlyingChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AltitudeChanged += DJIDrone_AltitudeChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AircraftLocationChanged += DJIDrone_AircraftLocationChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).SatelliteCountChanged += DJIDrone_SatelliteCountChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GPSSignalLevelChanged += DJIDrone_GPSSignalLevelChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).RemainingFlightTimeChanged += DJIDrone_RemainingFlightTimeChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).IsLandingConfirmationNeededChanged += DJIDrone_IsLandingConfirmationNeededChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightAssistantHandler(0, 0).IsAscentLimitedByObstacleChanged += DJIDrone_IsAscentLimitedByObstacleChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightAssistantHandler(0, 0).VissionDetectionStateChanged += DJIDrone_VissionDetectionStateChanged;

                    DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).VoltageChanged += DJIDrone_VoltageChanged;
                    DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).ChargeRemainingInPercentChanged += DJIDrone_ChargeRemainingInPercentChanged;
                    
                    DJISDKManager.Instance.ComponentManager.GetProductHandler(0).ProductTypeChanged += DJIDrone_ProductTypeChanged;
                }
            }
            else
            {
                Debug.WriteLine("REG: " + errorCode.ToString());
            }
        }

        private void DJIDrone_VissionDetectionStateChanged(object sender, VissionDetectionState? value)
        {
            if (value.HasValue)
            {
                var position = "?";
                position = value.Value.position.ToString();
                foreach (var sector in value.Value.detectionSectors)
                {
                    Debug.WriteLine("Distance " + position + " " + sector.obstacleDistanceInMeters + " " + sector.warningLevel);
                }
            }
        }

        private void DJIDrone_IsAscentLimitedByObstacleChanged(object sender, BoolMsg? value)
        {
            
        }

        private void DJIDrone_IsLandingConfirmationNeededChanged(object sender, BoolMsg? value)
        {
            
        }

        private void DJIDrone_GPSSignalLevelChanged(object sender, FCGPSSignalLevelMsg? value)
        {
            
        }

        private void DJIDrone_SatelliteCountChanged(object sender, IntMsg? value)
        {
            RunOnUIThread(() => GPSs.First().SateliteCount = value.HasValue ? value.Value.value : 0);
        }

        private void DJIDrone_RemainingFlightTimeChanged(object sender, IntMsg? value)
        {
            RunOnUIThread(() => Battery.TimeRemaining = TimeSpan.FromSeconds(value.HasValue ? value.Value.value : 0));
        }

        private void DJIDrone_ChargeRemainingInPercentChanged(object sender, IntMsg? value)
        {
            RunOnUIThread(() => Battery.RemainingPercent = value.HasValue ? value.Value.value : 0);
        }

        private void DJIDrone_IsFlyingChanged(object sender, BoolMsg? value)
        {
            RunOnUIThread(() => SystemStatus.IsFlying = value.HasValue ? value.Value.value : false);
        }

        private void DJIDrone_IsSeriousLowBatteryWarningChanged(object sender, BoolMsg? value)
        {
            RunOnUIThread(() => SystemStatus.CriticalBatteryWarning = value.HasValue ? value.Value.value : false);
        }

        private void DJIDrone_IsLowBatteryWarningChanged(object sender, BoolMsg? value)
        {
            RunOnUIThread(() => SystemStatus.LowBatteryWarning = value.HasValue ? value.Value.value : false);
        }

        private void DJIDrone_FlightTimeInSecondsChanged(object sender, IntMsg? value)
        {
            RunOnUIThread(() => SystemStatus.FlightTime = value.HasValue ? TimeSpan.FromSeconds(value.Value.value / 10) : TimeSpan.Zero);
        }

        private void DJIDrone_CompassHasErrorChanged(object sender, BoolMsg? value)
        {
            RunOnUIThread(() => SystemStatus.CompassNeedsCaliabration = value.HasValue ? value.Value.value : false);
        }

        private void DJIDrone_AreMotorsOnChanged(object sender, BoolMsg? value)
        {
            RunOnUIThread(() => SystemStatus.Armed = value.HasValue ? value.Value.value : false);
        }

        public async void RunOnUIThread(Action action)
        {
            await this._dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                action();
            });
        }

        private async void DJIDrone_ProductTypeChanged(object sender, ProductTypeMsg? value)
        {
            if (value.HasValue)
            {
                if (value.Value.value != ProductType.UNKNOWN &&
                    value.Value.value != ProductType.UNRECOGNIZED)
                {
                    var sn = (await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GetSerialNumberAsync()).value;
                    if (sn.HasValue)
                    {
                        this.UasSerialNumber = sn.Value.value;
                    }

                    var name = (await DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).GetAircraftNameAsync()).value;
                    if (name.HasValue)
                    {
                        this.UasName = name.Value.value;
                    }

                    this.UasType = value.Value.value.ToString();
                    RunOnUIThread(() => _mgr.SetActive(new ConnectedUas(this, new DJITransport(this))));
                    await DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).CalibrateGimbalAsync();
                }
                else
                {

                }

                Debug.WriteLine("PROD TYPE: " + value.Value.value.ToString());
            }

        }

        private void Instance_AircraftBindingStateChanged(AircraftBindingState state)
        {
            Debug.WriteLine($"AIRCRAFT STATE: {state}");
        }

        private void DJIDrone_VoltageChanged(object sender, IntMsg? value)
        {
            RunOnUIThread(() => this.Batteries.First().Voltage = value.HasValue ? Convert.ToSingle(value.Value.value / 1000.0) : 0);
        }

        private void DJIDrone_AircraftLocationChanged(object sender, LocationCoordinate2D? value)
        {
            RunOnUIThread(() =>
            {
                if (value.HasValue)
                {
                    if (Math.Abs(value.Value.latitude) > 0.5)
                    {
                        this.HasLocation = true;
                        this.Location = new LagoVista.Core.Models.Geo.GeoLocation(value.Value.latitude, value.Value.longitude);
                    }
                    else
                    {
                        this.HasLocation = false;
                    }
                }
            });
        }

        private void DJIDrone_AltitudeChanged(object sender, DoubleMsg? value)
        {
            RunOnUIThread(() =>
            {
                if (value.HasValue)
                {
                    this.RangeFinder.GaugeStatus = value.HasValue ? GaugeStatus.OK : GaugeStatus.Warning;
                    this.RangeFinder.Distance = Convert.ToSingle(value.Value.value);

                    if (this.Location != null && this.Location.Longitude.HasValue && this.Location.Latitude.HasValue)
                    {
                        this.Location  = new LagoVista.Core.Models.Geo.GeoLocation(this.Location.Latitude.Value, this.Location.Longitude.Value, Convert.ToSingle(value.Value.value));
                    }
                }
            });
        }

        private void DJIDrone_AttitudeChanged(object sender, DJI.WindowsSDK.Attitude? value)
        {
            RunOnUIThread(() =>
            {
                if (value.HasValue)
                {
                    this.Attitude.GaugeStatus = GaugeStatus.OK;
                    this.Attitude.Pitch = Convert.ToSingle(value.Value.pitch);
                    this.Attitude.Roll = Convert.ToSingle(value.Value.roll);
                    this.Attitude.Yaw = Convert.ToSingle(value.Value.yaw);
                }
                else
                {
                    this.Attitude.GaugeStatus = GaugeStatus.Warning;
                }
            });
        }

        private void DJIDrone_VelocityChanged(object sender, Velocity3D? value)
        {
            RunOnUIThread(() =>
            {
                if (value.HasValue)
                {
                    this.Attitude.GaugeStatus = GaugeStatus.OK;
                    this.Attitude.PitchSpeed = Convert.ToSingle(value.Value.x);
                    this.Attitude.RollSpeed = Convert.ToSingle(value.Value.y);
                    this.Attitude.YawSpeed = Convert.ToSingle(value.Value.z);
                }
                else
                {
                    this.Attitude.GaugeStatus = GaugeStatus.Warning;
                }
            });
        }
    }
}
