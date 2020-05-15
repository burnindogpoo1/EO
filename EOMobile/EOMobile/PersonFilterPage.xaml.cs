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
	public partial class PersonFilterPage : ContentPage
	{
        PersonAndAddressDTO person { get; set;}

        List<PersonAndAddressDTO> Persons = new List<PersonAndAddressDTO>();

        ContentPage Initiator;

		public PersonFilterPage (ContentPage initiator)
		{
            Initiator = initiator;

			InitializeComponent ();
        }

        public void OnSearchPersonClicked(object sender, EventArgs e)
        {
            GetPersonRequest request = new GetPersonRequest();

            request.FirstName = Name.Text;
            request.Email = Email.Text;
            request.PhonePrimary = Phone.Text;

            Persons = ((App)App.Current).GetCustomers(request);

            ObservableCollection<PersonAndAddressDTO> list1 = new ObservableCollection<PersonAndAddressDTO>();

            foreach(PersonAndAddressDTO p in Persons)
            {
                list1.Add(p);
            }

            PersonListView.ItemsSource = list1;
        }

        private void PersonListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var myListView = (ListView)sender;
            PersonAndAddressDTO p = new PersonAndAddressDTO();

            person = myListView.SelectedItem as PersonAndAddressDTO;

            p.Person.address_id = person.Person.address_id;
            p.Person.first_name = person.Person.first_name;
            p.Person.last_name = person.Person.last_name;
            p.Person.person_id = person.Person.person_id;
            p.Person.email = person.Person.email;
            p.Person.phone_primary = person.Person.phone_primary;

            if (p.Address != null && p.Address.address_id > 0)
            {
                p.Address.address_id = person.Address.address_id;
                p.Address.city = person.Address.city;
                p.Address.state = person.Address.state;
                p.Address.street_address = person.Address.street_address;
                p.Address.zipcode = person.Address.zipcode;
            }

            try
            {
                 MessagingCenter.Send<PersonAndAddressDTO>(p, "SearchCustomer");
            }
            catch (Exception ex)
            {
                int debug = 1;
            }

            Navigation.PopModalAsync();
        }
    }
}