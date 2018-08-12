using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LagoVista.Uas.BaseStation.UWP.Controls
{
    public class SystemStatus : HudControlBase
    {
        private TextBlock _batteryControl;
        private TextBlock _percentRemainingControl;
        private TextBlock _timeRemainingControl;

        private TextBlock _armedControl;

        public SystemStatus()
        {
            var container = new StackPanel();

            _percentRemainingControl = new TextBlock();
            _percentRemainingControl.Foreground = ForegroundBrush;
            _timeRemainingControl = new TextBlock();
            _timeRemainingControl.Foreground = ForegroundBrush;
            _batteryControl = new TextBlock();
            _batteryControl.Foreground = ForegroundBrush;
            _armedControl = new TextBlock();

            _armedControl.Foreground = new SolidColorBrush(Colors.Red);

            _armedControl.Text = "DISARMED";

            _batteryControl.Text = "Battery: ???";            
            
            _timeRemainingControl.Text = "TIME: -";
            _percentRemainingControl.Text = "PERCENT: -";

            container.Children.Add(_armedControl);
            container.Children.Add(new TextBlock()
            {
                Text="Battery",
                Foreground = ForegroundBrush,
                Margin = new Windows.UI.Xaml.Thickness(0,10,0,0)
            });
            container.Children.Add(_batteryControl);
            container.Children.Add(_percentRemainingControl);
            container.Children.Add(_timeRemainingControl);
            Children.Add(container);
        }

        bool _armed;
        public bool Armed
        {
            get { return _armed; }
            set
            {
                _armed = value;
                _armedControl.Text = value ? "ARMED" : "DISARMED";
            }
        }

        double _batteryVoltage;
        public double BatteryVoltage
        {
            get { return _batteryVoltage; }
            set
            {
                _batteryVoltage = value;
                _batteryControl.Text = String.Format("Battery {0:0.00}V", _batteryVoltage);
            }
        }

        double _percentRemaining;
        public double PercentRemaining
        {
            get { return _percentRemaining; }
            set
            {
                _percentRemaining = value;
                _percentRemainingControl.Text = String.Format("{0:0.0}%", _percentRemaining);
            }
        }

        int _timeRemaining;
        public int TimeRemaining
        {
            get { return _timeRemaining; }
            set
            {
                _timeRemaining = value;
                _timeRemainingControl.Text = String.Format("Time {0}", value);
            }
        }
    }
}
