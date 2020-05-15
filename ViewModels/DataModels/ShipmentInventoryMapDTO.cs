using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class ShipmentInventoryMapDTO
    {
        public ShipmentInventoryMapDTO()
        {

        }

        public ShipmentInventoryMapDTO(long shipmentId, long inventoryId, string inventoryName, int quantity)
        {
            ShipmentId = shipmentId;
            InventoryId = inventoryId;
            InventoryName = inventoryName;
            Quantity = quantity;
        }
        public long ShipmentInventoryMapId { get; set; }

        public long ShipmentId { get; set; }

        public long InventoryId { get; set; }

        public string InventoryName { get; set; }

        public int Quantity { get; set; }
    }
}
