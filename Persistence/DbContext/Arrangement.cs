using System;
using System.Collections.Generic;

namespace EO.DatabaseContext
{
    public partial class Arrangement
    {
        public Arrangement()
        {
            ArrangementImageMap = new HashSet<ArrangementImageMap>();
            ArrangementInventoryMap = new HashSet<ArrangementInventoryMap>();
        }

        public long ArrangementId { get; set; }
        public string ArrangementName { get; set; }
        public DateTime UpdateDate { get; set; }
        public long? ServiceCodeId { get; set; }

        public virtual ServiceCode ServiceCode { get; set; }
        public virtual ICollection<ArrangementImageMap> ArrangementImageMap { get; set; }
        public virtual ICollection<ArrangementInventoryMap> ArrangementInventoryMap { get; set; }
    }
}
