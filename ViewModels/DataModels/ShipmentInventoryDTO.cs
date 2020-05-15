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
    public class ShipmentInventoryDTO
    {
        public ShipmentDTO Shipment { get; set; }

        public List<ShipmentInventoryMapDTO> ShipmentInventoryMap { get; set; }

        public ShipmentInventoryDTO()
        {
            Shipment = new ShipmentDTO();
            ShipmentInventoryMap = new List<ShipmentInventoryMapDTO>();
        }

        public ShipmentInventoryDTO(ShipmentDTO shipment, List<ShipmentInventoryMapDTO> inventoryMap)
        {
            Shipment = shipment;
            ShipmentInventoryMap = inventoryMap;
        }
    }
}
