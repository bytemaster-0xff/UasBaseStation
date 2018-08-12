using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Interfaces
{
    public interface INavigationProvider
    {
        INavigation Navigation { get; }
    }
}
