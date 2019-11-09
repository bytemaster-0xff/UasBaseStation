using System;

namespace LagoVista.Uas.BaseStation.Core.Networking
{

    public class WiFiAdapterId
    {
        public WiFiAdapterId() { }

        public WiFiAdapterId(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return $"{Name}:[{Id}]";
        }
    }
}
