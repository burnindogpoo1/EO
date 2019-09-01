using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class Vendor
    {
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorPhone { get; set; }
        public string VendorEmail { get; set; }
        public virtual ICollection<VendorAddressMap> VendorAddressMap { get; set; }
    }
}
