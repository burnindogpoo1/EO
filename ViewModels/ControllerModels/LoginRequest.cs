﻿using EO.ViewModels.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EO.ViewModels.ControllerModels
{
    /// <summary>
    /// Login with username and password
    /// </summary>
    public class LoginRequest
    {
        public LoginRequest()
        {
            Login = new LoginDTO();
        }

        public LoginRequest(string userName, string password)
        {
            Login = new LoginDTO(userName, password);
        }

        /// <summary>
        /// Login data object
        /// </summary>
        public LoginDTO Login { get; set; }
    }
}
