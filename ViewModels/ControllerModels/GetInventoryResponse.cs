
using Android.Runtime;
using EO.ViewModels.ControllerModels;
using Newtonsoft.Json;
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

    [Serializable]
    [Preserve(AllMembers = true)]
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

    [Serializable]
    [Preserve(AllMembers = true)]
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

    [Serializable]
    [Preserve(AllMembers = true)]
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

    [Serializable]
    [Preserve(AllMembers = true)]
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

    [Serializable]
    [Preserve(AllMembers = true)]
    public class GetSimpleArrangementResponse // : ApiResponse
    {
        public ArrangementDTO Arrangement { get; set; }

        public InventoryDTO Inventory { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class GetArrangementResponse //: ApiResponse
    {
        public ArrangementDTO Arrangement { get; set; }
        public List<ArrangementInventoryDTO> ArrangementList { get; set; }

        public List<ImageResponse> Images { get; set; }
        public GetArrangementResponse()
        {
            Arrangement = new ArrangementDTO();

            ArrangementList = new List<ArrangementInventoryDTO>();

            Images = new List<ImageResponse>();
        }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class ImageResponse
    {
        public long ImageId { get; set; }
        public byte[] Image { get; set; }
    }
}
