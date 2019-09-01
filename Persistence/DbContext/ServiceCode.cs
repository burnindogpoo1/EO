using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class ServiceCode
    {
        public ServiceCode()
        {
            Arrangement = new HashSet<Arrangement>();
            Inventory = new HashSet<Inventory>();
        }

        public long ServiceCodeId { get; set; }
        public string ServiceCode1 { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public bool Taxable { get; set; }
        public string GeneralLedger { get; set; }

        public virtual ICollection<Arrangement> Arrangement { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
    }
}
