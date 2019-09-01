using EO.DatabaseContext;
using EO.ViewModels.DataModels;
using Microsoft.EntityFrameworkCore;
using SharedData;
//using SharedData.ListFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ViewModels.ControllerModels;
using ViewModels.DataModels;

namespace EO.Persistence
{
    public class EOPersistence : IEOPersistence
    {
        private eotestContext dbContext;

        public EOPersistence()
        {
            var optionsBuilder = new DbContextOptionsBuilder<eotestContext>();
            optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=jVW@696969;database=eotest");
            dbContext = new eotestContext(optionsBuilder.Options);
        }

        public LoginDTO GetUser(LoginDTO request)
        {
            LoginDTO loginDTO = new LoginDTO();
            try
            {
                User user = dbContext.User.Where(a => a.UserName == request.UserName && a.Password == request.Password).FirstOrDefault();

                if (user != null && user.UserId > 0)
                {
                    loginDTO.UserId = user.UserId;
                    loginDTO.RoleId = user.RoleId;
                }
            }
            catch (Exception ex)
            {
                //implement logging
            }

            return loginDTO;
        }

        public byte[] GetImage(long imageId)
        {
            byte[] imageData = new byte[0];

            Image img = dbContext.Image.Where(a => a.ImageId == imageId).FirstOrDefault();

            if (img != null && img.ImageId != 0)
            {
                imageData = img.ImageData;
            }

            return imageData;
        }

       

        public GetContainerTypeResponse GetContainerTypes()
        {
            GetContainerTypeResponse response = new GetContainerTypeResponse();

            List<ContainerTypeDTO> containerTypes = new List<ContainerTypeDTO>();

            dbContext.ContainerType.ToList().ForEach(item =>
            {
                containerTypes.Add(new ContainerTypeDTO()
                {
                    ContainerTypeId = item.ContainerTypeId,
                    ContainerTypeName = item.ContainerTypeName
                });
            });

            response.ContainerTypeList = containerTypes;

            return response;
        }

        public List<ContainerNameDTO> GetContainerNamesByType(long containerTypeId)
        {
            List<ContainerNameDTO> containerNames = new List<ContainerNameDTO>();

            dbContext.ContainerName.Where(a => a.ContainerTypeId == containerTypeId).ToList().ForEach(item =>
            {
                containerNames.Add(new ContainerNameDTO()
                {
                    ContainerNameId = item.ContainerNameId,
                    ContainerTypeId = item.ContainerTypeId,
                    ContainerName = item.Name
                });
            });

            return containerNames;
        }

        public GetContainerResponse GetContainersByType(long containerTypeId)
        {
            GetContainerResponse response = new GetContainerResponse();

            List<long> containerIds = dbContext.Container.Where(b => b.ContainerTypeId == containerTypeId).Select(a => a.ContainerId).ToList();

            List<InventoryContainerMap> inventoryContainerMap = dbContext.InventoryContainerMap.Where(a => containerIds.Contains(a.ContainerId)).ToList();

            List<long> inventoryIds = inventoryContainerMap.Select(a => a.InventoryId).ToList();

            List<InventoryImageMap> inventoryImageMap = dbContext.InventoryImageMap.Where(a => inventoryIds.Contains(a.InventoryId)).ToList();

            GetContainerTypeResponse containerTypeList = GetContainerTypes();

            List<ServiceCodeDTO> serviceCodeList = GetServiceCodes();

            foreach (InventoryContainerMap map in inventoryContainerMap)
            {
                Inventory i = dbContext.Inventory.Where(a => a.InventoryId == map.InventoryId).First();

                Container c = dbContext.Container.Where(b => b.ContainerId == map.ContainerId).First();

                InventoryDTO iDTO = new InventoryDTO()
                {
                    InventoryId = i.InventoryId,
                    InventoryName = i.InventoryName,
                    InventoryTypeId = i.InventoryTypeId,
                    InventoryTypeName = "Plants", // inventoryTypeList.Where(a => a.InventoryTypeId == i.InventoryTypeId).Select(b => b.InventoryTypeName).First(),
                    ServiceCodeId = i.ServiceCodeId,
                    ServiceCodeName = serviceCodeList.Where(a => a.ServiceCodeId == i.ServiceCodeId).Select(b => b.ServiceCode).First(),
                    NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                    Quantity = i.Quantity
                };

                ContainerDTO cDTO = new ContainerDTO()
                {
                    ContainerId = c.ContainerId,
                    ContainerName = c.ContainerName,
                    ContainerTypeId = c.ContainerTypeId,
                    ContainerTypeName = containerTypeList.ContainerTypeList.Where(a => a.ContainerTypeId == c.ContainerTypeId).Select(b => b.ContainerTypeName).First()
                };

                long imageId = inventoryImageMap.Where(a => a.InventoryId == i.InventoryId).Select(b => b.ImageId).FirstOrDefault();

                response.ContainerInventoryList.Add(new ContainerInventoryDTO(cDTO, iDTO, imageId));
            }
            return response;
        }

        public GetContainerResponse GetContainer(long containerId)
        {
            GetContainerResponse response = new GetContainerResponse();

            Container c = dbContext.Container.Where(a => a.ContainerId == containerId).First();

            ContainerType type = dbContext.ContainerType.Where(a => a.ContainerTypeId == c.ContainerTypeId).First();

            ContainerName name = dbContext.ContainerName.Where(a => a.ContainerTypeId == c.ContainerTypeId).First();

            ContainerDTO cDTO = new ContainerDTO()
            {
                ContainerId = c.ContainerId,
                ContainerTypeId = c.ContainerTypeId,
                ContainerName = name.Name,
                ContainerTypeName = type.ContainerTypeName
            };


            long inventoryId = dbContext.InventoryContainerMap.Where(a => a.ContainerId == containerId).Select(b => b.InventoryId).First();

            Inventory i = dbContext.Inventory.Where(a => a.InventoryId == inventoryId).First();

            ServiceCode sc = dbContext.ServiceCode.Where(a => a.ServiceCodeId == i.ServiceCodeId).First();

            InventoryDTO iDTO = new InventoryDTO()
            {
                InventoryId = i.InventoryId,
                InventoryName = i.InventoryName,
                InventoryTypeId = i.InventoryTypeId,
                NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                Quantity = i.Quantity,
                ServiceCodeId = i.ServiceCodeId,
                ServiceCodeName = sc.ServiceCode1
            };

            response.ContainerInventoryList.Add(new ContainerInventoryDTO(cDTO,iDTO,0));

            return response;
        }

        public GetContainerResponse GetContainers()
        {
            GetContainerResponse containers = new GetContainerResponse();

            List<ContainerType> types = dbContext.ContainerType.ToList();

            List<ContainerName> names = dbContext.ContainerName.ToList();

            List<InventoryImageMap> inventoryImageMap = dbContext.InventoryImageMap.ToList();

            dbContext.Container.ToList().ForEach(item =>
            {
                ContainerDTO cDTO = new ContainerDTO()
                {
                    ContainerId = item.ContainerId,
                    ContainerTypeId = item.ContainerTypeId,
                    ContainerSize = item.ContainerSize,
                    ContainerName = names.Where(a => a.ContainerTypeId == item.ContainerTypeId).Select(b => b.Name).First(),
                    ContainerTypeName = types.Where(a => a.ContainerTypeId == item.ContainerTypeId).Select(b => b.ContainerTypeName).First()
                };

                long inventoryId = dbContext.InventoryContainerMap.Where(a => a.ContainerId == item.ContainerId).Select(b => b.InventoryId).First();

                Inventory i = dbContext.Inventory.Where(a => a.InventoryId == inventoryId).First();

                ServiceCode sc = dbContext.ServiceCode.Where(a => a.ServiceCodeId == i.ServiceCodeId).First();

                InventoryDTO iDTO = new InventoryDTO()
                {
                    InventoryId = i.InventoryId,
                    InventoryName = i.InventoryName,
                    InventoryTypeId = i.InventoryTypeId,
                    NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                    Quantity = i.Quantity,
                    ServiceCodeId = i.ServiceCodeId,
                    ServiceCodeName = sc.ServiceCode1
                };

                long imageId = inventoryImageMap.Where(a => a.InventoryId == i.InventoryId).Select(b => b.ImageId).FirstOrDefault();

                containers.ContainerInventoryList.Add(new ContainerInventoryDTO(cDTO, iDTO,imageId));
            });

            return containers;
        }

        public List<ServiceCodeDTO> GetServiceCodes()
        {
            List<ServiceCodeDTO> serviceCodes = new List<ServiceCodeDTO>();

            dbContext.ServiceCode.ToList().ForEach(item =>
            {
                serviceCodes.Add(new ServiceCodeDTO()
                {
                    ServiceCodeId = item.ServiceCodeId,
                    ServiceCode = item.ServiceCode1,
                    Description = item.Description,
                    Size = item.Size,
                    Price = item.Price,
                    Taxable = item.Taxable,
                    GeneralLedger = item.GeneralLedger
                });
            });

            return serviceCodes;
        }

        public ServiceCodeDTO GetServiceCodeById(long serviceCodeId)
        {
            ServiceCodeDTO dto = new ServiceCodeDTO();

            ServiceCode code = dbContext.ServiceCode.Where(a => a.ServiceCodeId == serviceCodeId).FirstOrDefault();

            if(code.ServiceCodeId > 0)
            {
                dto.Cost = code.Cost;
                dto.Description = code.Description;
                dto.GeneralLedger = code.GeneralLedger;
                dto.Price = code.Price;
                dto.ServiceCode = code.ServiceCode1;
                dto.ServiceCodeId = code.ServiceCodeId;
                dto.Size = code.Size;
                dto.Taxable = code.Taxable;
            }

            return dto;
        }

        public GetServiceCodeResponse GetServiceCodesByType(string serviceCodePrefix)
        {
            GetServiceCodeResponse response = new GetServiceCodeResponse();

            List<ServiceCodeDTO> serviceCodes = new List<ServiceCodeDTO>();

            //dbContext.ServiceCode.Where(a => a.ServiceCode1.StartsWith(serviceCodePrefix)).ToList().ForEach(item =>
            //{
            //    serviceCodes.Add(new ServiceCodeDTO()
            //    {
            //        ServiceCodeId = item.ServiceCodeId,
            //        ServiceCode = item.ServiceCode1,
            //        Description = item.Description,
            //        Size = item.Size,
            //        Price = item.Price,
            //        Taxable = item.Taxable,
            //        GeneralLedger = item.GeneralLedger
            //    });
            //});

            return response;
        }

        public GetKvpLongStringResponse GetInventoryList()
        {
            GetKvpLongStringResponse response = new GetKvpLongStringResponse();

            List<KeyValuePair<long, string>> inventoryList = new List<KeyValuePair<long, string>>();

            dbContext.Inventory.Select(a => new { a.InventoryId, a.InventoryName }).ToList().ForEach(item =>
             {
                 inventoryList.Add(new KeyValuePair<long, string>(item.InventoryId, item.InventoryName));
             });

            response.KvpList = inventoryList;

            return response;
        }

        public long ServiceCodeIsNotUnique(string serviceCode)
        {
            long serviceCodeId = 0;

            ServiceCode sc = dbContext.ServiceCode.Where(a => a.ServiceCode1 == serviceCode).FirstOrDefault();

            if (sc != null && sc.ServiceCodeId > 0)
            {
                serviceCodeId = sc.ServiceCodeId;
            }

            return serviceCodeId;
        }

        public bool GeneralLedgerIsNotUnique(string generalLedger)
        {
            bool notUnique = false;

            ServiceCode sc = dbContext.ServiceCode.Where(a => a.GeneralLedger == generalLedger).FirstOrDefault();

            if (sc != null && sc.ServiceCodeId > 0)
            {
                notUnique = true;
            }

            return notUnique;
        }

        public long InventoryExists(InventoryDTO inventoryDTO)
        {
            long inventoryId = 0;

            Inventory i = dbContext.Inventory.Where(a => a.InventoryName == inventoryDTO.InventoryName &&
                a.InventoryTypeId == inventoryDTO.InventoryTypeId).FirstOrDefault();

            if(i != null && i.InventoryId > 0)
            {
                inventoryId = i.InventoryId;
            }

            return inventoryId;
        }

        public bool InventoryNameIsNotUnique(string inventoryName)
        {
            bool notUnique = false;

            Inventory i = dbContext.Inventory.Where(a => a.InventoryName == inventoryName).FirstOrDefault();

            if (i != null && i.InventoryId > 0)
            {
                notUnique = true;
            }

            return notUnique;
        }

        

        public bool ContainerNameIsnotUnique(string containerName)
        {
            bool notUnique = false;

            Container c = dbContext.Container.Where(a => a.ContainerName == containerName).FirstOrDefault();

            if (c != null && c.ContainerId > 0)
            {
                notUnique = true;
            }

            return notUnique;
        }

        public bool ArrangementNameIsnotUnique(string arrangementName)
        {
            bool notUnique = false;

            Arrangement ar = dbContext.Arrangement.Where(a => a.ArrangementName == arrangementName).FirstOrDefault();

            if (ar != null && ar.ArrangementId > 0)
            {
                notUnique = true;
            }

            return notUnique;
        }

