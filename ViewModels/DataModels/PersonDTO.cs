
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
    public class PersonDTO
    {
        public string CustomerName
        {
            get { return first_name + " " + last_name; }
        } 
        
        public long person_id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string phone_primary { get; set; } 

        public string phone_alt { get; set; }

        public DateTime last_updated { get; set; }

        public string email { get; set; }

        public long user_id { get; set; }

        public long address_id { get; set; }

        public string street_address { get; set; }

        public string unit_apt_suite { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zipcode { get; set; }

        public long address_type_id { get; set; }
        
        public long person_address_map_Id { get; set; }
    }
}
