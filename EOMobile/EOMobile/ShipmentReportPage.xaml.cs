using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShipmentReportPage : ContentPage
	{
		public ShipmentReportPage ()
		{
			InitializeComponent ();
		}

        public void OnShowReportsClicked(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new PersonFilterPage());
        }
    }
}