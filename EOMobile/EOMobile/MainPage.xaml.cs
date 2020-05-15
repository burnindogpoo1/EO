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
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

        public void OnInventoryClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InventoryPage());
        }

        public void OnArrangementsClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TabbedArrangementPage());
        }

        public void OnWorkOrdersClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TabbedWorkOrderPage());
        }

        public void OnCustomersClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CustomerPage());
        }

        public void OnVendorsClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VendorPage());
        }

        public void OnShipmentsClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TabbedShipmentPage());
        }

        public void OnReportsClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ReportsPage());
        }

        private void OnSiteServiceClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TabbedSiteServicePage());
        }
    }
}