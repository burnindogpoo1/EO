
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
    public class PlantInventoryDTO
    {
        public PlantDTO Plant { get; set; }

        public InventoryDTO Inventory { get; set; }

        public long ImageId { get; set; }

        public PlantInventoryDTO()
        {
            Plant = new PlantDTO();

            Inventory = new InventoryDTO();

            ImageId = 0;
        }

        public PlantInventoryDTO(PlantDTO plant, InventoryDTO inventory, long imageId)
        {
            Plant = plant;

            Inventory = inventory;

            ImageId = imageId;
        }
    }
}
