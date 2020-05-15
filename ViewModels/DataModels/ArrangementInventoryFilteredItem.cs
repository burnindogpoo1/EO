
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
    public class ArrangementInventoryFilteredItem
    {
        public long Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Size { get; set; }

        public string ServiceCode { get; set; }
    }
}
