using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    /// <summary>
    /// A Plant
    /// </summary>
    public class PlantDTO
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public long PlantId { get; set; }

        /// <summary>
        /// This plant's name
        /// </summary>
        public string PlantName { get; set; }

        public long PlantNameId { get; set; }

        /// <summary>
        /// This plant's size
        /// </summary>
        public string PlantSize { get; set; }
        /// <summary>
        /// 
        /// This plant's type
        /// </summary>
        public long PlantTypeId { get; set; }

        public string PlantTypeName { get; set; }
    }
}
