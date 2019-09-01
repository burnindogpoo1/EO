using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EO.ViewModels.ControllerModels;

namespace EO.ViewModels.ControllerModels
{
    /// <summary>
    /// Response to login with username and password
    /// </summary>
    public class LoginResponse : ApiResponse
    {
        public string EOAccess { get; set; }
    }
}
