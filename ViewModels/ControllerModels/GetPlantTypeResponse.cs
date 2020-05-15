
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
    public class GetPlantTypeResponse : ApiResponse
    {
        public List<PlantTypeDTO> PlantTypes { get; set; }
        public GetPlantTypeResponse()
        {
            PlantTypes = new List<PlantTypeDTO>();
        }
        public GetPlantTypeResponse(List<PlantTypeDTO> plantTypes)
        {
            PlantTypes = plantTypes;
        }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class GetMaterialTypeResponse : ApiResponse
    {
        public List<MaterialTypeDTO> MaterialTypes { get; set; }
        public GetMaterialTypeResponse()
        {
            MaterialTypes = new List<MaterialTypeDTO>();
        }
        public GetMaterialTypeResponse(List<MaterialTypeDTO> materialTypes)
        {
            MaterialTypes = materialTypes;
        }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class GetFoliageTypeResponse : ApiResponse
    {
        public List<FoliageTypeDTO> FoliageTypes { get; set; }
        public GetFoliageTypeResponse()
        {
            FoliageTypes = new List<FoliageTypeDTO>();
        }
        public GetFoliageTypeResponse(List<FoliageTypeDTO> foliageTypes)
        {
            FoliageTypes = foliageTypes;
        }
    }
}
