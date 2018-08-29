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
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;

namespace LagoVista.Uas.BaseStation.UWP.Renderers
{
    public class MapViewRenderer : FrameRenderer
    {
        IUas _uas;
        MapControl _mapControl;

        INavigation _navigation;
        Mission _mission;
        MapIcon _draggedWaypoint;

        MapPolyline _mapRoute;

        MenuFlyout _mapContextMenu;
        DateTime? _mapTapped;

        List<BasicGeoposition> _waypointPaths;

        public MapViewRenderer()
        {
            ArrangeNativeChildren = true;
            _mapControl = new MapControl
            {
                Style = MapStyle.AerialWithRoads,
                MapServiceToken = "s5miuLzzn4RuPyMXzOYF~pA3KRBwzLZ4JOHnyIaUAWA~AnoR9G-Mf6OR7_n8b6wVy_cd9wim48xfSp39TC31OlvLad6zT5Pf0XN35EPuEV5U"
            };
            _mapControl.Loaded += _mapControl_Loaded;
            _mapControl.PointerMoved += _mapControl_PointerMoved;
            _mapControl.MapElementClick += _mapControl_MapElementClick;
            _mapControl.MapRightTapped += _mapControl_MapRightTapped;
            _mapControl.MapTapped += _mapControl_MapTapped;
            Children.Add(_mapControl);
            AddContextMenu();
        }

        private void _mapControl_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            if (_mapTapped.HasValue && _draggedWaypoint != null && (DateTime.Now - _mapTapped).Value > TimeSpan.FromMilliseconds(100))
            {
                _draggedWaypoint = null;
                _mapTapped = null;
            }
        }

        private void _mapControl_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_draggedWaypoint != null)
            {
                e.Handled = true;
                var dragPosition = e.GetCurrentPoint(_mapControl).Position;

                Geopoint point;
                _mapControl.GetLocationFromOffset(dragPosition, out point);
                _draggedWaypoint.Location = point;

                var waypointIndex = Convert.ToInt32(_draggedWaypoint.Tag);
                var idx = 0;

                var newList = new List<BasicGeoposition>();

                foreach (var path in _waypointPaths)
                {
                    if (idx++ == waypointIndex)
                    {
                        newList.Add(new BasicGeoposition
                        {
                            Latitude = point.Position.Latitude,
                            Longitude = point.Position.Longitude
                        });
                    }
                    else
                    {
                        newList.Add(path);
                    }
                }

                _mapRoute.Path = new Geopath(newList);
                _waypointPaths = newList;
            }
        }

        private void _mapControl_MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            _draggedWaypoint = args.MapElements.FirstOrDefault() as MapIcon;
            _mapTapped = DateTime.Now;

        }

        void AddContextMenu()
        {
            _mapContextMenu = new MenuFlyout();
            _mapContextMenu.Items.Add(new MenuFlyoutItem() { Text = "Option1 " });
            _mapContextMenu.Items.Add(new MenuFlyoutItem() { Text = "Option2 " });
            _mapContextMenu.Items.Add(new MenuFlyoutSeparator());
            _mapContextMenu.Items.Add(new MenuFlyoutItem() { Text = "Option3 " });

            _mapControl.ContextFlyout = _mapContextMenu;

        }

        private void _mapControl_MapRightTapped(MapControl sender, MapRightTappedEventArgs args)
        {
            _mapContextMenu.ShowAt(sender, new Windows.Foundation.Point(args.Position.X, args.Position.Y));
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
                    /* disconnect the property changed handler from the prevous mission */
                    _mission.PropertyChanged -= _navigation_PropertyChanged;
                }

                _mission = _navigation.Mission;

                _mission.PropertyChanged += _navigation_PropertyChanged;
            
                if (_mission.Waypoints.Any())
                {
                    _waypointPaths = new List<BasicGeoposition>();

                    _mapRoute = new MapPolyline();
                    _mapRoute.StrokeColor = Colors.Yellow;
                    _mapRoute.StrokeThickness = 2;


                    foreach (var wp in _mission.Waypoints)
                    {
                        // get position
                        if (wp.X != 0 && wp.Y != 0)
                        {
                            var position = new BasicGeoposition() { Latitude = wp.Y, Longitude = wp.X };
                            _waypointPaths.Add(position);
                        }
                    }
                    _mapRoute.Path = new Geopath(_waypointPaths);
                    _mapControl.MapElements.Add(_mapRoute);

                    var idx = 0;
                    foreach (var wp in _mission.Waypoints)
                    {
                        if (wp.X != 0 && wp.Y != 0)
                        {
                            var position = new BasicGeoposition() { Latitude = wp.Y, Longitude = wp.X };

                            var myPoint = new Geopoint(position);
                            var waypointPin = new MapIcon { Location = myPoint, NormalizedAnchorPoint = new Windows.Foundation.Point(0.5f, 1.0f), Title = $"{wp.Sequence + 1}", ZIndex = 0 };
                            waypointPin.Tag = idx++;
                            _mapControl.MapElements.Add(waypointPin);

                        }
                    }


                    _mapControl.Center = new Geopoint(new BasicGeoposition() { Latitude = _mission.Waypoints[0].Y, Longitude = _mission.Waypoints[0].X });
                    _mapControl.ZoomLevel = 14;
                }

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