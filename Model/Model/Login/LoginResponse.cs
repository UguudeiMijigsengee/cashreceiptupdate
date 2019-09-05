using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.Login
{
    public class LoginResponse
    {
        public bool loggedIn { get; set; }        
        public string token { get; set; }
    }
}
