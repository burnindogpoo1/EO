using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class Inventory
    {
        public Inventory()
        {
            ArrangementInventoryMap = new HashSet<ArrangementInventoryMap>();
            InventoryContainerMap = new HashSet<InventoryContainerMap>();
            InventoryImageMap = new HashSet<InventoryImageMap>();
            InventoryPlantMap = new HashSet<InventoryPlantMap>();
            WorkOrderInventoryMap = new HashSet<WorkOrderInventoryMap>();
        }

        public long InventoryId { get; set; }
        public string InventoryName { get; set; }
        public int Quantity { get; set; }
        public int NotifyWhenLowAmount { get; set; }
        public long ServiceCodeId { get; set; }
        public long InventoryTypeId { get; set; }

        public virtual ServiceCode ServiceCode { get; set; }
        public virtual InventoryType InventoryType { get; set; }
        public virtual ICollection<ArrangementInventoryMap> ArrangementInventoryMap { get; set; }
        public virtual ICollection<InventoryContainerMap> InventoryContainerMap { get; set; }
        public virtual ICollection<InventoryImageMap> InventoryImageMap { get; set; }
        public virtual ICollection<InventoryPlantMap> InventoryPlantMap { get; set; }
        public virtual ICollection<WorkOrderInventoryMap> WorkOrderInventoryMap { get; set; }
        public virtual ICollection<ShipmentInventoryMap> ShipmentInventoryMap { get; set; }
    }
}
