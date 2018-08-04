using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class EKF : GaugeBase
    {
        float _status;
        public float Status
        {
            get { return _status; }
            set { Set(ref _status, value); }
        }

        int _flags;
        public int Flags
        {
            get { return _flags; }
            set { Set(ref _flags, value); }
        }
       
        float _airSpeedVariance;
        public float AirspeedVariance
        {
            get { return _airSpeedVariance; }
            set { Set(ref _airSpeedVariance, value); }
        }

        float _velocityVariance;
        public float VeolocityVariance
        {
            get { return _velocityVariance; }
            set { Set(ref _velocityVariance, value); }
        }


        float _compassVariance;
        public float CompassVariance
        {
            get { return _compassVariance; }
            set { Set(ref _compassVariance, value); }
        }

        float _positionHorizontal;
        public float PositionHorizontalVariance
        {
            get { return _positionHorizontal; }
            set { Set(ref _positionHorizontal, value); }
        }

        float _postionVertical;
        public float PositionVerticalVariance
        {
            get { return _postionVertical; }
            set { Set(ref _postionVertical, value); }
        }

        float _terrainAltitudeVariance;
        public float TerrainAltitudeVariance
        {
            get { return _terrainAltitudeVariance; }
            set { Set(ref _terrainAltitudeVariance, value); }
        }


        public void Update(UasEkfStatusReport value)
        {
            PositionHorizontalVariance = value.PosHorizVariance;
            PositionVerticalVariance = value.PosHorizVariance;
            TerrainAltitudeVariance = value.TerrainAltVariance;
            CompassVariance = value.CompassVariance;
            AirspeedVariance = value.AirspeedVariance;
            VeolocityVariance = value.VelocityVariance;

            Flags = value.Flags;
            Status = (float)
                        Math.Max(_velocityVariance,
                            Math.Max(CompassVariance, Math.Max(PositionHorizontalVariance, Math.Max(PositionVerticalVariance, TerrainAltitudeVariance))));


            Errors.Clear();

            if (PositionHorizontalVariance >= 1) Errors.Add(new Error(CoreResources.Common_Error + String.Format(CoreResources.Err_HPos, CompassVariance), ErrorLevel.Error)); 
            if (PositionVerticalVariance >= 1) Errors.Add(new Error(CoreResources.Common_Error + String.Format(CoreResources.Err_VPos, CompassVariance), ErrorLevel.Error));
            if (TerrainAltitudeVariance >= 1) Errors.Add(new Error(CoreResources.Common_Error + String.Format(CoreResources.Err_TerrAlt, CompassVariance), ErrorLevel.Error));
            if (CompassVariance >= 1) Errors.Add(new Error(CoreResources.Common_Error + String.Format(CoreResources.Err_Comp, CompassVariance), ErrorLevel.Error));
            if (AirspeedVariance >= 1) Errors.Add(new Error(CoreResources.Common_Error + String.Format(CoreResources.Err_AirSpeed, CompassVariance), ErrorLevel.Error));
            if (VeolocityVariance >= 1) Errors.Add(new Error(CoreResources.Common_Error + String.Format(CoreResources.Err_Velocity, CompassVariance), ErrorLevel.Error));

            GaugeStatus = Errors.Count > 0 ? GaugeStatus.Error : GaugeStatus.OK;
            TimeStamp = DateTime.Now;
        }
    }
}
