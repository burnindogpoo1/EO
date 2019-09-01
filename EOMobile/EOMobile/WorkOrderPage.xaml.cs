using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WorkOrderPage : ContentPage
	{
        private List<InventoryTypeDTO> inventoryTypeList = new List<InventoryTypeDTO>();
        List<WorkOrderInventoryItemDTO> workOrderInventoryList = new List<WorkOrderInventoryItemDTO>();
        WorkOrderInventoryItemDTO searchedForInventory = new WorkOrderInventoryItemDTO();  
 		public WorkOrderPage ()
		{
			InitializeComponent ();

            MessagingCenter.Subscribe<ArrangementFilterPage, WorkOrderInventoryItemDTO>(this, "UseFilter", async (sender, arg) =>
            {
                LoadFilter(arg);
            });

            //inventoryTypeList = ((App)App.Current).GetInventoryTypes();

            //ObservableCollection<KeyValuePair<long, string>> list1 = new ObservableCollection<KeyValuePair<long, string>>();
            //foreach (InventoryTypeDTO type in inventoryTypeList)
            //{
            //    list1.Add(new KeyValuePair<long, string>(type.InventoryTypeId, type.InventoryTypeName));
            //}

            //Inventory.ItemsSource = list1;

            //PlantType.SelectedIndexChanged += PlantType_SelectedIndexChanged;

            //PlantName.SelectedIndexChanged += PlantName_SelectedIndexChanged;

            //PlantSize.SelectedIndexChanged += PlantSize_SelectedIndexChanged;

            Seller.Text = ((App)App.Current).User;
        }

        public void LoadFilter(WorkOrderInventoryItemDTO arg)
        {
            searchedForInventory = arg;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(searchedForInventory.InventoryId != 0)
            {
                if (!workOrderInventoryList.Contains(searchedForInventory))
                {
                    searchedForInventory.Quantity = 1;

                    workOrderInventoryList.Add(searchedForInventory);
                    ObservableCollection<WorkOrderInventoryItemDTO> list1 = new ObservableCollection<WorkOrderInventoryItemDTO>();
                    
                    foreach(WorkOrderInventoryItemDTO wo in workOrderInventoryList)
                    {
                        list1.Add(wo);
                    }

                    InventoryItemsListView.ItemsSource = list1;

                    SetWorkOrderSalesData();
                }
            }
        }

        public void OnDeleteWorkOrderItem(object sender, EventArgs e)
        {
            long itemId = Int64.Parse((sender as Button).CommandParameter.ToString());

            WorkOrderInventoryItemDTO sel = workOrderInventoryList.Where(a => a.InventoryId == itemId).FirstOrDefault();

            if (sel.InventoryId != 0)
            {
                workOrderInventoryList.Remove(sel);

                ObservableCollection<WorkOrderInventoryItemDTO> list1 = new ObservableCollection<WorkOrderInventoryItemDTO>();

                foreach (WorkOrderInventoryItemDTO wo in workOrderInventoryList)
                {
                    list1.Add(wo);
                }

                InventoryItemsListView.ItemsSource = list1;

                SetWorkOrderSalesData();
            }
        }

        public void OnInventorySearchClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ArrangementFilterPage());
        }

        public void OnCustomerSearchClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PersonFilterPage());
        }

        public void OnSaveWorkOrder(object sender, EventArgs e)
        {
            if(workOrderInventoryList.Count > 0)
            {
                AddWorkOrder();
            }
        }

        public void AddWorkOrder()
        {
            AddWorkOrderRequest addWorkOrderRequest = new AddWorkOrderRequest();

            WorkOrderDTO dto = new WorkOrderDTO()
            {
                Seller = this.Seller.Text,
                Buyer = this.Buyer.Text,
                CreateDate = DateTime.Now,
                Comments = this.Comments.Text
            };

            List<WorkOrderInventoryMapDTO> workOrderInventoryMap = new List<WorkOrderInventoryMapDTO>();

            foreach (WorkOrderInventoryItemDTO woii in workOrderInventoryList)
            {
                workOrderInventoryMap.Add(new WorkOrderInventoryMapDTO()
                {
                    InventoryId = woii.InventoryId,
                    InventoryName = woii.InventoryName,
                    Quantity = woii.Quantity
                });
            }

            addWorkOrderRequest.WorkOrder = dto;
            addWorkOrderRequest.WorkOrderInventoryMap = workOrderInventoryMap;

            ((App)App.Current).AddWorkOrder(addWorkOrderRequest);

            this.workOrderInventoryList.Clear();
            this.InventoryItemsListView.ItemsSource = null;
        }

        private void SetWorkOrderSalesData()
        {
            GetWorkOrderSalesDetailResponse response = GetWorkOrderDetail();

            SubTotal.Text = response.SubTotal.ToString("C", CultureInfo.CurrentCulture);
            Tax.Text = response.Tax.ToString("C", CultureInfo.CurrentCulture);
            Total.Text = response.Total.ToString("C", CultureInfo.CurrentCulture);
        }

        private void Quantity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetWorkOrderSalesData();
        }

        public GetWorkOrderSalesDetailResponse GetWorkOrderDetail()
        {
            GetWorkOrderSalesDetailResponse response = new GetWorkOrderSalesDetailResponse();

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://192.168.1.3:9000/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("EO-Header", ((App)App.Current).User + " : " + ((App)App.Current).Pwd);

                string jsonData = JsonConvert.SerializeObject(new GetWorkOrderSalesDetailRequest(workOrderInventoryList));
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = client.PostAsync("api/Login/GetWorkOrderDetail", content).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    Stream streamData = httpResponse.Content.ReadAsStreamAsync().Result;
                    StreamReader strReader = new StreamReader(streamData);
                    string strData = strReader.ReadToEnd();
                    strReader.Close();
                    response = JsonConvert.DeserializeObject<GetWorkOrderSalesDetailResponse>(strData);
                }
                else
                {
                    //MessageBox.Show("There was an error retreiving work order sales detail");
                }
            }
            catch (Exception ex)
            {

            }

            return response;
        }
    }
}