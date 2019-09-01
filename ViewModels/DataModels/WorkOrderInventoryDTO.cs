using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    public class WorkOrderInventoryDTO
    {
        public WorkOrderDTO WorkOrder { get; set; }
        public List<WorkOrderInventoryMapDTO> InventoryList { get; set; }

        public WorkOrderInventoryDTO()
        {
            WorkOrder = new WorkOrderDTO();
            InventoryList = new List<WorkOrderInventoryMapDTO>();
        }

        public WorkOrderInventoryDTO(WorkOrderDTO workOrder, List<WorkOrderInventoryMapDTO> inventoryList)
        {
            WorkOrder = workOrder;
            InventoryList = inventoryList;
        }
    }
}
