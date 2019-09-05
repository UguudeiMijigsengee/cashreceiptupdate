using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkLoadPOValidateRequest
    {
        public string bolNbr { get; set; }
        public int buId { get; set; }
        public int caseQty { get; set; }
        public string loadType { get; set; }
        public int nodeId { get; set; }
        public int poLines { get; set; }
        public string poNbr { get; set; }
        public int proNbr { get; set; }
        public string specialPoType { get; set; }
        public string weight { get; set; }
    }
}