        public long ServiceCodeExists(ServiceCodeDTO serviceCodeDTO)
        {
            long serviceCodeId = 0;

            ServiceCode svcCode = dbContext.ServiceCode
                .Where(a => a.Cost.HasValue && a.Cost == serviceCodeDTO.Cost &&
                a.Price == serviceCodeDTO.Price && a.Taxable == serviceCodeDTO.Taxable &&
                a.ServiceCode1 == serviceCodeDTO.ServiceCode).FirstOrDefault();

            if (svcCode != null && svcCode.ServiceCodeId > 0)
            {
                serviceCodeId = svcCode.ServiceCodeId;
            }

            return serviceCodeId;
        }

        public long AddServiceCode(ServiceCodeDTO serviceCodeDTO)
        {
            long serviceCodeId = 0;

            try
            {
                ServiceCode serviceCode = new ServiceCode();
                serviceCode.Description = serviceCodeDTO.Description;
                serviceCode.GeneralLedger = serviceCodeDTO.GeneralLedger;
                serviceCode.Price = serviceCodeDTO.Price;
                serviceCode.ServiceCode1 = serviceCodeDTO.ServiceCode;
                serviceCode.Size = serviceCodeDTO.Size;
                serviceCode.Taxable = serviceCodeDTO.Taxable;

                dbContext.ServiceCode.Add(serviceCode);
                dbContext.SaveChanges();
                serviceCodeId = serviceCode.ServiceCodeId;
            }
            catch (Exception ex)
            {
                int debug = 0;
            }
            return serviceCodeId;
        }

       

        public long AddPlantName(AddPlantNameRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    PlantName plantName = new PlantName()
                    {
                        Name = request.PlantName,
                        PlantTypeId = request.PlantTypeId
                    };

                    dbContext.PlantName.Add(plantName);
                    dbContext.SaveChanges();
                    scope.Complete();

                    newId = plantName.PlantNameId;
                }
            }
            catch (Exception ex)
            {

            }

