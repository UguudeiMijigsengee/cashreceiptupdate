using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkSearchDeliveryResponse
    {
        public string appointmentDate { get; set; }
        public int buId { get; set; }
        public string carrierName { get; set; }
        public string deliveryId { get; set; }
        public string deliveryNumber { get; set; }
        public string deliveryStatus { get; set; }
        public RetailLinkSearchDeliveryResponseDestinationNodeDets destinationNodeDets { get; set; }
        public RetailLinkSearchDeliveryResponseDestinationSubNodeDets destinationSubNodeDets { get; set; }
        public string lastUpdatedDate { get; set; }
        public string loadNumber { get; set; }
        public RetailLinkSearchDeliveryResponseManageWindowDets manageWindowDets { get; set; }
        public bool nodeLevelCallbackEnabled { get; set; }
        public string poCount { get; set; }
        public object poQuantity { get; set; }
        public string purchaseOrders { get; set; }
        public string scacCarrierID { get; set; }
    }
}
