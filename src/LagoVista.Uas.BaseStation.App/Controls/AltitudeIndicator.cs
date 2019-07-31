using LagoVista.Core.Models.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LagoVista.Uas.BaseStation.App.Controls
{
    public class AltitudeIndicator : HudControlBase
    {
        TextBlock _altitude;

        public AltitudeIndicator()
        {
            _altitude = new TextBlock();
            _altitude.FontSize = 22;
            _altitude.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            _altitude.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            _altitude.Foreground = ForegroundBrush;
            Children.Add(_altitude);

            Children.Add(new Rectangle()
            {
                Width = 40,
                Height = 300,
                Stroke = ForegroundBrush,
                StrokeThickness = 2,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right
            });

        }


        public double Altitude
        {
            set { _altitude.Text = $"{value:0.00}m"; }
        }


        GeoLocation _location;
        public GeoLocation Location
        {
            get { return _location; }
            set
            {
                _location = value;
                if (_location != null)
                {
                    Altitude = _location.Altitude;
                }
            }
        }
    }
}
