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
	public partial class ArrangementPage : ContentPage
	{
        List<ArrangementInventoryDTO> arrangementInventoryList = new List<ArrangementInventoryDTO>();
        List<GetSimpleArrangementResponse> arrangementList = new List<GetSimpleArrangementResponse>();

        ObservableCollection<ArrangementInventoryDTO> arrangementInventoryListOC = new ObservableCollection<ArrangementInventoryDTO>();
        ObservableCollection<GetSimpleArrangementResponse> arrangementListOC = new ObservableCollection<GetSimpleArrangementResponse>();

        TabbedArrangementPage TabParent = null;

        List<long> inventoryImageIdsLoaded = new List<long>();

        public ArrangementPage (TabbedArrangementPage tabParent)
		{
			InitializeComponent ();

            arrangementList = ((App)App.Current).GetArrangements("jvw");

            foreach(GetSimpleArrangementResponse ar in arrangementList)
            {
                arrangementListOC.Add(ar);
            }

            ArrangementListView.ItemsSource = arrangementListOC;

            TabParent = tabParent;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ArrangementInventoryDTO searchedForInventory = ((App)App.Current).searchedForArrangementInventory;

            if (searchedForInventory != null && searchedForInventory.InventoryId != 0)
            {
                if (!arrangementInventoryList.Contains(searchedForInventory))
                {
                    searchedForInventory.Quantity = 1;

                    arrangementInventoryList.Add(searchedForInventory);

                    foreach (ArrangementInventoryDTO a in arrangementInventoryList)
                    {
                        arrangementInventoryListOC.Add(a);
                    }

                    ArrangementItemsListView.ItemsSource = arrangementInventoryListOC;

                    SetWorkOrderSalesData();

                    ((App)App.Current).searchedForArrangementInventory = null;
                }
            }
        }

        private void SetWorkOrderSalesData()
        {
            //GetWorkOrderSalesDetailResponse response = GetWorkOrderDetail();

            //SubTotal.Text = response.SubTotal.ToString("C", CultureInfo.CurrentCulture);
            //Tax.Text = response.Tax.ToString("C", CultureInfo.CurrentCulture);
            //Total.Text = response.Total.ToString("C", CultureInfo.CurrentCulture);
        }

        public void OnInventorySearchClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ArrangementFilterPage(this));
        }

        public void OnSearchArrangementsClicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Name.Text))
            {
                arrangementList.Clear();

                arrangementList = ((App)App.Current).GetArrangements(Name.Text);

                foreach (GetSimpleArrangementResponse ar in arrangementList)
                {
                    arrangementListOC.Add(ar);
                }

                ArrangementListView.ItemsSource = arrangementListOC;
            }
            else
            {
                DisplayAlert("Error", "To search arrangements, enter an arrangement name", "Ok");
            }
        }

        public void OnClearArrangementsClicked(object sender, EventArgs e)
        {
            ClearArrangements();
        }

        public void ClearArrangements()
        {
            Name.Text = String.Empty;
            arrangementInventoryList.Clear();
            arrangementInventoryListOC.Clear();
            ArrangementItemsListView.ItemsSource = arrangementInventoryListOC;
            inventoryImageIdsLoaded.Clear();
        }
        public void OnSaveArrangementsClicked(object sender, EventArgs e)
        {
            long arrangementId = 0;  //(long)((Button)sender).CommandParameter;
           
            if(!String.IsNullOrEmpty(Name.Text) && arrangementInventoryList.Count > 0)
            {
                foreach(ArrangementInventoryDTO dto in arrangementInventoryList)
                {
                    if(dto.ArrangementId != 0)
                    {
                        arrangementId = dto.ArrangementId;
                        break;
                    }
                }

                if (arrangementId == 0)
                {
                    AddArrangementRequest request = new AddArrangementRequest();
                    request.Arrangement = new ArrangementDTO();
                    request.Arrangement.ArrangementName = Name.Text;
                    request.Arrangement.UpdateDate = DateTime.Now;
                    request.InventoryIds = arrangementInventoryList.Select(a => a.InventoryId).ToList();

                    request.Inventory = new InventoryDTO()
                    {
                        InventoryName = Name.Text,
                        InventoryTypeId = 5,
                    };

                    arrangementId = ((App)App.Current).AddArrangement(request);

                    if(arrangementId > 0)
                    {
                        List<EOImgData> imageData = ((App)App.Current).GetImageData();

                        if (imageData.Count > 0)
                        {
                            foreach (EOImgData img in imageData)
                            {
                                AddArrangementImageRequest imgRequest = new AddArrangementImageRequest()
                                {
                                    ArrangementId = arrangementId,
                                    Image = img.imgData
                                };

                                ((App)App.Current).AddArrangementImage(imgRequest);
                            }
                        }
                    }
                }
                else
                {
                    GetSimpleArrangementResponse simpleArrangement = arrangementList.Where(a => a.Arrangement.ArrangementId == arrangementId).FirstOrDefault();

                    UpdateArrangementRequest request = new UpdateArrangementRequest();
                    request.Arrangement = new ArrangementDTO();
                    request.Arrangement.ArrangementId = arrangementId;
                    request.Arrangement.ArrangementName = Name.Text;
                    request.Arrangement.UpdateDate = DateTime.Now;

                    request.Inventory = simpleArrangement.Inventory;

                    request.ArrangementItems = arrangementInventoryList;

                    arrangementId = ((App)App.Current).UpdateArrangement(request);

                    if(arrangementId == request.Arrangement.ArrangementId)
                    {
                        //only saves new images
                        List<EOImgData> imageData = ((App)App.Current).GetImageData();

                        if (imageData.Count > 0)
                        {
                            foreach (EOImgData img in imageData)
                            {
                                AddArrangementImageRequest imgRequest = new AddArrangementImageRequest()
                                {
                                    ArrangementId = arrangementId,
                                    Image = img.imgData
                                };

                                ((App)App.Current).AddArrangementImage(imgRequest);
                            }
                        }
                    }
                }

                ClearArrangements();
            }
            else
            {
                DisplayAlert("Error", "Please enter an arrangement name and add at least one inventory item.", "OK");
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

        public void OnAddImageClicked(object sender, EventArgs e)
        {
            StartCamera();
        }

        public void OnDeleteArrangementItem(object sender, EventArgs e)
        {
            //Command parameter is InventoryId
            if (((Button)sender).CommandParameter != null)
            {
                long deleteItemId = (long)((Button)sender).CommandParameter;
                ArrangementInventoryDTO dto = arrangementInventoryList.Where(a => a.InventoryId == deleteItemId).FirstOrDefault();
                arrangementInventoryList.Remove(dto);
                arrangementInventoryListOC.Remove(dto);
                ArrangementItemsListView.ItemsSource = arrangementInventoryListOC;

                //clear images
            }
        }

        private void Quantity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string strQty = ((Xamarin.Forms.Editor)sender).Text;

            if (!String.IsNullOrEmpty(strQty))
            {
                int qty = Convert.ToInt32(strQty);
            }
        }

        public void OnDeleteArrangement(object sender, EventArgs e)
        {
            if (((Button)sender).CommandParameter != null)
            {
                long deleteArrangementId = (long)((Button)sender).CommandParameter;
                ((App)App.Current).DeleteArrangement(deleteArrangementId);

                //clear image tab sourceList
            }
        }

        private void ArrangementListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView lv = sender as ListView;

            if (lv.SelectedItem == null)
                return;

            GetSimpleArrangementResponse item = lv.SelectedItem as GetSimpleArrangementResponse;

            //call GetArrangementsById() and populate form
            GetArrangementResponse response = ((App)App.Current).GetArrangement(item.Arrangement.ArrangementId);

            ClearArrangements();

            Name.Text = response.Arrangement.ArrangementName;

            arrangementInventoryList = response.ArrangementList;

            ObservableCollection<ArrangementInventoryDTO> list1 = new ObservableCollection<ArrangementInventoryDTO>();

            foreach (ArrangementInventoryDTO a in arrangementInventoryList)
            {
                arrangementInventoryListOC.Add(a);
            }

            ArrangementItemsListView.ItemsSource = arrangementInventoryListOC;

            TabParent.LoadArrangmentImages(response.Images);

            lv.SelectedItem = null;
        }

        private void ShowImage_Clicked(object sender, EventArgs e)
        {
            if (((Button)sender).CommandParameter != null)
            {
                long inventoryId = (long)((Button)sender).CommandParameter;
                ArrangementInventoryDTO inventory = arrangementInventoryList.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                 if (inventory.ImageId != 0)
                {
                    if (!inventoryImageIdsLoaded.Contains(inventory.ImageId))
                    {
                        EOImgData imageData = ((App)App.Current).GetImage(inventory.ImageId);

                        if (imageData.imgData != null && imageData.imgData.Length > 0)
                        {
                            inventoryImageIdsLoaded.Add(inventory.ImageId);

                            TabParent.AddInventoryImage(imageData);
                        }
                    }
                }
            }
        }
    }
}