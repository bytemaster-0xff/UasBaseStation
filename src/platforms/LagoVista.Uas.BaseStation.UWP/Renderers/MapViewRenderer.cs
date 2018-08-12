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

namespace LagoVista.Uas.BaseStation.UWP.Renderers
{
    public class MapViewRenderer : FrameRenderer
    {
        IUas _uas;
        MapControl _mapControl;
        Geolocator _geoLocator;

        public MapViewRenderer()
        {
            ArrangeNativeChildren = true;
            _mapControl = new MapControl();
            _mapControl.Style = MapStyle.AerialWithRoads;
            _mapControl.MapServiceToken = "s5miuLzzn4RuPyMXzOYF~pA3KRBwzLZ4JOHnyIaUAWA~AnoR9G-Mf6OR7_n8b6wVy_cd9wim48xfSp39TC31OlvLad6zT5Pf0XN35EPuEV5U";

            Children.Add(_mapControl);
        }

        private async void SetLocation()
        {
            if (_geoLocator == null)
            {
                var accessStatus = await Geolocator.RequestAccessAsync();
                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        _geoLocator = new Geolocator();

                        // Subscribe to the StatusChanged event to get updates of location status changes.
                        _geoLocator.StatusChanged += Geolocator_StatusChanged; ;

                        // Carry out the operation.
                        var pos = await _geoLocator.GetGeopositionAsync();
                        _mapControl.Center = pos.Coordinate.Point;
                        _mapControl.ZoomLevel = 16;
                        break;
                }
            }

        }

        private void Geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
         
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Frame> e)
        {
            _uas = SLWIOC.Get<IConnectedUasManager>().Active.Uas;

            base.OnElementChanged(e);
            Background = new SolidColorBrush(Colors.SkyBlue);

            SetLocation();
        }
    }
}