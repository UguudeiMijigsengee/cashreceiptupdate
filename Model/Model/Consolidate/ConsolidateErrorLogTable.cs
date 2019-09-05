using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.Consolidate
{
    public class ConsolidateErrorLogTable
    {
        [Key]
        public DateTime dtstamp { get; set; }
        public string ErrorString { get; set; }
    }
}
