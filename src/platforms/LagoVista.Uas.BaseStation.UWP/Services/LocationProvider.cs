using LagoVista.Core.Models.Geo;
using LagoVista.Uas.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using LagoVista.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace LagoVista.Uas.BaseStation.UWP.Services
{
    public class LocationProvider : ILocationProvider, INotifyPropertyChanged
    {
        public event EventHandler<GeoLocation> LocationUpdated;
        public event PropertyChangedEventHandler PropertyChanged;

        Geolocator _geoLocator;

        private bool _initialized = false;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool HasLocation
        {
            get;
            private set;
        }

        public bool HasLocationAccess
        {
            get;
            private set;
        }

        public GeoLocation CurrentLocation
        {
            get;
            private set;
        }


        public async Task InitAsync()
        {
            lock (this)
            {
                if (_initialized)
                {
                    return;
                }

                _initialized = true;
            }

            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                    _geoLocator = new Geolocator();

                    // Subscribe to the StatusChanged event to get updates of location status changes.
                    _geoLocator.StatusChanged += _geoLocator_StatusChanged;
                    _geoLocator.PositionChanged += _geoLocator_PositionChanged;

                    // Carry out the operation.
                    var pos = await _geoLocator.GetGeopositionAsync();
                    CurrentLocation = new GeoLocation()
                    {
                        Altitude = pos.Coordinate.Point.Position.Altitude,
                        Latitude = pos.Coordinate.Point.Position.Latitude,
                        Longitude = pos.Coordinate.Point.Position.Longitude,
                        LastUpdated = DateTime.Now.ToJSONString()

                    };
                    HasLocation = true;
                    HasLocationAccess = true;

                    RaisePropertyChanged(nameof(HasLocation));
                    RaisePropertyChanged(nameof(HasLocationAccess));
                    RaisePropertyChanged(nameof(CurrentLocation));
                    break;
                default:
                    HasLocation = false;
                    HasLocationAccess = false;
                    break;
            }
        }

        private void _geoLocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            CurrentLocation = new GeoLocation()
            {
                Altitude = args.Position.Coordinate.Point.Position.Altitude,
                Latitude = args.Position.Coordinate.Point.Position.Latitude,
                Longitude = args.Position.Coordinate.Point.Position.Longitude,
                LastUpdated = DateTime.Now.ToJSONString()
            };

            RaisePropertyChanged(nameof(CurrentLocation));
            LocationUpdated?.Invoke(this, CurrentLocation);
        }

        private void _geoLocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            if (args.Status == PositionStatus.NotAvailable)
            {
                CurrentLocation = null;
                HasLocation = false;
                RaisePropertyChanged(nameof(CurrentLocation));
                RaisePropertyChanged(nameof(HasLocation));
            }
        }
    }
}
