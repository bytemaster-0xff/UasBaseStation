using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Commanding;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Uas.Core.Interfaces;

namespace LagoVista.Uas.BaseStation.Core.ViewModels.Missions
{
    public class MissionPlannerViewModel : AppViewModelBase, INavigationProvider
    {
        IConnectedUasManager _connectedUasManager;
        IMissionPlanner _missionPlanner;

        public MissionPlannerViewModel(IConnectedUasManager connectedUasManager, INavigation navigation)
        {
            Navigation = navigation;

            _connectedUasManager = connectedUasManager;
        }

        public override async Task InitAsync()
        {
            await Navigation.InitAsync();
            await base.InitAsync();
        }

        public INavigation Navigation { get; }
     }
}
