
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class AddPlantRequest
    {
        public AddPlantRequest()
        {
            Inventory = new InventoryDTO();

            Plant = new PlantDTO();

            ServiceCode = new ServiceCodeDTO();
        }

        public InventoryDTO Inventory { get; set; }

        public PlantDTO Plant { get; set; }

        public ServiceCodeDTO ServiceCode { get; set; }

        public long ImageId { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class AddFoliageRequest
    {
        public AddFoliageRequest()
        {
            Inventory = new InventoryDTO();

            Foliage = new FoliageDTO();

            ServiceCode = new ServiceCodeDTO();
        }

        public InventoryDTO Inventory { get; set; }

        public FoliageDTO Foliage { get; set; }

        public ServiceCodeDTO ServiceCode { get; set; }

        public long ImageId { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class AddMaterialRequest
    {
        public AddMaterialRequest()
        {
            Inventory = new InventoryDTO();

            Material = new MaterialDTO();

            ServiceCode = new ServiceCodeDTO();
        }

        public InventoryDTO Inventory { get; set; }

        public MaterialDTO Material { get; set; }

        public ServiceCodeDTO ServiceCode { get; set; }

        public long ImageId { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class AddContainerRequest
    {
        public AddContainerRequest()
        {
            Inventory = new InventoryDTO();

            Container = new ContainerDTO();

            ServiceCode = new ServiceCodeDTO();
        }

        public InventoryDTO Inventory { get; set; }

        public ContainerDTO Container { get; set; }

        public ServiceCodeDTO ServiceCode { get; set; }

        public long ImageId { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class AddArrangementRequest
    {
        public AddArrangementRequest()
        {
            InventoryIds = new List<long>(); 
        }

        public InventoryDTO Inventory { get; set; }

        public ArrangementDTO Arrangement { get; set; }

        public List<long> InventoryIds { get; set; }

        public long ImageId { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class UpdateArrangementRequest
    {
        public UpdateArrangementRequest()
        {
            ArrangementItems = new List<ArrangementInventoryDTO>(); 
        }

        public InventoryDTO Inventory { get; set; }

        public ArrangementDTO Arrangement { get; set; }

        public List<ArrangementInventoryDTO> ArrangementItems { get; set; }

        public long ImageId { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class ImportPlantRequest
    {
        public ImportPlantRequest()
        {
            AddPlantRequest = new AddPlantRequest();

            ServiceCode = new ServiceCodeDTO();
        }
        public AddPlantRequest AddPlantRequest { get; set; }

        public string PlantName { get; set; }

        public string PlantType { get; set; }

        public string PlantSize { get; set; }

        public ServiceCodeDTO ServiceCode { get; set; }

        public byte[] imageBytes { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class ImportFoliageRequest
    {
        public ImportFoliageRequest()
        {
            AddFoliageRequest = new AddFoliageRequest();

            ServiceCode = new ServiceCodeDTO();
        }
        public AddFoliageRequest AddFoliageRequest { get; set; }

        public string FoliageName { get; set; }

        public string FoliageType { get; set; }

        public string FoliageSize { get; set; }

        public ServiceCodeDTO ServiceCode { get; set; }

        public byte[] imageBytes { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class ImportMaterialRequest
    {
        public ImportMaterialRequest()
        {
            AddMaterialRequest = new AddMaterialRequest();

            ServiceCode = new ServiceCodeDTO();
        }
        public AddMaterialRequest AddMaterialRequest { get; set; }

        public string MaterialName { get; set; }

        public string MaterialType { get; set; }

        public string MaterialSize { get; set; }

        public ServiceCodeDTO ServiceCode { get; set; }

        public byte[] imageBytes { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class ImportContainerRequest
    {
        public ImportContainerRequest()
        {
            AddContainerRequest = new AddContainerRequest();

            ServiceCode = new ServiceCodeDTO();
        }
        public AddContainerRequest AddContainerRequest { get; set; }

        public string ContainerName { get; set; }

        public string ContainerType { get; set; }

        public string ContainerSize { get; set; }

        public ServiceCodeDTO ServiceCode { get; set; }

        public byte[] imageBytes { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class AddImageRequest
    {
        public byte[] imgBytes { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class AddArrangementImageRequest
    {
        public long ArrangementId { get; set; }
        public byte[] Image { get; set; }
    }
}
