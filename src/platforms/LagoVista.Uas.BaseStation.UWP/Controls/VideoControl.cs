using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LagoVista.Uas.BaseStation.UWP.Controls
{


    public class VideoControl : Grid, IDisposable
    {
        Button _startVideo;
        Button _stopVideo;
        ComboBox _cameras;
        DeviceInformationCollection _devices;
        MediaCapture _mediaCapture;
        DisplayRequest _displayRequest;
        CaptureElement _captureElement;

        public VideoControl()
        {
            _startVideo = new Windows.UI.Xaml.Controls.Button()
            {
                Width = 100,
                Height = 32,
                Content = "Start",
                IsEnabled = true
            };

            _startVideo.Click += _startVideo_Click;
            _stopVideo = new Windows.UI.Xaml.Controls.Button()
            {
                Width = 100,
                Height = 32,
                Content = "Stop",
                IsEnabled = false
            };
            _stopVideo.Click += _stopVideo_Click;

            _cameras = new ComboBox();
            _cameras.Width = 200;

            var sp = new StackPanel() { Orientation = Orientation.Horizontal };
            sp.Children.Add(_startVideo);
            sp.Children.Add(_stopVideo);
            sp.Children.Add(_cameras);
            sp.VerticalAlignment = VerticalAlignment.Bottom;
            sp.HorizontalAlignment = HorizontalAlignment.Left;
            Children.Add(sp);

            _captureElement = new CaptureElement();
            _captureElement.Width = 640;
            _captureElement.Height = 480;
            _captureElement.Visibility = Visibility.Collapsed;
            Children.Add(_captureElement);
        }
  
        public async void GetDevices()
        {
            _devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            _cameras.Items.Add("-select camera-");
            foreach (var device in _devices)
            {
                _cameras.Items.Add(device.Name);
            }

            _cameras.SelectedIndex = 0;
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

        private void _stopVideo_Click(object sender, RoutedEventArgs e)
        {
            _captureElement.Visibility = Visibility.Collapsed;
            _cameras.IsEnabled = true;
            _startVideo.IsEnabled = true;
            _stopVideo.IsEnabled = false;
            ShutDownVideo();
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

        public void Dispose()
        {
            ShutDownVideo();
        }
    }
}
