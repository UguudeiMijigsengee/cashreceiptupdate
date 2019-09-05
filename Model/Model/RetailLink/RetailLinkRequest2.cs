using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.RetailLink
{
    public class RetailLinkRequest2
    {
        [Required(ErrorMessage = "Retail Link User Name is required.")]
        public string username { get; set; }
        [Required(ErrorMessage = "Retail Link Password is required.")]
        public string password { get; set; }
        public List<RetailLinkRequest> loads { get; set; }
    }
}
