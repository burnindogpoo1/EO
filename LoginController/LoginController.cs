using EO.ViewModels.ControllerModels;
using EO.ViewModels.DataModels;
using InventoryServiceLayer;
using InventoryServiceLayer.Implementation;
using InventoryServiceLayer.Interface;
using LoginServiceLayer.Implementation;
using LoginServiceLayer.Interface;
using Microsoft.AspNetCore.Http;
//using SharedData.ListFilters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using static SharedData.Enums;

namespace EO.LoginController
{
    public class LoginController : ApiController
    {
        private ILoginManager loginManager;

        private IInventoryManager inventoryManager;

        public LoginController()
        {
            
            loginManager = new LoginManager();
            inventoryManager = new InventoryManager();
        }

        /// <summary>
        /// Login with username and password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public LoginResponse Login([FromBody]LoginRequest request)
        {
            LoginResponse response = new LoginResponse();


            LoginDTO loginDTO = loginManager.GetUser(request.Login);

            if (loginDTO.UserId == 0)
            {
                response.Messages.Add("login", new List<string>() { "user not found" });
            }
            else
            {
                response.EOAccess = "User/Pwd confirmed";
            }

            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetImage([FromUri]long imageId)
        {
            GetByteArrayResponse response = new GetByteArrayResponse();

            HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);

            response.HttpResponse = httpResponse;

            try
            {
                byte[] imgData = inventoryManager.GetImage(imageId);
                          
                httpResponse.Content = new ByteArrayContent(imgData);
                httpResponse.Content.Headers.ContentLength = imgData.Length;
                httpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                httpResponse.StatusCode = HttpStatusCode.OK;
            }
            catch(Exception ex)
            {

            }

            return httpResponse;
        }

        [HttpPost]
        public long AddPlantImage(AddImageRequest request)
        {
            return inventoryManager.AddPlantImage(request.imgBytes);
        }

        [HttpPost]
        public HttpResponseMessage UploadPlantImage()
        {
            long image_id = 0;

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            StreamContent sc = Request.Content as StreamContent;

            var filesReadToProvider = Request.Content.ReadAsMultipartAsync().Result;

            foreach (var stream in filesReadToProvider.Contents)
            {
                // Getting of content as byte[], picture name and picture type
                byte[] fileBytes = stream.ReadAsByteArrayAsync().Result;
                var pictureName = stream.Headers.ContentDisposition.FileName;
                var contentType = stream.Headers.ContentType.MediaType;

                //save bytes to db
                image_id = inventoryManager.AddPlantImage(fileBytes);
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(Convert.ToString(image_id));

            return response;
        }

        [HttpPost]
        public HttpResponseMessage UploadArrangementImage()
        {
            long image_id = 0;

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            StreamContent sc = Request.Content as StreamContent;

            var filesReadToProvider = Request.Content.ReadAsMultipartAsync().Result;

            foreach (var stream in filesReadToProvider.Contents)
            {
                // Getting of content as byte[], picture name and picture type
                byte[] fileBytes = stream.ReadAsByteArrayAsync().Result;
                var pictureName = stream.Headers.ContentDisposition.FileName;
                var contentType = stream.Headers.ContentType.MediaType;

                //save bytes to db
                image_id = inventoryManager.AddArrangementImage(fileBytes);
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(Convert.ToString(image_id));

            return response;
        }

        /// <summary>
        /// Get all service codes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ServiceCodeDTO> GetAllServiceCodes()
        {
            return inventoryManager.GetAllServiceCodes();
        }

        [HttpGet]
        public ServiceCodeDTO GetServiceCodeById(long serviceCodeId)
        {
            return inventoryManager.GetServiceCodeById(serviceCodeId);
        }

        /// <summary>
        /// Get all service codes by type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public GetServiceCodeResponse GetAllServiceCodesByType([FromUri]ServiceCodeType serviceCodeType)
        {
            return inventoryManager.GetAllServiceCodesByType(serviceCodeType);
        }

        [HttpGet]
        public GetKvpLongStringResponse GetInventoryList()
        {
            return inventoryManager.GetInventoryList();
        }

        [HttpPost]
        public GetLongIdResponse DoesServiceCodeExist(ServiceCodeDTO serviceCodeDTO)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesServiceCodeExist(serviceCodeDTO);
            return response;
        }

        [HttpGet]
        public GetLongIdResponse ServiceCodeIsNotUnique(string serviceCode)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.ServiceCodeIsNotUnique(serviceCode);
            return response;
        }

