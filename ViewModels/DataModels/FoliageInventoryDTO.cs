
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
    public class FoliageInventoryDTO
    {
        public FoliageDTO Foliage { get; set; }

        public InventoryDTO Inventory { get; set; }

        public long ImageId { get; set; }

        public FoliageInventoryDTO()
        {
            Foliage = new FoliageDTO();

            Inventory = new InventoryDTO();

            ImageId = 0;
        }

        public FoliageInventoryDTO(FoliageDTO foliage, InventoryDTO inventory, long imageId)
        {
            Foliage = foliage;

            Inventory = inventory;

            ImageId = imageId;
        }
    }
}
