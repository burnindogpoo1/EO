using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EO.ViewModels.ControllerModels;

namespace EO.ViewModels.ControllerModels
{
    public class LoginResponse : ApiResponse
    {
        public string EOAccess { get; set; }
    }
}
