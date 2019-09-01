using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    public class GetVendorResponse
    {
        public GetVendorResponse()
        {
            VendorList = new List<VendorDTO>();
        }
        public List<VendorDTO> VendorList { get; set; }
    }
}
