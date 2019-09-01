using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedData.Enums;

namespace InventoryServiceLayer.Helpers
{
  
    public static class ServiceCodePrefix
    {
        private static List<string> ServiceCodePrefixes = new List<string>(){"", "PL", "CT","MT","LB","TR"}; 

        public static string GetServiceCodePrefix(ServiceCodeType serviceCodeType)
        {
            string codePrefix = String.Empty;

            if((int)serviceCodeType < ServiceCodePrefixes.Count)
            {
                codePrefix = ServiceCodePrefixes[(int)serviceCodeType];
            }

            return codePrefix;
        }
    }

    
}
