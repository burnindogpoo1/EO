using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class PaymentRequest
    {       
        public byte[] payload { get; set; }
        public int test { get; set; }
        public string test2 { get; set; }
    }
}
