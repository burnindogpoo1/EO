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
	public partial class VendorPage : ContentPage
	{
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

        List<VendorDTO> vendorList = new List<VendorDTO>();

        public VendorPage ()
		{
			InitializeComponent ();

            GetAllVendors();

            VendorListView.ItemSelected += VendorListView_ItemSelected;
        }

        private void VendorListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            VendorDTO item = (sender as ListView).SelectedItem as VendorDTO;

            if (item != null)
            {
                Name.Text = item.VendorName;
                Phone.Text = item.VendorPhone;
                Email.Text = item.VendorEmail;
                Address.Text = item.StreetAddress;
                Address2.Text = item.UnitAptSuite;
                City.Text = item.City;
                State.Text = item.State;
                Zip.Text = item.ZipCode;
            }
        }

        void ClearForm()
        {
            Name.Text = String.Empty;
            Phone.Text = String.Empty;
            Email.Text = String.Empty;
            Address.Text = String.Empty;
            Address2.Text = String.Empty;
            City.Text = String.Empty;
            State.Text = String.Empty;
            Zip.Text = String.Empty;
        }

        void GetAllVendors()
        {
            ClearForm();

            vendorList = ((App)App.Current).GetVendors(new GetPersonRequest());

            ObservableCollection<VendorDTO> list1 = new ObservableCollection<VendorDTO>();

            foreach(VendorDTO v in vendorList)
            {
                list1.Add(v);
            }

            VendorListView.ItemsSource = list1;
        }

        public void OnSearchPersonClicked(object sender, EventArgs e)
        {
            GetPersonRequest request = new GetPersonRequest();
            request.FirstName = Name.Text;
            request.Email = Email.Text;

            vendorList = ((App)App.Current).GetVendors(request);

            ObservableCollection<VendorDTO> list1 = new ObservableCollection<VendorDTO>();

            foreach (VendorDTO v in vendorList)
            {
                list1.Add(v);
            }

            VendorListView.ItemsSource = list1;
        }

        public void OnShowAllPersonsClicked(object sender, EventArgs e)
        {
            GetAllVendors();
        }

        public void OnSavePersonClicked(object sender, EventArgs e)
        {

        }

        public void SearchResult(GetPersonRequest request)
        {

        }
	}
}