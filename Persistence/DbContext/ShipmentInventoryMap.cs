using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public partial class ShipmentInventoryMap
    {
        public long ShipmentInventoryMapId { get; set; }

        public long ShipmentId { get; set; }

        public long InventoryId { get; set; }

        public int Quantity { get; set; }

        public virtual Shipment Shipment { get; set; }

        public virtual Inventory Inventory { get; set; }
    }
}
