using Android.Runtime;
using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    [Serializable]
    [Preserve(AllMembers = true)]

    public partial class ArrangementInventoryMap
    {
        public long ArrangementInventoryMapId { get; set; }
        public long ArrangementId { get; set; }
        public long InventoryId { get; set; }

        public virtual Arrangement Arrangement { get; set; }
        public virtual Inventory Inventory { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]

    public partial class ArrangementInventoryInventoryMap
    {
        public long ArrangementInventoryInventoryMapId { get; set; }
        public long ArrangementId { get; set; }
        public long InventoryId { get; set; }
        public int Quantity { get; set; }
        public virtual Arrangement Arrangement { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}
