
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
    public class MaterialInventoryDTO
    {
        public MaterialDTO Material { get; set; }

        public InventoryDTO Inventory { get; set; }

        public long ImageId { get; set; }

        public MaterialInventoryDTO()
        {
            Material = new MaterialDTO();

            Inventory = new InventoryDTO();

            ImageId = 0;
        }

        public MaterialInventoryDTO(MaterialDTO material, InventoryDTO inventory, long imageId)
        {
            Material = material;

            Inventory = inventory;

            ImageId = imageId;
        }
    }
}
