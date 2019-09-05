using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.RetailLink
{
    public partial class RetailLinkLoad
    {        
        [Key]
        public int LOADNO { get; set; }
        public string SHIPTOCODE { get; set; }
        public int CNT { get; set; }
    }
}
