using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class PID : ModelBase
    {
        private float _ff;
        public float FF
        {
            get { return _ff; }
            set { Set(ref _ff, value); }
        }

        private float _p;
        public float P
        {
            get { return _p; }
            set { Set(ref _p, value); }
        }

        private float _i;
        public float I
        {
            get { return _i; }
            set { Set(ref _i, value); }
        }

        private float _d;
        public float D
        {
            get { return _d; }
            set { Set(ref _d, value); }
        }

        public byte Axis { get; }

        private float _desired;
        public float Desired
        {
            get { return _desired; }
            set { Set(ref _desired, value); }
        }

        private float _achieved;
        public float Achieved
        {
            get { return _achieved; }
            set { Set(ref _achieved, value); }
        }

    }
}
