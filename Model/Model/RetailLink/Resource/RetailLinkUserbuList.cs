using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkUserbuList
    {
        public bool access { get; set; }
        public string blockPoDeleteAfterArrival { get; set; }
        public string buId { get; set; }
        public string buName { get; set; }
        public string callbackIndicator { get; set; }
        public string countryId { get; set; }
        public string dockTypeOverride { get; set; }
        public string nmftaEnableIndicator { get; set; }
        public List<RetailLinkUserbuListNode> nodes { get; set; }
        public bool selected { get; set; }
        public string subnodeEnableIndicator { get; set; }
        public string wtmsEnableIndicator { get; set; }
    }
}
