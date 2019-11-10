using LagoVista.Core.Models.Geo;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class Terrain : GaugeBase
    {
        private UInt16 _spacing;
        private float _terrainHeight;
        private float _currentHeight;
        private UInt16 _pending;
        private UInt16 _loaded;
        
        GeoLocation _location;
        public GeoLocation Location
        {
            get { return _location; }
            set { Set(ref _location, value); }
        }
        
        public UInt16 Spacing
        {
            get { return _spacing; }
            set { Set(ref _spacing,value); }
        }
        
        public float TerrainHeight
        {
            get { return _terrainHeight; }
            set { Set(ref _terrainHeight, value); }
        }

        public float CurrentHeight
        {
            get { return _currentHeight; }
            set { Set(ref _currentHeight, value); }
        }

        public UInt16 Pending
        {
            get { return _pending; }
            set { Set(ref _pending, value); }
        }
        
        public UInt16 Loaded
        {
            get { return _loaded; }
            set { Set(ref _loaded, value); }
        }

        public void Update(UasTerrainReport terrain)
        {
            Loaded = terrain.Loaded;
            Pending = terrain.Pending;
            CurrentHeight = terrain.CurrentHeight;
            TerrainHeight = terrain.TerrainHeight;
            Spacing = terrain.Spacing;
            Location = new GeoLocation(terrain.Lat.ToLatLon(), terrain.Lon.ToLatLon());

            GaugeStatus = GaugeStatus.OK;
            TimeStamp = DateTime.Now;            
        }
    }
}
