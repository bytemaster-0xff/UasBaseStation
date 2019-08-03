using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LagoVista.Uas.BaseStation.App.Controls
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
                Text = "Battery",
                Foreground = ForegroundBrush,
                Margin = new Windows.UI.Xaml.Thickness(0, 10, 0, 0)
            });
            container.Children.Add(_batteryControl);
            container.Children.Add(_percentRemainingControl);
            container.Children.Add(_timeRemainingControl);
            Children.Add(container);
        }

        public static DependencyProperty ArmedProperty = DependencyProperty.Register(nameof(Armed), typeof(bool), typeof(SystemStatus), new PropertyMetadata(default(bool), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).Armed = Convert.ToBoolean(value.NewValue))));
        public bool Armed
        {
            get { return Convert.ToBoolean(GetValue(ArmedProperty)); }
            set
            {
                SetValue(ArmedProperty, value);

                RunOnUIThread(() => _armedControl.Text = value ? "ARMED" : "DISARMED");
            }
        }

        public static DependencyProperty BatteryVoltageProperty = DependencyProperty.Register(nameof(BatteryVoltage), typeof(float), typeof(SystemStatus), new PropertyMetadata(default(float), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).BatteryVoltage = Convert.ToSingle(value.NewValue))));

        public Single BatteryVoltage
        {
            get { return Convert.ToSingle(GetValue(BatteryVoltageProperty)); }
            set
            {
                SetValue(BatteryVoltageProperty, value);

                RunOnUIThread(() => _batteryControl.Text = String.Format("Battery {0:0.00}V", value));
            }
        }

        public static DependencyProperty PercentRemainingProperty = DependencyProperty.Register(nameof(PercentRemaining), typeof(float), typeof(SystemStatus), new PropertyMetadata(default(float), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).PercentRemaining = Convert.ToSingle(value.NewValue))));
        public Single PercentRemaining
        {
            get { return Convert.ToSingle(GetValue(PercentRemainingProperty)); }
            set
            {
                SetValue(PercentRemainingProperty, value);

                RunOnUIThread(() => _percentRemainingControl.Text = String.Format("{0:0.0}%", value));
            }
        }

        public static DependencyProperty TimeRemainingProperty = DependencyProperty.Register(nameof(TimeRemaining), typeof(float), typeof(SystemStatus), new PropertyMetadata(default(int), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).TimeRemaining = Convert.ToInt32(value.NewValue))));
        public int TimeRemaining
        {
            get { return Convert.ToInt32(GetValue(TimeRemainingProperty)); }
            set
            {
                SetValue(TimeRemainingProperty, value);
                RunOnUIThread(() => _timeRemainingControl.Text = String.Format("Time {0}", value));
            }
        }
    }
}
