using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.ViewModels.ControllerModels
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Messages = new Dictionary<string, List<string>>();
        }

        public bool Success { get; set; }

        public Dictionary<string, List<string>> Messages {get; set;}

        public void AddMessage(string fieldName, List<string> errorMessagesForField)
        {
            if(Messages.ContainsKey(fieldName))
            {
                Messages[fieldName].AddRange(errorMessagesForField);
            }
            else
            {
                Messages.Add(fieldName, errorMessagesForField);
            }
        }
    }
}
