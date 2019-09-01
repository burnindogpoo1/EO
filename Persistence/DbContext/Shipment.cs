using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public partial class Shipment
    {
        public long ShipmentId { get; set; }

        public long VendorId { get; set; }

        public DateTime ShipmentDate { get; set; }

        public virtual ICollection<ShipmentInventoryMap> ShipmentInventoryMap { get; set; }
    }
}
