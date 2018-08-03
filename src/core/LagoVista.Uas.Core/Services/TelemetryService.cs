using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Services
{
    public class TelemetryService : ITelemetryService
    {
        ITransport _transport;
        IConfigurationManager _configManager;

        public TelemetryService(IConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        public void Start(IUas uas, ITransport transport)
        {
            _transport = transport;

            foreach(var config in _configManager.Current.StreamReadConfigurations)
            {
                StartDataStream(config);
            }
        }


        public void Stop(ITransport transport)
        {
            _transport = transport;

            foreach (var config in _configManager.Current.StreamReadConfigurations)
            {
                StopDataStream(config.Stream);
            }
        }

        private void StartDataStream(StreamReadConfig config)
        {
            var msg = new UasRequestDataStream();
            msg.ReqMessageRate = config.UpdateHz;
            msg.ReqStreamId = (byte)config.Stream;
            msg.StartStop = 1; // start;

            _transport.SendMessage(msg);
            _transport.SendMessage(msg);
        }

        private void StopDataStream(MavDataStream stream)
        {
            var msg = new UasRequestDataStream();
            msg.ReqMessageRate = 0;
            msg.ReqStreamId = (byte)stream;
            msg.StartStop = 0; // start;

            _transport.SendMessage(msg);
            _transport.SendMessage(msg);
        }

    }
}
