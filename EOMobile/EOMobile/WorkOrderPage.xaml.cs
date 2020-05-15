using EOMobile.Interfaces;
using Newtonsoft.Json;
using Stripe;
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
        ObservableCollection<KeyValuePair<long, string>> deliveryTypeList = new ObservableCollection<KeyValuePair<long, string>>();
        ObservableCollection<KeyValuePair<long, string>> payTypeList = new ObservableCollection<KeyValuePair<long, string>>();
        ObservableCollection<KeyValuePair<long, string>> serviceTypeList = new ObservableCollection<KeyValuePair<long, string>>();

        TabbedWorkOrderPage TabParent = null;

        long currentWorkOrderId = 0;
        long currentWorkOrderPaymentId = 0;
        PersonAndAddressDTO searchedForPerson = null;

        public bool EnablePayment()
        {
            bool result = false;
            if(currentWorkOrderId == 0 || (currentWorkOrderId != 0 && currentWorkOrderPaymentId == 0))
            {
                result = true;
            }
            return result;
        }

        public bool EnableSave()
        {
            bool result = false;

            //if it hasn't been paid for, allow an update - check for duplication and only update with "newly added" items
            if (currentWorkOrderId == 0 || currentWorkOrderPaymentId == 0)
            {
                result = true;
            }

            return result;
        }

        private void Initialize(TabbedWorkOrderPage tabParent)
        {
            InitializeComponent();

            TabParent = tabParent;

            MessagingCenter.Subscribe<PaymentPage, string>(this, "MakePayment", async (sender, arg) =>
            {
                PaymentMade(arg);
            });

            Seller.Text = ((App)App.Current).User;

            
            deliveryTypeList.Add(new KeyValuePair<long, string>(1, "Pickup"));
            deliveryTypeList.Add(new KeyValuePair<long, string>(2, "Delivery"));

            DeliveryType.ItemsSource = deliveryTypeList;
            DeliveryType.SelectedIndex = 0;

            
            serviceTypeList.Add(new KeyValuePair<long, string>(1, "No"));
            serviceTypeList.Add(new KeyValuePair<long, string>(2, "Yes"));

            ServiceType.ItemsSource = serviceTypeList;
            ServiceType.SelectedIndex = 0;

            payTypeList.Add(new KeyValuePair<long, string>(1, "Cash"));
            payTypeList.Add(new KeyValuePair<long, string>(2, "Check"));
            payTypeList.Add(new KeyValuePair<long, string>(3, "Credit Card"));

            PaymentType.ItemsSource = payTypeList;

            Save.IsEnabled = true;

            //both buttons are disabled until the work order data is saved
            Payment.IsEnabled = false;
            PaymentType.IsEnabled = false;
        }

        public WorkOrderPage (TabbedWorkOrderPage tabParent)
		{
            Initialize(tabParent);
        }

        public WorkOrderPage(TabbedWorkOrderPage tabParent, long workOrderId) 
        {
            Initialize(tabParent);

            //load work order data for the id passed
            WorkOrderInventoryDTO workOrder = ((App)App.Current).GetWorkOrder(workOrderId);
            currentWorkOrderId = workOrder.WorkOrder.WorkOrderId;

            WorkOrderPaymentDTO workOrderPayment = ((App)App.Current).GetWorkOrderPayment(workOrderId);
            currentWorkOrderPaymentId = workOrderPayment.WorkOrderPaymentId;

            if (currentWorkOrderPaymentId == 0)
            {
                Save.IsEnabled = false;  //should this be a version 2 feature? - turn off for now
                Payment.IsEnabled = true;
                PaymentType.IsEnabled = true;
            }
            
            //BuyerId should be saved and restored
            Buyer.Text = workOrder.WorkOrder.Buyer;

            foreach(var x in workOrder.InventoryList)
            {
                workOrderInventoryList.Add(new WorkOrderInventoryItemDTO()
                {
                    WorkOrderId = x.WorkOrderId,
                    InventoryId = x.InventoryId,
                    InventoryName = x.InventoryName,
                    Quantity = x.Quantity,
                    Size = x.Size
                });
            }

            ObservableCollection<WorkOrderInventoryItemDTO> list1 = new ObservableCollection<WorkOrderInventoryItemDTO>(workOrderInventoryList);

            InventoryItemsListView.ItemsSource = list1;

            //load all inventory item images
        }

        private void TakePictureClicked(object sender, EventArgs e)
        {
            StartCamera();
        }

        public async void StartCamera()
        {
            var action = await DisplayActionSheet("Add Photo", "Cancel", null, "Choose Existing", "Take Photo");

            if (action == "Choose Existing")
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var fileName = SetImageFileName();
                    DependencyService.Get<ICameraInterface>().LaunchGallery(FileFormatEnum.JPEG, fileName);
                });
            }
            else if (action == "Take Photo")
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var fileName = SetImageFileName();
                    DependencyService.Get<ICameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
                });
            }
        }

        private string SetImageFileName(string fileName = null)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                if (fileName != null)
                    App.ImageIdToSave = fileName;
                else
                    App.ImageIdToSave = App.EOImageId;

                return App.ImageIdToSave;
            }
            else
            {
                //To iterate, on iOS, if you want to save images to the devie, set 
                if (fileName != null)
                {
                    App.ImageIdToSave = fileName;
                    return fileName;
                }
                else
                    return null;
            }
        }

        protected override void OnDisappearing()
        {
            ((App)App.Current).ClearImageData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            WorkOrderInventoryItemDTO searchedForInventory = ((App)App.Current).searchedForInventory;

            if(searchedForInventory != null && searchedForInventory.InventoryId != 0)
            {
                if (!workOrderInventoryList.Contains(searchedForInventory))
                {
                    searchedForInventory.Quantity = 1;

                    if (!workOrderInventoryList.Contains(searchedForInventory))
                    {
                        workOrderInventoryList.Add(searchedForInventory);
                        ObservableCollection<WorkOrderInventoryItemDTO> list1 = new ObservableCollection<WorkOrderInventoryItemDTO>();

                        foreach (WorkOrderInventoryItemDTO wo in workOrderInventoryList)
                        {
                            list1.Add(wo);
                        }

                        InventoryItemsListView.ItemsSource = list1;

                        SetWorkOrderSalesData();

                        if (searchedForInventory.ImageId != 0)
                        {
                            EOImgData imageData = ((App)App.Current).GetImage(searchedForInventory.ImageId);

                            if (imageData.imgData != null && imageData.imgData.Length > 0)
                            {
                                TabParent.AddInventoryImage(imageData);
                            }
                        }

                        ((App)App.Current).searchedForInventory = null;
                    }
                }
            }

            searchedForPerson = ((App)App.Current).searchedForPerson;

            if(searchedForPerson != null && searchedForPerson.Person.person_id != 0)
            {
                Buyer.Text = searchedForPerson.Person.CustomerName;

                ((App)App.Current).searchedForPerson = null;
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
            Navigation.PushModalAsync(new ArrangementFilterPage(this));
        }

        public void OnCustomerSearchClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PersonFilterPage(this));
        }

        private string ValidateSaveWorkOrder()
        {
            string errorMessage = String.Empty;

            if(workOrderInventoryList.Count == 0)
            {
                errorMessage += "Please at least one inventory item \n";
            }

            if (String.IsNullOrEmpty(Seller.Text))
            {
                errorMessage += "Please add the seller's name \n";
            }

            if (String.IsNullOrEmpty(Buyer.Text))
            {
                errorMessage += "Please add the buyer's name \n";
            }

            return errorMessage;
        }

        public void OnSaveWorkOrder(object sender, EventArgs e)
        {
            string errorMessage = ValidateSaveWorkOrder();

            if(String.IsNullOrEmpty(errorMessage))
            {
                AddWorkOrder();
            }
            else
            {
                DisplayAlert("Can't save work order", errorMessage, "Ok");
            }
        }

        public void OnClear(object sender, EventArgs e)
        {
            currentWorkOrderId = 0;
            currentWorkOrderPaymentId = 0;
            searchedForPerson = null;

            Buyer.Text = String.Empty;
            DeliveryDate.Date = DateTime.Now;

            DeliveryType.SelectedIndex = 0;
            ServiceType.SelectedIndex = 0;
            PaymentType.SelectedIndex = -1;

            this.workOrderInventoryList.Clear();
            this.InventoryItemsListView.ItemsSource = null;
        }

        public void AddWorkOrder()
        {
            AddWorkOrderRequest addWorkOrderRequest = new AddWorkOrderRequest();

            long customerId = 0;
            if(searchedForPerson != null)
            {
                customerId = searchedForPerson.Person.person_id;
            }

            WorkOrderDTO dto = new WorkOrderDTO()
            {
                Seller = this.Seller.Text,
                Buyer = this.Buyer.Text,
                CreateDate = DateTime.Now,
                Comments = this.Comments.Text,
                IsSiteService = false,
                CustomerId = customerId
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

            currentWorkOrderId = ((App)App.Current).AddWorkOrder(addWorkOrderRequest);

            if(currentWorkOrderId > 0)
            {
                //add any images
                List<EOImgData> imageData = ((App)App.Current).GetImageData();

                if(imageData.Count > 0)
                {
                    foreach (EOImgData img in imageData)
                    {
                        AddWorkOrderImageRequest request = new AddWorkOrderImageRequest()
                        {
                            WorkOrderId = currentWorkOrderId,
                            Image = img.imgData
                        };

                        ((App)App.Current).AddWorkOrderImage(request);
                    }
                }

                ((App)App.Current).ClearImageData();

                Save.IsEnabled = false;
                PaymentType.IsEnabled = true;
                Payment.IsEnabled = true;
            }
            else
            {
                DisplayAlert("Error", "There was an error saving this work order.", "Ok");
            }
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
                client.BaseAddress = new Uri(((App)App.Current).LAN_Address);
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
                    //strReader.Close();
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

        private void Payment_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Total.Text))
            {
                decimal salePrice = 0.0M;
                
                bool converted = decimal.TryParse(Total.Text, NumberStyles.Currency,CultureInfo.CurrentCulture.NumberFormat, out salePrice);
                
                if (salePrice > 0)
                {
                    if (PaymentType.SelectedIndex == 2) //cc
                    {
                        Navigation.PushModalAsync(new PaymentPage(salePrice, Buyer.Text));
                    }
                    else
                    {
                        string ccConfirm = String.Empty;
                        SavePaymentRecord(ccConfirm);
                    }
                }
            }
        }

        //MessagingCenter Response to "MakePayment"
        public void PaymentMade(string ccConfirm)
        {
            if (!String.IsNullOrEmpty(ccConfirm))
            {
                SavePaymentRecord(ccConfirm);
            }
            else
            {
                DisplayAlert("Credit Card Error", "There was an error processing your credit card transaction.", "Ok");
            }
        }

        private void SavePaymentRecord(string ccConfirm)
        {
            WorkOrderPaymentDTO workOrderPayment = new WorkOrderPaymentDTO();

            decimal workValue = 0;
            workOrderPayment.WorkOrderId = currentWorkOrderId;
            decimal.TryParse(Total.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out workValue);
            workOrderPayment.WorkOrderPaymentAmount = workValue;
            workValue = 0;
            workOrderPayment.WorkOrderPaymentType = (int)payTypeList[PaymentType.SelectedIndex].Key;
            workOrderPayment.WorkOrderPaymentCreditCardConfirmation = ccConfirm;
            decimal.TryParse(Tax.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out workValue);
            workOrderPayment.WorkOrderPaymentTax = workValue;
            currentWorkOrderPaymentId = ((App)App.Current).AddWorkOrderPayment(workOrderPayment);

            if(currentWorkOrderPaymentId > 0)
            {
                Payment.IsEnabled = false;
                PaymentType.IsEnabled = false;

                //mark work order paid - do inventory adjustment
            }
        }
    }
}