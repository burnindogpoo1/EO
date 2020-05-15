
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class MaterialDTO
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// This material's name
        /// </summary>
        public string MaterialName { get; set; }

        public long MaterialNameId { get; set; }

        /// <summary>
        /// This material's size
        /// </summary>
        public string MaterialSize { get; set; }
        /// <summary>
        /// 
        /// This material's type
        /// </summary>
        public long MaterialTypeId { get; set; }

        public string MaterialTypeName { get; set; }
    }
}
