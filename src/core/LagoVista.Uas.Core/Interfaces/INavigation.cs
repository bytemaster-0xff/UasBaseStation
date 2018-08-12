using LagoVista.Core.Models.Geo;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core
{
    public interface INavigation
    {
        void Takeoff();
        void GoToLocation();
        void Arm();
        void Disarm();
        void Land();
        void ReturnToHome();
        void GetHomePosition();
    }
}
