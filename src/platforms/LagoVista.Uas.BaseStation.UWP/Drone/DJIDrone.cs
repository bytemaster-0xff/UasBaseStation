using DJI.WindowsSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.UWP.Drone
{
    public class DJIDrone
    {
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
                DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).VelocityChanged += DJIDrone_VelocityChanged;
                DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AttitudeChanged += DJIDrone_AttitudeChanged;
                DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AltitudeChanged += DJIDrone_AltitudeChanged;
                DJISDKManager.Instance.ComponentManager.GetFlightControllerHandler(0, 0).AircraftLocationChanged += DJIDrone_AircraftLocationChanged;
                DJISDKManager.Instance.ComponentManager.GetBatteryHandler(0, 0).VoltageChanged += DJIDrone_VoltageChanged;
                DJISDKManager.Instance.ComponentManager.GetProductHandler(0).ProductTypeChanged += DJIDrone_ProductTypeChanged;
                Debug.WriteLine("Product Registered");
            }
            else
            {
                Debug.WriteLine(errorCode.ToString());
            }
        }

        private void DJIDrone_ProductTypeChanged(object sender, ProductTypeMsg? value)
        {
            if (value.HasValue)
            {
                Debug.WriteLine(value.Value.value.ToString());
            }

        }

        private void Instance_AircraftBindingStateChanged(AircraftBindingState state)
        {
            if (state == AircraftBindingState.BOUND)
            {
                
            }
        }

        private void DJIDrone_VoltageChanged(object sender, IntMsg? value)
        {
            if(value.HasValue)
            {
                Debug.WriteLine(value.Value.value);
            }
        }

        private void DJIDrone_AircraftLocationChanged(object sender, LocationCoordinate2D? value)
        {
            if (value.HasValue)
            {
                Debug.WriteLine(value.Value.latitude + " " + value.Value.longitude);
            }
        }

        private void DJIDrone_AltitudeChanged(object sender, DoubleMsg? value)
        {
            
        }

        private void DJIDrone_AttitudeChanged(object sender, Attitude? value)
        {
            if(value.HasValue)
            {
                Debug.WriteLine(value.Value.yaw);
                Debug.WriteLine(value.Value.pitch);
                Debug.WriteLine(value.Value.roll);
            }
        }

        private void DJIDrone_VelocityChanged(object sender, Velocity3D? value)
        {

        }
    }
}
