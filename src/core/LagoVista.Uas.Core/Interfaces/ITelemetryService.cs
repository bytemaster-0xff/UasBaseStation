using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core
{
    public interface ITelemetryService
    {
        void Start(IUas usa, ITransport transport);
        void Stop(ITransport transport);
    }
}
