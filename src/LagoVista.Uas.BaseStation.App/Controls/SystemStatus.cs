using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LagoVista.Uas.BaseStation.ControlApp.Controls
{
    public class SystemStatus : HudControlBase
    {
        private TextBlock _batteryControl;
        private TextBlock _percentRemainingControl;
        private TextBlock _timeRemainingControl;
        private TextBlock _flightTime;

        private TextBlock _armedControl;
        private TextBlock _lowBatt;
        private TextBlock _criticalBatt;

        public SystemStatus()
        {
            var container = new StackPanel();

            var fontFamily = new FontFamily("Consolas");

            _percentRemainingControl = new TextBlock
            {
                Foreground = ForegroundBrush,
                FontSize = FontSize
            };

            _timeRemainingControl = new TextBlock
            {
                Foreground = ForegroundBrush,
                FontSize = FontSize,
                FontFamily = fontFamily
            };

            _flightTime = new TextBlock
            {
                Foreground = ForegroundBrush,
                FontSize = FontSize,
                FontFamily = fontFamily
            };

            _batteryControl = new TextBlock
            {
                FontSize = FontSize,
                Foreground = ForegroundBrush,
                FontFamily = fontFamily
            };
            
            _lowBatt = new TextBlock()
            {
                Text = "Low Bat",
                FontSize = 24,
                Visibility = Visibility.Collapsed,
                Foreground = new SolidColorBrush(Colors.Red),
                FontFamily = fontFamily,
            };

            _criticalBatt = new TextBlock()
            {
                Text = "CRITICAL BATTERY",
                Visibility = Visibility.Collapsed,
                Foreground = new SolidColorBrush(Colors.Red),
                FontSize = 36,
                FontFamily = fontFamily,
            };

            _armedControl = new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.Lime),
                Text = "DISARMED",
                FontSize = 24,
                FontFamily = fontFamily,
            };

            _batteryControl.Text = "Batt: ???";
            _percentRemainingControl.Text = "Batt: ???";
            _timeRemainingControl.Text = "Batt Time: ???";
            _flightTime.Text = "Flight Time: ???";

            container.Children.Add(_armedControl);

            container.Children.Add(_batteryControl);
            container.Children.Add(_percentRemainingControl);
            container.Children.Add(_flightTime);
            container.Children.Add(_timeRemainingControl);
            container.Children.Add(_lowBatt);
            container.Children.Add(_criticalBatt);


            Children.Add(container);
        }

        private bool _lastArmed = false;
        public static DependencyProperty ArmedProperty = DependencyProperty.Register(nameof(Armed), typeof(bool), typeof(SystemStatus), new PropertyMetadata(default(bool), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).Armed = Convert.ToBoolean(value.NewValue))));
        public bool Armed
        {
            get { return Convert.ToBoolean(GetValue(ArmedProperty)); }
            set
            {
                if (value != _lastArmed)
                {
                    SetValue(ArmedProperty, value);

                    RunOnUIThread(() =>
                    {
                        if (value)
                        {
                            _armedControl.Text = "ARMED";
                            _armedControl.Foreground = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            _armedControl.Text = "DISAMRED";
                            _armedControl.Foreground = new SolidColorBrush(Colors.Green);
                        }                        
                    });
                    _lastArmed = value;
                };
            }
        }

        public static DependencyProperty BatteryVoltageProperty = DependencyProperty.Register(nameof(BatteryVoltage), typeof(float), typeof(SystemStatus), new PropertyMetadata(default(float), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).BatteryVoltage = Convert.ToSingle(value.NewValue))));

        public Single BatteryVoltage
        {
            get { return Convert.ToSingle(GetValue(BatteryVoltageProperty)); }
            set
            {
                SetValue(BatteryVoltageProperty, value);

                RunOnUIThread(() => _batteryControl.Text = $"Batt: {value:0.00}");
            }
        }

        public static DependencyProperty PercentRemainingProperty = DependencyProperty.Register(nameof(PercentRemaining), typeof(float), typeof(SystemStatus), new PropertyMetadata(default(float), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).PercentRemaining = Convert.ToSingle(value.NewValue))));
        public Single PercentRemaining
        {
            get { return Convert.ToSingle(GetValue(PercentRemainingProperty)); }
            set
            {
                SetValue(PercentRemainingProperty, value);
                RunOnUIThread(() => _percentRemainingControl.Text = $"Batt: {value:0.00}%");
            }
        }

        public static DependencyProperty LowBattWarningProperty = DependencyProperty.Register(nameof(LowBattWarning), typeof(bool), typeof(SystemStatus), new PropertyMetadata(default(bool), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).LowBattWarning = Convert.ToBoolean(value.NewValue))));
        public bool LowBattWarning
        {
            get { return Convert.ToBoolean(GetValue(LowBattWarningProperty)); }
            set
            {
                SetValue(LowBattWarningProperty, value);
                RunOnUIThread(() => _lowBatt.Visibility = value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public static DependencyProperty FontSizeProperty = DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(SystemStatus), new PropertyMetadata(14, new PropertyChangedCallback((obj, value) => (obj as SystemStatus).FontSize = Convert.ToDouble(value.NewValue))));
        public double FontSize
        {
            get => Convert.ToDouble(GetValue(FontSizeProperty));
            set => SetValue(FontSizeProperty, value);
        }

        public static DependencyProperty CriticalBattWarningProperty = DependencyProperty.Register(nameof(CriticalBattWarning), typeof(bool), typeof(SystemStatus), new PropertyMetadata(default(bool), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).CriticalBattWarning = Convert.ToBoolean(value.NewValue))));
        public bool CriticalBattWarning
        {
            get { return Convert.ToBoolean(GetValue(CriticalBattWarningProperty)); }
            set
            {
                SetValue(CriticalBattWarningProperty, value);
                RunOnUIThread(() => _criticalBatt.Visibility = value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public static DependencyProperty TimeRemainingProperty = DependencyProperty.Register(nameof(TimeRemaining), typeof(TimeSpan), typeof(SystemStatus), new PropertyMetadata(default(int), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).TimeRemaining = (TimeSpan)value.NewValue)));
        public TimeSpan TimeRemaining
        {
            get { return (TimeSpan)GetValue(TimeRemainingProperty); }
            set
            {
                SetValue(TimeRemainingProperty, value);
                RunOnUIThread(() =>
                {
                    if (value.TotalMinutes > 0)
                    {
                        _timeRemainingControl.Text = $"Batt Remaining: {Convert.ToInt32(value.TotalMinutes)} min, {Convert.ToInt32(value.Seconds)} sec";
                    }
                    else
                    {
                        _timeRemainingControl.Text = $"Batt Remaining: {Convert.ToInt32(value.TotalSeconds)} sec";
                    }
                });
            }
        }

        public static DependencyProperty FlightTimeProperty = DependencyProperty.Register(nameof(FlightTime), typeof(TimeSpan), typeof(SystemStatus), new PropertyMetadata(default(int), new PropertyChangedCallback((obj, value) => (obj as SystemStatus).FlightTime = (TimeSpan)value.NewValue)));
        public TimeSpan FlightTime
        {
            get { return (TimeSpan)GetValue(FlightTimeProperty); }
            set
            {
                SetValue(FlightTimeProperty, value);
                RunOnUIThread(() =>
                {
                    if (value.TotalMinutes > 0)
                    {
                        _flightTime.Text = $"Flight Time: {Convert.ToInt32(value.TotalMinutes)} min, {Convert.ToInt32(value.Seconds)} sec";
                    }
                    else
                    {
                        _flightTime.Text = $"Flight Time: {Convert.ToInt32(value.TotalSeconds)} sec";
                    }
                });
            }
        }
    }
}
