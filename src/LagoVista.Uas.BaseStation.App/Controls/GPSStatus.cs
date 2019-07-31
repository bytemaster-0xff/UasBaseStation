using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LagoVista.Uas.BaseStation.App.Controls
{
    public class GPSStatus : HudControlBase
    {
        TextBlock _gps;
        TextBlock _satCount;
        TextBlock _hdop;
        TextBlock _vdop;

        public GPSStatus()
        {
            var container = new StackPanel();
            var gpsLabel = new TextBlock();
            gpsLabel.Foreground = ForegroundBrush;
            gpsLabel.Text = "GPS";
            gpsLabel.FontWeight = new Windows.UI.Text.FontWeight() { Weight = 800 };

            _gps = new TextBlock();
            _gps.Foreground = ForegroundBrush;
            _gps.Text = "FIX: ??";

            _satCount = new TextBlock();
            _satCount.Foreground = ForegroundBrush;
            _satCount.Text = "SATS: ??";

            _hdop = new TextBlock();
            _hdop.Foreground = ForegroundBrush;
            _hdop.Text = "HDOP: ??";

            _vdop = new TextBlock();
            _vdop.Foreground = ForegroundBrush;
            _vdop.Text = "VDOP: ??";


            container.Children.Add(gpsLabel);
            container.Children.Add(_gps);
            container.Children.Add(_satCount);
            container.Children.Add(_hdop);
            container.Children.Add(_vdop);

            Children.Add(container);
        }

        public String FixType
        {
            get { return _gps.Text; }
            set { _gps.Text = $"FIX : {value};"; }
        }

        public int SatCount
        {
            get { return 0; }
            set { _satCount.Text = $"SATS: {value}"; }
        }

        public double HDOP
        {
            get { return 0; }
            set { _hdop.Text = $"HDOP: {value:0.00}m"; }
        }

        public double VDOP
        {
            get { return 0; }
            set { _vdop.Text = $"VDOP: {value:0.00}m"; }
        }
    }
}
