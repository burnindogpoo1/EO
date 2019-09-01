using EO.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DbContext
{
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
