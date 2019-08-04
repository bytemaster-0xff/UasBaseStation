using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Uas.Core.FlightRecorder
{
    public class FlightRecorder : ObservableCollection<FlightEvent>, IFlightRecorder
    {
        LagoVista.Core.IDispatcherServices _dispatcher;

        public FlightRecorder(LagoVista.Core.IDispatcherServices dispatcher)
        {
            this._dispatcher = dispatcher;
        }


        public void Publish(FlightEvent evt)
        {
            this._dispatcher.Invoke(() =>
            { 
                this.Insert(0, evt);
            });
        }

        public void Publish(string msg)
        {
            Publish(new FlightEvent()
            {
                EventName = msg
            });
        }
    }

    public interface IFlightRecorder
    {
        void Publish(FlightEvent evt);
        void Publish(string msg);
    }
}
