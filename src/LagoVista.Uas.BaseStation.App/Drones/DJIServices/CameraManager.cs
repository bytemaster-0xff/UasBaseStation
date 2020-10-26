using DJI.WindowsSDK;
using LagoVista.Uas.Core;

namespace LagoVista.Uas.BaseStation.ControlApp.Drones.DJIServices
{
    public class CameraManager : ICameraManager
    {
        public void AdjustView(double pitch, double roll, double yaw)
        {
            DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).RotateByAngleAsync(new GimbalAngleRotation()
            {
                 pitch = pitch,
                 roll = roll,
                 yaw = yaw,
            });
        }

        public void Calibrate()
        {
            DJISDKManager.Instance.ComponentManager.GetGimbalHandler(0, 0).CalibrateGimbalAsync();
        }

        public void StartRecording()
        {
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).StartRecordAsync();
        }

        public void StopRecording()
        {
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).StopRecordAsync();
        }

        public void TakePhoto()
        {
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).StartShootPhotoAsync();
        }
    }
}
