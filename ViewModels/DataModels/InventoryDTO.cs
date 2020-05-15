
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
    public class InventoryDTO
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public long InventoryId { get; set; }

        /// <summary>
        /// This inventory item's name
        /// </summary>
        public string InventoryName { get; set; }

        /// <summary>
        /// Quantity on hand
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Alert when qty reaches this value
        /// </summary>
        public int NotifyWhenLowAmount { get; set; }

        /// <summary>
        /// This inventory item's service code
        /// </summary>
        public long ServiceCodeId { get; set; }

        public string ServiceCodeName { get; set; }

        /// <summary>
        /// This inventory item's type
        /// </summary>
        public long InventoryTypeId { get; set; }

        public string InventoryTypeName { get; set; }
    }
}