        /// <summary>
        /// Add a new service code
        /// </summary>
        /// <param name="serviceCodeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse AddServiceCode(ServiceCodeDTO serviceCodeDTO)
        {
            ApiResponse response = new ApiResponse();

            long serviceCodeId = inventoryManager.ServiceCodeIsNotUnique(serviceCodeDTO.ServiceCode);
            if (serviceCodeId > 0)
            {
                response.AddMessage("ServiceCode", new List<string>() { "This service code is in use. Please choose another." });
            }
            else if(inventoryManager.GeneralLedgerIsNotUnique(serviceCodeDTO.GeneralLedger))
            {
                response.AddMessage("GeneralLedger", new List<string>() { "This General Ledger value is in use. Please choose another." });
            }
            else
            {
                serviceCodeId = inventoryManager.AddServiceCode(serviceCodeDTO);
                if ( serviceCodeId == 0)
                {
                    response.AddMessage("DbError", new List<string>() { "There was an error saving this service code." });
                }
            }    

            return response;
        }

        [HttpGet]
        public GetInventoryTypeResponse GetInventoryTypes()
        {
            return new GetInventoryTypeResponse(inventoryManager.GetInventoryTypes());
        }

        /// <summary>
        /// Get All inevntory or get by type
        /// inventoryType 0 = All Inventory 
        /// inventoryType = 1 Plants
        /// inventoryType = 2 Containers
        /// inventoryType = 3 arrangements
        /// </summary>
        /// <param name="inventoryType"></param>
        /// <returns></returns>
        [HttpGet]
        public GetInventoryResponse GetInventory([FromUri]int inventoryType)
        {
            return inventoryManager.GetInventory((InventoryType)inventoryType);
        }

        /// <summary>
        /// Get all Plant Types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public GetPlantTypeResponse GetPlantTypes()
        {
            return inventoryManager.GetPlantTypes();
        }

        [HttpGet]
        public GetPlantNameResponse GetPlantNamesByType([FromUri]long plantTypeId)
        {
            return inventoryManager.GetPlantNamesByType(plantTypeId);
        }

        [HttpGet]
        public GetPlantResponse GetPlantsByType(long plantTypeId)
        {
            return inventoryManager.GetPlantsByType(plantTypeId);
        }

        [HttpGet]
        public GetPlantResponse GetPlants()
        {
            return inventoryManager.GetPlants();
        }

        /// <summary>
        /// Get all Plant Types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public GetMaterialTypeResponse GetMaterialTypes()
        {
            return inventoryManager.GetMaterialTypes();
        }

        [HttpGet]
        public GetMaterialNameResponse GetMaterialNamesByType([FromUri]long materialTypeId)
        {
            return inventoryManager.GetMaterialNamesByType(materialTypeId);
        }

        [HttpGet]
        public GetMaterialResponse GetMaterialsByType(long materialTypeId)
        {
            return inventoryManager.GetMaterialsByType(materialTypeId);
        }

        [HttpGet]
        public GetMaterialResponse GetMaterials()
        {
            return inventoryManager.GetMaterials();
        }

        [HttpGet]
        public GetFoliageTypeResponse GetFoliageTypes()
        {
            return inventoryManager.GetFoliageTypes();
        }

        [HttpGet]
        public GetFoliageNameResponse GetFoliageNamesByType([FromUri]long foliageTypeId)
        {
            return inventoryManager.GetFoliageNamesByType(foliageTypeId);
        }

        [HttpGet]
        public GetFoliageResponse GetFoliageByType(long foliageTypeId)
        {
            return inventoryManager.GetFoliageByType(foliageTypeId);
        }

        [HttpGet]
        public GetFoliageResponse GetFoliage()
        {
            return inventoryManager.GetFoliage();
        }

        [HttpGet]
        public GetContainerTypeResponse GetContainerTypes()
        {
            return inventoryManager.GetContainerTypes();
        }

        [HttpGet]
        public List<ContainerNameDTO> GetContainerNamesByType([FromUri]long containerTypeId)
        {
            return inventoryManager.GetContainerNamesByType(containerTypeId);
        }

        [HttpGet]
        public GetContainerResponse GetContainers()
        {
            return inventoryManager.GetContainers();
        }

        [HttpGet]
        public GetContainerResponse GetContainersByType(long containerTypeId)
        {
            return inventoryManager.GetContainersByType(containerTypeId);
        }

