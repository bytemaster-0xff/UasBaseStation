using LagoVista.Core.Models.Geo;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core
{
    public interface INavigation : INotifyPropertyChanged
    {
        Task InitAsync();
        void Takeoff();
        void GoToLocation();
        void Arm();
        void Disarm();
        void Land();
        void ReturnToHome();
        void GetHomePosition();
        Mission Mission { get; set; }
    }
}
