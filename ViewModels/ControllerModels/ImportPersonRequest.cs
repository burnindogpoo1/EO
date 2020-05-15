
using Android.Runtime;
using System;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class ImportPersonRequest
    {
        public ImportPersonRequest()
        {
            Person = new PersonDTO();

            Address = new AddressDTO();
        }

        public ImportPersonRequest(PersonDTO person, AddressDTO address)
        {
            Person = person;
            Address = address;
        }
        public PersonDTO  Person {get; set;}

        public AddressDTO Address { get; set; }
    }
   
}
