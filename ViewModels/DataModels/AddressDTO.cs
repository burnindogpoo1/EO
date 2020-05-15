
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
    public class AddressDTO
    {
        public long address_id { get; set; }

        public string street_address { get; set; }

        public string unit_apt_suite { get; set; }

        public string state { get; set; }

        public string zipcode { get; set; }

        public long address_type_id { get; set; }

        public string city { get; set; }
    }
}
