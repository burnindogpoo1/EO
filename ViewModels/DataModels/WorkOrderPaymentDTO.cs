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
    public class WorkOrderPaymentDTO
    {
        public long WorkOrderPaymentId { get; set; }
        public long WorkOrderId { get; set; }
        public int WorkOrderPaymentType { get; set; } //1 = cash, 2 = check, 3 = credit card
        public decimal WorkOrderPaymentAmount { get; set; }
        public decimal WorkOrderPaymentTax { get; set; }
        public string WorkOrderPaymentCreditCardConfirmation { get; set; }
    }
}
