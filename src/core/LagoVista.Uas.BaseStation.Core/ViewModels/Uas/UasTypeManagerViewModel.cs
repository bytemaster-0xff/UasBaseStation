using LagoVista.Client.Core.ViewModels;
using LagoVista.Client.Devices;
using LagoVista.Core.Models;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.DeviceAdmin.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels.Uas
{
    public class UasTypeManagerViewModel : AppViewModelBase
    {

        ObservableCollection<DeploymentInstanceSummary> _applications;
        ObservableCollection<DeviceTypeSummary> _deviceTypes;
        ObservableCollection<EntityHeader> _includedDeviceTypes;

        IDeviceManagementClient _dmClient;
        public UasTypeManagerViewModel(IDeviceManagementClient dmClient)
        {
            _dmClient = dmClient;
        }

        public override async Task InitAsync()
        {
            await PerformNetworkOperation(async () =>
            {
                var items = (await RestClient.GetListResponseAsync<DeploymentInstanceSummary>("/api/deployment/instances", null)).Model;
                Applications = new ObservableCollection<DeploymentInstanceSummary>(items);
                DeviceTypes = new ObservableCollection<DeviceTypeSummary>( (await _dmClient.GetDeviceTypesAsync()).Model);
            });
        }

        public ObservableCollection<DeploymentInstanceSummary> Applications
        {
            get { return _applications; }
            set { Set(ref _applications, value); }
        }

        public ObservableCollection<DeviceTypeSummary> DeviceTypes
        {
            get { return _deviceTypes; }
            set { Set(ref _deviceTypes, value); }
        }

        public ObservableCollection<EntityHeader> IncludedDeviceTypes
        {
            get { return _includedDeviceTypes; }
            set { Set(ref _includedDeviceTypes, value); }
        }

        DeviceTypeSummary _deviceTypeSummary;
        public DeviceTypeSummary SelectedDeviceType
        {
            get { return _deviceTypeSummary; }
            set { Set(ref _deviceTypeSummary, value); }
        }

        DeploymentInstanceSummary _instanceSummary;
        public DeploymentInstanceSummary SelectedApp
        {
            get { return _instanceSummary; }
            set { Set(ref _instanceSummary, value); }
        }

    }
}
