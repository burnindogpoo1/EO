using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class PlantType
    {
        public PlantType()
        {
            Plant = new HashSet<Plant>();
        }

        public long PlantTypeId { get; set; }
        public string PlantTypeName { get; set; }
        //public long ImageId { get; set; }

        public virtual ICollection<Plant> Plant { get; set; }
    }
}
