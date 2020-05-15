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
    public class VendorDTO
    {
        public long VendorId { get; set; }

        public string VendorName { get; set; }

        public string VendorPhone { get; set; }

        public string VendorEmail { get; set; }

        public string StreetAddress { get; set; }

        public String UnitAptSuite { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public long VendorAddressMapId { get; set; }
    }
}
