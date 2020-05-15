using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public class WorkOrderPayment
    {
        public long WorkOrderPaymentId { get; set; }

        [ForeignKey("WorkOrderPayment")]
        public long WorkOrderId { get; set; }
        public int WorkOrderPaymentType { get; set; } //1 = cash, 2 = check, 3 = credit card
        public decimal WorkOrderPaymentAmount { get; set; }
        public decimal WorkOrderPaymentTax { get; set; }
        public string WorkOrderPaymentCreditCardConfirmation { get; set; }

        public virtual WorkOrder WorkOrder { get; set; }
    }
}
