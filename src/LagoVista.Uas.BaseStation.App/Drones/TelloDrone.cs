using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Models;
using Messenger.Udp;
using System;
using System.Net;
using System.Threading.Tasks;
using Tello.Controller;
using Windows.UI.Core;

namespace LagoVista.Uas.BaseStation.ControlApp.Drones
{
    public class TelloDrone : UasBase
    {
        private readonly CoreDispatcher _dispatcher;
        private readonly IConnectedUasManager _mgr;
        private readonly IVideoObserver _videoObserver;

        public TelloDrone(IConnectedUasManager mgr, CoreDispatcher dispatcher) : base(null)
        {
            this._dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            this._mgr = mgr ?? throw new ArgumentNullException(nameof(mgr));
            Task.Run(() =>
            {
                this.Init();
            });
        }

        public async void Init()
        {
            var transceiver = new UdpTransceiver(IPAddress.Parse("192.168.10.1"), 8889);
            var stateReceiver = new UdpReceiver(8890);
            var videoReceiver = new UdpReceiver(11111);

            Tello = new DroneMessenger(transceiver, stateReceiver, videoReceiver);

            Tello.Controller.ConnectionStateChanged += Controller_ConnectionStateChanged;
            Tello.Controller.PositionChanged += Controller_PositionChanged;
            Tello.StateObserver.StateChanged += StateObserver_StateChanged;
            Tello.VideoObserver.VideoSampleReady += VideoObserver_VideoSampleReady;

            await Tello.Controller.Connect();
        }

        private void VideoObserver_VideoSampleReady(object sender, Tello.Events.VideoSampleReadyArgs e)
        {
            
        }

        private void Controller_PositionChanged(object sender, Tello.Events.PositionChangedArgs e)
        {
            RunOnUIThread(() =>
            {

                this.Location = new LagoVista.Core.Models.Geo.GeoLocation(e.Position.X, e.Position.Y);
                this.Attitude.Yaw = e.Position.Heading;
            });
        }

        public DroneMessenger Tello { get; private set; }

        public async void RunOnUIThread(Action action)
        {
            await this._dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                action();
            });
        }

        private void Controller_ConnectionStateChanged(object sender, Tello.Events.ConnectionStateChangedArgs e)
        {
            RunOnUIThread(() => _mgr.SetActive(new ConnectedUas(this, new TelloTransport(this))));
        }

        private void StateObserver_StateChanged(object sender, Tello.Events.StateChangedArgs e)
        {
            RunOnUIThread(() =>
            {
                this.Battery.RemainingPercent = e.State.BatteryPercent;
                this.AngleOfAttack = e.State.Attitude.Pitch;
                this.Attitude.Pitch = e.State.Attitude.Pitch;
                this.Attitude.Roll = e.State.Attitude.Roll;
                this.Attitude.Yaw = e.State.Attitude.Yaw;
                this.TemperatureHigh = e.State.TemperatureHighC;
                this.TemperatureLow = e.State.TemperatureLowC;
            });
        }
    }
}
