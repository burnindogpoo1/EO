using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    public class ShipmentDTO
    {
        public long ShipmentId { get; set; }

        public DateTime ShipmentDate { get; set; }

        public long VendorId { get; set; }
    }
}
