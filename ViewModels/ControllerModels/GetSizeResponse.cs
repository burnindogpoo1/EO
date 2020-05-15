
using Android.Runtime;
using EO.ViewModels.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class  GetSizeResponse : ApiResponse
    {
        public long InventoryTypeId { get; set; }

        public List<string> Sizes { get; set; }

        public GetSizeResponse()
        {
            Sizes = new List<string>();
        }

        public GetSizeResponse(List<string> sizes)
        {
            Sizes = sizes;
        }
    }
}
