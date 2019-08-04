using LagoVista.Core;
using LagoVista.Core.IOC;
using LagoVista.Uas.BaseStation.App.Drones;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.FlightRecorder;
using LagoVista.Uas.Core.Models;
using LagoVista.Uas.Core.Services;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LagoVista.Uas.BaseStation.App
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            var dispatcherService = new DispatcherService(Window.Current.Dispatcher);

            var uasMgr = new ConnectedUasManager();
            SLWIOC.Register<IHeartBeatManager, HeartBeatManager>();
            SLWIOC.RegisterSingleton<IConnectedUasManager>(uasMgr);
            SLWIOC.Register<IMissionPlanner, MissionPlanner>();
            SLWIOC.RegisterSingleton<IConfigurationManager>(new ConfigurationManager());
            SLWIOC.RegisterSingleton<IDispatcherServices>(dispatcherService);
            SLWIOC.RegisterSingleton<ITelemetryService, TelemetryService>();
            SLWIOC.RegisterSingleton<IFlightRecorder>(new FlightRecorder(dispatcherService));

            new DJIDrone(uasMgr, Window.Current.Dispatcher);

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }

                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }

    public class DispatcherService : IDispatcherServices
    {
        CoreDispatcher _dispatcher;
        public DispatcherService(CoreDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        public async void Invoke(Action action)
        {
            await this._dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                action();
            });
        }
    }
}
