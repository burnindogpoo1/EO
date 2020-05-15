using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public partial class WorkOrderImageMap
    {
        public long WorkOrderImageMapId { get; set; }
        public long WorkOrderId { get; set; }
        public long ImageId { get; set; }

        public virtual Image Image { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
    }
}
