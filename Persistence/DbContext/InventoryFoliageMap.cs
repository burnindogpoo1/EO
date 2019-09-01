using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public partial class InventoryFoliageMap
    {
        public long InventoryFoliageMapId { get; set; }
        public long InventoryId { get; set; }
        public long FoliageId { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Foliage Foliage { get; set; }
    }
}
