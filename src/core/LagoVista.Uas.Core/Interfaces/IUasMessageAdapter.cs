using LagoVista.Uas.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core
{
    public interface IUasMessageAdapter
    {
        void UpdateUas(IUas uas, UasMessage message);
    }
}
