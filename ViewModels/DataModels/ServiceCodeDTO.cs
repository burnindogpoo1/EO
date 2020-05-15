using Android.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.DataModels
{
    [Serializable]
    [Preserve(AllMembers = true)]
    public class ServiceCodeDTO
    {
        public long ServiceCodeId { get; set; }
        [Required]
        public string ServiceCode { get; set; }

        public string Description { get; set; }

        public string Size { get; set; }

        public decimal? Cost { get; set; }

        public decimal? Price { get; set; }

        public bool Taxable { get; set; }
        [Required]
        public string GeneralLedger { get; set; }
    }
}
