using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class Address
    {
        public Address()
        {
            PersonAddressMap = new HashSet<PersonAddressMap>();
        }

        public long AddressId { get; set; }
        public string StreetAddress { get; set; }
        public string UnitAptSuite { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public long? AddressTypeId { get; set; }
        public string City { get; set; }

        public virtual ICollection<PersonAddressMap> PersonAddressMap { get; set; }

        public virtual ICollection<VendorAddressMap> VendorAddressMap { get; set; }
    }
}
