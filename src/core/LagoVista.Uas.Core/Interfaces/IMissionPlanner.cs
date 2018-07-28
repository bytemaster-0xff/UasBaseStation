using System.Threading.Tasks;

namespace LagoVista.Uas.Core
{
    public interface IMissionPlanner
    {
        Task GetWayPoints(IUas drone, ITransport link);
    }
}
