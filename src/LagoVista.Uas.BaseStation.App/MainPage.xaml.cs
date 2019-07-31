using LagoVista.Core.IOC;
using LagoVista.Uas.BaseStation.App.Drones;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Models;
using LagoVista.Uas.Core.Services;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LagoVista.Uas.BaseStation.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var uasMgr = new ConnectedUasManager();
            SLWIOC.RegisterSingleton<IConnectedUasManager>(uasMgr);
            SLWIOC.Register<IHeartBeatManager, HeartBeatManager>();
            SLWIOC.Register<IMissionPlanner, MissionPlanner>();
            SLWIOC.RegisterSingleton<IConfigurationManager>(new ConfigurationManager());
            SLWIOC.RegisterSingleton<ITelemetryService, TelemetryService>();

            var _djiDrone = new DJIDrone(uasMgr);
            this.MissionPlanner = new MissionPlanner(uasMgr);

            this.Navigation  = new LagoVista.Uas.Core.Services.Navigation(uasMgr, MissionPlanner);

            AOAControl.GetDevices();

            NotifyPropertyChanged(nameof(Navigation));
            NotifyPropertyChanged(nameof(MissionPlanner));
            DataContext = this;
        }

        public LagoVista.Uas.Core.Services.Navigation Navigation { get; private set; }

        public MissionPlanner MissionPlanner { get; private set; }
    }
}
