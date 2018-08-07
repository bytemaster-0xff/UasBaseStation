using LagoVista.Core.IOC;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Models;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms.Platform.UWP;



namespace LagoVista.Uas.BaseStation.UWP.Renderers
{

    public class AttitudeIndicatorRenderer : FrameRenderer
    {
        RotateTransform _rollTransform;
        TranslateTransform _pitchTransform;
        TranslateTransform _altitudeTransform;
        RotateTransform _compassTransform;

        Canvas _compass;

        CaptureElement _captureElement;
        DisplayRequest _displayRequest;
        MediaCapture _mediaCapture;
        Windows.UI.Xaml.Controls.Grid _aoaCircle;
        Windows.UI.Xaml.Controls.Grid _pageContainer;
        DeviceInformationCollection _devices;


        Windows.UI.Xaml.Controls.TextBlock _gps;
        Windows.UI.Xaml.Controls.TextBlock _battery;
        Windows.UI.Xaml.Controls.TextBlock _altitude;
        Windows.UI.Xaml.Controls.TextBlock _heading;
        MapControl _mapControl;

        IUas _uas;

        Windows.UI.Xaml.Controls.Button _startVideo;
        Windows.UI.Xaml.Controls.Button _stopVideo;
        Windows.UI.Xaml.Controls.ComboBox _cameras;

