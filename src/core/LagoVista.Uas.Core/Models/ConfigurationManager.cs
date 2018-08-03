using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class ConfigurationManager : ModelBase, IConfigurationManager
    {
        public ConfigurationManager()
        {
            Current = new Configuration()
            {
                StreamReadConfigurations = new ObservableCollection<StreamReadConfig>()
                 {
                     new StreamReadConfig() { Stream = MavLink.MavDataStream.ExtendedStatus, UpdateHz = 2},
                     new StreamReadConfig() { Stream = MavLink.MavDataStream.Position, UpdateHz = 2},
                     new StreamReadConfig() { Stream = MavLink.MavDataStream.Extra1, UpdateHz = 4},
                     new StreamReadConfig() { Stream = MavLink.MavDataStream.Extra2, UpdateHz = 4},
                     new StreamReadConfig() { Stream = MavLink.MavDataStream.Extra3, UpdateHz = 4},
                     new StreamReadConfig() { Stream = MavLink.MavDataStream.RawSensors, UpdateHz = 4},
                     new StreamReadConfig() { Stream = MavLink.MavDataStream.RcChannels, UpdateHz = 2},
                 }
            };
        }

        Configuration _current;
        public Configuration Current
        {
            get { return _current; }
            set { Set(ref _current, value); }
        }
    }
}
