using EO.ViewModels.ControllerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    public class GetInventoryResponse : ApiResponse
    {
        public List<InventoryDTO> InventoryList { get; set; }
        public GetInventoryResponse()
        {

        }
        public GetInventoryResponse(List<InventoryDTO> inventoryList)
        {
            InventoryList = inventoryList;
        }
    }

    public class GetPlantResponse : ApiResponse
    {
        public List<PlantInventoryDTO> PlantInventoryList { get; set; }
        public GetPlantResponse()
        {
            PlantInventoryList = new List<PlantInventoryDTO>();
        }

        public GetPlantResponse(List<PlantInventoryDTO> plantInventoryList)
        {
            PlantInventoryList = plantInventoryList;
        }
    }

    public class GetMaterialResponse : ApiResponse
    {
        public List<MaterialInventoryDTO> MaterialInventoryList { get; set; }
        public GetMaterialResponse()
        {
            MaterialInventoryList = new List<MaterialInventoryDTO>();
        }

        public GetMaterialResponse(List<MaterialInventoryDTO> materialInventoryList)
        {
            MaterialInventoryList = materialInventoryList;
        }
    }

    public class GetFoliageResponse : ApiResponse
    {
        public List<FoliageInventoryDTO> FoliageInventoryList { get; set; }
        public GetFoliageResponse()
        {
            FoliageInventoryList = new List<FoliageInventoryDTO>();
        }

        public GetFoliageResponse(List<FoliageInventoryDTO> foliageInventoryList)
        {
            FoliageInventoryList = foliageInventoryList;
        }
    }
    public class GetContainerResponse : ApiResponse
    {
        public List<ContainerInventoryDTO> ContainerInventoryList { get; set; }

        public GetContainerResponse()
        {
            ContainerInventoryList = new List<ContainerInventoryDTO>();
        }

        public GetContainerResponse(List<ContainerInventoryDTO> containerInventoryList)
        {
            ContainerInventoryList = containerInventoryList;
        }
    }

    public class GetArrangementResponse : ApiResponse
    {
        public List<ArrangementInventoryDTO> ArrangementList { get; set; }
        public GetArrangementResponse()
        {
            ArrangementList = new List<ArrangementInventoryDTO>();
        }

        public GetArrangementResponse(List<ArrangementInventoryDTO> arrangementList)
        {
            ArrangementList = arrangementList;
        }
    }
}
