﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LagoVista.Uas.BaseStation.ControlApp.Controls
{
    public class HudControlBase : Grid
    {
        public HudControlBase()
        {

        }

        Color _foreground = Colors.White;
        public Color Foreground
        {
            get { return _foreground; }
            set
            {
                _foreground = value;
                _foregroundBrush.Color = value;
            }
        }

        SolidColorBrush _foregroundBrush = new SolidColorBrush(Colors.White);
        protected SolidColorBrush ForegroundBrush
        {
            get { return _foregroundBrush; }
        }

        public async void RunOnUIThread(Action action)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
             {
                 action();
             });
        }
    }
}
