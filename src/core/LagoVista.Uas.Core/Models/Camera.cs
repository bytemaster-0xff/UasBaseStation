using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Camera : ModelBase
    {
        private short _pan;
        public short Pan
        {
            get => _pan;
            set => Set(ref _pan, value);
        }


        private short _tilt;
        public short Tilt
        {
            get => _tilt;
            set => Set(ref _tilt, value);
        }
    }
}
