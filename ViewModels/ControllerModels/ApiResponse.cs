using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.ViewModels.ControllerModels
{
    /// <summary>
    /// Base object for Response objects
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse()
        {
            Messages = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get { return Messages.Count == 0; }  }

        /// <summary>
        /// Error Messages
        /// </summary>
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
