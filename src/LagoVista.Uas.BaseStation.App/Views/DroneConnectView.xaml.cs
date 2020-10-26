using LagoVista.Core.ViewModels;
using LagoVista.Uas.BaseStation.Core.ViewModels;
using LagoVista.Uas.Core.Models;
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
    public sealed partial class DroneConnectView : LagoVistaPage
    {
        public DroneConnectView()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = ViewModel as DroneConnectViewModel;
            var result = await vm.WiFiNetworkService.ConnectAsync(vm.MavicAirSSID);
            if(result.Successful)
            {
                var drone = new Drones.DJIDrone(vm.ConnectedUasMgr, this.Dispatcher);
                var transport = new Drones.DJITransport(drone);
                vm.ConnectedUasMgr.SetActive(new ConnectedUas(drone, transport));
                vm.ConnectedUasMgr.Active.Transport.Initialize();

                var args = new ViewModelLaunchArgs()
                {
                    ParentViewModel = vm,
                    LaunchType = LaunchTypes.View,
                    ViewModelType = typeof(FlightViewModel),
                };

                await vm.ViewModelNavigation.NavigateAsync(args);
            }
            else
            {
                await vm.Popups.ShowAsync($"Could not connect to {vm.MavicAirSSID}");
            }
        }
    }
}
