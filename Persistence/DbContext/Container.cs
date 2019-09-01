using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class Container
    {
        public Container()
        {
            ContainerImageMap = new HashSet<ContainerImageMap>();
            InventoryContainerMap = new HashSet<InventoryContainerMap>();
        }

        public long ContainerId { get; set; }
        public string ContainerColor { get; set; }
        public long ContainerTypeId { get; set; }
        public string ContainerSize { get; set; }
        public string ContainerName { get; set; }
        public long ContainerNameId { get; set; }
        public string ContainerTypeName { get; set; }
        public string ContainerPrice { get; set; }
        public string ContainerVendor { get; set; }
        public string ContainerSku { get; set; }
        public DateTime? ContainerLastshipdate { get; set; }

        public virtual ICollection<ContainerImageMap> ContainerImageMap { get; set; }
        public virtual ICollection<InventoryContainerMap> InventoryContainerMap { get; set; }
    }
}
