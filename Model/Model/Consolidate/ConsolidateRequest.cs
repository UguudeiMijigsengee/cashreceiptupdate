using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.Consolidate
{
    public class ConsolidateRequest
    {
        [Required(ErrorMessage = "Load is required.")]
        public string load { get; set; }
    }
}
