//using SharedData.ListFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using static SharedData.Enums;

namespace InventoryServiceLayer.Interface
{
    public interface IInventoryManager
    {
        byte[] GetImage(long imageId);

        GetPlantResponse GetPlant(long plantId);
               
        GetArrangementResponse GetArrangement(long arrangementId);

        List<GetSimpleArrangementResponse> GetArrangements(string arrangementName);

        GetPlantTypeResponse GetPlantTypes();

        GetPlantResponse GetPlantsByType(long plantTypeId);

        GetPlantNameResponse GetPlantNamesByType(long plantTypeId);

        GetPlantResponse GetPlants();

        GetMaterialTypeResponse GetMaterialTypes();

        GetMaterialResponse GetMaterialsByType(long materialTypeId);

        GetMaterialNameResponse GetMaterialNamesByType(long materialTypeId);

        GetMaterialResponse GetMaterials();

        GetFoliageTypeResponse GetFoliageTypes();

        GetFoliageResponse GetFoliageByType(long foliageTypeId);

        GetFoliageNameResponse GetFoliageNamesByType(long foliageTypeId);

        GetFoliageResponse GetFoliage();

        GetContainerTypeResponse GetContainerTypes();

        List<ContainerNameDTO> GetContainerNamesByType(long containerTypeId);

        GetContainerResponse GetContainersByType(long containerTypeId);

        GetContainerResponse GetContainer(long containerId);

        GetContainerResponse GetContainers();

        List<ServiceCodeDTO> GetAllServiceCodes();

        GetKvpLongStringResponse GetInventoryList();

        ServiceCodeDTO GetServiceCodeById(long serviceCodeId);

        GetServiceCodeResponse GetAllServiceCodesByType(ServiceCodeType serviceCodetype);

        long ServiceCodeIsNotUnique(string serviceCode);

        bool GeneralLedgerIsNotUnique(string serviceCode);

        long DoesServiceCodeExist(ServiceCodeDTO serviceCodeDTO);

        long AddServiceCode(ServiceCodeDTO serviceCodeDTO);

        List<InventoryTypeDTO> GetInventoryTypes();

        GetInventoryResponse GetInventory(InventoryType inventoryType);

        bool InventoryNameIsNotUnique(string inventoryName);

        bool PlantNameIsNotUnique(string plantName);

        bool ContainerNameIsNotUnique(string containerName);

        bool ArrangementNameIsnotUnique(string arrangementname);

        long AddPlantName(AddPlantNameRequest request);

        long AddPlantType(AddPlantTypeRequest request);

        long DoesPlantNameExist(string plantName);

        long DoesPlantTypeExist(string plantType);

        long DoesPlantExist(PlantDTO plantDTO);

        long ImportPlant(ImportPlantRequest request);

        long AddPlant(AddPlantRequest plantRequest);

        bool DeletePlant(long plantId);

        long AddContainer(AddContainerRequest containerRequest);

        long AddArrangement(AddArrangementRequest arrangementRequest);

        long UpdateArrangement(UpdateArrangementRequest arrangementRequest);

        long AddPlantImage(byte[] imageBytes);

        long AddArrangementImage(AddArrangementImageRequest request);

        bool DeleteArrangement(long arrangementId);

        GetVendorResponse GetVendors(GetPersonRequest request);

        GetVendorResponse GetVendorById(long vendorId);

        long AddVendor(AddVendorRequest request);

        long AddCustomer(AddCustomerRequest request);

        long AddShipment(AddShipmentRequest request);

        ShipmentInventoryDTO GetShipment(long shipmentId);

        GetShipmentResponse GetShipments(ShipmentFilter filter);

        long AddWorkOrderPayment(WorkOrderPaymentDTO workOrderPayment);

        WorkOrderPaymentDTO GetWorkOrderPayment(long workOrderId);

        WorkOrderInventoryDTO GetWorkOrder(long workOrderId);

        List<WorkOrderResponse> GetWorkOrders(DateTime afterDate);

        long CancelWorkOrder(long workOrderId);
        
        long AddWorkOrder(AddWorkOrderRequest workOrderRequest);

        long AddWorkOrderImage(AddWorkOrderImageRequest workOrderImageRequest);

        WorkOrderResponse GetWorkOrders(WorkOrderListFilter filter);

        long DoesPersonExist(PersonDTO person);

        long ImportPerson(ImportPersonRequest request);

        GetPersonResponse GetPerson(GetPersonRequest request);

        long DoesFoliageNameExist(string foliageName);

        long DoesFoliageTypeExist(string foliageType);

        long DoesFoliageExist(FoliageDTO foliageDTO);

        long ImportFoliage(ImportFoliageRequest request);

        long DoesMaterialNameExist(string materialName);

        long DoesMaterialTypeExist(string materialType);

        long DoesMaterialExist(MaterialDTO foliageDTO);

        long ImportMaterial(ImportMaterialRequest request);

        long DoesContainerNameExist(string containerName);

        long DoesContainerTypeExist(string containerType);

        long DoesContainerExist(ContainerDTO containerDTO);

        long ImportContainer(ImportContainerRequest request);

        List<string> GetSizeByInventoryType(long inventoryTypeId);

        GetWorkOrderSalesDetailResponse GetWorkOrderDetail(GetWorkOrderSalesDetailRequest request);
    }
}
