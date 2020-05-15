
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers=true)]
    public class AddCustomerRequest
    {
        public PersonAndAddressDTO Customer { get; set; }

        public AddCustomerRequest()
        {
            Customer = new PersonAndAddressDTO();
        }

        public AddCustomerRequest(PersonAndAddressDTO customer)
        {
            Customer = customer;
        }
    }
}
