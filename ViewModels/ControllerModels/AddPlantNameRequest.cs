using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ControllerModels
{
    public class AddPlantNameRequest
    {
        public string PlantName { get; set; }

        public long PlantTypeId { get; set; }
    }

    public class AddPlantTypeRequest
    {
        public string PlantTypeName { get; set; }
    }

    public class AddFoliageNameRequest
    {
        public string FoliageName { get; set; }

        public long FoliageTypeId { get; set; }
    }

    public class AddFoliageTypeRequest
    {
        public string FoliageTypeName { get; set; }
    }

    public class AddMaterialNameRequest
    {
        public string MaterialName { get; set; }

        public long MaterialTypeId { get; set; }
    }

    public class AddMaterialTypeRequest
    {
        public string MaterialTypeName { get; set; }
    }

    public class AddContainerNameRequest
    {
        public string ContainerName { get; set; }

        public long ContainerTypeId { get; set; }
    }

    public class AddContainerTypeRequest
    {
        public string ContainerTypeName { get; set; }
    }
}
