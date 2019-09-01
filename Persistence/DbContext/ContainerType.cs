using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class ContainerType
    {
        public ContainerType()
        {
            Container = new HashSet<Container>();
        }

        public long ContainerTypeId { get; set; }
        public string ContainerTypeName { get; set; }
        //public long ImageId { get; set; }

        public virtual ICollection<Container> Container { get; set; }
    }
}
