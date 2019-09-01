using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class InventoryImageMap
    {
        public long InventoryImageMapId { get; set; }
        public long InventoryId { get; set; }
        public long ImageId { get; set; }

        public virtual Image Image { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}
