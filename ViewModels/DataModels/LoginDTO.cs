
using System;
using Android.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.ViewModels.DataModels
{
    /// <summary>
    /// Login object
    /// </summary>
    /// 
    [Serializable]
    [Preserve(AllMembers=true)]
    public class LoginDTO
    {
        public LoginDTO()
        {

        }

        public LoginDTO(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// User ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Role Id
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set;}
    }
}
