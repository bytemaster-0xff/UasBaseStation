using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Targets : ModelBase
    {
        private float _airSpeed;
        public float AirSpeed
        {
            get { return _airSpeed; }
            set { Set(ref _airSpeed, value); }
        }
    }
}

