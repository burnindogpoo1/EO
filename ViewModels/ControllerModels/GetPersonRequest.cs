
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class GetPersonRequest
    {
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhonePrimary { get; set; }
        public string PhoneAlt { get; set; }
        public string Email { get; set; }

        public bool IsEmpty()
        {
            bool isEmpty = true;

            if(PersonId != 0 || !String.IsNullOrEmpty(FirstName) || !String.IsNullOrEmpty(LastName) ||
                !String.IsNullOrEmpty(PhonePrimary) || !String.IsNullOrEmpty(PhoneAlt) || !String.IsNullOrEmpty(Email))
            {
                isEmpty = false;
            }

            return isEmpty;
        }
    }
}
