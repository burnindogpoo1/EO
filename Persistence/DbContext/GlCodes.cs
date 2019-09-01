using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class GlCodes
    {
        public short GlCodeId { get; set; }
        public string GlCode { get; set; }
        public string GlDepartment { get; set; }
        public string GlDescription { get; set; }
    }
}
