using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class WorkOrder
    {
        public WorkOrder()
        {
            WorkOrderInventoryMap = new HashSet<WorkOrderInventoryMap>();
        }

        public long WorkOrderId { get; set; }
        public string PersonInitiator { get; set; }
        public string PersonReceiver { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string Comments { get; set; }
        public short Paid { get; set; }

        public virtual ICollection<WorkOrderInventoryMap> WorkOrderInventoryMap { get; set; }
    }
}
