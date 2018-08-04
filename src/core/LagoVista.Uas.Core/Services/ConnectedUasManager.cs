using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LagoVista.Core.Models;
using LagoVista.Uas.Core.Models;

namespace LagoVista.Uas.Core.Services
{
    public class ConnectedUasManager : ModelBase, IConnectedUasManager, INotifyPropertyChanged
    {
        public ConnectedUasManager()
        {
            All = new ObservableCollection<ConnectedUas>();
        }

        ConnectedUas _active;
        public ConnectedUas Active
        {
            get { return _active; }
            set { Set(ref _active, value); }
        }

        public ObservableCollection<ConnectedUas> All
        {
            get;
            private set;
        }
    }
}
