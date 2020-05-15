
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
    public class ArrangementDTO
    {
        public long ArrangementId { get; set; }
        public string ArrangementName { get; set; }
        public DateTime UpdateDate { get; set; }
        public long? ServiceCodeId { get; set; }
    }
}
