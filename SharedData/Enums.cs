using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData
{
    public static class Enums
    {
        public enum ServiceCodeType
        {
            Plant = 1,
            Container = 2,
            DryMaterial = 3,
            Labor = 4,
            Delivery = 5
        }

        public enum InventoryType
        {
            AllInventoryTypes = 0,
            Orchids = 1,
            Containers = 2,
            Arrangements = 3,
            Foliage = 4,
            Materials = 5
        }
    }
}
