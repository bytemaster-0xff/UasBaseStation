using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class ConnectedUas : IConnectedUas
    {
        public ConnectedUas(IUas uas, ITransport transport)
        {
            Uas = uas;
            Transport = transport;
        }

        public IUas Uas { get; }
        public ITransport Transport { get; }
    }

    public interface IConnectedUas
    {
        IUas Uas { get; }

        ITransport Transport { get; }
    }
}
