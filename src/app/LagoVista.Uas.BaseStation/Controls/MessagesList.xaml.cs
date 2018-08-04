using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LagoVista.Uas.BaseStation.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessagesList : ListView
	{
		public MessagesList ()
		{
			InitializeComponent ();
		}
	}
}