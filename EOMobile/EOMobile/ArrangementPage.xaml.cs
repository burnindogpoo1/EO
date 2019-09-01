using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ArrangementPage : ContentPage
	{
        List<WorkOrderInventoryItemDTO> arrangementInventoryList = new List<WorkOrderInventoryItemDTO>();
        List<ArrangementInventoryDTO> arrangementList = new List<ArrangementInventoryDTO>();
        WorkOrderInventoryItemDTO searchedForInventory = new WorkOrderInventoryItemDTO();

        public ArrangementPage ()
		{
			InitializeComponent ();

            arrangementList = ((App)App.Current).GetArrangements();

            ObservableCollection<ArrangementInventoryDTO> list1 = new ObservableCollection<ArrangementInventoryDTO>();

            foreach(ArrangementInventoryDTO ai in arrangementList)
            {
                list1.Add(ai);
            }

            ArrangementListView.ItemsSource = list1;

            MessagingCenter.Subscribe<ArrangementFilterPage, WorkOrderInventoryItemDTO>(this, "UseFilter", async (sender, arg) =>
            {
                LoadFilter(arg);
            });
        }

        public void LoadFilter(WorkOrderInventoryItemDTO arg)
        {
            searchedForInventory = arg;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (searchedForInventory.InventoryId != 0)
            {
                if (!arrangementInventoryList.Contains(searchedForInventory))
                {
                    searchedForInventory.Quantity = 1;

                    arrangementInventoryList.Add(searchedForInventory);
                    ObservableCollection<WorkOrderInventoryItemDTO> list1 = new ObservableCollection<WorkOrderInventoryItemDTO>();

                    foreach (WorkOrderInventoryItemDTO a in arrangementInventoryList)
                    {
                        list1.Add(a);
                    }

                    ArrangementItemsListView.ItemsSource = list1;

                    SetWorkOrderSalesData();
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
            Navigation.PushModalAsync(new ArrangementFilterPage());
        }

        public void OnAddImageClicked(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new PersonFilterPage());
        }

        public void OnDeleteArrangementItem(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new PersonFilterPage());
        }

        private void Quantity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //SetWorkOrderSalesData();
        }

        public void OnDeleteArrangement(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new PersonFilterPage());
        }
    }
}