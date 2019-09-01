using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public class ContainerName
    {
        public long ContainerNameId { get; set; }

        public string Name { get; set; }

        public long ContainerTypeId { get; set; }
    }
}
