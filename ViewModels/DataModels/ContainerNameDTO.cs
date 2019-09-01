using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    public class ContainerNameDTO
    {
        public long ContainerNameId { get; set; }

        public string ContainerName { get; set; }

        public long ContainerTypeId { get; set; }
    }

    public class ContainerTypeDTO
    {
        public long ContainerTypeId { get; set; }

        public string ContainerTypeName { get; set; }
    }
}
