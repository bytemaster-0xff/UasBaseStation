using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LagoVista.Uas.BaseStation.ControlApp.Controls
{
    public class VideoControl : Grid
    {
        Button _startVideo;
        Button _stopVideo;
        ComboBox _cameras;
        DeviceInformationCollection _devices;

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

            var sp = new StackPanel() { Orientation = Orientation.Vertical };
            sp.Children.Add(_startVideo);
            sp.Children.Add(_stopVideo);
            sp.Children.Add(_cameras);
            sp.VerticalAlignment = VerticalAlignment.Bottom;
            sp.HorizontalAlignment = HorizontalAlignment.Left;
            Children.Add(sp);
        }

        public async void GetDevices()
        {
            _devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            _cameras.Items.Add("-select camera-");
            _cameras.Items.Add("DJI - Video Feed");

            foreach (var device in _devices)
            {
                _cameras.Items.Add(device.Name);
            }

            _cameras.SelectedIndex = 0;
        }

        private void _startVideo_Click(object sender, RoutedEventArgs e)
        {
         /*   if (_cameras.SelectedIndex > 0)
            {

                if (_cameras.SelectedIndex == 1)
                {
                    ConnectDJICam();
                }
                else
                {
                    ConnectWebCam();
                }
            }*/
        }

        private void _stopVideo_Click(object sender, RoutedEventArgs e)
        {
            //_captureElement.Visibility = Visibility.Collapsed;
            _cameras.IsEnabled = true;
            _startVideo.IsEnabled = true;
            _stopVideo.IsEnabled = false;
        }
    }
}
