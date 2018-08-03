using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Configuration : ModelBase
    {
        ObservableCollection<StreamReadConfig> _streamReadConfigs;
        public ObservableCollection<StreamReadConfig> StreamReadConfigurations
        {
            get { return _streamReadConfigs; }
            set { Set(ref _streamReadConfigs, value); }
        }

    }
}
