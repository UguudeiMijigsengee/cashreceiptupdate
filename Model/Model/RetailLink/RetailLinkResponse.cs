using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink
{
    public class RetailLinkResponse
    {
        public bool isScheduled { get; set; }
        public string deliveryId { get; set; }
        public string walmartEnforcedDate { get; set; }
        public List<string> message { get; set; }        
        public List<string> loads { get; set; }
        public RetailLinkResponse()
        {
            message = new List<string>();
            loads = new List<string>();
            isScheduled = true;
            deliveryId = "";
            walmartEnforcedDate = "";
        }
    }
}
