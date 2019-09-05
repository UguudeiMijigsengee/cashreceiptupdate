using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.RetailLink
{
    public class RJWLoad
    {
        public string PO { get; set; }
        public string POLINES { get; set; }
        public  string CASECOUNT { get; set; }
        public decimal WEIGHTORDER { get; set; }
        public string  BOL { get; set; }
        [Key]
        public string PRO { get; set; }
        public string LoadType { get; set; }
    }
}
