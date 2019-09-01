using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class PersonAddressMap
    {
        public long PersonAddressMapId { get; set; }
        public long PersonId { get; set; }
        public long AddresId { get; set; }

        public virtual Address Addres { get; set; }
        public virtual Person Person { get; set; }
    }
}
