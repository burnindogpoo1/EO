using EO.Persistence;
using InventoryServiceLayer.Helpers;
using InventoryServiceLayer.Interface;
//using SharedData.ListFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using static SharedData.Enums;

namespace InventoryServiceLayer.Implementation
{
    public class InventoryManager : IInventoryManager
    {
        private IEOPersistence persistence;

        public InventoryManager()
        {
            persistence = new EOPersistence();
        }

        public byte[] GetImage(long imageId)
        {
            return persistence.GetImage(imageId);
        }

        public long DoesPlantExist(PlantDTO plantDTO)
        {
            return persistence.PlantExists(plantDTO);
        }

        public GetPlantResponse GetPlant(long plantId)
        {
            return persistence.GetPlant(plantId);
        }

        public GetArrangementResponse GetArrangement(long arrangementId)
        {
            return persistence.GetArrangement(arrangementId);
        }

        public List<GetSimpleArrangementResponse> GetArrangements(string arrangementName)
        {
            return persistence.GetArrangements(arrangementName);
        }

        public GetPlantTypeResponse GetPlantTypes()
        {
            return persistence.GetPlantTypes();
        }

        public GetPlantResponse GetPlantsByType(long plantTypeId)
        {
            return persistence.GetPlantsByType(plantTypeId);
        }

        public GetPlantNameResponse GetPlantNamesByType(long plantTypeId)
        {
            return persistence.GetPlantNamesByType(plantTypeId);
        }
        public GetPlantResponse GetPlants()
        {
            return persistence.GetPlants();
        }

        public GetMaterialTypeResponse GetMaterialTypes()
        {
            return persistence.GetMaterialTypes();
        }

        public GetMaterialResponse GetMaterialsByType(long materialTypeId)
        {
            return persistence.GetMaterialsByType(materialTypeId);
        }

        public GetMaterialNameResponse GetMaterialNamesByType(long materialTypeId)
        {
            return persistence.GetMaterialNamesByType(materialTypeId);
        }
        public GetMaterialResponse GetMaterials()
        {
            return persistence.GetMaterials();
        }

        public GetFoliageTypeResponse GetFoliageTypes()
        {
            return persistence.GetFoliageTypes();
        }

        public GetFoliageResponse GetFoliageByType(long foliageTypeId)
        {
            return persistence.GetFoliageByType(foliageTypeId);
        }

        public GetFoliageNameResponse GetFoliageNamesByType(long foliageTypeId)
        {
            return persistence.GetFoliageNamesByType(foliageTypeId);
        }
        public GetFoliageResponse GetFoliage()
        {
            return persistence.GetFoliage();
        }
        public GetContainerTypeResponse GetContainerTypes()
        {
            return persistence.GetContainerTypes();
        }

        public List<ContainerNameDTO> GetContainerNamesByType(long containerTypeId)
        {
            return persistence.GetContainerNamesByType(containerTypeId);
        }

        public GetContainerResponse GetContainersByType(long containerTypeId)
        {
            return persistence.GetContainersByType(containerTypeId);
        }

        public GetContainerResponse GetContainer(long containerId)
        {
            return persistence.GetContainer(containerId);
        }

        public GetContainerResponse GetContainers()
        {
            return persistence.GetContainers();
        }

        public ServiceCodeDTO GetServiceCodeById(long serviceCodeId)
        {
            return persistence.GetServiceCodeById(serviceCodeId);
        }

        public List<ServiceCodeDTO> GetAllServiceCodes()
        {
            return persistence.GetServiceCodes();
        }

        public GetServiceCodeResponse GetAllServiceCodesByType(ServiceCodeType serviceCodeType)
        {
            return persistence.GetServiceCodesByType(ServiceCodePrefix.GetServiceCodePrefix(serviceCodeType));
        }

        public GetKvpLongStringResponse GetInventoryList()
        {
            return persistence.GetInventoryList();
        }
        public long ServiceCodeIsNotUnique(string serviceCode)
        {
            return persistence.ServiceCodeIsNotUnique(serviceCode);
        }

        public bool GeneralLedgerIsNotUnique(string generalLedger)
        {
            return persistence.GeneralLedgerIsNotUnique(generalLedger);
        }

        public long DoesServiceCodeExist(ServiceCodeDTO serviceCodeDTO)
        {
            return persistence.ServiceCodeExists(serviceCodeDTO);
        }

        public long AddServiceCode(ServiceCodeDTO serviceCodeDTO)
        {
            return persistence.AddServiceCode(serviceCodeDTO);
        }

        public List<InventoryTypeDTO> GetInventoryTypes()
        {
            return persistence.GetInventoryTypes();
        }

        public GetInventoryResponse GetInventory(InventoryType inventoryType)
        {
            return persistence.GetInventory(inventoryType);
        }

        public bool InventoryNameIsNotUnique(string inventoryName)
        {
            return persistence.InventoryNameIsNotUnique(inventoryName);
        }

        public bool PlantNameIsNotUnique(string plantName)
        {
            return persistence.PlantNameIsNotUnique(plantName);
        }

        public bool ContainerNameIsNotUnique(string containerName)
        {
            return persistence.ContainerNameIsnotUnique(containerName);
        }

        public bool ArrangementNameIsnotUnique(string arrangementName)
        {
            return persistence.ArrangementNameIsnotUnique(arrangementName);
        }

        public long AddPlantName(AddPlantNameRequest request)
        {
            return persistence.AddPlantName(request);
        }

        public long DoesPlantNameExist(string plantName)
        {
            return persistence.PlantNameExists(plantName);
        }

