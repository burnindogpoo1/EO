using EO.ViewModels.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServiceLayer.Implementation
{
    public interface ILoginManager
    {
        LoginDTO GetUser(LoginDTO request);
    }
}
