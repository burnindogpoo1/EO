
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
    public class PlantNameDTO
    {
        public long PlantNameId { get; set; }

        public long PlantTypeId { get; set; }

        public string PlantName { get; set; }
    }
    [Serializable]
    [Preserve(AllMembers = true)]
    public class FoliageNameDTO
    {
        public long FoliageNameId { get; set; }

        public long FoliageTypeId { get; set; }

        public string FoliageName { get; set; }
    }

    [Serializable]
    [Preserve(AllMembers = true)]
    public class MaterialNameDTO
    {
        public long MaterialNameId { get; set; }

        public long MaterialTypeId { get; set; }

        public string MaterialName { get; set; }
    }
}
