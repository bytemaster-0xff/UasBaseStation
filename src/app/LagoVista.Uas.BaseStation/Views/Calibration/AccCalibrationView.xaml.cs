﻿using LagoVista.XPlat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LagoVista.Uas.BaseStation.Views.Calibration
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccCalibrationView : LagoVistaContentPage
    {
		public AccCalibrationView()
		{
			InitializeComponent ();
		}
	}
}