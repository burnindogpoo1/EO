
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

    public class ArrangementInventoryDTO
    {
        public ArrangementInventoryDTO()
        {

        }
        public ArrangementInventoryDTO(long arrangementId, long inventoryId, string name, string size, long imageId, int quantity = 1)
        {
            ArrangementId = arrangementId;
            InventoryId = inventoryId;
            ArrangementInventoryName = name;
            Size = size;
            ImageId = imageId;
            Quantity = quantity;
        }

        public long ArrangementId { get; set; }
        public long InventoryId { get; set; }
        public long ArrangementInventoryInventoryMapId { get; set; }
        public string ArrangementInventoryName { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public long ImageId { get; set; }
    }
}
