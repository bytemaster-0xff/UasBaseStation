using LagoVista.Core.Validation;
using LagoVista.Uas.Core.Models;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core
{
    public interface IMissionPlanner
    {
        Task<InvokeResult<Mission>> GetWayPointsAsync(ConnectedUas uas);
    }
}
