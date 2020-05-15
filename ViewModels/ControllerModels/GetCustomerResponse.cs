
using Android.Runtime;
using EO.ViewModels.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class GetCustomerResponse : ApiResponse
    {
        PersonAndAddressDTO Customer { get; set; }

        public GetCustomerResponse()
        {
            Customer = new PersonAndAddressDTO();
        }

        public GetCustomerResponse(PersonAndAddressDTO customer)
        {
            Customer = customer;
        }
    }
}
