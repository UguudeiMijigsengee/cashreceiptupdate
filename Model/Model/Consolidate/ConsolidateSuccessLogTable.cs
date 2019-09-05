using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.Consolidate
{
    public class ConsolidateSuccessLogTable
    {
        public DateTime dtstamp { get; set; }
        [Key]
        public int TMWMove { get; set; }
    }
}
