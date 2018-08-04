using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Uas.Core
{
    public interface IConnectedUasManager
    {
        ConnectedUas Active {get; set;}
    
        ObservableCollection<ConnectedUas> All { get; }
    }
}
