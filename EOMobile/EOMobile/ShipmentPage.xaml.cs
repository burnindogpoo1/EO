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
        TabbedShipmentPage TabParent = null;

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

        public ShipmentPage (TabbedShipmentPage tabParent)
		{
			Initialize(tabParent);

            
        }

        public ShipmentPage(TabbedShipmentPage tabParent, long shipmentId)
        {
            Initialize(tabParent);

            ShipmentInventoryDTO shipment = ((App)(App.Current)).GetShipment(shipmentId);

            shipmentInventoryList.Clear();

            foreach(ShipmentInventoryMapDTO map in shipment.ShipmentInventoryMap)
            {
                shipmentInventoryList.Add(new ShipmentInventoryItemDTO()
                {
                    InventoryId = map.InventoryId,
                    InventoryName = map.InventoryName,
                    Quantity = map.Quantity,
                    ShipmentId = map.ShipmentId,
                    //Size = map.Size
                });
            }

            Vendor.SelectedItem = shipment.Shipment.VendorId;
            Receiver.Text = shipment.Shipment.Receiver;

            ObservableCollection<ShipmentInventoryItemDTO> list1 = new ObservableCollection<ShipmentInventoryItemDTO>();

            foreach (ShipmentInventoryItemDTO wo in shipmentInventoryList)
            {
                list1.Add(wo);
            }

            ShipmentItemsListView.ItemsSource = list1;
        }

        private void Initialize(TabbedShipmentPage tabParent)
        {
            InitializeComponent();

            TabParent = tabParent;

            Receiver.Text = User;

            vendorList = ((App)(App.Current)).GetVendors(new GetPersonRequest());

            ObservableCollection<KeyValuePair<long, string>> list1 = new ObservableCollection<KeyValuePair<long, string>>();
            foreach (VendorDTO v in vendorList)
            {
                list1.Add(new KeyValuePair<long, string>(v.VendorId, v.VendorName));
            }

            Vendor.ItemsSource = list1;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ShipmentInventoryItemDTO searchedForShipmentInventory = ((App)App.Current).searchedForShipmentInventory;

            if (searchedForShipmentInventory != null && searchedForShipmentInventory.InventoryId != 0)
            {
                if (!shipmentInventoryList.Contains(searchedForShipmentInventory))
                {
                    shipmentInventoryList.Add(searchedForShipmentInventory);
                    ObservableCollection<ShipmentInventoryItemDTO> list1 = new ObservableCollection<ShipmentInventoryItemDTO>();

                    foreach (ShipmentInventoryItemDTO so in shipmentInventoryList)
                    {
                        list1.Add(so);
                    }

                    ShipmentItemsListView.ItemsSource = list1;

                    ((App)App.Current).searchedForShipmentInventory = null;
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        public void OnInventorySearchClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ArrangementFilterPage(this));
        }

        public void OnShipmentSaveClicked(object sender, EventArgs e)
        {
            if (shipmentInventoryList.Count > 0)
            {
                AddShipment();
            }
        }

        public void AddShipment()
        {
            AddShipmentRequest addShipmentRequest = new AddShipmentRequest();

            ShipmentDTO dto = new ShipmentDTO()
            {
                VendorId = ((KeyValuePair<long, string>)this.Vendor.SelectedItem).Key,
                VendorName = ((KeyValuePair<long, string>)this.Vendor.SelectedItem).Value, 
                Receiver = this.Receiver.Text,
                ShipmentDate = DateTime.Now,
                //Comments = this.Comments.Text
            };

            List<ShipmentInventoryMapDTO> shipmentInventoryMap = new List<ShipmentInventoryMapDTO>();

            foreach (ShipmentInventoryItemDTO woii in shipmentInventoryList)
            {
                shipmentInventoryMap.Add(new ShipmentInventoryMapDTO()
                {
                    InventoryId = woii.InventoryId,
                    InventoryName = woii.InventoryName,
                    Quantity = woii.Quantity
                });
            }

            addShipmentRequest.ShipmentDTO = dto;
            addShipmentRequest.ShipmentInventoryMap = shipmentInventoryMap;

            ((App)App.Current).AddShipment(addShipmentRequest);

            this.Vendor.SelectedIndex = -1;
            this.shipmentInventoryList.Clear();
            this.ShipmentItemsListView.ItemsSource = null;
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