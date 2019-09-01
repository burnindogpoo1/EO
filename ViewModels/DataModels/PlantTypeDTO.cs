using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    public class PlantTypeDTO
    {
        public long PlantTypeId { get; set; }

        public string PlantTypeName { get; set; }
    }

    public class MaterialTypeDTO
    {
        public long MaterialTypeId { get; set; }

        public string MaterialTypeName { get; set; }
    }

    public class FoliageTypeDTO
    {
        public long FoliageTypeId { get; set; }

        public string FoliageTypeName { get; set; }
    }
}