            return newId;
        }
               
        public long AddPlantType(AddPlantTypeRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    PlantType plantType = new PlantType()
                    {
                        PlantTypeName = request.PlantTypeName,
                    };

                    dbContext.PlantType.Add(plantType);
                    dbContext.SaveChanges();
                    scope.Complete();

                    newId = plantType.PlantTypeId;
                }
            }
            catch (Exception ex)
            {

            }

            return newId;
        }

        public long PlantExists(PlantDTO plantDTO)
        {
            long plantId = 0;

            Plant p = dbContext.Plant.Where(a => a.PlantName == plantDTO.PlantName && a.PlantTypeId == plantDTO.PlantTypeId && a.PlantSize == plantDTO.PlantSize).FirstOrDefault();

            if (p != null && p.PlantId > 0)
            {
                plantId = p.PlantId;
            }

            return plantId;
        }

        public long PlantNameExists(string plantName)
        {
            long plantNameId = 0;

            PlantName plntName = dbContext.PlantName.Where(a => a.Name == plantName).FirstOrDefault();

            if (plntName != null && plntName.PlantNameId > 0)
            {
                plantNameId = plntName.PlantNameId;
            }

            return plantNameId;
        }

        public long PlantTypeExists(string typeName)
        {
            long plantTypeId = 0;

            PlantType plntType = dbContext.PlantType.Where(a => a.PlantTypeName == typeName).FirstOrDefault();

            if (plntType != null && plntType.PlantTypeId > 0)
            {
                plantTypeId = plntType.PlantTypeId;
            }

            return plantTypeId;
        }

        public bool PlantNameIsNotUnique(string plantName)
        {
            bool notUnique = false;

            Plant p = dbContext.Plant.Where(a => a.PlantName == plantName).FirstOrDefault();

            if (p != null && p.PlantId > 0)
            {
                notUnique = true;
            }

            return notUnique;
        }

        public long AddPlant(AddPlantRequest plantRequest)
        {
           long plant_id = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Plant p = new Plant()
                    {
                        PlantName = plantRequest.Plant.PlantName,
                        PlantTypeId = plantRequest.Plant.PlantTypeId,
                        PlantNameId = plantRequest.Plant.PlantNameId,
                        PlantSize = plantRequest.Plant.PlantSize
                    };

                    dbContext.Plant.Add(p);

                    Inventory i = new Inventory()
                    {
                        InventoryName = plantRequest.Inventory.InventoryName,
                        InventoryTypeId = plantRequest.Inventory.InventoryTypeId,
                        ServiceCodeId = plantRequest.Inventory.ServiceCodeId
                    };

                    dbContext.Inventory.Add(i);

                    InventoryPlantMap invPlantMap = new InventoryPlantMap();
                    invPlantMap.InventoryId = i.InventoryId;
                    invPlantMap.PlantId = p.PlantId;

                    dbContext.InventoryPlantMap.Add(invPlantMap);

                    if (plantRequest.ImageId > 0)
                    {
                        InventoryImageMap iImgMap = new InventoryImageMap();
                        iImgMap.InventoryId = i.InventoryId;
                        iImgMap.ImageId = plantRequest.ImageId;

                        dbContext.InventoryImageMap.Add(iImgMap);
                    }

                    dbContext.SaveChanges();
                    scope.Complete();
                    plant_id = p.PlantId;
                }
            }
            catch (Exception ex)
            {

            }

            return plant_id;
        }

        public long ImportPlant(ImportPlantRequest request)
        {
            long plantId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    long plantTypeId = request.AddPlantRequest.Plant.PlantTypeId;

                    if (!String.IsNullOrEmpty(request.PlantType))
                    {
                        //add plant type
                        if (plantTypeId == 0)
                        {
                            AddPlantTypeRequest plantTypeRequest = new AddPlantTypeRequest();
                            plantTypeRequest.PlantTypeName = request.PlantType;
                            plantTypeId = AddPlantType(plantTypeRequest);
                        }

                        request.AddPlantRequest.Plant.PlantTypeId = plantTypeId;
                    }

                    string plantTypeName = dbContext.PlantType.Where(a => a.PlantTypeId == plantTypeId).Select(b => b.PlantTypeName).First();

                   long plantNameId = request.AddPlantRequest.Plant.PlantNameId;

                    if (!String.IsNullOrEmpty(request.PlantName))
                    {
                        //add plant name
                        if (plantNameId == 0)
                        {
                            AddPlantNameRequest plantNameRequest = new AddPlantNameRequest();
                            plantNameRequest.PlantName = request.PlantName;
                            plantNameRequest.PlantTypeId = plantTypeId;
                            plantNameId = AddPlantName(plantNameRequest);
                        }

                        request.AddPlantRequest.Plant.PlantNameId = plantNameId;
                    }

                    string plantName = dbContext.PlantName.Where(a => a.PlantNameId == plantNameId).Select(b => b.Name).First();

                    long serviceCodeId = request.ServiceCode.ServiceCodeId;
                    string serviceCodeName = request.ServiceCode.ServiceCode;

                    if (serviceCodeId == 0)
                    {
                        serviceCodeId = AddServiceCode(request.ServiceCode);
                    }

                    ServiceCode svcCode = dbContext.ServiceCode.Where(a => a.ServiceCodeId == serviceCodeId).First();

                    serviceCodeName = svcCode.ServiceCode1;
                   

                    request.AddPlantRequest.Plant.PlantName = plantTypeName + "-" + plantName;
                    request.AddPlantRequest.Plant.PlantNameId = plantNameId;
                    request.AddPlantRequest.Plant.PlantTypeId = plantTypeId;
                    request.AddPlantRequest.Plant.PlantTypeName = plantTypeName;
                    request.AddPlantRequest.Plant.PlantSize = request.PlantSize;

                    request.AddPlantRequest.Inventory.InventoryTypeId = (long)Enums.InventoryType.Orchids;
                    request.AddPlantRequest.Inventory.InventoryTypeName = "Orchids";
                    request.AddPlantRequest.Inventory.InventoryName = plantTypeName + "-" + plantName;
                    request.AddPlantRequest.Inventory.ServiceCodeId = serviceCodeId;
                    request.AddPlantRequest.Inventory.ServiceCodeName = serviceCodeName;

                    plantId = PlantExists(request.AddPlantRequest.Plant);

                    long inventoryId = 0;
                    if(plantId > 0)
                    {
                        inventoryId = dbContext.InventoryPlantMap.Where(a => a.PlantId == plantId).Select(b => b.InventoryId).First();
                    }

                    long imageId = 0;

                    if (inventoryId > 0)
                    {
                        request.AddPlantRequest.Inventory.InventoryId = inventoryId;

                        InventoryPlantMap ipm = dbContext.InventoryPlantMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if(ipm.InventoryPlantMapId != 0)
                        {
                            request.AddPlantRequest.Plant.PlantId = ipm.PlantId;
                        }

                        InventoryImageMap iim = dbContext.InventoryImageMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if(iim != null && iim.InventoryImageMapId > 0)
                        {
                            if(iim.ImageId == 0)
                            {
                                if (request.imageBytes != null && request.imageBytes.Length > 0)
                                {
                                    //add image
                                    imageId = AddPlantImage(request.imageBytes);
                                }

                                request.AddPlantRequest.ImageId = imageId;
                            }
                        }
                    }
                    else
                    {
                        if (request.imageBytes != null && request.imageBytes.Length > 0)
                        {
                            //add image
                            imageId = AddPlantImage(request.imageBytes);
                        }

                        request.AddPlantRequest.ImageId = imageId;
                    }

                    if (plantId == 0)
                    {
                        plantId = AddPlant(request.AddPlantRequest);
                    }
                    else
                    {
                        UpdatePlant(request);
                    }

                    scope.Complete();
                }
            }
            catch(Exception ex)
            {
                int debug = 1;
            }

            return plantId;
        }

        /// <summary>
        /// At this point, not that much can be updated - inventory cost, retail, image - plant - size
        /// </summary>
        /// <param name="plantRequest"></param>
        /// <returns></returns>
        public long UpdatePlant(ImportPlantRequest plantRequest)
        {
           long plantId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if(plantRequest.AddPlantRequest.Plant.PlantId != 0)
                   {
                        Plant p = dbContext.Plant.Where(a => a.PlantId == plantRequest.AddPlantRequest.Plant.PlantId).FirstOrDefault();

                        if(p.PlantId > 0)
                        {
                            p.PlantSize = plantRequest.AddPlantRequest.Plant.PlantSize;
                        }
                    }

                    if(plantRequest.AddPlantRequest.Inventory.InventoryId > 0)
                    {
                        ServiceCode code = dbContext.ServiceCode.Where(a => a.ServiceCodeId == plantRequest.AddPlantRequest.Inventory.ServiceCodeId).FirstOrDefault();

                        if(code.ServiceCodeId > 0)
                        {
                            code.Cost = plantRequest.ServiceCode.Cost;
                            code.Description = plantRequest.ServiceCode.Description;
                            code.GeneralLedger = plantRequest.ServiceCode.GeneralLedger;
                            code.Price = plantRequest.ServiceCode.Price;
                            code.Taxable = plantRequest.ServiceCode.Taxable;
                        }
                    }

                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

            }

            return plantId;
        }

        /// <summary>
        /// What if this plant is part of an arrangement?
        /// </summary>
        /// <param name="plantId"></param>
        /// <returns></returns>
        public bool DeletePlant(long plantId)
        {
            bool result = false;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    InventoryPlantMap ipm = dbContext.InventoryPlantMap.Where(a => a.PlantId == plantId).FirstOrDefault();

                    ArrangementInventoryInventoryMap aiim = dbContext.ArrangementInventoryInventoryMap.Where(a => a.InventoryId == ipm.InventoryId).FirstOrDefault();

                    //What if this plant is part of an arrangement? - you can't delete it
                    if (aiim.ArrangementId == 0)
                    {
                        Plant p = dbContext.Plant.Where(a => a.PlantId == plantId).FirstOrDefault();

                        if (p != null && p.PlantId != 0)
                        {
                            dbContext.Remove(p);

                        }

                        long inventoryId = 0;
                        if (ipm != null && ipm.PlantId != 0)
                        {
                            inventoryId = ipm.InventoryId;
                            dbContext.Remove(ipm);
                        }

                        InventoryImageMap iim = dbContext.InventoryImageMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if (iim != null && iim.InventoryId != 0)
                        {
                            dbContext.Remove(iim);
                        }

                        WorkOrderInventoryMap woim = dbContext.WorkOrderInventoryMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if (woim != null && woim.WorkOrderInventoryMapId > 0)
                        {
                            dbContext.Remove(woim);
                        }

                        Inventory i = dbContext.Inventory.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if (i != null && i.InventoryId != 0)
                        {
                            dbContext.Remove(i);
                        }

                        dbContext.SaveChanges();
                        scope.Complete();
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return result;
        }

        /// <summary>
        /// What if this container is part of an arrangement?
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public bool DeleteContainer(long containerId)
        {
            bool result = false;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Container c = dbContext.Container.Where(a => a.ContainerId == containerId).FirstOrDefault();

                    if (c != null && c.ContainerId != 0)
                    {
                        dbContext.Remove(c);

                    }

                    InventoryContainerMap icm = dbContext.InventoryContainerMap.Where(a => a.ContainerId == containerId).FirstOrDefault();

                   long inventoryId = 0;
                    if (icm != null && icm.ContainerId != 0)
                    {
                        inventoryId = icm.InventoryId;
                        dbContext.Remove(icm);
                    }

                    InventoryImageMap iim = dbContext.InventoryImageMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                    if (iim != null && iim.InventoryId != 0)
                    {
                        dbContext.Remove(iim);
                    }

                    Inventory i = dbContext.Inventory.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                    if (i != null && i.InventoryId != 0)
                    {
                        dbContext.Remove(i);
                    }

                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public long AddContainer(AddContainerRequest containerRequest)
        {
            bool success = false;
            long container_id = 0;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Container c = new Container()
                    {
                        ContainerName = containerRequest.Container.ContainerName,
                        ContainerTypeId = containerRequest.Container.ContainerTypeId,
                        ContainerTypeName = containerRequest.Container.ContainerTypeName,
                        ContainerSize = containerRequest.Container.ContainerSize
                    };

                    dbContext.Container.Add(c);

                    Inventory i = new Inventory()
                    {
                        InventoryName = containerRequest.Inventory.InventoryName,
                        InventoryTypeId = containerRequest.Inventory.InventoryTypeId,
                        ServiceCodeId = containerRequest.Inventory.ServiceCodeId
                    };

                    dbContext.Inventory.Add(i);

                    InventoryContainerMap invContainerMap = new InventoryContainerMap();
                    invContainerMap.InventoryId = i.InventoryId;
                    invContainerMap.ContainerId = c.ContainerId;

                    dbContext.InventoryContainerMap.Add(invContainerMap);

                    if (containerRequest.ImageId > 0)
                    {
                        ContainerImageMap cImgMap = new ContainerImageMap();
                        cImgMap.ContainerId = c.ContainerId;
                        cImgMap.ImageId = containerRequest.ImageId;

                        dbContext.ContainerImageMap.Add(cImgMap);
                    }

                    if (containerRequest.ImageId > 0)
                    {
                        InventoryImageMap iImgMap = new InventoryImageMap();
                        iImgMap.InventoryId = i.InventoryId;
                        iImgMap.ImageId = containerRequest.ImageId;

                        dbContext.InventoryImageMap.Add(iImgMap);
                    }

                    dbContext.SaveChanges();
                    scope.Complete();
                    container_id = c.ContainerId;
                }
                catch (Exception ex)
                {
                    int debug = 1;
                }
            }

            return container_id;
        }

        public long AddArrangement(AddArrangementRequest arrangementRequest)
        {
            bool success = false;
            long arrangement_id = 0;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    List<long> svcCodeIds = dbContext.Inventory.Where(b => arrangementRequest.InventoryIds.Contains(b.InventoryId)).Select(b => b.ServiceCodeId).ToList();

                    List<ServiceCode> svcCodes = dbContext.ServiceCode.Where(b => svcCodeIds.Contains(b.ServiceCodeId)).ToList();

                    ServiceCode newSvcCode = new ServiceCode();
                    newSvcCode.Taxable = true;
                    newSvcCode.Price = svcCodes.Sum(b => b.Price);
                    newSvcCode.Cost = svcCodes.Sum(b => b.Cost);
                    newSvcCode.ServiceCode1 = arrangementRequest.Arrangement.ArrangementName;

                    dbContext.ServiceCode.Add(newSvcCode);
                    dbContext.SaveChanges();

                    Arrangement a = new Arrangement()
                    {
                        ArrangementName = arrangementRequest.Arrangement.ArrangementName,
                        ServiceCodeId = newSvcCode.ServiceCodeId
                    };

                    dbContext.Arrangement.Add(a);

                    Inventory i = new Inventory()
                    {
                        InventoryName = arrangementRequest.Inventory.InventoryName,
                        InventoryTypeId = arrangementRequest.Inventory.InventoryTypeId,
                        ServiceCodeId = newSvcCode.ServiceCodeId
                    };

                    dbContext.Inventory.Add(i);

                    ArrangementInventoryMap invArrangementMap = new ArrangementInventoryMap();
                    invArrangementMap.InventoryId = i.InventoryId;
                    invArrangementMap.ArrangementId = a.ArrangementId;

                    dbContext.ArrangementInventoryMap.Add(invArrangementMap);

                    if (arrangementRequest.ImageId > 0)
                    {
                        ArrangementImageMap aImgMap = new ArrangementImageMap();
                        aImgMap.ArrangmentId = a.ArrangementId;
                        aImgMap.ImageId = arrangementRequest.ImageId;

                        dbContext.ArrangementImageMap.Add(aImgMap);
                    }

                    foreach(long inventoryId in arrangementRequest.InventoryIds)
                    {
                        dbContext.ArrangementInventoryInventoryMap.Add(new ArrangementInventoryInventoryMap()
                        {
                            ArrangementId = a.ArrangementId,
                            InventoryId = inventoryId
                        });
                    }

                    dbContext.SaveChanges();
                    scope.Complete();
                    arrangement_id = a.ArrangementId;
                }
                catch (Exception ex)
                {

                }
            }

            return arrangement_id;
        }

        public List<InventoryTypeDTO> GetInventoryTypes()
        {
            List<InventoryTypeDTO> iDTO = new List<InventoryTypeDTO>();

            dbContext.InventoryType.ToList().ForEach(item =>
            {
                iDTO.Add(new InventoryTypeDTO()
                {
                    InventoryTypeId = item.InventoryTypeId,
                    InventoryTypeName = item.InventoryTypeName
                });
            });
            return iDTO;
        }

        public GetInventoryResponse GetInventory(Enums.InventoryType inventoryType)
        {
            GetInventoryResponse response = new GetInventoryResponse();

            List<InventoryDTO> inventoryList = new List<InventoryDTO>();

            if(inventoryType == Enums.InventoryType.AllInventoryTypes)
            {
                dbContext.Inventory.ToList().ForEach(item => 
                {
                    inventoryList.Add(new InventoryDTO()
                    {
                        InventoryId = item.InventoryId,
                        InventoryName = item.InventoryName,
                        InventoryTypeId = item.InventoryTypeId,
                        NotifyWhenLowAmount = item.NotifyWhenLowAmount,
                        Quantity = item.Quantity,
                        ServiceCodeId = item.ServiceCodeId
                    });
                });
            }
            else
            {
               long invType = (long)inventoryType;
                dbContext.Inventory.Where(a => a.InventoryTypeId == invType).ToList().ForEach(item =>
                {
                    inventoryList.Add(new InventoryDTO()
                    {
                        InventoryId = item.InventoryId,
                        InventoryName = item.InventoryName,
                        InventoryTypeId = item.InventoryTypeId,
                        NotifyWhenLowAmount = item.NotifyWhenLowAmount,
                        Quantity = item.Quantity,
                        ServiceCodeId = item.ServiceCodeId
                    });
                });
            }

            response.InventoryList = inventoryList;

            return response;
        }

        public WorkOrderResponse GetWorkOrder(long workOrderId)
        {
            WorkOrderResponse workOrderResponse = new WorkOrderResponse();

            return workOrderResponse;
        }

        public List<WorkOrderResponse> GetWorkOrders(DateTime afterDate)
        {
            List<WorkOrderResponse> response = new List<WorkOrderResponse>();

            return response;
        }

        public WorkOrderResponse GetWorkOrders(WorkOrderListFilter filter)
        {
            WorkOrderResponse workOrderList = new WorkOrderResponse();

            if(filter.FromDate != null && filter.FromDate > DateTime.MinValue && filter.ToDate != null && filter.ToDate > DateTime.MinValue)
            {
                dbContext.WorkOrder.Where(a => a.CreateDate >= filter.FromDate && a.CreateDate <= filter.ToDate).ToList().ForEach(wo =>
                {
                    WorkOrderDTO w = new WorkOrderDTO()
                    {
                        Buyer = wo.PersonReceiver,
                        Seller = wo.PersonInitiator,
                        ClosedDate = wo.ClosedDate.HasValue ? wo.ClosedDate.Value : DateTime.MinValue,
                        Comments = wo.Comments,
                        CreateDate = wo.CreateDate.HasValue ? wo.CreateDate.Value : DateTime.MinValue,
                        DeliveryDate = wo.DeliveryDate.HasValue ? wo.DeliveryDate.Value : DateTime.MinValue,
                        //Paid = wo.Paid,
                        UpdateDate = wo.UpdateDate.HasValue ? wo.UpdateDate.Value : DateTime.MinValue,
                        WorkOrderId = wo.WorkOrderId
                    };

                    List<WorkOrderInventoryMapDTO> inventoryList = new List<WorkOrderInventoryMapDTO>();

                    dbContext.WorkOrderInventoryMap.Where(a => a.WorkOrderId == wo.WorkOrderId).ToList().ForEach(map =>
                    {
                        { 
                            inventoryList.Add(new WorkOrderInventoryMapDTO()
                            {
                                WorkOrderInventoryMapId = map.WorkOrderInventoryMapId,
                                WorkOrderId = map.WorkOrderId,
                                InventoryId = map.InventoryId,
                                //InventoryName = map.Inventory.InventoryName,
                                Quantity = map.Quantity
                            });
                        }
                    });

                    WorkOrderInventoryDTO r = new WorkOrderInventoryDTO()
                    {
                        WorkOrder = w,
                        InventoryList = inventoryList
                    };

                    workOrderList.WorkOrderList.Add(r);
                });
            }  
            
            return workOrderList;
        }

        public GetPlantResponse GetPlant(long plantId)
        {
            GetPlantResponse response = new GetPlantResponse();

            Plant p = dbContext.Plant.Where(b => b.PlantId == plantId).FirstOrDefault();

            if(p != null && p.PlantId != 0)
            {
                long inventoryId = dbContext.InventoryPlantMap.Where(a => a.PlantId == p.PlantId).Select(b => b.InventoryId).First();

                Inventory i = dbContext.Inventory.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                long imageId = dbContext.InventoryImageMap.Where(a => a.InventoryId == inventoryId).Select(b => b.ImageId).FirstOrDefault();

                PlantType plantType = dbContext.PlantType.Where(a => a.PlantTypeId == p.PlantTypeId).First();

                InventoryType inventoryType = dbContext.InventoryType.Where(a => a.InventoryTypeId == i.InventoryTypeId).First();

                ServiceCode serviceCode = dbContext.ServiceCode.Where(a => a.ServiceCodeId == i.ServiceCodeId).First();
                
                PlantDTO pDTO = new PlantDTO();
                pDTO.PlantId = p.PlantId;
                pDTO.PlantName = p.PlantName;
                pDTO.PlantTypeId = p.PlantTypeId;
                pDTO.PlantTypeName = plantType.PlantTypeName;

                InventoryDTO iDTO = new InventoryDTO();
                iDTO.InventoryId = i.InventoryId;
                iDTO.InventoryName = i.InventoryName;
                iDTO.InventoryTypeId = i.InventoryTypeId;
                iDTO.InventoryTypeName = inventoryType.InventoryTypeName;
                iDTO.NotifyWhenLowAmount = i.NotifyWhenLowAmount;
                iDTO.Quantity = i.Quantity;
                iDTO.ServiceCodeId = i.ServiceCodeId;
                iDTO.ServiceCodeName = serviceCode.ServiceCode1;

                response.PlantInventoryList.Add(new PlantInventoryDTO(pDTO, iDTO, imageId));
            }

            return response;
        }

        public GetPlantTypeResponse GetPlantTypes()
        {
            GetPlantTypeResponse response = new GetPlantTypeResponse(new List<PlantTypeDTO>());

            dbContext.PlantType.ToList().ForEach(item =>
            {
                response.PlantTypes.Add(new PlantTypeDTO()
                {
                    PlantTypeId = item.PlantTypeId,
                    PlantTypeName = item.PlantTypeName
                });
            });

            return response;
        }

        public GetPlantNameResponse GetPlantNamesByType(long plantTypeId)
        {
            GetPlantNameResponse response = new GetPlantNameResponse();

            List<PlantNameDTO> plantNames = new List<PlantNameDTO>();

            dbContext.PlantName.Where(a => a.PlantTypeId == plantTypeId).ToList().ForEach(item =>
            {
                plantNames.Add(new PlantNameDTO()
                {
                    PlantNameId = item.PlantNameId,
                    PlantTypeId = item.PlantTypeId,
                    PlantName = item.Name
                });
            });

            response.PlantNames = plantNames;

            return response;
        }
        public GetPlantResponse GetPlantsByType(long plantTypeId)
        {
            GetPlantResponse response = new GetPlantResponse();

            List<long> plantIds = dbContext.Plant.Where(b => b.PlantTypeId == plantTypeId).Select(a => a.PlantId).ToList();

            List<InventoryPlantMap> inventoryPlantMap = dbContext.InventoryPlantMap.Where(a => plantIds.Contains(a.PlantId)).ToList();

            List<long> inventoryIds = inventoryPlantMap.Select(a => a.InventoryId).ToList();

            List<InventoryImageMap> inventoryImageMap = dbContext.InventoryImageMap.Where(a => inventoryIds.Contains(a.InventoryId)).ToList();

            GetPlantTypeResponse plantTypeList = GetPlantTypes();

            List<ServiceCodeDTO> serviceCodeList = GetServiceCodes();

            foreach (InventoryPlantMap map in inventoryPlantMap)
            {
                Inventory i = dbContext.Inventory.Where(a => a.InventoryId == map.InventoryId).First();

                Plant p = dbContext.Plant.Where(b => b.PlantId == map.PlantId).First();

                InventoryDTO iDTO = new InventoryDTO()
                {
                    InventoryId = i.InventoryId,
                    InventoryName = i.InventoryName,
                    InventoryTypeId = i.InventoryTypeId,
                    InventoryTypeName = "Plants", // inventoryTypeList.Where(a => a.InventoryTypeId == i.InventoryTypeId).Select(b => b.InventoryTypeName).First(),
                    ServiceCodeId = i.ServiceCodeId,
                    ServiceCodeName = serviceCodeList.Where(a => a.ServiceCodeId == i.ServiceCodeId).Select(b => b.ServiceCode).First(),
                    NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                    Quantity = i.Quantity
                };

                PlantDTO pDTO = new PlantDTO()
                {
                    PlantId = p.PlantId,
                    PlantName = p.PlantName,
                    PlantTypeId = p.PlantTypeId,
                    PlantTypeName = plantTypeList.PlantTypes.Where(a => a.PlantTypeId == p.PlantTypeId).Select(b => b.PlantTypeName).First()
                };

                //plantResponse.Plant = pDTO;
                //plantResponse.Inventory = iDTO;
                long imageId = inventoryImageMap.Where(a => a.InventoryId == i.InventoryId).Select(b => b.ImageId).FirstOrDefault();

                response.PlantInventoryList.Add(new PlantInventoryDTO(pDTO,iDTO,imageId));
            }
            return response;
        }

        public GetPlantResponse GetPlants()
        {
            GetPlantResponse response = new GetPlantResponse();

            GetPlantTypeResponse plantTypeList = GetPlantTypes();

            List<ServiceCodeDTO> serviceCodeList = GetServiceCodes();

            List<InventoryType> inventoryTypeList = dbContext.InventoryType.ToList();
            
            List<InventoryPlantMap> inventoryPlantMap = dbContext.InventoryPlantMap.ToList();

            List<InventoryImageMap> inventoryImageMap = dbContext.InventoryImageMap.ToList();

            foreach(InventoryPlantMap map in inventoryPlantMap)
            {
                //GetPlantResponse plantResponse = new GetPlantResponse();

                Inventory i = dbContext.Inventory.Where(a => a.InventoryId == map.InventoryId).First();

                Plant p = dbContext.Plant.Where(b => b.PlantId == map.PlantId).First();

                InventoryDTO iDTO = new InventoryDTO()
                {
                    InventoryId = i.InventoryId,
                    InventoryName = i.InventoryName,
                    InventoryTypeId = i.InventoryTypeId,
                    InventoryTypeName = inventoryTypeList.Where(a => a.InventoryTypeId == i.InventoryTypeId).Select(b => b.InventoryTypeName).First(),
                    ServiceCodeId = i.ServiceCodeId,
                    ServiceCodeName = serviceCodeList.Where(a => a.ServiceCodeId == i.ServiceCodeId).Select(b => b.ServiceCode).First(),
                    NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                    Quantity = i.Quantity
                };

                PlantDTO pDTO = new PlantDTO()
                {
                    PlantId = p.PlantId,
                    PlantName = p.PlantName,
                    PlantTypeId = p.PlantTypeId,
                    PlantSize = p.PlantSize,
                    PlantTypeName = plantTypeList.PlantTypes.Where(a => a.PlantTypeId == p.PlantTypeId).Select(b => b.PlantTypeName).First()
                };

                //plantResponse.Plant = pDTO;
                //plantResponse.Inventory = iDTO;
                long imageId = inventoryImageMap.Where(a => a.InventoryId == i.InventoryId).Select(b => b.ImageId).FirstOrDefault();

                response.PlantInventoryList.Add(new PlantInventoryDTO(pDTO,iDTO,imageId));
            }

            return response;
        }

        public GetMaterialTypeResponse GetMaterialTypes()
        {
            GetMaterialTypeResponse response = new GetMaterialTypeResponse(new List<MaterialTypeDTO>());

            dbContext.MaterialType.ToList().ForEach(item =>
            {
                response.MaterialTypes.Add(new MaterialTypeDTO()
                {
                    MaterialTypeId = item.MaterialTypeId,
                    MaterialTypeName = item.MaterialTypeName
                });
            });

            return response;
        }

        public GetMaterialNameResponse GetMaterialNamesByType(long materialTypeId)
        {
            GetMaterialNameResponse response = new GetMaterialNameResponse();

            List<MaterialNameDTO> materialNames = new List<MaterialNameDTO>();

            dbContext.MaterialName.Where(a => a.MaterialTypeId == materialTypeId).ToList().ForEach(item =>
            {
                materialNames.Add(new MaterialNameDTO()
                {
                    MaterialNameId = item.MaterialNameId,
                    MaterialTypeId = item.MaterialTypeId,
                    MaterialName = item.Name
                });
            });

            response.MaterialNames = materialNames;

            return response;
        }
        public GetMaterialResponse GetMaterialsByType(long materialTypeId)
        {
            GetMaterialResponse response = new GetMaterialResponse();

            List<long> materialIds = dbContext.Material.Where(b => b.MaterialTypeId == materialTypeId).Select(a => a.MaterialId).ToList();

            List<InventoryMaterialMap> inventoryMaterialMap = dbContext.InventoryMaterialMap.Where(a => materialIds.Contains(a.MaterialId)).ToList();

            List<long> inventoryIds = inventoryMaterialMap.Select(a => a.InventoryId).ToList();

            List<InventoryImageMap> inventoryImageMap = dbContext.InventoryImageMap.Where(a => inventoryIds.Contains(a.InventoryId)).ToList();

            GetMaterialTypeResponse materialTypeList = GetMaterialTypes();

            List<ServiceCodeDTO> serviceCodeList = GetServiceCodes();

            foreach (InventoryMaterialMap map in inventoryMaterialMap)
            {
                Inventory i = dbContext.Inventory.Where(a => a.InventoryId == map.InventoryId).First();

                Material m = dbContext.Material.Where(b => b.MaterialId == map.MaterialId).First();

                InventoryDTO iDTO = new InventoryDTO()
                {
                    InventoryId = i.InventoryId,
                    InventoryName = i.InventoryName,
                    InventoryTypeId = i.InventoryTypeId,
                    InventoryTypeName = "Plants", // inventoryTypeList.Where(a => a.InventoryTypeId == i.InventoryTypeId).Select(b => b.InventoryTypeName).First(),
                    ServiceCodeId = i.ServiceCodeId,
                    ServiceCodeName = serviceCodeList.Where(a => a.ServiceCodeId == i.ServiceCodeId).Select(b => b.ServiceCode).First(),
                    NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                    Quantity = i.Quantity
                };

                MaterialDTO mDTO = new MaterialDTO()
                {
                    MaterialId = m.MaterialId,
                    MaterialName = m.MaterialName,
                    MaterialTypeId = m.MaterialTypeId,
                    MaterialTypeName = materialTypeList.MaterialTypes.Where(a => a.MaterialTypeId == m.MaterialTypeId).Select(b => b.MaterialTypeName).First()
                };

                //plantResponse.Plant = pDTO;
                //plantResponse.Inventory = iDTO;
                long imageId = inventoryImageMap.Where(a => a.InventoryId == i.InventoryId).Select(b => b.ImageId).FirstOrDefault();

                response.MaterialInventoryList.Add(new MaterialInventoryDTO(mDTO, iDTO, imageId));
            }
            return response;
        }
        public GetMaterialResponse GetMaterials()
        {
            GetMaterialResponse response = new GetMaterialResponse();

            GetMaterialTypeResponse materialTypeList = GetMaterialTypes();

            List<ServiceCodeDTO> serviceCodeList = GetServiceCodes();

            List<InventoryType> inventoryTypeList = dbContext.InventoryType.ToList();

            List<InventoryMaterialMap> inventoryMaterialMap = dbContext.InventoryMaterialMap.ToList();

            List<InventoryImageMap> inventoryImageMap = dbContext.InventoryImageMap.ToList();

            foreach (InventoryMaterialMap map in inventoryMaterialMap)
            {
                //GetPlantResponse plantResponse = new GetPlantResponse();

                Inventory i = dbContext.Inventory.Where(a => a.InventoryId == map.InventoryId).First();

                Material m = dbContext.Material.Where(b => b.MaterialId == map.MaterialId).First();

                InventoryDTO iDTO = new InventoryDTO()
                {
                    InventoryId = i.InventoryId,
                    InventoryName = i.InventoryName,
                    InventoryTypeId = i.InventoryTypeId,
                    InventoryTypeName = inventoryTypeList.Where(a => a.InventoryTypeId == i.InventoryTypeId).Select(b => b.InventoryTypeName).First(),
                    ServiceCodeId = i.ServiceCodeId,
                    ServiceCodeName = serviceCodeList.Where(a => a.ServiceCodeId == i.ServiceCodeId).Select(b => b.ServiceCode).First(),
                    NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                    Quantity = i.Quantity
                };

                MaterialDTO mDTO = new MaterialDTO()
                {
                    MaterialId = m.MaterialId,
                    MaterialName = m.MaterialName,
                    MaterialSize = m.MaterialSize,
                    MaterialTypeId = m.MaterialTypeId,
                    MaterialTypeName = materialTypeList.MaterialTypes.Where(a => a.MaterialTypeId == m.MaterialTypeId).Select(b => b.MaterialTypeName).First()
                };

                //plantResponse.Plant = pDTO;
                //plantResponse.Inventory = iDTO;
                long imageId = inventoryImageMap.Where(a => a.InventoryId == i.InventoryId).Select(b => b.ImageId).FirstOrDefault();

                response.MaterialInventoryList.Add(new MaterialInventoryDTO(mDTO, iDTO, imageId));
            }

            return response;
        }

        public GetFoliageTypeResponse GetFoliageTypes()
        {
            GetFoliageTypeResponse response = new GetFoliageTypeResponse(new List<FoliageTypeDTO>());

            dbContext.FoliageType.ToList().ForEach(item =>
            {
                response.FoliageTypes.Add(new FoliageTypeDTO()
                {
                    FoliageTypeId = item.FoliageTypeId,
                    FoliageTypeName = item.FoliageTypeName
                });
            });

            return response;
        }

        public GetFoliageNameResponse GetFoliageNamesByType(long foliageTypeId)
        {
            GetFoliageNameResponse response = new GetFoliageNameResponse();

            List<FoliageNameDTO> foliageNames = new List<FoliageNameDTO>();

            dbContext.FoliageName.Where(a => a.FoliageTypeId == foliageTypeId).ToList().ForEach(item =>
            {
                foliageNames.Add(new FoliageNameDTO()
                {
                    FoliageNameId = item.FoliageNameId,
                    FoliageTypeId = item.FoliageTypeId,
                    FoliageName = item.Name
                });
            });

            response.FoliageNames = foliageNames;

            return response;
        }
        public GetFoliageResponse GetFoliageByType(long foliageTypeId)
        {
            GetFoliageResponse response = new GetFoliageResponse();

            List<long> foliageIds = dbContext.Foliage.Where(b => b.FoliageTypeId == foliageTypeId).Select(a => a.FoliageId).ToList();

            List<InventoryFoliageMap> inventoryFoliageMap = dbContext.InventoryFoliageMap.Where(a => foliageIds.Contains(a.FoliageId)).ToList();

            List<long> inventoryIds = inventoryFoliageMap.Select(a => a.InventoryId).ToList();

            List<InventoryImageMap> inventoryImageMap = dbContext.InventoryImageMap.Where(a => inventoryIds.Contains(a.InventoryId)).ToList();

            GetFoliageTypeResponse foliageTypeList = GetFoliageTypes();

            List<ServiceCodeDTO> serviceCodeList = GetServiceCodes();

            foreach (InventoryFoliageMap map in inventoryFoliageMap)
            {
                Inventory i = dbContext.Inventory.Where(a => a.InventoryId == map.InventoryId).First();

                Foliage f = dbContext.Foliage.Where(b => b.FoliageId == map.FoliageId).First();

                InventoryDTO iDTO = new InventoryDTO()
                {
                    InventoryId = i.InventoryId,
                    InventoryName = i.InventoryName,
                    InventoryTypeId = i.InventoryTypeId,
                    InventoryTypeName = "Foliage", // inventoryTypeList.Where(a => a.InventoryTypeId == i.InventoryTypeId).Select(b => b.InventoryTypeName).First(),
                    ServiceCodeId = i.ServiceCodeId,
                    ServiceCodeName = serviceCodeList.Where(a => a.ServiceCodeId == i.ServiceCodeId).Select(b => b.ServiceCode).First(),
                    NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                    Quantity = i.Quantity
                };

                FoliageDTO fDTO = new FoliageDTO()
                {
                    FoliageId = f.FoliageId,
                    FoliageName = f.FoliageName,
                    FoliageSize = f.FoliageSize,
                    FoliageTypeId = f.FoliageTypeId,
                    FoliageTypeName = foliageTypeList.FoliageTypes.Where(a => a.FoliageTypeId == f.FoliageTypeId).Select(b => b.FoliageTypeName).First()
                };

                //plantResponse.Plant = pDTO;
                //plantResponse.Inventory = iDTO;
                long imageId = inventoryImageMap.Where(a => a.InventoryId == i.InventoryId).Select(b => b.ImageId).FirstOrDefault();

                response.FoliageInventoryList.Add(new FoliageInventoryDTO(fDTO, iDTO, imageId));
            }
            return response;
        }
        public GetFoliageResponse GetFoliage()
        {
            GetFoliageResponse response = new GetFoliageResponse();

            GetFoliageTypeResponse foliageTypeList = GetFoliageTypes();

            List<ServiceCodeDTO> serviceCodeList = GetServiceCodes();

            List<InventoryType> inventoryTypeList = dbContext.InventoryType.ToList();

            List<InventoryFoliageMap> inventoryFoliageMap = dbContext.InventoryFoliageMap.ToList();

            List<InventoryImageMap> inventoryImageMap = dbContext.InventoryImageMap.ToList();

            foreach (InventoryFoliageMap map in inventoryFoliageMap)
            {
                //GetPlantResponse plantResponse = new GetPlantResponse();

                Inventory i = dbContext.Inventory.Where(a => a.InventoryId == map.InventoryId).First();

                Foliage f = dbContext.Foliage.Where(b => b.FoliageId == map.FoliageId).First();

                InventoryDTO iDTO = new InventoryDTO()
                {
                    InventoryId = i.InventoryId,
                    InventoryName = i.InventoryName,
                    InventoryTypeId = i.InventoryTypeId,
                    InventoryTypeName = inventoryTypeList.Where(a => a.InventoryTypeId == i.InventoryTypeId).Select(b => b.InventoryTypeName).First(),
                    ServiceCodeId = i.ServiceCodeId,
                    ServiceCodeName = serviceCodeList.Where(a => a.ServiceCodeId == i.ServiceCodeId).Select(b => b.ServiceCode).First(),
                    NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                    Quantity = i.Quantity
                };

                FoliageDTO fDTO = new FoliageDTO()
                {
                    FoliageId = f.FoliageId,
                    FoliageName = f.FoliageName,
                    FoliageSize = f.FoliageSize,
                    FoliageTypeId = f.FoliageTypeId,
                    FoliageTypeName = foliageTypeList.FoliageTypes.Where(a => a.FoliageTypeId == f.FoliageTypeId).Select(b => b.FoliageTypeName).First()
                };

                //plantResponse.Plant = pDTO;
                //plantResponse.Inventory = iDTO;
                long imageId = inventoryImageMap.Where(a => a.InventoryId == i.InventoryId).Select(b => b.ImageId).FirstOrDefault();

                response.FoliageInventoryList.Add(new FoliageInventoryDTO(fDTO, iDTO, imageId));
            }

            return response;
        }
        public GetArrangementResponse GetArrangements()
        {
            GetArrangementResponse response = new GetArrangementResponse();

            List<ServiceCodeDTO> serviceCodeList = GetServiceCodes();

            List<Arrangement> arrangements = dbContext.Arrangement.ToList();

            foreach (Arrangement a in arrangements)
            {
                ArrangementImageMap arrangementImageMap = dbContext.ArrangementImageMap.Where(b => b.ArrangmentId == a.ArrangementId).FirstOrDefault();

                ArrangementInventoryMap arrangementInventoryMap = dbContext.ArrangementInventoryMap.Where(b => b.ArrangementId == a.ArrangementId).First();

                Inventory i = dbContext.Inventory.Where(b => b.InventoryId == arrangementInventoryMap.InventoryId).First();

                List<long> inventoryIds = dbContext.ArrangementInventoryInventoryMap.Where(b => b.ArrangementId == a.ArrangementId).Select(c => c.InventoryId).ToList();

                List<InventoryImageMap> iim = dbContext.InventoryImageMap.Where(b => inventoryIds.Contains(b.InventoryId)).ToList();

                List<KeyValuePair<long,string>> inventoryList = new List<KeyValuePair<long,string>>();

                dbContext.Inventory.Where(b => inventoryIds.Contains(b.InventoryId)).ToList().ForEach(item =>
                {
                    inventoryList.Add(new KeyValuePair<long, string>(item.InventoryId, item.InventoryName));
                });

                ArrangementDTO one = new ArrangementDTO()
                {
                    ArrangementId = a.ArrangementId,
                    ArrangementName = a.ArrangementName,
                    ServiceCodeId = serviceCodeList.Where(b => b.ServiceCodeId == a.ServiceCodeId).Select(c => c.ServiceCodeId).First()
                };

                InventoryDTO two = new InventoryDTO()
                {
                    InventoryId = i.InventoryId,
                    InventoryTypeId = i.InventoryTypeId,
                    InventoryName = i.InventoryName,
                    NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                    Quantity = i.Quantity,
                    ServiceCodeId = i.ServiceCodeId,
                    ServiceCodeName = serviceCodeList.Where(b => b.ServiceCodeId == i.ServiceCodeId).Select(c => c.ServiceCode).First()
                };

                response.ArrangementList.Add(new ArrangementInventoryDTO()
                {
                    Arrangement = new ArrangementDTO()
                    {
                        ArrangementId = a.ArrangementId,
                        ArrangementName = a.ArrangementName,
                        ServiceCodeId = serviceCodeList.Where(b => b.ServiceCodeId == a.ServiceCodeId).Select(c => c.ServiceCodeId).First()
                    },

                    Inventory = new InventoryDTO()
                    {
                        InventoryId = i.InventoryId,
                        InventoryTypeId = i.InventoryTypeId,
                        InventoryName = i.InventoryName,
                        NotifyWhenLowAmount = i.NotifyWhenLowAmount,
                        Quantity = i.Quantity,
                        ServiceCodeId = i.ServiceCodeId,
                        ServiceCodeName = serviceCodeList.Where(b => b.ServiceCodeId == i.ServiceCodeId).Select(c => c.ServiceCode).First()
                    },

                    InventoryList = inventoryList,

                    ImageId = 0

                });
            }

            return response;
        }
        public long AddPlantImage(byte[] imageBytes)
        {
            long plantImageId = 0;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Image plantImage = new Image()
                    {
                        ImageData = imageBytes
                    };

                    dbContext.Image.Add(plantImage);
                    dbContext.SaveChanges();
                    scope.Complete();
                    plantImageId = plantImage.ImageId;
                }
                catch (Exception ex)
                {
                    int debug = 1;
                }
            }

            return plantImageId;
        }

        public long AddArrangementImage(byte[] imageBytes)
        {
            long arrangementImageId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                
                    Image arrangementImage = new Image()
                    {
                        ImageData = imageBytes
                    };

                    dbContext.Image.Add(arrangementImage);
                    dbContext.SaveChanges();
                    scope.Complete();
                    arrangementImageId = arrangementImage.ImageId;
                }
            }
            catch (Exception ex)
            {
                int debug = 1;
            }

            return arrangementImageId;
        }

        public GetVendorResponse GetVendors(GetPersonRequest request)
        {
            GetVendorResponse response = new GetVendorResponse();

            List<long> vendorIds = new List<long>();

            if (request.IsEmpty())
            {
                vendorIds = dbContext.Vendor.Select(a => a.VendorId).ToList();
            }
            else
            {
                if (!String.IsNullOrEmpty(request.FirstName))
                {
                    List<long> x = dbContext.Vendor.Where(a => a.VendorName.Contains(request.FirstName)).Select(b => b.VendorId).ToList();

                    vendorIds = vendorIds.Union(x).ToList();
                }

                if (!String.IsNullOrEmpty(request.PhonePrimary))
                {
                    List<long> x = dbContext.Vendor.Where(a => a.VendorPhone == request.PhonePrimary).Select(b => b.VendorId).ToList();

                    vendorIds = vendorIds.Union(x).ToList();
                }

                if (!String.IsNullOrEmpty(request.Email))
                {
                    List<long> x = dbContext.Vendor.Where(a => a.VendorEmail == request.Email).Select(b => b.VendorId).ToList();

                    vendorIds = vendorIds.Union(x).ToList();
                }
            }

            List<VendorDTO> vendorList = new List<VendorDTO>();

            dbContext.Vendor.Where(a => vendorIds.Contains(a.VendorId)).ToList().ForEach(item =>
            {
                vendorList.Add(new VendorDTO()
                {
                    VendorId = item.VendorId,
                    VendorName = item.VendorName,
                    VendorPhone = item.VendorPhone,
                    VendorEmail = item.VendorEmail,
                });
            });

            List<VendorAddressMap> vendorAdxMap = dbContext.VendorAddressMap.Where(a => vendorIds.Contains(a.VendorId)).ToList();

            List<long> addressIds = vendorAdxMap.Where(a => vendorIds.Contains(a.VendorId)).Select(b => b.AddressId).ToList();

            List<AddressDTO> addresses = new List<AddressDTO>();

            dbContext.Address.Where(a => addressIds.Contains(a.AddressId)).ToList().ForEach(item =>
            {
                addresses.Add(new AddressDTO()
                {
                    address_type_id = item.AddressTypeId.HasValue ? item.AddressTypeId.Value : 0,
                    address_id = item.AddressId,
                    street_address = item.StreetAddress,
                    unit_apt_suite = item.UnitAptSuite,
                    state = item.State,
                    zipcode = item.Zipcode,
                    city = item.City
                });
            });

            foreach (VendorDTO v in vendorList)
            {
                AddressDTO a = null;

                long addressId = vendorAdxMap.Where(b => b.VendorId == v.VendorId).Select(c => c.AddressId).FirstOrDefault();

                if (addressId > 0)
                {
                    a = addresses.Where(b => b.address_id == addressId).FirstOrDefault();

                    v.StreetAddress = a.street_address;
                    v.UnitAptSuite = a.unit_apt_suite;
                    v.City = a.city;
                    v.State = a.state;
                    v.ZipCode = a.zipcode;
                }
            }

            response.VendorList = vendorList;

            return response;
        }

        public GetVendorResponse GetVendorById(long vendorId)
        {
            GetVendorResponse response = new GetVendorResponse();

            Vendor v = dbContext.Vendor.Where(a => a.VendorId == vendorId).FirstOrDefault();

            VendorDTO dto = new VendorDTO();

            dto.VendorName = v.VendorName;
            dto.VendorEmail = v.VendorEmail;
            dto.VendorPhone = v.VendorPhone;

            if(v != null && v.VendorId == vendorId)
            {
                VendorAddressMap vam = dbContext.VendorAddressMap.Where(a => a.VendorId == vendorId).FirstOrDefault();

                if(vam != null && vam.VendorAddressMapId > 0)
                {
                    Address adx = dbContext.Address.Where(a => a.AddressId == vam.AddressId).FirstOrDefault();

                    if(adx != null && adx.AddressId > 0)
                    {
                        dto.StreetAddress = adx.StreetAddress;
                        dto.UnitAptSuite = adx.UnitAptSuite;
                        dto.City = adx.City;
                        dto.State = adx.State;
                        dto.ZipCode = adx.Zipcode;
                        dto.VendorAddressMapId = vam.VendorAddressMapId;
                    }
                }
            }

            response.VendorList.Add(dto);

            return response;
        }

        public long AddVendor(AddVendorRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {

                    Vendor v = new Vendor()
                    {
                        VendorName = request.Vendor.VendorName,
                        VendorPhone = request.Vendor.VendorPhone,
                        VendorEmail = request.Vendor.VendorEmail
                    };

                    dbContext.Vendor.Add(v);

                    Address a = new Address()
                    {
                        AddressTypeId = 4,
                        StreetAddress = request.Vendor.StreetAddress,
                        UnitAptSuite = request.Vendor.UnitAptSuite,
                        City = request.Vendor.City,
                        State = request.Vendor.State,
                        Zipcode = request.Vendor.ZipCode
                    };

                    dbContext.Address.Add(a);

                    dbContext.SaveChanges();

                    VendorAddressMap vam = new VendorAddressMap()
                    {
                        AddressId = a.AddressId,
                        VendorId = v.VendorId
                    };

                    dbContext.VendorAddressMap.Add(vam);

                    dbContext.SaveChanges();
                    scope.Complete();
                    newId = v.VendorId;
                }
            }
            catch (Exception ex)
            {
                int debug = 1;
            }

            return newId;
        }

        public long AddCustomer(AddCustomerRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Person p = new Person()
                    {
                        FirstName = request.Customer.Person.first_name,
                        LastName = request.Customer.Person.last_name,
                        Email = request.Customer.Person.email,
                        PhonePrimary = request.Customer.Person.phone_primary,
                        PhoneAlt = request.Customer.Person.phone_alt,
                        UserId = 1
                    };

                    dbContext.Person.Add(p);

                    Address a = new Address()
                    {
                        AddressTypeId = 1,
                        StreetAddress = request.Customer.Address.street_address,
                        UnitAptSuite = request.Customer.Address.unit_apt_suite,
                        City = request.Customer.Address.city,
                        State = request.Customer.Address.state,
                        Zipcode = request.Customer.Address.zipcode
                    };

                    dbContext.Address.Add(a);

                    PersonAddressMap map = new PersonAddressMap()
                    {
                        AddresId = a.AddressId,
                        PersonId = p.PersonId,
                    };

                    dbContext.PersonAddressMap.Add(map);
                    dbContext.SaveChanges();
                    scope.Complete();

                    newId = p.PersonId;
                }
            }
            catch(Exception ex)
            {

            }

            return newId;
        }

        public long AddShipment(AddShipmentRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {

                    Shipment s = new Shipment()
                    {
                        ShipmentDate = request.ShipmentDTO.ShipmentDate,
                        VendorId = request.ShipmentDTO.VendorId
                    };

                    dbContext.Shipment.Add(s);

                    List<ShipmentInventoryMap> mapList = new List<ShipmentInventoryMap>();

                    foreach(ShipmentInventoryMapDTO map in request.ShipmentInventoryMap)
                    {
                        mapList.Add(new ShipmentInventoryMap()
                        {
                            InventoryId = map.InventoryId,
                            Quantity = map.Quantity,
                            ShipmentId = s.ShipmentId
                        });
                    }

                    dbContext.ShipmentInventoryMap.AddRange(mapList);

                    dbContext.SaveChanges();
                    scope.Complete();
                    newId = s.ShipmentId;
                }
            }
            catch (Exception ex)
            {
                int debug = 1;
            }

            return newId;
        }

        public GetShipmentResponse GetShipments(ShipmentFilter filter)
        {
            GetShipmentResponse shipmentResponse = new GetShipmentResponse();

            dbContext.Shipment.Where(a => a.ShipmentDate >= filter.FromDate && a.ShipmentDate <= filter.ToDate).ToList().ForEach(item =>
            {
                List<ShipmentInventoryMapDTO> inventoryMap = new List<ShipmentInventoryMapDTO>();

                dbContext.ShipmentInventoryMap.Where(a => a.ShipmentId == item.ShipmentId).ToList().ForEach(map =>
                {
                    inventoryMap.Add(new ShipmentInventoryMapDTO()
                    {
                        InventoryId = map.InventoryId,
                        //InventoryName = map.Inventory.InventoryName,
                        Quantity = map.Quantity,
                        ShipmentId = item.ShipmentId,
                        ShipmentInventoryMapId = map.ShipmentInventoryMapId
                    });
                });

                ShipmentDTO shipment = new ShipmentDTO()
                {
                    ShipmentId = item.ShipmentId,
                    ShipmentDate = item.ShipmentDate,
                    VendorId = item.VendorId
                };

                shipmentResponse.ShipmentList.Add(new ShipmentInventoryDTO(shipment, inventoryMap));
            });

            return shipmentResponse;
        }

        public long AddWorkOrder(AddWorkOrderRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {

                    WorkOrder w = new WorkOrder()
                    {
                        PersonInitiator = request.WorkOrder.Buyer,
                        PersonReceiver = request.WorkOrder.Seller,
                        CreateDate = request.WorkOrder.CreateDate,
                        Comments = request.WorkOrder.Comments
                    };

                    dbContext.WorkOrder.Add(w);

                    List<WorkOrderInventoryMap> mapList = new List<WorkOrderInventoryMap>();

                    foreach (WorkOrderInventoryMapDTO map in request.WorkOrderInventoryMap)
                    {
                        mapList.Add(new WorkOrderInventoryMap()
                        {
                            InventoryId = map.InventoryId,
                            Quantity = map.Quantity,
                            WorkOrderId = w.WorkOrderId
                        });
                    }

                    dbContext.WorkOrderInventoryMap.AddRange(mapList);

                    dbContext.SaveChanges();
                    scope.Complete();
                    newId = w.WorkOrderId;
                }
            }
            catch (Exception ex)
            {
                int debug = 1;
            }

            return newId;
        }

        public long DoesPersonExist(PersonDTO person)
        {
            long personId = 0;

            Person p = dbContext.Person.Where(a => a.FirstName == person.first_name && a.LastName == person.last_name && a.PhonePrimary == person.phone_primary).FirstOrDefault();

            if(p != null && p.PersonId > 0)
            {
                personId = p.PersonId;
            }

            return personId;
        }

        public long ImportPerson(ImportPersonRequest request)
        {
            long personId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Person p = new Person()
                    {
                        FirstName = request.Person.first_name,
                        LastName = request.Person.last_name,
                        PhonePrimary = request.Person.phone_primary,
                        PhoneAlt = request.Person.phone_alt,
                        LastUpdated = DateTime.Now,
                        Email = request.Person.email,
                        UserId = 1  // fix this - person should not depend on FK_user
                    };

                    dbContext.Person.Add(p);
                    dbContext.SaveChanges();
                    personId = p.PersonId;

                    if(!String.IsNullOrEmpty(request.Address.street_address))
                    {
                        Address a = new Address()
                        {
                            State ="FL",
                            StreetAddress = request.Address.street_address,
                            UnitAptSuite = request.Address.unit_apt_suite,
                            Zipcode = request.Address.zipcode,
                            AddressTypeId = request.Address.address_type_id,
                            City = request.Address.city
                        };

                        dbContext.Address.Add(a);
                        dbContext.SaveChanges();
                        long addressId = a.AddressId;

                        if(personId > 0 && addressId > 0)
                        {
                            PersonAddressMap pam = new PersonAddressMap()
                            {
                                AddresId = addressId,
                                PersonId = personId
                            };

                            dbContext.PersonAddressMap.Add(pam);
                            dbContext.SaveChanges();
                        }
                    }

                    scope.Complete();
                }
  
            }
            catch (Exception ex)
            {

            }

            return personId;
        }

        public GetPersonResponse GetPerson(GetPersonRequest request)
        {
            GetPersonResponse response = new GetPersonResponse();

            List<long> personIds = new List<long>();

            if(request.IsEmpty())
            {
                personIds = dbContext.Person.Select(a => a.PersonId).ToList();
            }
            else
            {
                if(!String.IsNullOrEmpty(request.FirstName))
                {
                    List<long> x = dbContext.Person.Where(a => a.FirstName.Contains(request.FirstName)).Select(b => b.PersonId).ToList();

                    personIds = personIds.Union(x).ToList();
                }

                if (!String.IsNullOrEmpty(request.LastName))
                {
                    List<long> x = dbContext.Person.Where(a => a.LastName.Contains(request.LastName)).Select(b => b.PersonId).ToList();

                    personIds = personIds.Union(x).ToList();
                }

                if (!String.IsNullOrEmpty(request.PhonePrimary))
                {
                    List<long> x = dbContext.Person.Where(a => a.PhonePrimary == request.PhonePrimary).Select(b => b.PersonId).ToList();

                    personIds = personIds.Union(x).ToList();
                }

                if (!String.IsNullOrEmpty(request.PhoneAlt))
                {
                    List<long> x = dbContext.Person.Where(a => a.PhoneAlt == request.PhoneAlt).Select(b => b.PersonId).ToList();

                    personIds = personIds.Union(x).ToList();
                }

                if (!String.IsNullOrEmpty(request.Email))
                {
                    List<long> x = dbContext.Person.Where(a => a.Email == request.Email).Select(b => b.PersonId).ToList();

                    personIds = personIds.Union(x).ToList();
                }
            }

            List<PersonDTO> personList = new List<PersonDTO>();

            dbContext.Person.Where(a => personIds.Contains(a.PersonId)).ToList().ForEach(item =>
            {
                personList.Add(new PersonDTO()
                {
                    person_id = item.PersonId,
                    first_name = item.FirstName,
                    last_name = item.LastName,
                    phone_primary = item.PhonePrimary,
                    phone_alt = item.PhoneAlt,
                    email = item.Email,
                    last_updated = item.LastUpdated
                });
            });

            List<PersonAddressMap> personAdxMap = dbContext.PersonAddressMap.Where(a => personIds.Contains(a.PersonId)).ToList();

            List<long> addressIds = personAdxMap.Where(a => personIds.Contains(a.PersonId)).Select(b => b.AddresId).ToList();

            List<AddressDTO> addresses = new List<AddressDTO>();

            dbContext.Address.Where(a => addressIds.Contains(a.AddressId)).ToList().ForEach(item =>
            {
                addresses.Add(new AddressDTO()
                {
                    address_type_id = item.AddressTypeId.HasValue ? item.AddressTypeId.Value : 0,
                    address_id = item.AddressId,
                    street_address = item.StreetAddress,
                    unit_apt_suite = item.UnitAptSuite,
                    state = item.State,
                    zipcode = item.Zipcode,
                    city = item.City
                });       
            });

            foreach(PersonDTO p in personList)
            {
                AddressDTO a = null;

                long addressId = personAdxMap.Where(b => b.PersonId == p.person_id).Select(c => c.AddresId).FirstOrDefault();

                if (addressId != null && addressId > 0)
                {
                    a = addresses.Where(b => b.address_id == addressId).FirstOrDefault();
                }

                response.PersonAndAddress.Add(new PersonAndAddressDTO(p, a));
            }

            return response;
        }

        #region Foliage
        public long FoliageExists(FoliageDTO foliageDTO)
        {
            long foliageId = 0;

            Foliage f = dbContext.Foliage.Where(a => a.FoliageName == foliageDTO.FoliageName && a.FoliageTypeId == foliageDTO.FoliageTypeId).FirstOrDefault();

            if (f != null && f.FoliageId > 0)
            {
                foliageId = f.FoliageId;
            }

            return foliageId;
        }

        public long FoliageNameExists(string foliageName)
        {
            long foliageNameId = 0;

            FoliageName folName = dbContext.FoliageName.Where(a => a.Name == foliageName).FirstOrDefault();

            if (folName != null && folName.FoliageNameId > 0)
            {
                foliageNameId = folName.FoliageNameId;
            }

            return foliageNameId;
        }

        public long FoliageTypeExists(string typeName)
        {
            long foliageTypeId = 0;

            FoliageType folType = dbContext.FoliageType.Where(a => a.FoliageTypeName == typeName).FirstOrDefault();

            if (folType != null && folType.FoliageTypeId > 0)
            {
                foliageTypeId = folType.FoliageTypeId;
            }

            return foliageTypeId;
        }

        public bool FoliageNameIsNotUnique(string foliageName)
        {
            bool notUnique = false;

            Foliage f = dbContext.Foliage.Where(a => a.FoliageName == foliageName).FirstOrDefault();

            if (f != null && f.FoliageId > 0)
            {
                notUnique = true;
            }

            return notUnique;
        }
        public long AddFoliageType(AddFoliageTypeRequest request)
        {
            long newId = 0;
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    FoliageType foliageType = new FoliageType()
                    {
                        FoliageTypeName = request.FoliageTypeName,
                    };

                    dbContext.FoliageType.Add(foliageType);
                    dbContext.SaveChanges();
                    scope.Complete();

                    newId = foliageType.FoliageTypeId;
                }
            }
            catch (Exception ex)
            {

            }

            return newId;
        }

        public long AddFoliageName(AddFoliageNameRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    FoliageName foliageName = new FoliageName()
                    {
                        Name = request.FoliageName,
                        FoliageTypeId = request.FoliageTypeId
                    };

                    dbContext.FoliageName.Add(foliageName);
                    dbContext.SaveChanges();
                    scope.Complete();

                    newId = foliageName.FoliageNameId;
                }
            }
            catch (Exception ex)
            {

            }

            return newId;
        }

        public long AddFoliage(AddFoliageRequest request)
        {
            long foliage_id = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Foliage f = new Foliage()
                    {
                        FoliageName = request.Foliage.FoliageName,
                        FoliageTypeId = request.Foliage.FoliageTypeId,
                        FoliageNameId = request.Foliage.FoliageNameId,
                        FoliageSize = request.Foliage.FoliageSize 
                    };

                    dbContext.Foliage.Add(f);

                    Inventory i = new Inventory()
                    {
                        InventoryName = request.Inventory.InventoryName,
                        InventoryTypeId = request.Inventory.InventoryTypeId,
                        ServiceCodeId = request.Inventory.ServiceCodeId
                    };

                    dbContext.Inventory.Add(i);

                    InventoryFoliageMap invFoliageMap = new InventoryFoliageMap();
                    invFoliageMap.InventoryId = i.InventoryId;
                    invFoliageMap.FoliageId = f.FoliageId;

                    dbContext.InventoryFoliageMap.Add(invFoliageMap);

                    if (request.ImageId > 0)
                    {
                        InventoryImageMap iImgMap = new InventoryImageMap();
                        iImgMap.InventoryId = i.InventoryId;
                        iImgMap.ImageId = request.ImageId;

                        dbContext.InventoryImageMap.Add(iImgMap);
                    }

                    dbContext.SaveChanges();
                    scope.Complete();
                    foliage_id = f.FoliageId;
                }
            }
            catch (Exception ex)
            {

            }

            return foliage_id;
        }

        public long UpdateFoliage(ImportFoliageRequest request)
        {
            long foliageId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (request.AddFoliageRequest.Foliage.FoliageId != 0)
                    {
                        Foliage f = dbContext.Foliage.Where(a => a.FoliageId == request.AddFoliageRequest.Foliage.FoliageId).FirstOrDefault();

                        if (f.FoliageId > 0)
                        {
                            f.FoliageSize = request.AddFoliageRequest.Foliage.FoliageSize;
                        }
                    }

                    if (request.AddFoliageRequest.Inventory.InventoryId > 0)
                    {
                        ServiceCode code = dbContext.ServiceCode.Where(a => a.ServiceCodeId == request.AddFoliageRequest.Inventory.ServiceCodeId).FirstOrDefault();

                        if (code.ServiceCodeId > 0)
                        {
                            code.Cost = request.ServiceCode.Cost;
                            code.Description = request.ServiceCode.Description;
                            code.GeneralLedger = request.ServiceCode.GeneralLedger;
                            code.Price = request.ServiceCode.Price;
                            code.Taxable = request.ServiceCode.Taxable;
                        }
                    }

                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

            }
            return foliageId;
        }

        public long ImportFoliage(ImportFoliageRequest request)
        {
            long foliageId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    long foliageTypeId = request.AddFoliageRequest.Foliage.FoliageTypeId;

                    if (!String.IsNullOrEmpty(request.FoliageType))
                    {
                        //add plant type
                        if (foliageTypeId == 0)
                        {
                            AddFoliageTypeRequest foliageTypeRequest = new AddFoliageTypeRequest();
                            foliageTypeRequest.FoliageTypeName = request.FoliageType;
                            foliageTypeId = AddFoliageType(foliageTypeRequest);
                        }

                        request.AddFoliageRequest.Foliage.FoliageTypeId = foliageTypeId;
                    }

                    string foliageTypeName = dbContext.FoliageType.Where(a => a.FoliageTypeId == foliageTypeId).Select(b => b.FoliageTypeName).First();

                    long foliageNameId = request.AddFoliageRequest.Foliage.FoliageNameId;

                    if (!String.IsNullOrEmpty(request.FoliageName))
                    {
                        //add foliage name
                        if (foliageNameId == 0)
                        {
                            AddFoliageNameRequest foliageNameRequest = new AddFoliageNameRequest();
                            foliageNameRequest.FoliageName = request.FoliageName;
                            foliageNameRequest.FoliageTypeId = foliageTypeId;
                            foliageNameId = AddFoliageName(foliageNameRequest);
                        }

                        request.AddFoliageRequest.Foliage.FoliageNameId = foliageNameId;
                    }

                    string foliageName = dbContext.FoliageName.Where(a => a.FoliageNameId == foliageNameId).Select(b => b.Name).First();

                    long serviceCodeId = request.ServiceCode.ServiceCodeId;
                    string serviceCodeName = request.ServiceCode.ServiceCode;

                    if (serviceCodeId == 0)
                    {
                        serviceCodeId = AddServiceCode(request.ServiceCode);
                    }

                    ServiceCode svcCode = dbContext.ServiceCode.Where(a => a.ServiceCodeId == serviceCodeId).First();

                    serviceCodeName = svcCode.ServiceCode1;


                    request.AddFoliageRequest.Foliage.FoliageName = foliageTypeName + "-" + foliageName;
                    request.AddFoliageRequest.Foliage.FoliageNameId = foliageNameId;
                    request.AddFoliageRequest.Foliage.FoliageSize = request.AddFoliageRequest.Foliage.FoliageSize;
                    request.AddFoliageRequest.Foliage.FoliageTypeId = foliageTypeId;
                    request.AddFoliageRequest.Foliage.FoliageTypeName = foliageTypeName;

                    request.AddFoliageRequest.Inventory.InventoryTypeId = (long)Enums.InventoryType.Foliage;
                    request.AddFoliageRequest.Inventory.InventoryTypeName = "Foliage";
                    request.AddFoliageRequest.Inventory.InventoryName = foliageTypeName + "-" + foliageName;
                    request.AddFoliageRequest.Inventory.ServiceCodeId = serviceCodeId;
                    request.AddFoliageRequest.Inventory.ServiceCodeName = serviceCodeName;

                    long inventoryId = InventoryExists(request.AddFoliageRequest.Inventory);

                    long imageId = 0;

                    if (inventoryId > 0)
                    {
                        request.AddFoliageRequest.Inventory.InventoryId = inventoryId;

                        InventoryFoliageMap ipm = dbContext.InventoryFoliageMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if (ipm.InventoryFoliageMapId != 0)
                        {
                            request.AddFoliageRequest.Foliage.FoliageId = ipm.FoliageId;
                        }

                        InventoryImageMap iim = dbContext.InventoryImageMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if (iim != null && iim.InventoryImageMapId > 0)
                        {
                            if (iim.ImageId == 0)
                            {
                                if (request.imageBytes != null && request.imageBytes.Length > 0)
                                {
                                    //add image
                                    imageId = AddPlantImage(request.imageBytes);
                                }

                                request.AddFoliageRequest.ImageId = imageId;
                            }
                        }
                    }
                    else
                    {
                        if (request.imageBytes != null && request.imageBytes.Length > 0)
                        {
                            //add image
                            imageId = AddPlantImage(request.imageBytes);
                        }

                        request.AddFoliageRequest.ImageId = imageId;
                    }

                    if (inventoryId == 0)
                    {
                        foliageId = AddFoliage(request.AddFoliageRequest);
                    }
                    else
                    {
                        UpdateFoliage(request);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

            }

            return foliageId;
        }
        #endregion

        #region Materials
        public long MaterialExists(MaterialDTO materialDTO)
        {
            long materialId = 0;

            Material m = dbContext.Material.Where(a => a.MaterialName == materialDTO.MaterialName && a.MaterialTypeId == materialDTO.MaterialTypeId).FirstOrDefault();

            if (m != null && m.MaterialId > 0)
            {
                materialId = m.MaterialId;
            }

            return materialId;
        }

        public long MaterialNameExists(string materialName)
        {
            long materialNameId = 0;

            MaterialName matName = dbContext.MaterialName.Where(a => a.Name == materialName).FirstOrDefault();

            if (matName != null && matName.MaterialNameId > 0)
            {
                materialNameId = matName.MaterialNameId;
            }

            return materialNameId;
        }

        public long MaterialTypeExists(string typeName)
        {
            long materialTypeId = 0;

            MaterialType matType = dbContext.MaterialType.Where(a => a.MaterialTypeName == typeName).FirstOrDefault();

            if (matType != null && matType.MaterialTypeId > 0)
            {
                materialTypeId = matType.MaterialTypeId;
            }

            return materialTypeId;
        }

        public bool MaterialNameIsNotUnique(string materialName)
        {
            bool notUnique = false;

            Material m = dbContext.Material.Where(a => a.MaterialName == materialName).FirstOrDefault();

            if (m != null && m.MaterialId > 0)
            {
                notUnique = true;
            }

            return notUnique;
        }
        public long AddMaterialType(AddMaterialTypeRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    MaterialType materialType = new MaterialType()
                    {
                        MaterialTypeName = request.MaterialTypeName,
                    };

                    dbContext.MaterialType.Add(materialType);
                    dbContext.SaveChanges();
                    scope.Complete();

                    newId = materialType.MaterialTypeId;
                }
            }
            catch (Exception ex)
            {

            }

            return newId;
        }
        public long AddMaterialName(AddMaterialNameRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    MaterialName materialName = new MaterialName()
                    {
                        Name = request.MaterialName,
                        MaterialTypeId = request.MaterialTypeId
                    };

                    dbContext.MaterialName.Add(materialName);
                    dbContext.SaveChanges();
                    scope.Complete();

                    newId = materialName.MaterialNameId;
                }
            }
            catch (Exception ex)
            {

            }

            return newId;
        }
        public long AddMaterial(AddMaterialRequest request)
        {
            long material_id = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Material m = new Material()
                    {
                        MaterialName = request.Material.MaterialName,
                        MaterialTypeId = request.Material.MaterialTypeId,
                        MaterialNameId = request.Material.MaterialNameId,
                        MaterialSize = request.Material.MaterialSize
                    };

                    dbContext.Material.Add(m);

                    Inventory i = new Inventory()
                    {
                        InventoryName = request.Inventory.InventoryName,
                        InventoryTypeId = request.Inventory.InventoryTypeId,
                        ServiceCodeId = request.Inventory.ServiceCodeId
                    };

                    dbContext.Inventory.Add(i);

                    InventoryMaterialMap invMaterialMap = new InventoryMaterialMap();
                    invMaterialMap.InventoryId = i.InventoryId;
                    invMaterialMap.MaterialId = m.MaterialId;

                    dbContext.InventoryMaterialMap.Add(invMaterialMap);

                    if (request.ImageId > 0)
                    {
                        InventoryImageMap iImgMap = new InventoryImageMap();
                        iImgMap.InventoryId = i.InventoryId;
                        iImgMap.ImageId = request.ImageId;

                        dbContext.InventoryImageMap.Add(iImgMap);
                    }

                    dbContext.SaveChanges();
                    scope.Complete();
                    material_id = m.MaterialId;
                }
            }
            catch (Exception ex)
            {

            }

            return material_id;
        }
        public long UpdateMaterial(ImportMaterialRequest request)
        {
            long materialId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (request.AddMaterialRequest.Material.MaterialId != 0)
                    {
                        Material m = dbContext.Material.Where(a => a.MaterialId == request.AddMaterialRequest.Material.MaterialId).FirstOrDefault();

                        if (m.MaterialId > 0)
                        {
                            m.MaterialSize = request.AddMaterialRequest.Material.MaterialSize;
                        }
                    }

                    if (request.AddMaterialRequest.Inventory.InventoryId > 0)
                    {
                        ServiceCode code = dbContext.ServiceCode.Where(a => a.ServiceCodeId == request.AddMaterialRequest.Inventory.ServiceCodeId).FirstOrDefault();

                        if (code.ServiceCodeId > 0)
                        {
                            code.Cost = request.ServiceCode.Cost;
                            code.Description = request.ServiceCode.Description;
                            code.GeneralLedger = request.ServiceCode.GeneralLedger;
                            code.Price = request.ServiceCode.Price;
                            code.Taxable = request.ServiceCode.Taxable;
                        }
                    }

                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

            }

            return materialId;
        }
        public long ImportMaterial(ImportMaterialRequest request)
        {
            long materialId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    long materialTypeId = request.AddMaterialRequest.Material.MaterialTypeId;

                    if (!String.IsNullOrEmpty(request.MaterialType))
                    {
                        //add plant type
                        if (materialTypeId == 0)
                        {
                            AddMaterialTypeRequest materialTypeRequest = new AddMaterialTypeRequest();
                            materialTypeRequest.MaterialTypeName = request.MaterialType;
                            materialTypeId = AddMaterialType(materialTypeRequest);
                        }

                        request.AddMaterialRequest.Material.MaterialTypeId = materialTypeId;
                    }

                    string materialTypeName = dbContext.MaterialType.Where(a => a.MaterialTypeId == materialTypeId).Select(b => b.MaterialTypeName).First();

                    long materialNameId = request.AddMaterialRequest.Material.MaterialNameId;

                    if (!String.IsNullOrEmpty(request.MaterialName))
                    {
                        //add plant name
                        if (materialNameId == 0)
                        {
                            AddMaterialNameRequest materialNameRequest = new AddMaterialNameRequest();
                            materialNameRequest.MaterialName = request.MaterialName;
                            materialNameRequest.MaterialTypeId = materialTypeId;
                            materialNameId = AddMaterialName(materialNameRequest);
                        }

                        request.AddMaterialRequest.Material.MaterialNameId = materialNameId;
                    }

                    string materialName = dbContext.MaterialName.Where(a => a.MaterialNameId == materialNameId).Select(b => b.Name).First();

                    long serviceCodeId = request.ServiceCode.ServiceCodeId;
                    string serviceCodeName = request.ServiceCode.ServiceCode;

                    if (serviceCodeId == 0)
                    {
                        serviceCodeId = AddServiceCode(request.ServiceCode);
                    }

                    ServiceCode svcCode = dbContext.ServiceCode.Where(a => a.ServiceCodeId == serviceCodeId).First();

                    serviceCodeName = svcCode.ServiceCode1;


                    request.AddMaterialRequest.Material.MaterialName = materialTypeName + "-" + materialName;
                    request.AddMaterialRequest.Material.MaterialNameId = materialNameId;
                    request.AddMaterialRequest.Material.MaterialTypeId = materialTypeId;
                    request.AddMaterialRequest.Material.MaterialTypeName = materialTypeName;

                    request.AddMaterialRequest.Inventory.InventoryTypeId = (long)Enums.InventoryType.Materials;
                    request.AddMaterialRequest.Inventory.InventoryTypeName = "Materials";
                    request.AddMaterialRequest.Inventory.InventoryName = materialTypeName + "-" + materialName;
                    request.AddMaterialRequest.Inventory.ServiceCodeId = serviceCodeId;
                    request.AddMaterialRequest.Inventory.ServiceCodeName = serviceCodeName;

                    long inventoryId = InventoryExists(request.AddMaterialRequest.Inventory);

                    long imageId = 0;

                    if (inventoryId > 0)
                    {
                        request.AddMaterialRequest.Inventory.InventoryId = inventoryId;

                        InventoryMaterialMap ipm = dbContext.InventoryMaterialMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if (ipm.InventoryMaterialMapId != 0)
                        {
                            request.AddMaterialRequest.Material.MaterialId = ipm.MaterialId;
                        }

                        InventoryImageMap iim = dbContext.InventoryImageMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if (iim != null && iim.InventoryImageMapId > 0)
                        {
                            if (iim.ImageId == 0)
                            {
                                if (request.imageBytes != null && request.imageBytes.Length > 0)
                                {
                                    //add image
                                    imageId = AddPlantImage(request.imageBytes);
                                }

                                request.AddMaterialRequest.ImageId = imageId;
                            }
                        }
                    }
                    else
                    {
                        if (request.imageBytes != null && request.imageBytes.Length > 0)
                        {
                            //add image
                            imageId = AddPlantImage(request.imageBytes);
                        }

                        request.AddMaterialRequest.ImageId = imageId;
                    }

                    if (inventoryId == 0)
                    {
                        materialId = AddMaterial(request.AddMaterialRequest);
                    }
                    else
                    {
                        UpdateMaterial(request);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

            }

            return materialId;
        }
        #endregion

        #region Container
        public long ContainerExists(ContainerDTO containerDTO)
        {
            long containerId = 0;

            Container c = dbContext.Container.Where(a => a.ContainerName == containerDTO.ContainerName && a.ContainerTypeId == containerDTO.ContainerTypeId).FirstOrDefault();

            if (c != null && c.ContainerId > 0)
            {
                containerId = c.ContainerId;
            }

            return containerId;
        }

        public long ContainerNameExists(string containerName)
        {
            long containerNameId = 0;

            ContainerName conName = dbContext.ContainerName.Where(a => a.Name == containerName).FirstOrDefault();

            if (conName != null && conName.ContainerNameId > 0)
            {
                containerNameId = conName.ContainerNameId;
            }

            return containerNameId;
        }

        public long ContainerTypeExists(string typeName)
        {
            long containerTypeId = 0;

            ContainerType conType = dbContext.ContainerType.Where(a => a.ContainerTypeName == typeName).FirstOrDefault();

            if (conType != null && conType.ContainerTypeId > 0)
            {
                containerTypeId = conType.ContainerTypeId;
            }

            return containerTypeId;
        }

        public bool ContainerNameIsNotUnique(string containerName)
        {
            bool notUnique = false;

            Container c = dbContext.Container.Where(a => a.ContainerName == containerName).FirstOrDefault();

            if (c != null && c.ContainerId > 0)
            {
                notUnique = true;
            }

            return notUnique;
        }
        public long AddContainerType(AddContainerTypeRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    ContainerType containerType = new ContainerType()
                    {
                        ContainerTypeName = request.ContainerTypeName,
                    };

                    dbContext.ContainerType.Add(containerType);
                    dbContext.SaveChanges();
                    scope.Complete();

                    newId = containerType.ContainerTypeId;
                }
            }
            catch (Exception ex)
            {

            }

            return newId;
        }
        public long AddContainerName(AddContainerNameRequest request)
        {
            long newId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    ContainerName containerName = new ContainerName()
                    {
                        Name = request.ContainerName,
                        ContainerTypeId = request.ContainerTypeId
                    };

                    dbContext.ContainerName.Add(containerName);
                    dbContext.SaveChanges();
                    scope.Complete();

                    newId = containerName.ContainerNameId;
                }
            }
            catch (Exception ex)
            {

            }

            return newId;
        }

        //public long AddContainer(AddContainerRequest request)
        //{
        //    long container_id = 0;

        //    try
        //    {
        //        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        //        {
        //            Container c = new Container()
        //            {
        //                ContainerName = request.Container.ContainerName,
        //                ContainerTypeId = request.Container.ContainerTypeId,
        //            };

        //            dbContext.Container.Add(c);

        //            Inventory i = new Inventory()
        //            {
        //                InventoryName = request.Inventory.InventoryName,
        //                InventoryTypeId = request.Inventory.InventoryTypeId,
        //                ServiceCodeId = request.Inventory.ServiceCodeId
        //            };

        //            dbContext.Inventory.Add(i);

        //            InventoryContainerMap invContainerMap = new InventoryContainerMap();
        //            invContainerMap.InventoryId = i.InventoryId;
        //            invContainerMap.ContainerId = c.ContainerId;

        //            dbContext.InventoryContainerMap.Add(invContainerMap);

        //            if (request.ImageId > 0)
        //            {
        //                InventoryImageMap iImgMap = new InventoryImageMap();
        //                iImgMap.InventoryId = i.InventoryId;
        //                iImgMap.ImageId = request.ImageId;

        //                dbContext.InventoryImageMap.Add(iImgMap);
        //            }

        //            dbContext.SaveChanges();
        //            scope.Complete();
        //            container_id = c.ContainerId;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return container_id;
        //}
        public long UpdateContainer(ImportContainerRequest request)
        {
            long containerId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (request.AddContainerRequest.Container.ContainerId != 0)
                    {
                        Container c = dbContext.Container.Where(a => a.ContainerId == request.AddContainerRequest.Container.ContainerId).FirstOrDefault();

                        if (c.ContainerId > 0)
                        {
                            c.ContainerSize = request.AddContainerRequest.Container.ContainerSize;
                        }
                    }

                    if (request.AddContainerRequest.Inventory.InventoryId > 0)
                    {
                        ServiceCode code = dbContext.ServiceCode.Where(a => a.ServiceCodeId == request.AddContainerRequest.Inventory.ServiceCodeId).FirstOrDefault();

                        if (code.ServiceCodeId > 0)
                        {
                            code.Cost = request.ServiceCode.Cost;
                            code.Description = request.ServiceCode.Description;
                            code.GeneralLedger = request.ServiceCode.GeneralLedger;
                            code.Price = request.ServiceCode.Price;
                            code.Taxable = request.ServiceCode.Taxable;
                        }
                    }

                    dbContext.SaveChanges();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

            }

            return containerId;
        }
        public long ImportContainer(ImportContainerRequest request)
        {
            long containerId = 0;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    long containerTypeId = request.AddContainerRequest.Container.ContainerTypeId;

                    if (!String.IsNullOrEmpty(request.ContainerType))
                    {
                        //add plant type
                        if (containerTypeId == 0)
                        {
                            AddContainerTypeRequest containerTypeRequest = new AddContainerTypeRequest();
                            containerTypeRequest.ContainerTypeName = request.ContainerType;
                            containerTypeId = AddContainerType(containerTypeRequest);
                        }

                        request.AddContainerRequest.Container.ContainerTypeId = containerTypeId;
                    }

                    string containerTypeName = dbContext.ContainerType.Where(a => a.ContainerTypeId == containerTypeId).Select(b => b.ContainerTypeName).First();

                    long containerNameId = request.AddContainerRequest.Container.ContainerNameId;

                    if (!String.IsNullOrEmpty(request.ContainerName))
                    {
                        //add plant name
                        if (containerNameId == 0)
                        {
                            AddContainerNameRequest containerNameRequest = new AddContainerNameRequest();
                            containerNameRequest.ContainerName = request.ContainerName;
                            containerNameRequest.ContainerTypeId = containerTypeId;
                            containerNameId = AddContainerName(containerNameRequest);
                        }

                        request.AddContainerRequest.Container.ContainerNameId = containerNameId;
                    }

                    string containerName = dbContext.ContainerName.Where(a => a.ContainerNameId == containerNameId).Select(b => b.Name).First();

                    long serviceCodeId = request.ServiceCode.ServiceCodeId;
                    string serviceCodeName = request.ServiceCode.ServiceCode;

                    if (serviceCodeId == 0)
                    {
                        serviceCodeId = AddServiceCode(request.ServiceCode);
                    }

                    ServiceCode svcCode = dbContext.ServiceCode.Where(a => a.ServiceCodeId == serviceCodeId).First();

                    serviceCodeName = svcCode.ServiceCode1;


                    request.AddContainerRequest.Container.ContainerName = containerTypeName + "-" + containerName;
                    request.AddContainerRequest.Container.ContainerNameId = containerNameId;
                    request.AddContainerRequest.Container.ContainerTypeId = containerTypeId;
                    request.AddContainerRequest.Container.ContainerTypeName = containerTypeName;
                    request.AddContainerRequest.Container.ContainerSize = request.ContainerSize;

                    request.AddContainerRequest.Inventory.InventoryTypeId = (long)Enums.InventoryType.Containers;
                    request.AddContainerRequest.Inventory.InventoryTypeName = "Containers";
                    request.AddContainerRequest.Inventory.InventoryName = containerTypeName + "-" + containerName;
                    request.AddContainerRequest.Inventory.ServiceCodeId = serviceCodeId;
                    request.AddContainerRequest.Inventory.ServiceCodeName = serviceCodeName;

                    long inventoryId = InventoryExists(request.AddContainerRequest.Inventory);

                    long imageId = 0;

                    if (inventoryId > 0)
                    {
                        request.AddContainerRequest.Inventory.InventoryId = inventoryId;

                        InventoryContainerMap ipm = dbContext.InventoryContainerMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if (ipm.InventoryContainerMapId != 0)
                        {
                            request.AddContainerRequest.Container.ContainerId = ipm.ContainerId;
                        }

                        InventoryImageMap iim = dbContext.InventoryImageMap.Where(a => a.InventoryId == inventoryId).FirstOrDefault();

                        if (iim != null && iim.InventoryImageMapId > 0)
                        {
                            if (iim.ImageId == 0)
                            {
                                if (request.imageBytes != null && request.imageBytes.Length > 0)
                                {
                                    //add image
                                    imageId = AddPlantImage(request.imageBytes);
                                }

                                request.AddContainerRequest.ImageId = imageId;
                            }
                        }
                    }
                    else
                    {
                        if (request.imageBytes != null && request.imageBytes.Length > 0)
                        {
                            //add image
                            imageId = AddPlantImage(request.imageBytes);
                        }

                        request.AddContainerRequest.ImageId = imageId;
                    }

                    if (inventoryId == 0)
                    {
                        containerId = AddContainer(request.AddContainerRequest);
                    }
                    else
                    {
                        UpdateContainer(request);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

            }

            return containerId;
        }

        #endregion

        public List<string> GetSizesByInventoryType(long inventoryTypeId)
        {
            List<string> sizes = new List<string>();

            switch(inventoryTypeId)
            {
                case 1: //Orchids
                    sizes = dbContext.Plant.Select(a => a.PlantSize).Distinct().ToList();
                    break;

                case 2: //Containers
                    sizes = dbContext.Container.Select(a => a.ContainerSize).Distinct().ToList();
                    break;

                case 4: //Foliage
                    sizes = dbContext.Foliage.Select(a => a.FoliageSize).Distinct().ToList();
                    break;

                case 5: //Materials
                    sizes = dbContext.Material.Select(a => a.MaterialSize).Distinct().ToList();
                    break;
            }

            return sizes;
        }

        public GetWorkOrderSalesDetailResponse GetWorkOrderDetail(GetWorkOrderSalesDetailRequest request)
        {
            GetWorkOrderSalesDetailResponse response = new GetWorkOrderSalesDetailResponse();

            try
            {
                foreach (WorkOrderInventoryItemDTO workOrderItem in request.WorkOrderItems)
                {
                    Inventory i = dbContext.Inventory.Where(a => a.InventoryId == workOrderItem.InventoryId).FirstOrDefault();

                    if (i != null && i.InventoryId > 0)
                    {
                        ServiceCode code = dbContext.ServiceCode.Where(a => a.ServiceCodeId == i.ServiceCodeId).FirstOrDefault();

                        decimal subTotal = 0;

                        if (code.Price.HasValue)
                        {
                            subTotal = code.Price.Value * workOrderItem.Quantity;
                        }

                        decimal tax = 0;

                        if(code.Taxable)
                        {
                            tax = subTotal * 0.06M;
                        }

                        response.SubTotal += subTotal;

                        response.Tax += tax;
                    }
                }

                response.Total = response.SubTotal + response.Tax;
            }
            catch(Exception ex)
            {

            }

            return response;
        }
    }
}
