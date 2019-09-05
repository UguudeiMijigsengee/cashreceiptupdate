using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.SynapseItemGenerator
{
    public class SynapseItemGeneratorResponse
    {
        public bool isUploaded { get; set; }
        public List<string> message { get; set; }

        public SynapseItemGeneratorResponse()
        {
            message = new List<string>();
            isUploaded = true;
        }
    }
}
