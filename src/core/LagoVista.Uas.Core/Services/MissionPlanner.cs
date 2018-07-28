using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core.Services
{
    public class MissionPlanner : IMissionPlanner
    {
        public Task GetWayPoints(IUas drone, ITransport link)
        {
            throw new NotImplementedException();
        }
    }
}
