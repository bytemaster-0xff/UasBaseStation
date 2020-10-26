using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core
{
    public interface ICameraManager
    {
        void Calibrate();
        void AdjustView(double pitch, double roll, double yaw);
        void StartRecording();
        void StopRecording();
        void TakePhoto();
    }
}
