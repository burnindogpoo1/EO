
using Android.Runtime;
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
    /// 
    [Serializable]
    [Preserve(AllMembers = true)]
    public class ApiResponse
    {
        public ApiResponse()
        {
            Messages = new Dictionary<string, List<string>>();
            Id = 0;
        }

        public long Id { get; set; }

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
