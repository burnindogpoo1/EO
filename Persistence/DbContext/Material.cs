using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public partial class Material
    {
        public Material()
        {
            InventoryMaterialMap = new HashSet<InventoryMaterialMap>();
            //PlantImageMap = new HashSet<PlantImageMap>();
        }

        public long MaterialId { get; set; }
        public string MaterialName { get; set; }
        public long MaterialTypeId { get; set; }
        public long MaterialNameId { get; set; }
        public string MaterialSize { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public virtual ICollection<InventoryMaterialMap> InventoryMaterialMap { get; set; }
        //public virtual ICollection<PlantImageMap> PlantImageMap { get; set; }
    }
}
