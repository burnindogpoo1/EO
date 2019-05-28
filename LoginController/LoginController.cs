using EO.ViewModels.ControllerModels;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace EO.LoginController
{
    public class LoginController : ApiController
    {
        [HttpGet]
        public LoginResponse Login(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();

            response.Success = false;

            List<string> errorMessages = new List<string>();

            errorMessages.Add("not implemented");

            response.AddMessage("Login", errorMessages);

            return response;
        }
    }
}
