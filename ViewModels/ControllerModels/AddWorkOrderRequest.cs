using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    public class AddWorkOrderRequest
    {
        public WorkOrderDTO WorkOrder { get; set; }

        public List<WorkOrderInventoryMapDTO> WorkOrderInventoryMap { get; set; }
    }
}
