using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public class VendorAddressMap
    {
        public long VendorAddressMapId {get; set;}

        public long VendorId { get; set; }

        public long AddressId { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual Address Address { get; set; }
    }
}
