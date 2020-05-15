
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
    public class ContainerDTO
    {
        public long ContainerId { get; set; }
        public string ContainerColor { get; set; }
        public long ContainerTypeId { get; set; }
        public long ContainerNameId { get; set; }
        public string ContainerTypeName { get; set; }
        public double? ContainerHeight { get; set; }
        public double? ContainerWidth { get; set; }
        public double? ContainerWeight { get; set; }
        public int? ContainerQtyInStock { get; set; }
        public string ContainerName { get; set; }
        public string ContainerImage { get; set; }
        public string ContainerCost { get; set; }
        public string ContainerPrice { get; set; }
        public string ContainerSize { get; set; }
        public string ContainerVendor { get; set; }
        public string ContainerSku { get; set; }
        public DateTime? ContainerLastshipdate { get; set; }
        public long ServiceCodeId { get; set; }
        public long ServiceCodeName { get; set; }
    }
}
