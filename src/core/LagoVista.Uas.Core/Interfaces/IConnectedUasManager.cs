using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Uas.Core
{
    public interface IConnectedUasManager
    {
        event EventHandler<IConnectedUas> DroneConnected;

        event EventHandler<IConnectedUas> DroneDiconnected;

        event EventHandler<IConnectedUas> ActiveDroneChanged;


        IConnectedUas Active {get; }

        void SetActive(IConnectedUas connectedUas);

        ObservableCollection<IConnectedUas> All { get; }
    }
}
