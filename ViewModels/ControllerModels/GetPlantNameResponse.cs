using EO.ViewModels.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    public class GetPlantNameResponse : ApiResponse
    {
        public List<PlantNameDTO> PlantNames { get; set; }

        public GetPlantNameResponse()
        {
            PlantNames = new List<PlantNameDTO>();
        }

        public GetPlantNameResponse(List<PlantNameDTO> plantNames)
        {
            PlantNames = plantNames;
        }
    }

    public class GetMaterialNameResponse : ApiResponse
    {
        public List<MaterialNameDTO> MaterialNames { get; set; }

        public GetMaterialNameResponse()
        {
            MaterialNames = new List<MaterialNameDTO>();
        }

        public GetMaterialNameResponse(List<MaterialNameDTO> materialNames)
        {
            MaterialNames = materialNames;
        }
    }

    public class GetFoliageNameResponse : ApiResponse
    {
        public List<FoliageNameDTO> FoliageNames { get; set; }

        public GetFoliageNameResponse()
        {
            FoliageNames = new List<FoliageNameDTO>();
        }

        public GetFoliageNameResponse(List<FoliageNameDTO> materialNames)
        {
            FoliageNames = materialNames;
        }
    }
}
