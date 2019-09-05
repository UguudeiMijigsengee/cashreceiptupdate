using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.Consolidate
{
    public class ConsolidateResponse
    {
        public bool isConsolidated { get; set; }
        public List<string> message { get; set; }

        public ConsolidateResponse()
        {
            message = new List<string>();
            isConsolidated = true;
        }
    }
}
