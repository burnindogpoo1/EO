
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
    public class GetPersonResponse : ApiResponse
    {
        public List<PersonAndAddressDTO> PersonAndAddress { get; set; }

        public GetPersonResponse()
        {
            PersonAndAddress = new List<PersonAndAddressDTO>();
        }

        public GetPersonResponse(List<PersonAndAddressDTO> personAndAddress)
        {
            PersonAndAddress = personAndAddress;
        }
    }
}
