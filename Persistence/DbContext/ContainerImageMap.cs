using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class ContainerImageMap
    {
        public long ContainerImageMapId { get; set; }
        public long ContainerId { get; set; }
        public long ImageId { get; set; }

        public virtual Container Container { get; set; }
        public virtual Image Image { get; set; }
    }
}
