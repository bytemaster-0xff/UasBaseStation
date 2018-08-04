using LagoVista.Uas.Core.MavLink;
using System;

namespace LagoVista.Uas.Core.Models
{
    public class OpticFlow : GaugeBase
    {
        private float _x;
        public float X
        {
            get { return _x; }
            set { Set(ref _x, value); }
        }

        private float _y;
        public float Y
        {
            get { return _y; }
            set { Set(ref _y, value); }
        }

        private float _componentX;
        public float ComponentX
        {
            get { return _componentX; }
            set { Set(ref _componentX, value); }
        }

        private float _componentY;
        public float ComponentY
        {
            get { return _componentY; }
            set { Set(ref _componentY, value); }
        }

        private float _rateX;
        public float RateX
        {
            get { return _rateX; }
            set { Set(ref _rateX, value); }
        }

        private float _rateY;
        public float RateY
        {
            get { return _rateY; }
            set { Set(ref _rateY, value); }
        }

        private float _quality;
        public float Quality
        {
            get { return _quality; }
            set { Set(ref _quality, value); }
        }

        public void Update(UasOpticalFlow flow)
        {
            X = flow.FlowX;
            Y = flow.FlowY;
            RateX = flow.FlowRateX;
            RateY = flow.FlowRateY;
            Quality = flow.Quality;
            ComponentX = flow.FlowCompMX;
            ComponentY = flow.FlowCompMY;
            TimeStamp = DateTime.Now;

            //TODO: need to look at quality to determine error
            GaugeStatus = GaugeStatus.OK;
        }
    }
}
