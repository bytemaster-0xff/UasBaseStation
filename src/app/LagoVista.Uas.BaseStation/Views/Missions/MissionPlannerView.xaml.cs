﻿using LagoVista.XPlat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LagoVista.Uas.BaseStation.Views.Missions
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MissionPlannerView : LagoVistaContentPage
    {
		public MissionPlannerView ()
		{
			InitializeComponent ();
		}
	}
}