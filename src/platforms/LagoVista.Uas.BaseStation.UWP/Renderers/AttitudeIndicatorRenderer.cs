using LagoVista.Core.IOC;
using LagoVista.Uas.BaseStation.UWP.Controls;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Models;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;

namespace LagoVista.Uas.BaseStation.UWP.Renderers
{

    public class AttitudeIndicatorRenderer : FrameRenderer
    {
        RotateTransform _rollTransform;
        TranslateTransform _pitchTransform;

        

        Controls.AltitudeIndicator _altitudeIndicator;
        Controls.Compass _compass;
        Controls.ArtificialHorizon _artificialHorizon;
        Controls.GPSStatus _gpsStatus;
        Controls.VideoControl _video;
        Controls.AoACircle _aoaCircle;
        Controls.SystemStatus _systemStatus;
  
        IUas _uas;
     
        SolidColorBrush _hudGreen = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 51, 255, 85));
        SolidColorBrush _hudWhite = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
        SolidColorBrush _hudColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));

        public AttitudeIndicatorRenderer()
        {
            ArrangeNativeChildren = true;
  
            RenderHud();

            this.Height = 480;
            this.Width = 640;
         }
      

        private void RenderHud()
        {
            var hudContainer = new Windows.UI.Xaml.Controls.Grid();
            hudContainer.Width = 640;
            hudContainer.Height = 480;

            

            _artificialHorizon = new Controls.ArtificialHorizon();
            hudContainer.Children.Add(_artificialHorizon);

            _video = new Controls.VideoControl();
            hudContainer.Children.Add(_video);

            _pitchTransform = new TranslateTransform() { Y = 0 };
            _rollTransform = new RotateTransform() { Angle = 0, CenterX = 240, CenterY = 240, };

            _compass = new Controls.Compass();
            _compass.VerticalAlignment = VerticalAlignment.Top;
            hudContainer.Children.Add(_compass);
         
            _altitudeIndicator = new Controls.AltitudeIndicator();
            _altitudeIndicator.HorizontalAlignment = HorizontalAlignment.Right;
            _altitudeIndicator.VerticalAlignment = VerticalAlignment.Center;
            hudContainer.Children.Add(_altitudeIndicator);

            _systemStatus = new Controls.SystemStatus();
            _systemStatus.Margin = new Thickness(0, 0, 0, 60);
            _systemStatus.VerticalAlignment = VerticalAlignment.Bottom;
            _systemStatus.HorizontalAlignment = HorizontalAlignment.Left;
            hudContainer.Children.Add(_systemStatus);

            _aoaCircle = new Controls.AoACircle();
            hudContainer.Children.Add(_aoaCircle);

            _gpsStatus = new Controls.GPSStatus
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            hudContainer.Children.Add(_gpsStatus);

            hudContainer.SetValue(Windows.UI.Xaml.Controls.Grid.RowProperty, 0);
            hudContainer.SetValue(Windows.UI.Xaml.Controls.Grid.ColumnProperty, 1);

            Children.Add(hudContainer);
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Frame> e)
        {
            _uas = SLWIOC.Get<IConnectedUasManager>().Active.Uas;
            _uas.Attitude.PropertyChanged += Attitude_PropertyChanged;
            DataContext = _uas;

            var gpsBindingHelper = new BindingHelper<GPSStatus>(_gpsStatus);
            gpsBindingHelper.Add(_uas.GPSs.First()).For(mod => mod.FixType, ctl=>ctl.FixType);
            gpsBindingHelper.Add(_uas.GPSs.First()).For(mod => mod.HDOP, ctl=>ctl.HDOP);
            gpsBindingHelper.Add(_uas.GPSs.First()).For(mod => mod.VDOP, ctl => ctl.VDOP);
            gpsBindingHelper.Add(_uas.GPSs.First()).For(mod => mod.SateliteCount, ctl=>ctl.SatCount);
            
            var sysStatusbindingHelper = new BindingHelper<Controls.SystemStatus>(_systemStatus);
            sysStatusbindingHelper.Add(_uas.SystemStatus).For(mod => mod.Armed, ctl=>ctl.Armed);
            sysStatusbindingHelper.Add(_uas.Batteries.First()).For(mod => mod.Voltage, ctl=>ctl.BatteryVoltage);
            sysStatusbindingHelper.Add(_uas.Batteries.First()).For(mod => mod.RemainingPercent, ctl=>ctl.PercentRemaining);
            sysStatusbindingHelper.Add(_uas.Batteries.First()).For(mod => mod.TimeRemaining, ctl=>ctl.TimeRemaining);
        
            var altStatusbindingHelper = new BindingHelper<Controls.AltitudeIndicator>(_altitudeIndicator);
            altStatusbindingHelper.Add(_uas).For(mod => mod.CurrentLocation, ctl=>ctl.Location);


            //            _bindignHelper.Add(_uas.GPSs.First(), () => _gpsStatus.FixType, nameof(GPS.FixType));

            base.OnElementChanged(e);
            Background = new SolidColorBrush(Colors.LightBlue);

            _video.GetDevices();
        }


        private void Attitude_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Attitude.Roll))
            {
                _aoaCircle.Roll = _uas.Attitude.Roll;
                _rollTransform.Angle = _uas.Attitude.Roll;
                _artificialHorizon.Roll = _uas.Attitude.Roll;
            }
            else if (e.PropertyName == nameof(Attitude.Pitch))
            {
                _aoaCircle.Pitch = _uas.Attitude.Pitch;
                _pitchTransform.Y = _uas.Attitude.Pitch * 3;
                _artificialHorizon.Pitch = _uas.Attitude.Pitch;
            }
            else if (e.PropertyName == nameof(Attitude.Yaw))
            {
                _compass.Heading = _uas.Attitude.Yaw;
            }           
        }

        protected override void Dispose(bool disposing)
        {
            _video.Dispose();

            base.Dispose(disposing);
        }
    }
}
