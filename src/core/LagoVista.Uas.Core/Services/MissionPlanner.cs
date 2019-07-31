using LagoVista.Core.Validation;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core.Services
{
    public class MissionPlanner : IMissionPlanner
    {
        private readonly IConnectedUasManager _uasMgr;

        public MissionPlanner(IConnectedUasManager uasMgr)
        {
            this._uasMgr = uasMgr;
        }

        public async Task<InvokeResult<Mission>> GetWayPointsAsync(IConnectedUas uas)
        {
            var mission = new Mission();

            var missionRequest = new UasMissionRequestList
            {
                TargetSystem = uas.Uas.SystemId,
                TargetComponent = uas.Uas.ComponentId
            };

            var result = await uas.Transport.RequestDataAsync<UasMissionCount>(missionRequest, UasMessages.MissionCount);
            if(result.Successful)
            {
                for (ushort idx = 0; idx < result.Result.Count; ++idx)
                {
                    var reqf = new UasMissionRequestInt
                    {
                        TargetSystem = uas.Uas.SystemId,
                        TargetComponent = uas.Uas.ComponentId,
                        Seq = idx
                    };

                    var wpResult = await uas.Transport.RequestDataAsync<UasMissionItemInt>(reqf, UasMessages.MissionItemInt);
                    if (wpResult.Successful)
                    {
                        mission.Waypoints.Add(Waypoint.Create(wpResult.Result));
                    }
                }
            }

            return InvokeResult<Mission>.Create(mission);
        }

        public Task<InvokeResult> UpdateWaypoints(IConnectedUas uas, Mission mission)
        {
            var msg = new UasMissionWritePartialList();
            uas.Transport.SendMessage(msg);
            return Task.FromResult(InvokeResult.Success);
        }
    }
}
