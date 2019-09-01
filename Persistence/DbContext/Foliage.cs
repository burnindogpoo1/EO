using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public partial class Foliage
    {
        public Foliage()
        {
            InventoryFoliageMap = new HashSet<InventoryFoliageMap>();
            //PlantImageMap = new HashSet<PlantImageMap>();
        }

        public long FoliageId { get; set; }
        public string FoliageName { get; set; }
        public long FoliageTypeId { get; set; }
        public long FoliageNameId { get; set; }
        public string FoliageSize { get; set; }
        public virtual FoliageType FoliageType { get; set; }
        public virtual ICollection<InventoryFoliageMap> InventoryFoliageMap { get; set; }
        //public virtual ICollection<PlantImageMap> PlantImageMap { get; set; }
    }
}