        public long AddPlantType(AddPlantTypeRequest request)
        {
            return persistence.AddPlantType(request);
        }

        public long DoesPlantTypeExist(string plantType)
        {
            return persistence.PlantTypeExists(plantType);
        }

        public long ImportPlant(ImportPlantRequest request)
        {
            return persistence.ImportPlant(request);
        }
        public long AddPlant(AddPlantRequest plantRequest)
        {
            return persistence.AddPlant(plantRequest);
        }

        public bool DeletePlant(long plantId)
        {
            return persistence.DeletePlant(plantId);
        }

        public long AddContainer(AddContainerRequest containerRequest)
        {
            return persistence.AddContainer(containerRequest);
        }

        public long AddArrangement(AddArrangementRequest arrangementrequest)
        {
            return persistence.AddArrangement(arrangementrequest);
        }

        public long UpdateArrangement(UpdateArrangementRequest arrangementrequest)
        {
            return persistence.UpdateArrangement(arrangementrequest);
        }

        public long AddPlantImage(byte[] imageBytes)
        {
            return persistence.AddPlantImage(imageBytes);
        }

        public long AddArrangementImage(AddArrangementImageRequest request)
        {
            return persistence.AddArrangementImage(request);
        }

        public bool DeleteArrangement(long arrangementId)
        {
            return persistence.DeleteArrangement(arrangementId);
        }

        public GetVendorResponse GetVendors(GetPersonRequest request)
        {
            return persistence.GetVendors(request);
        }

        public GetVendorResponse GetVendorById(long vendorId)
        {
            return persistence.GetVendorById(vendorId);
        }

        public long AddVendor(AddVendorRequest request)
        {
            return persistence.AddVendor(request);
        }

        public long AddCustomer(AddCustomerRequest request)
        {
            return persistence.AddCustomer(request);
        }

        public long AddShipment(AddShipmentRequest request)
        {
            return persistence.AddShipment(request);
        }

        public ShipmentInventoryDTO GetShipment(long shipmentId)
        {
            return persistence.GetShipment(shipmentId);
        }
        public GetShipmentResponse GetShipments(ShipmentFilter filter)
        {
            return persistence.GetShipments(filter);
        }

        public long AddWorkOrderPayment(WorkOrderPaymentDTO workOrderPayment)
        {
            return persistence.AddWorkOrderPayment(workOrderPayment);
        }

        public WorkOrderPaymentDTO GetWorkOrderPayment(long workOrderId)
        {
            return persistence.GetWorkOrderPayment(workOrderId);
        }

        public WorkOrderInventoryDTO GetWorkOrder(long workOrderId)
        {
            return persistence.GetWorkOrder(workOrderId);
        }

        public List<WorkOrderResponse> GetWorkOrders(DateTime afterDate)
        {
            return persistence.GetWorkOrders(afterDate);
        }

        public long CancelWorkOrder(long workOrderId)
        {
            return persistence.CancelWorkOrder(workOrderId);
        }

        public long AddWorkOrder(AddWorkOrderRequest workOrderRequest)
        {
            return persistence.AddWorkOrder(workOrderRequest);
        }

        public long AddWorkOrderImage(AddWorkOrderImageRequest workOrderImageRequest)
        {
            return persistence.AddWorkOrderImage(workOrderImageRequest);
        }

        public WorkOrderResponse GetWorkOrders(WorkOrderListFilter filter)
        {
           return persistence.GetWorkOrders(filter);
        }

        public long DoesPersonExist(PersonDTO person)
        {
            return persistence.DoesPersonExist(person);
        }

        public long ImportPerson(ImportPersonRequest request)
        {
            return persistence.ImportPerson(request);
        }

        public GetPersonResponse GetPerson(GetPersonRequest request)
        {
            return persistence.GetPerson(request);
        }

        public long DoesFoliageNameExist(string foliageName)
        {
            return persistence.FoliageNameExists(foliageName);
        }

        public long DoesFoliageTypeExist(string foliageType)
        {
            return persistence.FoliageTypeExists(foliageType);
        }

        public long DoesFoliageExist(FoliageDTO foliageDTO)
        {
            return persistence.FoliageExists(foliageDTO);
        }

        public long ImportFoliage(ImportFoliageRequest request)
        {
            return persistence.ImportFoliage(request);
        }

        public long DoesMaterialNameExist(string materialName)
        {
            return persistence.MaterialNameExists(materialName);
        }

        public long DoesMaterialTypeExist(string materialType)
        {
            return persistence.MaterialTypeExists(materialType);
        }

        public long DoesMaterialExist(MaterialDTO materialDTO)
        {
            return persistence.MaterialExists(materialDTO);
        }

        public long ImportMaterial(ImportMaterialRequest request)
        {
            return persistence.ImportMaterial(request);
        }
        public long DoesContainerNameExist(string containerName)
        {
            return persistence.ContainerNameExists(containerName);
        }

        public long DoesContainerTypeExist(string containerType)
        {
            return persistence.ContainerTypeExists(containerType);
        }

        public long DoesContainerExist(ContainerDTO containerDTO)
        {
            return persistence.ContainerExists(containerDTO);
        }

        public long ImportContainer(ImportContainerRequest request)
        {
            return persistence.ImportContainer(request);
        }

        public List<string> GetSizeByInventoryType(long inventoryTypeId)
        {
            return persistence.GetSizesByInventoryType(inventoryTypeId);
        }

        public GetWorkOrderSalesDetailResponse GetWorkOrderDetail(GetWorkOrderSalesDetailRequest request)
        {
            return persistence.GetWorkOrderDetail(request);
        }
    }
}
