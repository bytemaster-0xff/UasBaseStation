using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Battery : ModelBase
    {
        public Battery()
        {
            Cells = new ObservableCollection<float>();
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

        public float Watts { get { return _current * _voltage; } }
    }
}
