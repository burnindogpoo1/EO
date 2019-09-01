using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    public class FoliageDTO
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public long FoliageId { get; set; }

        /// <summary>
        /// This material's name
        /// </summary>
        public string FoliageName { get; set; }

        public long FoliageNameId { get; set; }

        /// <summary>
        /// This material's size
        /// </summary>
        public string FoliageSize { get; set; }
        /// <summary>
        /// 
        /// This material's type
        /// </summary>
        public long FoliageTypeId { get; set; }

        public string FoliageTypeName { get; set; }
    }
}
