using EO.ViewModels.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    public class GetWorkOrderSalesDetailRequest
    {
        public List<WorkOrderInventoryItemDTO> WorkOrderItems { get; set; }

        public GetWorkOrderSalesDetailRequest()
        {
            WorkOrderItems = new List<WorkOrderInventoryItemDTO>();
        }

        public GetWorkOrderSalesDetailRequest(List<WorkOrderInventoryItemDTO> workOrderItems)
        {
            WorkOrderItems = workOrderItems;
        }
    }

    public class GetWorkOrderSalesDetailResponse : ApiResponse
    {
        public decimal SubTotal { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }
    }
}