        /// <summary>
        /// Plant Types 
        /// Brassidium=15
        /// Laeliocattleya=16
        /// Cycnodes=17
        /// Dendrobium=24
        /// Laelia=25
        /// Miltassia=27
        /// Odontobrassia=28
        /// Oncidium=30
        /// Paphiopedilum=33
        /// Phalaenopsis=37
        /// Vuylstekeara=38
        /// Zygopetalum=39
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse AddPlantName(AddPlantNameRequest request)
        {
            ApiResponse response = new ApiResponse();

            inventoryManager.AddPlantName(request);

            return response;
        }



        [HttpPost]
        public ApiResponse AddPlantType(AddPlantTypeRequest request)
        {
            ApiResponse response = new ApiResponse();

            inventoryManager.AddPlantType(request);

            return response;
        }

        [HttpGet]
        public GetLongIdResponse DoesPlantNameExist(string plantName)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesPlantNameExist(plantName);
            return response;
        }

        [HttpGet]
        public GetLongIdResponse DoesPlantTypeExist(string plantType)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesPlantTypeExist(plantType);
            return response;
        }

        [HttpPost]
        public GetLongIdResponse DoesPlantExist(PlantDTO plantDTO)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesPlantExist(plantDTO);
            return response;
        }

        [HttpPost]
        public GetLongIdResponse ImportPlant(ImportPlantRequest request)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.ImportPlant(request);
            return response;
        }

        /// <summary>
        /// Add a new plant to inventory
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public GetPlantResponse AddPlant(AddPlantRequest request)
        {
            GetPlantResponse response = new GetPlantResponse();

            if(inventoryManager.InventoryNameIsNotUnique(request.Inventory.InventoryName))
            {
                response.AddMessage("InventoryName", new List<string>() { "This inventory name is in use. Please choose another." });
            }
            else if(inventoryManager.PlantNameIsNotUnique(request.Plant.PlantName))
            {
                response.AddMessage("PlantName", new List<string>() { "This plant name is in use. Please choose another." });
            }
            else
            {
                long plantId = inventoryManager.AddPlant(request);

                if (plantId == 0)
                {
                    response.AddMessage("DbError", new List<string>() { "There was an error saving this plant." });
                }
                else
                {
                    response = inventoryManager.GetPlant(plantId);
                }
            }

            return response;
        }
        


        /// <summary>
        /// Add a new container to inventory
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public GetContainerResponse AddContainer(AddContainerRequest request)
        {
            GetContainerResponse response = new GetContainerResponse();

            if (inventoryManager.InventoryNameIsNotUnique(request.Inventory.InventoryName))
            {
                response.AddMessage("InventoryName", new List<string>() { "This inventory name is in use. Please choose another." });
            }
            else if (inventoryManager.ContainerNameIsNotUnique(request.Container.ContainerName))
            {
                response.AddMessage("ContainerName", new List<string>() { "This container name is in use. Please choose another." });
            }
            else
            {
                long containerId = inventoryManager.AddContainer(request);
                if (containerId == 0)
                {
                    response.AddMessage("DbError", new List<string>() { "There was an error saving this container." });
                }
                else
                {
                    response = inventoryManager.GetContainer(containerId);
                }
            }

            return response;
        }

        /// <summary>
        /// Add a new arrangement to inventory
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse AddArrangement(AddArrangementRequest request)
        {
            ApiResponse response = new ApiResponse();

            if (inventoryManager.InventoryNameIsNotUnique(request.Inventory.InventoryName))
            {
                response.AddMessage("InventoryName", new List<string>() { "This inventory name is in use. Please choose another." });
            }
            else if (inventoryManager.ArrangementNameIsnotUnique(request.Arrangement.ArrangementName))
            {
                response.AddMessage("ArrangementName", new List<string>() { "This arrangement name is in use. Please choose another." });
            }
            else
            {
                long arrangement_id = inventoryManager.AddArrangement(request);
                if (arrangement_id == 0)
                {
                    response.AddMessage("DbError", new List<string>() { "There was an error saving this arrangement." });
                }
            }

            return response;
        }

        [HttpGet]
        public GetArrangementResponse GetArrangements()
        {
            return inventoryManager.GetArrangements();
        }

        [HttpPost]
        public bool DeletePlant([FromBody]long plantId)
        {
            return inventoryManager.DeletePlant(plantId);
        }

        [HttpPost]
        public GetVendorResponse GetVendors(GetPersonRequest request)
        {
            return inventoryManager.GetVendors(request);
        }

        [HttpGet]
        public GetVendorResponse GetVendorById(long vendorId)
        {
            return inventoryManager.GetVendorById(vendorId);
        }

        [HttpPost]
        public GetCustomerResponse AddCustomer(AddCustomerRequest request)
        {
            GetCustomerResponse response = new GetCustomerResponse();

            long newId = inventoryManager.AddCustomer(request);
                        
            return response;
        }

        [HttpPost]
        public GetVendorResponse AddVendor(AddVendorRequest request)
        {
            GetVendorResponse response = new GetVendorResponse();

            long newId = inventoryManager.AddVendor(request);

            if(newId > 0)
            {
                response = GetVendorById(newId);
            }

            return response;
        }

        [HttpPost]
        public ApiResponse AddShipment(AddShipmentRequest request)
        {
            ApiResponse response = new ApiResponse();

            long newId = inventoryManager.AddShipment(request);

            return response;
        }

        [HttpPost]
        public GetShipmentResponse GetShipments(ShipmentFilter filter)
        {
            return inventoryManager.GetShipments(filter);
        }

        [HttpGet]
        public WorkOrderResponse GetWorkOrder([FromUri]long workOrderId)
        {
            return inventoryManager.GetWorkOrder(workOrderId);
        }

        [HttpGet]
        public List<WorkOrderResponse> GetWorkOrders([FromUri]DateTime afterDate)
        {
            return inventoryManager.GetWorkOrders(afterDate);
        }

        [HttpPost]
        public long AddWorkOrder(AddWorkOrderRequest workOrderRequest)
        {
            return inventoryManager.AddWorkOrder(workOrderRequest);
        }

        [HttpPost]
        public WorkOrderResponse GetWorkOrders(WorkOrderListFilter filter)
        {
            return inventoryManager.GetWorkOrders(filter);
        }

        [HttpPost]
        public long DoesPersonExist(PersonDTO person)
        {
            return inventoryManager.DoesPersonExist(person);
        }

        [HttpPost]
        public long ImportPerson(ImportPersonRequest request)
        {
            return inventoryManager.ImportPerson(request);
        }

        [HttpPost]
        public GetPersonResponse GetPerson(GetPersonRequest request)
        {
            return inventoryManager.GetPerson(request);
        }

        [HttpGet]
        public GetLongIdResponse DoesFoliageTypeExist(string foliageType)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesFoliageTypeExist(foliageType);
            return response;
        }

        [HttpGet]
        public GetLongIdResponse DoesFoliageNameExist(string foliageName)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesFoliageNameExist(foliageName);
            return response;
        }

        [HttpPost]
        public GetLongIdResponse DoesFoliageExist(FoliageDTO foliage)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesFoliageExist(foliage);
            return response;
        }

        [HttpPost]
        public GetLongIdResponse ImportFoliage(ImportFoliageRequest request)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId= inventoryManager.ImportFoliage(request);
            return response;
        }

        [HttpGet]
        public GetLongIdResponse DoesMaterialTypeExist(string materialType)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesMaterialTypeExist(materialType);
            return response;
        }

        [HttpGet]
        public GetLongIdResponse  DoesMaterialNameExist(string materialName)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesMaterialNameExist(materialName);
            return response;
        }

        [HttpPost]
        public GetLongIdResponse DoesMaterialExist(MaterialDTO material)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesMaterialExist(material);
            return response;
        }

        [HttpPost]
        public GetLongIdResponse ImportMaterial(ImportMaterialRequest request)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.ImportMaterial(request);
            return response;
        }

        [HttpGet]
        public GetLongIdResponse DoesContainerTypeExist(string containerType)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesContainerTypeExist(containerType);
            return response;
        }

        [HttpGet]
        public GetLongIdResponse DoesContainerNameExist(string containerName)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesContainerNameExist(containerName);
            return response;
        }

        [HttpPost]
        public GetLongIdResponse DoesContainerExist(ContainerDTO container)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.DoesContainerExist(container);
            return response;
        }

        [HttpPost]
        public GetLongIdResponse ImportContainer(ImportContainerRequest request)
        {
            GetLongIdResponse response = new GetLongIdResponse();
            response.returnedId = inventoryManager.ImportContainer(request);
            return response;
        }

        [HttpGet]
        public GetSizeResponse GetSizeByInventoryType(long inventoryTypeId)
        {
            GetSizeResponse response = new GetSizeResponse();
            response.InventoryTypeId = inventoryTypeId;
            response.Sizes = inventoryManager.GetSizeByInventoryType(inventoryTypeId);
            return response;
        }

        [HttpPost]
        public GetWorkOrderSalesDetailResponse GetWorkOrderDetail(GetWorkOrderSalesDetailRequest request)
        {
            return inventoryManager.GetWorkOrderDetail(request);
        }
    }
}
