using Android.Content;
using EOMobile.Interfaces;
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
    public partial class SiteServicePage : ContentPage
    {
        List<WorkOrderInventoryItemDTO> siteServiceInventoryList = new List<WorkOrderInventoryItemDTO>();

        TabbedSiteServicePage TabParent;
        public SiteServicePage(TabbedSiteServicePage tabParent)
        {
            InitializeComponent();
            TabParent = tabParent;
            ServicedBy.Text = ((App)App.Current).User;
        }

        public void PictureTaken(byte[] arg)
        {
            int debug = 1;
        }

        protected override void OnDisappearing()
        {
            ((App)App.Current).ClearImageData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            WorkOrderInventoryItemDTO searchedForInventory = ((App)App.Current).searchedForInventory;

            if (searchedForInventory != null && searchedForInventory.InventoryId != 0)
            {
                if (!siteServiceInventoryList.Contains(searchedForInventory))
                {
                    searchedForInventory.Quantity = 1;

                    siteServiceInventoryList.Add(searchedForInventory);
                    ObservableCollection<WorkOrderInventoryItemDTO> list1 = new ObservableCollection<WorkOrderInventoryItemDTO>();

                    foreach (WorkOrderInventoryItemDTO wo in siteServiceInventoryList)
                    {
                        list1.Add(wo);
                    }

                    SiteServiceInventoryItemsListView.ItemsSource = list1;

                    //SetWorkOrderSalesData();

                    ((App)App.Current).searchedForInventory = null;
                }
            }

            PersonAndAddressDTO searchedForCustomer = ((App)App.Current).searchedForPerson;

            if (searchedForCustomer != null && searchedForCustomer.Person.person_id != 0)
            {
                Customer.Text = searchedForCustomer.Person.CustomerName;

                ((App)App.Current).searchedForPerson = null;
            }
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

        private void TakePictureClicked(object sender, EventArgs e)
        {
            StartCamera();
        }

        private void Customers_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PersonFilterPage(this));
        }

        private void Inventory_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ArrangementFilterPage(this));
        }

        private void Quantity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //SetWorkOrderSalesData();
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            //remove the selected item from the "master" list
            long itemId = Int64.Parse((sender as Button).CommandParameter.ToString());

            WorkOrderInventoryItemDTO sel = siteServiceInventoryList.Where(a => a.InventoryId == itemId).FirstOrDefault();

            if (sel.InventoryId != 0)
            {
                siteServiceInventoryList.Remove(sel);

                ObservableCollection<WorkOrderInventoryItemDTO> list1 = new ObservableCollection<WorkOrderInventoryItemDTO>();

                foreach (WorkOrderInventoryItemDTO wo in siteServiceInventoryList)
                {
                    list1.Add(wo);
                }

                SiteServiceInventoryItemsListView.ItemsSource = list1;

                //SetWorkOrderSalesData();
            }
        }

        public void AddWorkOrder()
        {
            AddWorkOrderRequest addWorkOrderRequest = new AddWorkOrderRequest();

            WorkOrderDTO dto = new WorkOrderDTO()
            {
                Seller = this.ServicedBy.Text,
                Buyer = this.Customer.Text,
                CreateDate = DateTime.Now,
                Comments = this.Comments.Text,
                IsSiteService = true
            };

            List<WorkOrderInventoryMapDTO> workOrderInventoryMap = new List<WorkOrderInventoryMapDTO>();

            foreach (WorkOrderInventoryItemDTO woii in siteServiceInventoryList)
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

            long newWorkOrderId = ((App)App.Current).AddWorkOrder(addWorkOrderRequest);

            if (newWorkOrderId > 0)
            {
                //add any images
                List<EOImgData> imageData = ((App)App.Current).GetImageData();

                if (imageData.Count > 0)
                {
                    foreach (EOImgData img in imageData)
                    {
                        AddWorkOrderImageRequest request = new AddWorkOrderImageRequest()
                        {
                            WorkOrderId = newWorkOrderId,
                            Image = img.imgData
                        };

                        ((App)App.Current).AddWorkOrderImage(request);
                    }
                }

                ((App)App.Current).ClearImageData();
            }

            this.siteServiceInventoryList.Clear();
            this.SiteServiceInventoryItemsListView.ItemsSource = null;
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            AddWorkOrder();
        }
    }
}