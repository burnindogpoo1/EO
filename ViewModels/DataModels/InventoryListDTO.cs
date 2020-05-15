
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
    public class InventoryListDTO
    {
        public long InventoryId { get; set; }

        public string InventoryName { get; set; }

        public long ImageId { get; set; }
    }
}
