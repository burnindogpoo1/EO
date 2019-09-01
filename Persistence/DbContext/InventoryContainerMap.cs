using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class InventoryContainerMap
    {
        public long InventoryContainerMapId { get; set; }
        public long InventoryId { get; set; }
        public long ContainerId { get; set; }

        public virtual Container Container { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}
