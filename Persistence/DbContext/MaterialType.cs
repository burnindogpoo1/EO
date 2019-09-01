using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.DatabaseContext
{
    public partial class MaterialType
    {
        public MaterialType()
        {
            Material = new HashSet<Material>();
        }

        public long MaterialTypeId { get; set; }
        public string MaterialTypeName { get; set; }
        //public long ImageId { get; set; }

        public virtual ICollection<Material> Material { get; set; }
    }
}
