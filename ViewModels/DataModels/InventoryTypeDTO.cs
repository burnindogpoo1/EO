
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
    public class InventoryTypeDTO
    {
        public long InventoryTypeId { get; set; }

        public string InventoryTypeName { get; set; }
    }
}
