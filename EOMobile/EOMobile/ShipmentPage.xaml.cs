using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShipmentPage : ContentPage
	{
        private List<VendorDTO> vendorList;
        List<ShipmentInventoryItemDTO> shipmentInventoryList = new List<ShipmentInventoryItemDTO>();
        private List<InventoryTypeDTO> inventoryTypeList = new List<InventoryTypeDTO>();
        ShipmentInventoryItemDTO searchedForInventory = new ShipmentInventoryItemDTO();
        public string User
        {
            get { return ((App)App.Current).User; }
            set { ((App)App.Current).User = value; }
        }

        public string Pwd
        {
            get { return ((App)App.Current).Pwd; }
            set { ((App)App.Current).Pwd = value; }
        }

        public ShipmentPage ()
		{
			InitializeComponent ();

            MessagingCenter.Subscribe<ArrangementFilterPage, WorkOrderInventoryItemDTO>(this, "UseFilter", async (sender, arg) =>
            {
                LoadFilter(arg);
            });

            vendorList = ((App)(App.Current)).GetVendors(new GetPersonRequest());

            ObservableCollection<KeyValuePair<long, string>> list1 = new ObservableCollection<KeyValuePair<long, string>>();
            foreach(VendorDTO v in vendorList)
            {
                list1.Add(new KeyValuePair<long, string>(v.VendorId, v.VendorName));
            }

            Vendor.ItemsSource = list1;
        }

        public void LoadFilter(WorkOrderInventoryItemDTO arg)
        {
            searchedForInventory = new ShipmentInventoryItemDTO(0,arg.InventoryId,arg.InventoryName,arg.ImageId);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (searchedForInventory.InventoryId != 0)
            {
                if (!shipmentInventoryList.Contains(searchedForInventory))
                {
                    shipmentInventoryList.Add(searchedForInventory);
                    ObservableCollection<ShipmentInventoryItemDTO> list1 = new ObservableCollection<ShipmentInventoryItemDTO>();

                    foreach (ShipmentInventoryItemDTO so in shipmentInventoryList)
                    {
                        list1.Add(so);
                    }

                    ShipmentItemsListView.ItemsSource = list1;
                }
            }
        }

        public void OnInventorySearchClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ArrangementFilterPage());
        }

        public void OnDeleteShipmentItem(object sender, EventArgs e)
        {
            long itemId = Int64.Parse((sender as Button).CommandParameter.ToString());

            ShipmentInventoryItemDTO sel = shipmentInventoryList.Where(a => a.InventoryId == itemId).FirstOrDefault();

            if (sel.InventoryId != 0)
            {
                shipmentInventoryList.Remove(sel);

                ObservableCollection<ShipmentInventoryItemDTO> list1 = new ObservableCollection<ShipmentInventoryItemDTO>();

                foreach (ShipmentInventoryItemDTO wo in shipmentInventoryList)
                {
                    list1.Add(wo);
                }

                ShipmentItemsListView.ItemsSource = list1;
            }
        }
    }
}