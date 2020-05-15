
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class PlantTypeDTO
    {
        public long PlantTypeId { get; set; }

        public string PlantTypeName { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class MaterialTypeDTO
    {
        public long MaterialTypeId { get; set; }

        public string MaterialTypeName { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class FoliageTypeDTO
    {
        public long FoliageTypeId { get; set; }

        public string FoliageTypeName { get; set; }
    }
}
