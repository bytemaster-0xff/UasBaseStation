using LagoVista.Core.IOC;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Models;
using System;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms.Platform.UWP;



namespace LagoVista.Uas.BaseStation.UWP.Renderers
{

    public class AttitudeIndicatorRenderer : FrameRenderer
    {
        RotateTransform _rollTransform;
        TranslateTransform _pitchTransform;
        CaptureElement _captureElement;
        DisplayRequest _displayRequest;
        MediaCapture _mediaCapture;
        Windows.UI.Xaml.Controls.Grid _aoaCircle;
        Windows.UI.Xaml.Controls.Grid _pageContainer;
        DeviceInformationCollection _devices;

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
            _mapControl.Width = 648;
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

        private void RenderHud()
        {
            var hudContainer = new Windows.UI.Xaml.Controls.Grid();
            hudContainer.SetValue(Windows.UI.Xaml.Controls.Grid.ColumnProperty, 1);

            _pitchTransform = new TranslateTransform()
            {
                Y = 0
            };

            _rollTransform = new RotateTransform()
            {
                Angle = 0,
                CenterX = 240,
                CenterY = 240,
            };

            _aoaCircle = new Windows.UI.Xaml.Controls.Grid();
            _aoaCircle.Width = 480;
            _aoaCircle.Height = 480;
            _aoaCircle.RenderTransform = _rollTransform;

            _heading = new TextBlock();
            _heading.FontSize = 28;
            _heading.Foreground = _hudGreen;
            _heading.VerticalAlignment = VerticalAlignment.Top;
            _heading.HorizontalAlignment = HorizontalAlignment.Center;

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
            hudContainer.Children.Add(_heading);
            hudContainer.SetValue(Windows.UI.Xaml.Controls.Grid.RowProperty, 0);
            hudContainer.SetValue(Windows.UI.Xaml.Controls.Grid.ColumnProperty, 1);
            _captureElement = new CaptureElement();
            _captureElement.Width = 640;
            _captureElement.Height = 480;
            _captureElement.Visibility = Visibility.Collapsed;
            hudContainer.Children.Add(_captureElement);

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

            base.OnElementChanged(e);
            Background = new SolidColorBrush(Colors.LightBlue);

            GetDevices();
        }        

        private void Attitude_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Attitude.Roll)) _rollTransform.Angle = _uas.Attitude.Roll;
            if (e.PropertyName == nameof(Attitude.Pitch)) _pitchTransform.Y = _uas.Attitude.Pitch * 3;
            if (e.PropertyName == nameof(Attitude.Yaw)) _heading.Text = $"{_uas.Attitude.Yaw:0.}°";

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
