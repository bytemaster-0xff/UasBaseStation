using LagoVista.Uas.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core
{
    public interface IMissionPlanner
    {
        Task GetWayPoints(IUas drone, IChannel link);
    }
}
