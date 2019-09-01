using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    public class WorkOrderDTO
    {
        public long WorkOrderId { get; set; }

        public string Seller { get; set; }

        public string Buyer { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public DateTime ClosedDate { get; set; }

        public string Comments { get; set; }

        public bool Paid { get; set; }
    }
}
