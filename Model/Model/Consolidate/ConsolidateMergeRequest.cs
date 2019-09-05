using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.Consolidate
{
    public class ConsolidateMergeRequest
    {
        [Required(ErrorMessage = "Loads are required.")]
        public List<string> consolidatedLoads { get; set; }
    }
}
