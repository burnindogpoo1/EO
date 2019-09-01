using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public class MaterialName
    {
        public long MaterialNameId { get; set; }

        public string Name { get; set; }

        public long MaterialTypeId { get; set; }
    }
}
