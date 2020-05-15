using EO.DatabaseContext;
using EO.ViewModels.DataModels;
using SharedData;
//using SharedData.ListFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ControllerModels;
using ViewModels.DataModels;

namespace EO.Persistence
{
    public interface IEOPersistence
    {
        LoginDTO GetUser(LoginDTO loginDTO);

        byte[] GetImage(long image_id);

        long ServiceCodeIsNotUnique(string serviceCode);

        bool GeneralLedgerIsNotUnique(string generalLedger);

        long InventoryExists(InventoryDTO inventoryDTO);

        bool InventoryNameIsNotUnique(string inventoryName);

        bool PlantNameIsNotUnique(string plantaName);

        bool ContainerNameIsnotUnique(string containerName);

        bool ArrangementNameIsnotUnique(string arrangementName);

        GetPlantResponse GetPlant(long plantId);

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

        GetArrangementResponse GetArrangement(long arrangementId);

        List<GetSimpleArrangementResponse> GetArrangements(string arrangementName);

        List<ServiceCodeDTO> GetServiceCodes();

        ServiceCodeDTO GetServiceCodeById(long serviceCodeId);

        GetServiceCodeResponse GetServiceCodesByType(string serviceCodePrefix);

        List<ContainerNameDTO> GetContainerNamesByType(long plantTypeId);

        GetContainerResponse GetContainersByType(long containerTypeId);

        GetContainerResponse GetContainer(long containerId);

        GetContainerResponse GetContainers();

        GetContainerTypeResponse GetContainerTypes();

        GetKvpLongStringResponse GetInventoryList();

        long ServiceCodeExists(ServiceCodeDTO serviceCodeDTO);

        long AddServiceCode(ServiceCodeDTO serviceCodeDTO);

        long PlantNameExists(string plantName);

        long AddPlantName(AddPlantNameRequest request);

        long PlantTypeExists(string plantTypeName);

        long AddPlantType(AddPlantTypeRequest request);

        long PlantExists(PlantDTO plantDTO);

        long ImportPlant(ImportPlantRequest request);

        long AddPlant(AddPlantRequest plantRequest);

        long UpdatePlant(ImportPlantRequest plantRequest);

        bool DeletePlant(long planrId);
        
        long AddContainer(AddContainerRequest containerRequest);

        long AddArrangement(AddArrangementRequest arrangementRequest);

        long UpdateArrangement(UpdateArrangementRequest arrangementRequest);

        List<InventoryTypeDTO> GetInventoryTypes();

        GetInventoryResponse GetInventory(Enums.InventoryType inventoryType);

        WorkOrderInventoryDTO GetWorkOrder(long workOrderId);

        List<WorkOrderResponse> GetWorkOrders(DateTime afterDate);

        long CancelWorkOrder(long workOrderId);

        long AddWorkOrder(AddWorkOrderRequest workOrderRequest);

        long AddWorkOrderImage(AddWorkOrderImageRequest workOrderImageRequest);

        WorkOrderResponse GetWorkOrders(WorkOrderListFilter filter);

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

        long DoesPersonExist(PersonDTO person);

        long ImportPerson(ImportPersonRequest request);

        GetPersonResponse GetPerson(GetPersonRequest request);
        long FoliageExists(FoliageDTO foliageDTO);
        long FoliageNameExists(string foliageName);
        long FoliageTypeExists(string typeName);
        bool FoliageNameIsNotUnique(string foliageName);


        long AddFoliageType(AddFoliageTypeRequest foliageTypeRequest);
        long AddFoliageName(AddFoliageNameRequest foliageNameRequest);
        long AddFoliage(AddFoliageRequest request);
        long UpdateFoliage(ImportFoliageRequest request);
        long ImportFoliage(ImportFoliageRequest request);

        long MaterialExists(MaterialDTO materialDTO);
        long MaterialNameExists(string materialName);
        long MaterialTypeExists(string typeName);
        bool MaterialNameIsNotUnique(string materialName);

        long AddMaterialType(AddMaterialTypeRequest materialTypeRequest);
        long AddMaterialName(AddMaterialNameRequest materialNameRequest);
        long AddMaterial(AddMaterialRequest request);
        long UpdateMaterial(ImportMaterialRequest request);
        long ImportMaterial(ImportMaterialRequest request);

        long ContainerExists(ContainerDTO containerDTO);
        long ContainerNameExists(string containerName);
        long ContainerTypeExists(string typeName);
        bool ContainerNameIsNotUnique(string containerName);

        long AddContainerType(AddContainerTypeRequest containerTypeRequest);
        long AddContainerName(AddContainerNameRequest containerNameRequest);
        //long AddContainer(AddContainerRequest request);
        long UpdateContainer(ImportContainerRequest request);

        long ImportContainer(ImportContainerRequest request);

        List<string> GetSizesByInventoryType(long inventoryTypeId);

        GetWorkOrderSalesDetailResponse GetWorkOrderDetail(GetWorkOrderSalesDetailRequest request);
    }
}
