using EO.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public partial class InventoryType
    {
        public InventoryType()
        {
            Inventory = new HashSet<Inventory>();
        }

        public long InventoryTypeId { get; set; }
        public string InventoryTypeName { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
    }
}
