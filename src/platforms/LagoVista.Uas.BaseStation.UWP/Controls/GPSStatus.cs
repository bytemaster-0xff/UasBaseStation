using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LagoVista.Uas.BaseStation.UWP.Controls
{
    public class GPSStatus : HudControlBase
    {
        TextBlock _gps;

        public GPSStatus()
        {
            _gps = new TextBlock();
            _gps.SetValue(Grid.RowProperty, 1);
            _gps.FontSize = 28;
            _gps.Foreground = ForegroundBrush;
            _gps.Margin = new Thickness(0, 0, 30, 0);
            _gps.TextAlignment = TextAlignment.Right;
            Children.Add(_gps);
        } 
    }
}
