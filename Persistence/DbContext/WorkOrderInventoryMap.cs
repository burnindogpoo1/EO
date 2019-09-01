using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class WorkOrderInventoryMap
    {
        public long WorkOrderInventoryMapId { get; set; }
        public long WorkOrderId { get; set; }
        public long InventoryId { get; set; }
        public int Quantity { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
    }
}
