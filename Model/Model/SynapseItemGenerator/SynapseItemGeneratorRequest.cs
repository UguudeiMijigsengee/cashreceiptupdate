using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Model.SynapseItemGenerator
{
    public class SynapseItemGeneratorRequest
    {
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
    }
}
