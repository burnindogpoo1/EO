using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class Plant
    {
        public Plant()
        {
            InventoryPlantMap = new HashSet<InventoryPlantMap>();
            PlantImageMap = new HashSet<PlantImageMap>();
        }

        public long PlantId { get; set; }
        public string PlantName { get; set; }
        public long PlantTypeId { get; set; }
        public long PlantNameId { get; set; }
        public string PlantSize { get; set; }
        public virtual PlantType PlantType { get; set; }
        public virtual ICollection<InventoryPlantMap> InventoryPlantMap { get; set; }
        public virtual ICollection<PlantImageMap> PlantImageMap { get; set; }
    }
}
