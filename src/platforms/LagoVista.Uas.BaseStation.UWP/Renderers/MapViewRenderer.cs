using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace LagoVista.Uas.BaseStation.UWP.Renderers
{
    public class MapViewRenderer : FrameRenderer
    {
        MapControl _mapControl;

        public MapViewRenderer()
        {
            _mapControl = new MapControl();
            _mapControl.Width = 640;
            _mapControl.Height = 480;
            _mapControl.MapServiceToken = "s5miuLzzn4RuPyMXzOYF~pA3KRBwzLZ4JOHnyIaUAWA~AnoR9G-Mf6OR7_n8b6wVy_cd9wim48xfSp39TC31OlvLad6zT5Pf0XN35EPuEV5U";


            //           Children.Add(_mapControl);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Frame> e)
        {
            base.OnElementChanged(e);
            Background = new SolidColorBrush(Colors.SkyBlue);
            Children.Add(_mapControl);
        }
    }
}