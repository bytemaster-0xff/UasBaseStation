using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Comms : GaugeBase
    {
        private float _distanceToHome;

        //TODO: No idea!
        public static float _multiplierDistance = 1;

        private float _rssi;
        public float RSSI
        {
            get { return _rssi; }
            set { Set(ref _rssi, value); }
        }

        private float _remoteRSSI;
        public float RemoteRSSI
        {
            get { return _remoteRSSI; }
            set { Set(ref _remoteRSSI, value); }
        }

        int _txBuffer;
        public int TxBuffer
        {
            get { return _txBuffer; }
            set { Set(ref _txBuffer, value); }
        }

        private float _noise;
        public float Noise
        {
            get { return _noise; }
            set { Set(ref _noise, value); }
        }

        private float _remoteNoise;
        public float RemoteNoise
        {
            get { return _remoteNoise; }
            set { Set(ref _remoteNoise, value); }
        }

        DateTime _lastRSSI;
        public DateTime LastRSSI
        {
            get { return _lastRSSI; }
            set { Set(ref _lastRSSI, value); }
        }


        DateTime _lastRemoteRSSI;
        public DateTime LastRemoteRSSI
        {
            get { return _lastRemoteRSSI; }
            set { Set(ref _lastRemoteRSSI, value); }
        }

        private float _lcoalSNRDb;

        public float LocalSNRDb
        {
            get
            {
                if (_lastRSSI.AddSeconds(1) > DateTime.Now)
                {
                    return _lcoalSNRDb;
                }
                _lastRSSI = DateTime.Now;
                _lcoalSNRDb = ((_rssi - _noise) / 1.9f) * 0.5f + _lcoalSNRDb * 0.5f;
                return _lcoalSNRDb;
            }
        }

        private float _remoteSNRDb;

        public float RemoteSNRDb
        {
            get
            {
                if (_lastRemoteRSSI.AddSeconds(1) > DateTime.Now)
                {
                    return _remoteSNRDb;
                }
                _lastRemoteRSSI = DateTime.Now;
                _remoteSNRDb = ((_remoteRSSI - _remoteNoise) / 1.9f) * 0.5f + _remoteSNRDb * 0.5f;
                return _remoteSNRDb;
            }
        }

        public float DistRSSIRemaining
        {
            get
            {
                float work = 0;
                if (_lcoalSNRDb == 0)
                {
                    return 0;
                }
                if (_lcoalSNRDb > _remoteSNRDb)
                {
                    // remote
                    // minus fade margin
                    work = _remoteSNRDb - 5;
                }
                else
                {
                    // local
                    // minus fade margin
                    work = _lcoalSNRDb - 5;
                }

                {
                    float dist = _distanceToHome / _multiplierDistance;

                    work = dist * (float)Math.Pow(2.0, work / 6.0);
                }

                return work;
            }
        }

        public void Update(UasRadioStatus status, float distanceToHome)
        {
            Noise = status.Noise;
            RemoteNoise = status.Remnoise;
            RSSI = status.Rssi;
            RemoteRSSI = status.Remrssi;

            LastRemoteRSSI = DateTime.Now;
            LastRSSI = DateTime.Now;

            TxBuffer = status.Txbuf;

            RaisePropertyChanged(nameof(RemoteSNRDb));
            RaisePropertyChanged(nameof(LocalSNRDb));
            RaisePropertyChanged(nameof(DistRSSIRemaining));

            GaugeStatus = GaugeStatus.OK;
            TimeStamp = DateTime.Now;
        }
    }
}
