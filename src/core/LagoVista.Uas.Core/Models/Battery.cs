using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Battery : GaugeBase
    {
        const int CELL_COUNT = 6;

        public Battery()
        {
            Cells = new ObservableCollection<float>();
            for (var idx = 0; idx < CELL_COUNT; ++idx) Cells.Add(0);
        }

        float _voltage;
        public float Voltage
        {
            get { return _voltage; }
            set
            {
                Set(ref _voltage, value);
                RaisePropertyChanged(nameof(Watts));
            }
        }

        public ObservableCollection<float> Cells { get; private set; }

        float _remainingPercent;
        public float RemainingPercent
        {
            get { return _remainingPercent; }
            set { Set(ref _remainingPercent, value); }
        }

        float _usedMah;
        public float UsedMAH
        {
            get { return _usedMah; }
            set { Set(ref _usedMah, value); }
        }

        float _current;
        public float Current
        {
            get { return _current; }
            set
            {
                Set(ref _current, value);
                RaisePropertyChanged(nameof(Watts));
            }
        }

        float _temperature;
        public float Temperature
        {
            get { return _temperature; }
            set
            {
                Set(ref _temperature, value);
            }
        }

        int _timeRemaining;
        public int TimeRemaining
        {
            get { return _timeRemaining; }
            set
            {
                Set(ref _timeRemaining, value);
            }
        }

        public float Watts { get { return _current * _voltage; } }

        public void Update(UasBatteryStatus status)
        {
            if (status.Voltages[0] != ushort.MaxValue)
            {
                for (var idx = 0; idx < CELL_COUNT; ++idx)
                {
                    Cells[idx] = status.Voltages[idx] / 1000.0f;
                }
            }

            Temperature = status.Temperature;
            TimeRemaining = status.TimeRemaining;
            UsedMAH = status.CurrentConsumed;
            RemainingPercent = status.BatteryRemaining;

            TimeStamp = DateTime.Now;
        }
    }
}
