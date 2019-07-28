using System;
using System.Collections.ObjectModel;
using LagoVista.Core.Models;
using LagoVista.Uas.Core.Models;

namespace LagoVista.Uas.Core.Services
{
    public class ConnectedUasManager : ModelBase, IConnectedUasManager
    {
        public ConnectedUasManager()
        {
            All = new ObservableCollection<IConnectedUas>();
        }

        IConnectedUas _active;

        public event EventHandler<IConnectedUas> DroneConnected;
        public event EventHandler<IConnectedUas> DroneDiconnected;
        public event EventHandler<IConnectedUas> ActiveDroneChanged;

        public IConnectedUas Active
        {
            get { return _active; }
            private set { Set(ref _active, value); }
        }

        public ObservableCollection<IConnectedUas> All { get; }

        ObservableCollection<IConnectedUas> IConnectedUasManager.All => throw new NotImplementedException();

        public void SetActive(IConnectedUas connectedUas)
        {
            Active = connectedUas;
            if(!All.Contains(connectedUas))
            {
                All.Add(connectedUas);
            }
            
            if(Active != null)
            {
                ActiveDroneChanged?.Invoke(this, connectedUas);
            }
        }
    }
}
