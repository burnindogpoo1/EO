using EO.ViewModels.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
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
