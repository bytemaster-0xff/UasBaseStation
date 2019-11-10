//#define ENV_DEV
#define ENV_MASTER

using LagoVista.Client.Core;
using LagoVista.Client.Core.Auth;
using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.Net;
using LagoVista.Client.Core.ViewModels;
using LagoVista.Client.Core.ViewModels.Auth;
using LagoVista.Core;
using LagoVista.Core.Authentication.Interfaces;
using LagoVista.Core.Geo;
using LagoVista.Core.Interfaces;
using LagoVista.Core.IOC;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.UWP.Loggers;
using LagoVista.Core.UWP.Services;
using LagoVista.Core.ViewModels;
using LagoVista.Uas.BaseStation.ControlApp.Drones;
using LagoVista.Uas.BaseStation.ControlApp.Views;
using LagoVista.Uas.BaseStation.Core.ViewModels;
using LagoVista.Uas.Core;
using LagoVista.Uas.Core.FlightRecorder;
using LagoVista.Uas.Core.Interfaces;
using LagoVista.Uas.Core.Models;
using LagoVista.Uas.Core.Services;
using LagoVista.XPlat.Core.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LagoVista.Uas.BaseStation.ControlApp
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            UnhandledException += (sender, e) =>
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.Exception.StackTrace);

                if (global::System.Diagnostics.Debugger.IsAttached) global::System.Diagnostics.Debugger.Break();
            };

        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if ENV_STAGE
            var mobileCenterKey = "d3f162da-76c5-4880-b19c-7a038d6af46f";
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "api.nuviot.com",
            };
#elif ENV_DEV
            var mobileCenterKey = "d3f162da-76c5-4880-b19c-7a038d6af46f";
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "dev-api.nuviot.com",
            };
#elif ENV_LOCALDEV
            var mobileCenterKey = "d3f162da-76c5-4880-b19c-7a038d6af46f";
            var serverInfo = new ServerInfo()
            {
                SSL = false,
                RootUrl = "localhost:5001",
            };
#elif ENV_MASTER
            var mobileCenterKey = "d3f162da-76c5-4880-b19c-7a038d6af46f";
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "api.nuviot.com",
            };
#endif

            var rootFrame = Window.Current.Content as Frame;

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

            this.DebugSettings.EnableFrameRateCounter = false;

            var uasMgr = new ConnectedUasManager();
            
            SLWIOC.Register<IHeartBeatManager, HeartBeatManager>();
            SLWIOC.RegisterSingleton<IConnectedUasManager>(uasMgr);
            SLWIOC.Register<IMissionPlanner, MissionPlanner>();
            SLWIOC.RegisterSingleton<IConfigurationManager>(new ConfigurationManager());
            SLWIOC.RegisterSingleton<ITelemetryService, TelemetryService>();
            SLWIOC.RegisterSingleton<IFlightRecorder>(new FlightRecorder(dispatcherService));

            SLWIOC.RegisterSingleton<IClientAppInfo>(new ClientAppInfo());
            SLWIOC.RegisterSingleton<IAppConfig>(new UwpAppConfig());
            
            SLWIOC.RegisterSingleton<IDeviceInfo>(new UWPDeviceInfo());
            

            LagoVista.Core.UWP.Startup.Init(this, rootFrame.Dispatcher, mobileCenterKey);

            Startup.Init(serverInfo);

            var navigation = new LagoVista.UWP.UI.Navigation();
            navigation.Initialize(rootFrame);

            navigation.Add<SplashViewModel, SplashView>();
            navigation.Add<LoginViewModel, LoginView>();
            navigation.Add<DroneConnectViewModel, DroneConnectView>();

            SLWIOC.Register<IViewModelNavigation>(navigation);

            //new DJIDrone(uasMgr, Window.Current.Dispatcher);
            new TelloDrone(uasMgr, Window.Current.Dispatcher);

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    navigation.Navigate<SplashViewModel>();
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
