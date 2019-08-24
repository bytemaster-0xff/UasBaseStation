using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Services
{
    public class CameraGimbal
    {
        private IConnectedUasManager _connectUasManager;

        public CameraGimbal(IConnectedUasManager connectUasManager)
        {
            this._connectUasManager = connectUasManager ?? throw new ArgumentNullException(nameof(connectUasManager));
        }

        public void SetManualControl()
        {
            if(this._connectUasManager != null)
            {
                //var msg = UasCommands.SetCameraMode(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, CAMERA_MO)

                //this._connectUasManager.Active.Uas.
            }
        }

        public void SetAutoControl()
        {

        }

        public void SetPitchRoll(int pitch, int roll)
        {

        }
    
    }
}
