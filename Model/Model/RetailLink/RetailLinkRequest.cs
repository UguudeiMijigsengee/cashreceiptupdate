using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.RetailLink
{
    public class RetailLinkRequest
    {
        [Required(ErrorMessage = "Load(s) is required.")]
        public List<string> loads { get; set; }
        [Required(ErrorMessage = "Drop Date is required.")]
        public DateTime dropDate { get; set; }
        public bool overrideExisting { get; set; }        
    }
}
