
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
    public class ContainerInventoryDTO
    {
        public ContainerDTO Container { get; set; }

        public InventoryDTO Inventory { get; set; }

        public long ImageId { get; set; }

        public ContainerInventoryDTO()
        {
            Container = new ContainerDTO();

            Inventory = new InventoryDTO();

            ImageId = 0;
        }

        public ContainerInventoryDTO(ContainerDTO container, InventoryDTO inventory, long imageId)
        {
            Container = container;
            Inventory = inventory;
            ImageId = imageId;
        }
    }
}
