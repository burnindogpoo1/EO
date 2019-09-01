using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class PlantImageMap
    {
        public long PlantImageMapId { get; set; }
        public long PlantId { get; set; }
        public long ImageId { get; set; }

        public virtual Image Image { get; set; }
        public virtual Plant Plant { get; set; }
    }
}
