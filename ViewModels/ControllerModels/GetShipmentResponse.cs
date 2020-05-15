
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
    public class GetShipmentResponse : ApiResponse
    {
        public List<ShipmentInventoryDTO> ShipmentList { get; set; }

        public GetShipmentResponse()
        {
            ShipmentList = new List<ShipmentInventoryDTO>();
        }

        public GetShipmentResponse(List<ShipmentInventoryDTO> shipmentList)
        {
            ShipmentList = shipmentList;
        }
    }
}
