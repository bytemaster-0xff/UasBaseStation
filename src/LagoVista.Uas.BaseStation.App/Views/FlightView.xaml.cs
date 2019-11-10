using LagoVista.Core.IOC;
using LagoVista.Core.ViewModels;
using LagoVista.Uas.BaseStation.Core.ViewModels;
using LagoVista.UWP.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LagoVista.Uas.BaseStation.ControlApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlightView : LagoVistaPage
    {
        public FlightView()
        {
            this.InitializeComponent();
        }

        /*protected override void SetViewModel(ViewModelBase vm)
        {
            ViewModel = vm as FlightViewModel;
            this.DataContext = this;
        }*/


        public new FlightViewModel ViewModel { get; private set; }
    }
}
