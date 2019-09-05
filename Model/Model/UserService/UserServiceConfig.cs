using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.UserService
{
    public class UserServiceConfig
    {
        public string Domain { get; set; }
        public string Key { get; set; }
        public string user_name { get; set; }
        public string userNotFound { get; set; }
        public List<string> accountMessages { get; set; }
        public int expirationDurationHours { get; set; }
        public List<UserRoute> UserRoutes { get; set; }
    }
}
