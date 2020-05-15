
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class AddWorkOrderRequest
    {
        public WorkOrderDTO WorkOrder { get; set; }

        public List<WorkOrderInventoryMapDTO> WorkOrderInventoryMap { get; set; }
    }

    /// <summary>
    /// Save the work order first - then save any images
    /// </summary>
    [Serializable]
    [Preserve(AllMembers = true)]
    public class AddWorkOrderImageRequest
    {
        public long WorkOrderId { get; set; }

        public byte[] Image { get; set; }
    }
}
