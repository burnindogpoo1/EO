
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class WorkOrderListFilter
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime ClosedDate { get; set; }

        public bool? Paid { get; set; }

        public bool? Delivery { get; set; }

        public bool? SiteService { get; set; }

        public long? CustomerId { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class ShipmentFilter
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public long? VendorId { get; set; }
    }
}
