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
            Navigation.PushAsync(new ArrangementPage());
        }

        public void OnWorkOrdersClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WorkOrderPage());
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
            Navigation.PushAsync(new ShipmentPage());
        }

        public void OnReportsClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ReportsPage());
        }
    }
}