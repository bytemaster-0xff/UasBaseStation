using DJI.WindowsSDK;
using DJIVideoParser;
using LagoVista.Core.Commanding;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LagoVista.Uas.BaseStation.ControlApp.Controls
{
    public class H264FPVView : Grid
    {
        private DJIVideoParser.Parser _videoParser;

        SwapChainPanel _swapChainPannel;

        public H264FPVView()
        {
            _swapChainPannel = new SwapChainPanel
            {
                Width = 640,
                Height = 480,
                Visibility = Visibility.Collapsed
            };
            Children.Add(_swapChainPannel);

            StartVideoCommand = new RelayCommand(StartVideo);
            StopVideoCommand = new RelayCommand(StopVideo);
        }

        public async void StartVideo()
        {
            //Raw data and decoded data listener
            if (_videoParser == null)
            {
                _videoParser = new DJIVideoParser.Parser();
                _videoParser.Initialize(delegate (byte[] data)
                {                 
                    return DJISDKManager.Instance.VideoFeeder.ParseAssitantDecodingInfo(0, data);                    
                });

                _videoParser.SetSurfaceAndVideoCallback(0, 0, _swapChainPannel, ReceiveDecodedData);
                DJISDKManager.Instance.VideoFeeder.GetPrimaryVideoFeed(0).VideoDataUpdated += OnVideoPush;
            }
            //get the camera type and observe the CameraTypeChanged event.
            DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).CameraTypeChanged += OnCameraTypeChanged;
            var type = await DJISDKManager.Instance.ComponentManager.GetCameraHandler(0, 0).GetCameraTypeAsync();
            OnCameraTypeChanged(this, type.value);
            this._swapChainPannel.Visibility = Visibility.Visible;
        }

        public void StopVideo()
        {
            if(_videoParser != null)
            {
                DJISDKManager.Instance.VideoFeeder.GetPrimaryVideoFeed(0).VideoDataUpdated -= OnVideoPush;
                _videoParser = null;
            }
        }

        void ReceiveDecodedData(byte[] data, int width, int height)
        {

        }

        void OnVideoPush(VideoFeed sender, byte[] bytes)
        {
            _videoParser.PushVideoData(0, 0, bytes, bytes.Length);
        }

        //We need to set the camera type of the aircraft to the DJIVideoParser. After setting camera type, DJIVideoParser would correct the distortion of the video automatically.
        private void OnCameraTypeChanged(object sender, CameraTypeMsg? value)
        {
            if (value != null)
            {
                switch (value.Value.value)
                {
                    case CameraType.MAVIC_2_ZOOM:
                        this._videoParser.SetCameraSensor(AircraftCameraType.Mavic2Zoom);
                        break;
                    case CameraType.MAVIC_2_PRO:
                        this._videoParser.SetCameraSensor(AircraftCameraType.Mavic2Pro);
                        break;
                    default:
                        this._videoParser.SetCameraSensor(AircraftCameraType.Others);
                        break;
                }
            }
        }

        public RelayCommand StartVideoCommand { get; }
        public RelayCommand StopVideoCommand { get; }
    }
}
