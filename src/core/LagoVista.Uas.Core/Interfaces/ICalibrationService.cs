using LagoVista.Core.Commanding;

namespace LagoVista.Uas.Core
{
    public interface ICalibrationService
    {
        RelayCommand DoneCommand { get; }
        bool IsDone { get; set; }
        string UserMessage { get; set; }

        void BeginAccelerometer(IUas uas);
        void Done();
    }
}