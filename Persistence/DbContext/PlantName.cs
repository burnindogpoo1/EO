using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public class PlantName
    {
        public long PlantNameId {get; set;}

        public string Name { get; set; }

        public long PlantTypeId { get; set; }
    }
}
