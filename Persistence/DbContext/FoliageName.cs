using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public class FoliageName
    {
        public long FoliageNameId { get; set; }

        public string Name { get; set; }

        public long FoliageTypeId { get; set; }
    }
}
