
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
    public class GetVendorResponse
    {
        public GetVendorResponse()
        {
            VendorList = new List<VendorDTO>();
        }
        public List<VendorDTO> VendorList { get; set; }
    }
}
