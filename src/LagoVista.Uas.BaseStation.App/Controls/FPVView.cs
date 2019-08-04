using DJI.WindowsSDK;
using DJIVideoParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LagoVista.Uas.BaseStation.ControlApp.Controls
{
    public class FPVView : Grid, IDisposable
    {
        private DJIVideoParser.Parser _videoParser;

        MediaCapture _mediaCapture;
        DisplayRequest _displayRequest;
        CaptureElement _captureElement;
        DeviceInformationCollection _devices;

        public FPVView()
        {
            _captureElement = new CaptureElement();
            _captureElement.Width = 640;
            _captureElement.Height = 480;
            _captureElement.Visibility = Visibility.Collapsed;
            Children.Add(_captureElement);
        }

        public async void GetDevices()
        {
            _devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
        }

        private async void ConnectWebCam(int cameraIndex)
        {
            var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            var device = devices[cameraIndex];

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
        }

       
        private void _stopVideo_Click(object sender, RoutedEventArgs e)
        {
            //_captureElement.Visibility = Visibility.Collapsed;            
            ShutDownVideo();
        }

        private async void ShutDownVideo()
        {
            if (_displayRequest != null)
            {
                _displayRequest.RequestRelease();
                _displayRequest = null;
            }

            //_captureElement.Source = null;

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
