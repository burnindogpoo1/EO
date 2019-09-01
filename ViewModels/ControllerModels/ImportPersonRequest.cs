using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
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
