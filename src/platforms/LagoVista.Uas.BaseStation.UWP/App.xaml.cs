using LagoVista.Core.IOC;
using LagoVista.Uas.BaseStation.UWP.Drone;
using LagoVista.Uas.Core.Interfaces;
using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LagoVista.Uas.BaseStation.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += (object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e) =>
            {
                Debug.WriteLine(String.Empty);
                Debug.WriteLine("======================================================================================");
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.Exception.StackTrace);
                Debug.WriteLine("======================================================================================");
                Debug.WriteLine(String.Empty);
            };
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                SLWIOC.RegisterSingleton<ILocationProvider>(new Services.LocationProvider());

                rootFrame.NavigationFailed += OnNavigationFailed;

                Xamarin.Forms.Forms.Init(e);
                Xamarin.FormsMaps.Init("s5miuLzzn4RuPyMXzOYF~pA3KRBwzLZ4JOHnyIaUAWA~AnoR9G-Mf6OR7_n8b6wVy_cd9wim48xfSp39TC31OlvLad6zT5Pf0XN35EPuEV5U");

                LagoVista.Core.UWP.Startup.Init(this, rootFrame.Dispatcher, "a8d44f67-212b-4357-aea8-d55d44156c59");

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
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
}
