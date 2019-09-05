using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.Consolidate
{
    public class ConsolidateMergeResponse
    {
        public bool isMerged { get; set; }
        public List<string> message { get; set; }

        public ConsolidateMergeResponse()
        {
            message = new List<string>();
            isMerged = true;
        }
    }
}
