using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class InventoryPlantMap
    {
        public long InventoryPlantMapId { get; set; }
        public long InventoryId { get; set; }
        public long PlantId { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Plant Plant { get; set; }
    }
}
