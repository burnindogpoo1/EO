using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    public class ArrangementInventoryDTO
    {
        public InventoryDTO Inventory { get; set; }
        public ArrangementDTO Arrangement { get; set; }
        public List<KeyValuePair<long, string>> InventoryList { get; set; }
        public long ImageId { get; set; }

        public ArrangementInventoryDTO()
        {
            Inventory = new InventoryDTO();
            Arrangement = new ArrangementDTO();
            InventoryList = new List<KeyValuePair<long, string>>();
            ImageId = 0;
        }

        public ArrangementInventoryDTO(InventoryDTO inventory, ArrangementDTO arrangement, List<KeyValuePair<long, string>> inventoryList, long imageId)
        {
            Inventory = inventory;
            Arrangement = arrangement;
            InventoryList = inventoryList;
            ImageId = imageId;
        }
    }
}
