using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkAvailableAppWindowsRequest
    {
        public int buId { get; set; }
        public string carrierId { get; set; }
        public int deliveryTypeId { get; set; }
        public int nodeId { get; set; }
        public string schedulingFlag { get; set; }
        public List<RetailLinkAvailableAppWindowsRequestShipment> shipment { get; set; }
        public string subnodeId { get; set; }
        public int timezoneId { get; set; }
        public RetailLinkAvailableAppWindowsRequest()
        {
            shipment = new List<RetailLinkAvailableAppWindowsRequestShipment>();
        }
    }
}
