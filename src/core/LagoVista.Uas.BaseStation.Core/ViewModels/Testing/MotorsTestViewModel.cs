using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Commanding;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels.Testing
{
    public class MotorsTestViewModel : AppViewModelBase
    {
        IConnectedUasManager _mgr;


        public MotorsTestViewModel(IConnectedUasManager mgr)
        {
            _mgr = mgr;
            StartTestCommand = new RelayCommand(StartTest);
            StopTestCommand = new RelayCommand(StopTest);
        }

        public override Task InitAsync()
        {
            return base.InitAsync();
        }

        public void StartTest()
        {
            var cmd = UasCommands.DoMotorTest(_mgr.Active.Uas.SystemId, _mgr.Active.Uas.ComponentId, 1, (float)MotorTestThrottleType.MotorTestThrottlePercent, 25,5,4,(float)MotorTestOrder.Board);
            _mgr.Active.Transport.SendMessage(cmd);
        }

        public void StopTest()
        {

        }

        public RelayCommand StartTestCommand { get; }
        public RelayCommand StopTestCommand { get; }
    }
}
