using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public partial class FoliageType
    {
        public FoliageType()
        {
            Foliage = new HashSet<Foliage>();
        }

        public long FoliageTypeId { get; set; }
        public string FoliageTypeName { get; set; }
        //public long ImageId { get; set; }

        public virtual ICollection<Foliage> Foliage { get; set; }
    }
}
