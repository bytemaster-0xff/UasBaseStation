using LagoVista.Core.IOC;
using System.Linq;
using LagoVista.Uas.BaseStation.Core.ViewModels;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.Models;
using LagoVista.Uas.Core.Services;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.Gaming.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LagoVista.Uas.BaseStation.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        Timer _timer;
        IConnectedUasManager _uasMgr;
        Gamepad _xboxController;
        Controller.GamePad _gamePad = new Controller.GamePad();

        public MainPage()
        {
            this.InitializeComponent();
            _timer = new Timer(Timer_callBack, null, 50, 50);   
        }
       
        private void Timer_callBack(object obj)
        {
            if(_gamePad != null)
            {
                _gamePad.Refresh(_xboxController);

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _uasMgr = SLWIOC.Get<IConnectedUasManager>();
            var missionPlanner = new MissionPlanner(_uasMgr);
            var navigation  = new LagoVista.Uas.Core.Services.Navigation(_uasMgr, missionPlanner);

            AOAControl.GetDevices();

            DataContext = new HudViewModel(uasMgr, navigation);

            NotifyPropertyChanged(nameof(ViewModel));

            Windows.Gaming.Input.Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Windows.Gaming.Input.FlightStick.FlightStickAdded += FlightStick_FlightStickAdded; 
        }

        private void FlightStick_FlightStickAdded(object sender, Windows.Gaming.Input.FlightStick e)
        {
            var fs = e;
        }

        private void Gamepad_GamepadAdded(object sender, Windows.Gaming.Input.Gamepad e)
        {
            this._xboxController = e;
        }

        public HudViewModel ViewModel { get; private set; }
    }
}
