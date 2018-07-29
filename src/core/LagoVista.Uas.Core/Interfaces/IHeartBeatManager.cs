using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core
{
    public interface IHeartBeatManager
    {
        void Start(ITransport transport, TimeSpan interval);
        void Stop();
    }
}
