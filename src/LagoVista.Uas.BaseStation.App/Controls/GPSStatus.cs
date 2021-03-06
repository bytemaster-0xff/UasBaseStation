﻿using LagoVista.Uas.Core.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LagoVista.Uas.BaseStation.ControlApp.Controls
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
            _gps.Text = "FIX: No Fix";

            _satCount = new TextBlock();
            _satCount.Foreground = ForegroundBrush;
            _satCount.Text = "SATS: ??";

            _hdop = new TextBlock();
            _hdop.Foreground = ForegroundBrush;
            _hdop.Text = "HDOP: ??";
            _hdop.Visibility = Visibility.Collapsed;

            _vdop = new TextBlock();
            _vdop.Foreground = ForegroundBrush;
            _vdop.Text = "VDOP: ??";
            _vdop.Visibility = Visibility.Collapsed;

            container.Children.Add(gpsLabel);
            container.Children.Add(_gps);
            container.Children.Add(_satCount);
            container.Children.Add(_hdop);
            container.Children.Add(_vdop);

            Children.Add(container);
        }

        public static DependencyProperty FixTypeProperty = DependencyProperty.Register(nameof(FixType), typeof(string), typeof(GPSStatus), new PropertyMetadata(default(string), new PropertyChangedCallback((obj, value) => (obj as GPSStatus).FixType = Convert.ToString(value.NewValue))));
        public String FixType
        {
            get { return Convert.ToString(GetValue(FixTypeProperty)); }
            set
            {
                SetValue(FixTypeProperty, value);
                RunOnUIThread(() => _gps.Text = $"FIX : {value};");
            }
        }

        public static DependencyProperty HasFixProperty = DependencyProperty.Register(nameof(HasFix), typeof(string), typeof(bool), new PropertyMetadata(default(Boolean), new PropertyChangedCallback((obj, value) => (obj as GPSStatus).HasFix = Convert.ToBoolean(value.NewValue))));
        public bool HasFix
        {
            get { return Convert.ToBoolean(GetValue(HasFixProperty)); }
            set
            {
                SetValue(HasFixProperty, value);

                var fixMessage = value ? "Fix" : "No Fix";
                RunOnUIThread(() => _gps.Text = $"FIX {fixMessage}");
            }
        }

        public static DependencyProperty SatCountProperty = DependencyProperty.Register(nameof(SatCount), typeof(int), typeof(GPSStatus), new PropertyMetadata(default(int), new PropertyChangedCallback((obj, value) => (obj as GPSStatus).SatCount = Convert.ToInt32(value.NewValue))));
        public int SatCount
        {
            get { return Convert.ToInt32(GetValue(SatCountProperty)); }
            set
            {
                SetValue(SatCountProperty, value);
                RunOnUIThread(() => _satCount.Text = $"SATS: {value}");
            }
        }

        public static DependencyProperty HDOPProperty = DependencyProperty.Register(nameof(HDOP), typeof(float), typeof(GPSStatus), new PropertyMetadata(default(float), new PropertyChangedCallback((obj, value) => (obj as GPSStatus).HDOP = Convert.ToSingle(value.NewValue))));
        public float HDOP
        {
            get { return Convert.ToSingle(GetValue(HDOPProperty)); }
            set
            {
                SetValue(HDOPProperty, value);
                RunOnUIThread(() => _hdop.Text = $"HDOP: {value:0.00}m");
            }
        }

        public static DependencyProperty VDOPProperty = DependencyProperty.Register(nameof(VDOP), typeof(float), typeof(GPSStatus), new PropertyMetadata(default(float), new PropertyChangedCallback((obj, value) => (obj as GPSStatus).VDOP = Convert.ToSingle(value.NewValue))));
        public float VDOP
        {
            get { return Convert.ToSingle(GetValue(VDOPProperty)); }
            set
            {
                SetValue(VDOPProperty, value);
                RunOnUIThread(() => _vdop.Text = $"VDOP: {value:0.00}m");
            }
        }

        public static DependencyProperty GPSProperty = DependencyProperty.Register(nameof(GPS), typeof(GPS), typeof(GPSStatus), new PropertyMetadata(default(float), new PropertyChangedCallback((obj, value) => (obj as GPSStatus).GPS = value.NewValue as GPS)));
        public GPS GPS
        {
            get => GetValue(GPSProperty) as GPS;
            set
            {
                SetValue(GPSProperty, value);
                RunOnUIThread(() =>
                {
                    if (!String.IsNullOrEmpty(value.FixType))
                    {
                        _satCount.Text = $"FIX: {value.FixType}";
                    }

                    _satCount.Text = $"SATS: {value.SateliteCount}";
                    if (value.VDOP > 0)
                    {
                        _vdop.Text = $"VDOP: {value.VDOP}";
                        _vdop.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _vdop.Visibility = Visibility.Collapsed;
                    }

                    if (value.HDOP > 0)
                    {
                        _hdop.Text = $"HDOP: {value.HDOP}";
                        _hdop.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _hdop.Visibility = Visibility.Collapsed;
                    }
                });
            }
        }
    }
}