        SolidColorBrush _hudGreen = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 51, 255, 85));

        public AttitudeIndicatorRenderer()
        {
            ArrangeNativeChildren = true;

            _pageContainer = new Windows.UI.Xaml.Controls.Grid();
            _pageContainer.RowDefinitions.Add(new Windows.UI.Xaml.Controls.RowDefinition() { Height = new Windows.UI.Xaml.GridLength(480, Windows.UI.Xaml.GridUnitType.Auto) });
            _pageContainer.RowDefinitions.Add(new Windows.UI.Xaml.Controls.RowDefinition() { Height = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Star) });

            _pageContainer.ColumnDefinitions.Add(new Windows.UI.Xaml.Controls.ColumnDefinition() { Width = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Star) });
            _pageContainer.ColumnDefinitions.Add(new Windows.UI.Xaml.Controls.ColumnDefinition() { Width = new Windows.UI.Xaml.GridLength(640, Windows.UI.Xaml.GridUnitType.Pixel) });
            _pageContainer.ColumnDefinitions.Add(new Windows.UI.Xaml.Controls.ColumnDefinition() { Width = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Star) });

            AddToolBar();
            RenderHud();

            Children.Add(_pageContainer);

            _mapControl = new MapControl();
            _mapControl.Width = 640;
            _mapControl.MapServiceToken = "s5miuLzzn4RuPyMXzOYF~pA3KRBwzLZ4JOHnyIaUAWA~AnoR9G-Mf6OR7_n8b6wVy_cd9wim48xfSp39TC31OlvLad6zT5Pf0XN35EPuEV5U";
            _mapControl.SetValue(Windows.UI.Xaml.Controls.Grid.ColumnProperty, 1);
            _mapControl.SetValue(Windows.UI.Xaml.Controls.Grid.RowProperty, 1);
            _pageContainer.Children.Add(_mapControl);
        }

        private void AddToolBar()
        {
            _startVideo = new Windows.UI.Xaml.Controls.Button()
            {
                Width = 200,
                Height = 48,
                Content = "Start",
                IsEnabled = true
            };

            _startVideo.Click += _startVideo_Click;
            _stopVideo = new Windows.UI.Xaml.Controls.Button()
            {
                Width = 200,
                Height = 48,
                Content = "Stop",
                IsEnabled = false
            };
            _stopVideo.Click += _stopVideo_Click;

            _cameras = new ComboBox();
            _cameras.Width = 200;

            var sp = new StackPanel();
            sp.Children.Add(_startVideo);
            sp.Children.Add(_stopVideo);
            sp.Children.Add(_cameras);
            _pageContainer.Children.Add(sp);

            sp.VerticalAlignment = VerticalAlignment.Top;
        }

        private void _stopVideo_Click(object sender, RoutedEventArgs e)
        {
            _captureElement.Visibility = Visibility.Collapsed;
            _cameras.IsEnabled = true;
            _startVideo.IsEnabled = true;
            _stopVideo.IsEnabled = false;
            ShutDownVideo();
        }

        private async void _startVideo_Click(object sender, RoutedEventArgs e)
        {
            if (_cameras.SelectedIndex > 0)
            {
                var device = _devices[_cameras.SelectedIndex - 1];

                var mediaInitSettings = new MediaCaptureInitializationSettings { VideoDeviceId = device.Id };


                var profiles = MediaCapture.FindAllVideoProfiles(device.Id);

                var match = (from profile in profiles
                             from desc in profile.SupportedRecordMediaDescription
                             where desc.Width == 640 && desc.Height == 480 && Math.Round(desc.FrameRate) == 30
                             select new { profile, desc }).FirstOrDefault();

                if (match != null)
                {
                    mediaInitSettings.VideoProfile = match.profile;
                    mediaInitSettings.RecordMediaDescription = match.desc;
                }
                else if (profiles.Count > 0)
                {
                    mediaInitSettings.VideoProfile = profiles[0];
                }

                _displayRequest = new DisplayRequest();
                _mediaCapture = new MediaCapture();

                _displayRequest.RequestActive();
                await _mediaCapture.InitializeAsync(mediaInitSettings);

                _captureElement.Source = _mediaCapture;
                _captureElement.Visibility = Visibility.Visible;

                _captureElement.SetValue(Windows.UI.Xaml.Controls.Grid.RowProperty, 0);
                _captureElement.SetValue(Windows.UI.Xaml.Controls.Grid.ColumnProperty, 1);

                await _mediaCapture.StartPreviewAsync();
                _cameras.IsEnabled = false;
                _startVideo.IsEnabled = false;
                _stopVideo.IsEnabled = true;

            }
        }

        private Grid CreateCompass()
        {
            var compassContainer = new Grid()
            {
                Width = 200,
                Height = 60,
                Clip = new RectangleGeometry() { Rect = new Windows.Foundation.Rect() { Height = 60, Width = 250 } }
            };

            _compass = new Canvas()
            {
                Width = 200,
                Height = 200,
                Margin = new Thickness(0, 0, 0, 0)
            };

            _compassTransform = new RotateTransform()
            {
                Angle = -90,
                CenterX = 100,
                CenterY = 100,
            };

            _compass.RenderTransform = _compassTransform;

            _compass.Children.Add(new Ellipse()
            {
                Stroke = _hudGreen,
                StrokeThickness = 2,
                Width = 200,
                Height = 200,
            });

            for (var idx = 0; idx < 360; idx += 15)
            {
                var line = new Line()
                {
                    Stroke = _hudGreen,
                    StrokeThickness = 2,
                    Height = 200,
                    Width = 200,
                    X1 = 100 + Math.Cos((idx * Math.PI) / 180.0f) * ((idx % 30 == 0) ? 93 : 80),
                    Y1 = 100 + Math.Sin((idx * Math.PI) / 180.0f) * ((idx % 30 == 0) ? 93 : 80),
                    X2 = 100 + Math.Cos((idx * Math.PI) / 180.0f) * 100,
                    Y2 = 100 + Math.Sin((idx * Math.PI) / 180.0f) * 100,
                };

                Debug.WriteLine($"{idx} - {line.X1},{line.X2} - {line.Y1},{line.Y2}");

                _compass.Children.Add(line);

                if (idx % 30 == 0)
                {
                    var indicatorText = idx.ToString();
                    var fontSize = 12;
                    var top = 80;
                    switch (idx)
                    {
                        case 0: indicatorText = "N"; fontSize = 14; top = 85; break;
                        case 90: indicatorText = "E"; fontSize = 14; top = 85; break;
                        case 180: indicatorText = "S"; fontSize = 14; top = 85; break;
                        case 270: indicatorText = "W"; fontSize = 14; top = 85; break;
                    }

                    var txt = new TextBlock()
                    {
                        Text = indicatorText,
                        FontSize = fontSize,
                        Width = 30,
                        Height = 20,
                        Margin = new Thickness(-15, -10, 0, 0),

                        TextAlignment = TextAlignment.Center,
                        Foreground = _hudGreen,
                        RenderTransform = new RotateTransform()
                        {
                            Angle = idx + 90,
                            CenterX = 15,
                            CenterY = 10,
                        }
                    };

                    txt.SetValue(Canvas.LeftProperty, 100 + (Math.Cos((idx * Math.PI) / 180.0f) * 85));
                    txt.SetValue(Canvas.TopProperty, 100 + (Math.Sin((idx * Math.PI) / 180.0f) * 85));
                    _compass.Children.Add(txt);
                }
            }

            compassContainer.Children.Add(_compass);
            return compassContainer;
        }

        private void RenderHud()
        {
            var hudContainer = new Windows.UI.Xaml.Controls.Grid();

            _captureElement = new CaptureElement();
            _captureElement.Width = 640;
            _captureElement.Height = 480;
            _captureElement.Visibility = Visibility.Collapsed;
            hudContainer.Children.Add(_captureElement);
            hudContainer.SetValue(Windows.UI.Xaml.Controls.Grid.ColumnProperty, 1);

            _pitchTransform = new TranslateTransform() { Y = 0 };
            _rollTransform = new RotateTransform() { Angle = 0, CenterX = 240, CenterY = 240, };
            _compassTransform = new RotateTransform() { Angle = 0, CenterX = 240, CenterY = 240, };

            _aoaCircle = new Windows.UI.Xaml.Controls.Grid() { Width = 480, Height = 480, RenderTransform = _rollTransform };

            var compass = CreateCompass();
            compass.VerticalAlignment = VerticalAlignment.Top;
            hudContainer.Children.Add(compass);

            _heading = new TextBlock
            {
                FontSize = 20,
                Foreground = _hudGreen,
                Margin = new Thickness(0, 30, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            hudContainer.Children.Add(_heading);

            _altitude = new TextBlock();
            _altitude.FontSize = 28;
            _altitude.Foreground = _hudGreen;
            _altitude.VerticalAlignment = VerticalAlignment.Center;
            _altitude.HorizontalAlignment = HorizontalAlignment.Right;
            hudContainer.Children.Add(_altitude);

            _gps = new TextBlock();
            _gps.FontSize = 28;
            _gps.Foreground = _hudGreen;
            _gps.VerticalAlignment = VerticalAlignment.Bottom;
            _gps.HorizontalAlignment = HorizontalAlignment.Right;
            _gps.Margin = new Thickness(0, 0, 30, 0);
            _gps.TextAlignment = TextAlignment.Right;
            hudContainer.Children.Add(_gps);

            _aoaCircle.Children.Add(new Line()
            {
                Stroke = _hudGreen,
                StrokeThickness = 10,
                Height = 10,
                Width = 300,
                X1 = 0,
                X2 = 300,
                RenderTransform = _pitchTransform
            });

            hudContainer.Width = 640;
            hudContainer.Height = 480;
            hudContainer.Children.Add(_aoaCircle);
            hudContainer.Background = new SolidColorBrush(Windows.UI.Colors.Black);
            hudContainer.SetValue(Windows.UI.Xaml.Controls.Grid.RowProperty, 0);
            hudContainer.SetValue(Windows.UI.Xaml.Controls.Grid.ColumnProperty, 1);

            _pageContainer.Children.Add(hudContainer);

        }

        private async void GetDevices()
        {
            _devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            _cameras.Items.Add("-select camera-");
            foreach (var device in _devices)
            {
                _cameras.Items.Add(device.Name);
            }

            _cameras.SelectedIndex = 0;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Frame> e)
        {
            _uas = SLWIOC.Get<IConnectedUasManager>().Active.Uas;
            _uas.Attitude.PropertyChanged += Attitude_PropertyChanged;
            _uas.PropertyChanged += _uas_PropertyChanged;
            _uas.GPSs.First().PropertyChanged += AttitudeIndicatorRenderer_PropertyChanged;
            _pageContainer.DataContext = _uas;

            base.OnElementChanged(e);
            Background = new SolidColorBrush(Colors.LightBlue);

            GetDevices();
        }

        private void AttitudeIndicatorRenderer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(GPS.HorizontalAccuracy):
                    _gps.Text = _uas.GPSs.First().FixType;
                    break;
            }
        }

        private void _uas_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUas.CurrentLocation))
            {
                if (_uas.CurrentLocation != null)
                {
                    _altitude.Text = $"{_uas.CurrentLocation.Altitude:0.000}";
                }
            }
        }

        private void Attitude_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Attitude.Roll)) _rollTransform.Angle = _uas.Attitude.Roll;
            if (e.PropertyName == nameof(Attitude.Pitch)) _pitchTransform.Y = _uas.Attitude.Pitch * 3;
            if (e.PropertyName == nameof(Attitude.Yaw))
            {
                _heading.Text = $"{_uas.Attitude.Yaw:0.0}°";
                _compassTransform.Angle = 270 - _uas.Attitude.Yaw;
            }

            //if (e.PropertyName == nameof(Attitude.Pitch)) _aoaRotation.Angle = _iUas.Attitude.Roll;
        }

        private async void ShutDownVideo()
        {
            if (_displayRequest != null)
            {
                _displayRequest.RequestRelease();
                _displayRequest = null;
            }

            _captureElement.Source = null;

            if (_mediaCapture != null)
            {
                await _mediaCapture.StopPreviewAsync();
                _mediaCapture.Dispose();
                _mediaCapture = null;
            }
        }

        protected override async void Dispose(bool disposing)
        {

            ShutDownVideo();

            base.Dispose(disposing);
        }
    }
}
