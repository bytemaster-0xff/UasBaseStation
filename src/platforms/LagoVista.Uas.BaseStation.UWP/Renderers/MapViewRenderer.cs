using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using LagoVista.Uas.Core;
using LagoVista.Core.IOC;
using LagoVista.Uas.Core.Interfaces;
using LagoVista.Uas.Core.Models;
using System.Drawing;

namespace LagoVista.Uas.BaseStation.UWP.Renderers
{
    public class MapViewRenderer : FrameRenderer
    {
        IUas _uas;
        MapControl _mapControl;
        ILocationProvider _locationProvier;

        INavigation _navigation;
        Mission _mission;

        public MapViewRenderer()
        {
            ArrangeNativeChildren = true;
            _mapControl = new MapControl();
            _mapControl.Style = MapStyle.AerialWithRoads;
            _mapControl.MapServiceToken = "s5miuLzzn4RuPyMXzOYF~pA3KRBwzLZ4JOHnyIaUAWA~AnoR9G-Mf6OR7_n8b6wVy_cd9wim48xfSp39TC31OlvLad6zT5Pf0XN35EPuEV5U";
            _mapControl.Loaded += _mapControl_Loaded;
            Children.Add(_mapControl);
        }

        private void _mapControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var locationProvier = SLWIOC.Get<ILocationProvider>();
            if (locationProvier.HasLocation)
            {
                _mapControl.Center = new Geopoint(new BasicGeoposition()
                {
                    Latitude = locationProvier.CurrentLocation.Latitude,
                    Longitude = locationProvier.CurrentLocation.Longitude,
                    Altitude = locationProvier.CurrentLocation.Altitude,
                });
                _mapControl.ZoomLevel = 16;
            }
        }

        private void Geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Frame> e)
        {
            if (e.NewElement != null)
            {
                SLWIOC.Get<ILocationProvider>().LocationUpdated += _locationProvier_LocationUpdated;

                _uas = SLWIOC.Get<IConnectedUasManager>().Active.Uas;

                _navigation = (e.NewElement.BindingContext as INavigationProvider).Navigation;
                if (_navigation == null)
                {
                    throw new ArgumentException("Argument Not Set");
                }

                _navigation.PropertyChanged += _navigation_PropertyChanged;
            }
            base.OnElementChanged(e);

        }

        private void _locationProvier_LocationUpdated(object sender, LagoVista.Core.Models.Geo.GeoLocation e)
        {

        }

        private void _navigation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(INavigation.Mission))
            {
                if (_mission != null)
                {
                    _mission.PropertyChanged -= _navigation_PropertyChanged;
                }
                _mission = _navigation.Mission;
                _mission.PropertyChanged += _navigation_PropertyChanged;

                var path = new List<BasicGeoposition>();
                
                foreach (var wp in _mission.Waypoints)
                {
                    // get position
                    if (wp.X != 0&& wp.Y != 0)
                    {
                        var position = new BasicGeoposition() { Latitude = wp.Y, Longitude = wp.X };
                        path.Add(position);
                        var myPoint = new Geopoint(position);
                        var myPOI = new MapIcon { Location = myPoint, NormalizedAnchorPoint = new Windows.Foundation.Point(0.5f, 1.0f), Title = $"{wp.Sequence + 1}", ZIndex = 0 };
                        // add to map and center it
                        _mapControl.MapElements.Add(myPOI);
                    }
                }

                _mapControl.Center = new Geopoint(new BasicGeoposition() { Latitude = _mission.Waypoints[0].Y, Longitude = _mission.Waypoints[0].X });
                _mapControl.ZoomLevel = 14;

                var mapPolyline = new MapPolyline();
                mapPolyline.Path = new Geopath(path);
                mapPolyline.StrokeColor = Colors.Yellow;
                mapPolyline.StrokeThickness = 3;

                _mapControl.MapElements.Add(mapPolyline);
            }
            else if (e.PropertyName == nameof(Mission.CurrentWaypoint))
            {
                if (_navigation.Mission.CurrentWaypoint != null &&
                    _navigation.Mission.CurrentWaypoint.X != 0 &&
                    _navigation.Mission.CurrentWaypoint.Y != 0)
                    _mapControl.Center = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = _navigation.Mission.CurrentWaypoint.Y,
                        Longitude = _navigation.Mission.CurrentWaypoint.X,
                    });

            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            SLWIOC.Get<ILocationProvider>().LocationUpdated -= _locationProvier_LocationUpdated;
        }
    }
}