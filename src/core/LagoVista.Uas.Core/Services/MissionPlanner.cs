using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core.Services
{
    public class MissionPlanner : IMissionPlanner
    {
        public async Task GetWayPoints(IUas uas, ITransport link)
        {
            var missionRequest = new UasMissionRequestList();
            missionRequest.TargetSystem = uas.SystemId;
            missionRequest.TargetComponent = uas.ComponentId;

            var result = await link.RequestDataAsync<UasMissionCount>(missionRequest, UasMessages.MissionCount);
            if(result.Successful)
            {
                Debug.WriteLine($"Get go our response {result.Result.Count}");

                for (ushort idx = 0; idx < result.Result.Count; ++idx)
                {
                    var reqf = new UasMissionRequestInt();

                    reqf.TargetSystem = uas.SystemId;
                    reqf.TargetComponent = uas.ComponentId;

                    reqf.Seq = idx;
                    var wpResult = await link.RequestDataAsync<UasMissionItemInt>(reqf, UasMessages.MissionItemInt);
                    if (wpResult.Successful)
                    {
                        Debug.WriteLine(wpResult.Result.X + " " + wpResult.Result.Y + " " + wpResult.Result.Z + " " + wpResult.Result.Param1 + " " + wpResult.Result.Param2 + " " + wpResult.Result.Param3);
                    }
                    else
                    {
                        Debug.WriteLine($"No joy on {idx}");
                    }
                }

                
            }

        }
    }
}
