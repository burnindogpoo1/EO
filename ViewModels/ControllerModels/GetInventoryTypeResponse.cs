
using Android.Runtime;
using EO.ViewModels.ControllerModels;
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
    public class GetInventoryTypeResponse : ApiResponse
    {
        public List<InventoryTypeDTO> InventoryType { get; set; }

        public GetInventoryTypeResponse()
        {

        }

        public GetInventoryTypeResponse(List<InventoryTypeDTO> dtoList)
        {
            InventoryType = dtoList;
        }
    }
}
