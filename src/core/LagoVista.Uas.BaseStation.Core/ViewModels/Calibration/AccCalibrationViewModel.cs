using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Commanding;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.MavLink;
using LagoVista.Uas.Core.Models;
using System;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.Core.ViewModels.Calibration
{
    public class AccCalibrationViewModel : AppViewModelBase
    {
        IConnectedUasManager _connectedUasManager;
        public AccCalibrationViewModel(IConnectedUasManager connectedUasManager)
        {
            _connectedUasManager = connectedUasManager;
            DoneCommand = new RelayCommand(Done, () => IsDone);
            BeginCommand = new RelayCommand(Begin, () => !IsActive);
            CancelCommand = new RelayCommand(Cancel, () => IsActive);
            NextCommand = new RelayCommand(Next, () => IsActive);
        }

        public override Task InitAsync()
        {
            _connectedUasManager.Active.Transport.OnMessageReceived += _transport_OnMessageReceived;
            return base.InitAsync();
        }

        public override Task IsClosingAsync()
        {
            _connectedUasManager.Active.Transport.OnMessageReceived -= _transport_OnMessageReceived;
            return base.IsClosingAsync();
        }

        private void _transport_OnMessageReceived(object sender, UasMessage arg)
        {
            switch (arg.MessageId)
            {
                case UasMessages.Statustext:
                    var stat = arg as UasStatustext;
                    var message = new String(stat.Text);

                    if (message.ToLower().Contains("calibration successful") || message.ToLower().Contains("calibration failed"))
                    {
                        IsDone = true;
                        IsActive = false;
                        DoneCommand.RaiseCanExecuteChanged();
                        UserMessage = message;
                    }

                    break;
                case UasMessages.CommandLong:
                    var cmd = arg as UasCommandLong;
                    if (cmd.Command == (ushort)MavCmd.AccelcalVehiclePos)
                    {
                        var pos = (AccelcalVehiclePos)cmd.Param1;
                        UserMessage = "Please place vehicle " + pos.ToString();
                    }

                    break;
            }
        }

        private bool _isDone = false;
        public bool IsDone
        {
            get { return _isDone; }
            set { Set(ref _isDone, value); }
        }

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set { Set(ref _isActive, value); }
        }

        public void Done()
        {
            _connectedUasManager.Active.Transport.SendMessage(UasCommands.PreflightCalibration(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, 0, 0, 2, 0, 0));
            BeginCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
            DoneCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
        }

        public void Cancel()
        {
            IsActive = false;

            _connectedUasManager.Active.Transport.SendMessage(UasCommands.PreflightCalibration(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, 0, 0, 0, 0, 0));
            BeginCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
            DoneCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
        }

        public void Next()
        {
            if(IsActive)
            {
                var ack = new UasCommandAck()
                {
                    TargetComponent = _connectedUasManager.Active.Uas.ComponentId,
                    TargetSystem = _connectedUasManager.Active.Uas.SystemId,
                    Command = 1,
                    Result = 1,
                };

                _connectedUasManager.Active.Transport.SendMessage(ack);
            }

            _connectedUasManager.Active.Transport.SendMessage(UasCommands.PreflightCalibration(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, 0, 0, 1, 0, 0));
            BeginCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
            DoneCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
        }

        public void Begin()
        {
            IsDone = false;
            IsActive = true;

            _connectedUasManager.Active.Transport.SendMessage(UasCommands.PreflightCalibration(_connectedUasManager.Active.Uas.SystemId, _connectedUasManager.Active.Uas.ComponentId, 0, 0, 0, 0, 1, 0, 0));
            BeginCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
            DoneCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
        }

        private String _userMessage;
        public string UserMessage
        {
            get { return _userMessage; }
            set { Set(ref _userMessage, value); }
        }

        public RelayCommand BeginCommand { get; }
        public RelayCommand NextCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand DoneCommand { get; }
    }
}
