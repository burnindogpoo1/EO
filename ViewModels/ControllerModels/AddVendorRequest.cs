
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
    public class AddVendorRequest
    {
        public AddVendorRequest()
        {
            Vendor = new VendorDTO();
        }
        public VendorDTO Vendor { get; set; }
    }
}
