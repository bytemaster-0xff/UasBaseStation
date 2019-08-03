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

namespace LagoVista.Uas.BaseStation.App.Drones
{
    public class DJIDrone : UasBase
    {
        ComponentManager _componentMgr;
        IConnectedUasManager _mgr;

        private readonly CoreDispatcher _dispatcher;

        public DJIDrone(IConnectedUasManager mgr, CoreDispatcher dispatcher) : base(null)
        {
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

        private async void Instance_SDKRegistrationStateChanged(SDKRegistrationState state, SDKError errorCode)
        {
            if (errorCode == SDKError.NO_ERROR)
            {
                if (_componentMgr == null)
                {
                    this._componentMgr = DJISDKManager.Instance.ComponentManager;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).VelocityChanged += DJIDrone_VelocityChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AttitudeChanged += DJIDrone_AttitudeChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AltitudeChanged += DJIDrone_AltitudeChanged;
                    DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AircraftLocationChanged += DJIDrone_AircraftLocationChanged;
                    DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).VoltageChanged += DJIDrone_VoltageChanged;
                    DJISDKManager.Instance.ComponentManager.GetProductHandler(0).ProductTypeChanged += DJIDrone_ProductTypeChanged;
                }

                if (_mgr.Active == null)
                {
                    await this._dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        _mgr.SetActive(new ConnectedUas(this, new DJITransport(this)));
                    });
                }
                Debug.WriteLine("Product Registered");
            }
            else
            {
                Debug.WriteLine("REG: " + errorCode.ToString());
            }
        }

        public async void RunOnUIThread(Action action)
        {
            await this._dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                action();
            });
        }

        private void DJIDrone_ProductTypeChanged(object sender, ProductTypeMsg? value)
        {
            if (value.HasValue)
            {
                Debug.WriteLine("PROD TYPE: " + value.Value.value.ToString());
            }

        }

        private void Instance_AircraftBindingStateChanged(AircraftBindingState state)
        {
            if (state == AircraftBindingState.BOUND)
            {
                Debug.WriteLine("AIRCRAFT BOUND");
            }
            else
            {
                Debug.WriteLine(state);
            }

        }

        private void DJIDrone_VoltageChanged(object sender, IntMsg? value)
        {
            RunOnUIThread(() =>
            {
                if (value.HasValue)
                {
                    Debug.WriteLine(value.Value.value);
                    if (!this.Batteries.Any())
                    {
                        var batt = new Battery();
                    }

                    this.Batteries.First().Voltage = Convert.ToSingle(value.Value.value / 1000.0);
                }
            });
        }

        private void DJIDrone_AircraftLocationChanged(object sender, LocationCoordinate2D? value)
        {
            RunOnUIThread(() =>
            {
                if (value.HasValue)
                {
                    this.Location.Latitude = value.Value.latitude;
                    this.Location.Longitude = value.Value.longitude;
                }
            });
        }

        private void DJIDrone_AltitudeChanged(object sender, DoubleMsg? value)
        {
            RunOnUIThread(() =>
            {
                this.RangeFinder.GaugeStatus = value.HasValue ? GaugeStatus.OK : GaugeStatus.Warning;
                if (value.HasValue)
                {
                    this.RangeFinder.Distance = Convert.ToSingle(value.Value.value);
                    this.Location.Altitude = Convert.ToSingle(value.Value.value);
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
