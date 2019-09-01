using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public partial class InventoryMaterialMap
    {
        public long InventoryMaterialMapId { get; set; }
        public long InventoryId { get; set; }
        public long MaterialId { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Material Material { get; set; }
    }
}
