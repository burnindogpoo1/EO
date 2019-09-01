using EO.Persistence;
using EO.ViewModels.DataModels;
using LoginServiceLayer.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServiceLayer.Interface
{
    public class LoginManager : ILoginManager
    {
        private IEOPersistence persistence;

        public LoginManager()
        {
            persistence = new EOPersistence();
        }

        public LoginDTO GetUser(LoginDTO request)
        {
            return persistence.GetUser(request);
        }
    }
}
