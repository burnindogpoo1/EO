using EO.ViewModels.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    public class WorkOrderResponse : ApiResponse
    {
        public List<WorkOrderInventoryDTO> WorkOrderList { get; set; }

        public WorkOrderResponse()
        {
            WorkOrderList = new List<WorkOrderInventoryDTO>();
        }
    }
}
