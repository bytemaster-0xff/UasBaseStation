using LagoVista.Core.PlatformSupport;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;

namespace LagoVista.Uas.Core.Services
{
    public class HeartBeatManager : IHeartBeatManager
    {
        ITimerFactory _timerFactory;
        ITimer _timer;
        MavLinkState _uavState = new MavLinkState();

        public HeartBeatManager(ITimerFactory timerFactory)
        {
            _timerFactory = timerFactory;
        }

        ITransport _transport;
        public void Start(ITransport transport, TimeSpan interval)
        {
            _transport = transport;
            if(_timer != null)
            {
                throw new Exception("Timer already started.");
            }

            var versionRequest = new UasAutopilotVersionRequest();

            _transport.SendMessage(versionRequest);

            _timer = _timerFactory.Create(interval);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            foreach (var m in _uavState.GetHeartBeatObjects())
            {
                _transport.SendMessage(m);
            }
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
        }
    }
}
