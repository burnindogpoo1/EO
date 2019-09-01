using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class ContainerUploads
    {
        public int ContaineruploadId { get; set; }
        public string QtyInStock { get; set; }
        public string Price { get; set; }
        public string Filename { get; set; }
    }
}
