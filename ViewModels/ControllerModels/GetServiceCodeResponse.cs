
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
    public class GetServiceCodeResponse : ApiResponse
    {
        public List<ServiceCodeDTO>  ServiceCodeList { get; set; }

        public GetServiceCodeResponse()
        {
            ServiceCodeList = new List<ServiceCodeDTO>();
        }

        public GetServiceCodeResponse(List<ServiceCodeDTO> serviceCodeList)
        {
            ServiceCodeList = serviceCodeList;
        }
    }
}
