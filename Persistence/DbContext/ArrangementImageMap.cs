using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class ArrangementImageMap
    {
        public long ArrangementImageMapId { get; set; }
        public long ArrangmentId { get; set; }
        public long ImageId { get; set; }

        public virtual Arrangement Arrangment { get; set; }
        public virtual Image Image { get; set; }
    }
}
