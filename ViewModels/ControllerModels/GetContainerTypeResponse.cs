
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
    public class GetContainerTypeResponse : ApiResponse
    {
        public List<ContainerTypeDTO> ContainerTypeList { get; set; }

        public GetContainerTypeResponse()
        {
            ContainerTypeList = new List<ContainerTypeDTO>();
        }

        public GetContainerTypeResponse(List<ContainerTypeDTO> containerTypeList)
        {
            ContainerTypeList = containerTypeList;
        }
    }
}
