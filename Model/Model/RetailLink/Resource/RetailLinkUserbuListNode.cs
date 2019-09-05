using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkUserbuListNode
    {
        public bool access { get; set; }
        public string dockTypeOverride { get; set; }
        public string nodeDesc { get; set; }
        public string nodeId { get; set; }
        public string nodeName { get; set; }
        public bool selected { get; set; }
        public List<object> subnodes { get; set; }
    }
}
