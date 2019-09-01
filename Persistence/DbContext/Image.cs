using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class Image
    {
        public Image()
        {
            ArrangementImageMap = new HashSet<ArrangementImageMap>();
            ContainerImageMap = new HashSet<ContainerImageMap>();
            InventoryImageMap = new HashSet<InventoryImageMap>();
            PlantImageMap = new HashSet<PlantImageMap>();
        }

        public long ImageId { get; set; }
        public byte[] ImageData { get; set; }

        public virtual ICollection<ArrangementImageMap> ArrangementImageMap { get; set; }
        public virtual ICollection<ContainerImageMap> ContainerImageMap { get; set; }
        public virtual ICollection<InventoryImageMap> InventoryImageMap { get; set; }
        public virtual ICollection<PlantImageMap> PlantImageMap { get; set; }
    }
}
