
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class PersonAndAddressDTO
    {
        public PersonDTO Person { get; set; }

        public AddressDTO Address { get; set; }

        public PersonAndAddressDTO()
        {
            Person = new PersonDTO();

            Address = new AddressDTO();
        }

        public PersonAndAddressDTO(PersonDTO person, AddressDTO address)
        {
            Person = person;
            Address = address;
        }
    }
}
