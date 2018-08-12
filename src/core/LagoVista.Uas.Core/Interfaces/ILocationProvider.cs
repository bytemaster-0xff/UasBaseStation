using LagoVista.Core.Models.Geo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.Core.Interfaces
{
    public interface ILocationProvider
    {
        Task InitAsync();

        event EventHandler<GeoLocation> LocationUpdated;

        bool HasLocation { get; }

        bool HasLocationAccess { get; }

        GeoLocation CurrentLocation { get; }
    }
}
