
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ControllerModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class AddPlantNameRequest
    {
        public string PlantName { get; set; }

        public long PlantTypeId { get; set; }
    }

    [Serializable]
    
    public class AddPlantTypeRequest
    {
        public string PlantTypeName { get; set; }
    }

    [Serializable]
    
    public class AddFoliageNameRequest
    {
        public string FoliageName { get; set; }

        public long FoliageTypeId { get; set; }
    }

    [Serializable]
    
    public class AddFoliageTypeRequest
    {
        public string FoliageTypeName { get; set; }
    }

    [Serializable]
    
    public class AddMaterialNameRequest
    {
        public string MaterialName { get; set; }

        public long MaterialTypeId { get; set; }
    }

    [Serializable]
    
    public class AddMaterialTypeRequest
    {
        public string MaterialTypeName { get; set; }
    }

    [Serializable]
    
    public class AddContainerNameRequest
    {
        public string ContainerName { get; set; }

        public long ContainerTypeId { get; set; }
    }

    [Serializable]
    
    public class AddContainerTypeRequest
    {
        public string ContainerTypeName { get; set; }
    }
}
