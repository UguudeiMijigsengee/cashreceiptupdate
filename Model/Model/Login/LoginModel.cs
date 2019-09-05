using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string userName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string passWord { get; set; }
    }
}
