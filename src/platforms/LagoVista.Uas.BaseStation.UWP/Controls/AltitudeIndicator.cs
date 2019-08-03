using LagoVista.Core.Models.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LagoVista.Uas.BaseStation.UWP.Controls
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


        public static DependencyProperty AltitudeProperty = DependencyProperty.Register(nameof(Altitude), typeof(float), typeof(AltitudeIndicator), new PropertyMetadata(default(float)));
        public float Altitude
        {
            get { return Convert.ToSingle(GetValue(AltitudeProperty)); }
            set
            {
                SetValue(AltitudeProperty, value);
                _altitude.Text = $"{value:0.00}m";
            }
        }


        public static DependencyProperty LocationProperty = DependencyProperty.Register(nameof(Location), typeof(GeoLocation), typeof(AltitudeIndicator), new PropertyMetadata(default(GeoLocation)));
        public GeoLocation Location
        {
            get { return GetValue(LocationProperty) as GeoLocation; }
            set
            {
                SetValue(LocationProperty, value);
                
                if (value != null)
                {
                    Altitude = Convert.ToSingle(value.Altitude);
                }
            }
        }
    }
}
