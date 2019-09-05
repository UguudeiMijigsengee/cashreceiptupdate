using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkApptDeliveryRequest
    {
        public string appointmentDate { get; set; }
        public int buId { get; set; }
        public int capacityConfigId { get; set; }
        public string carrierId { get; set; }
        public string delReturns { get; set; }
        public string deliveryStatusId { get; set; }
        public RetailLinkApptDeliveryRequestDeliveryTypeDets deliveryTypeDets { get; set; }
        public int deliveryTypeId { get; set; }
        public int desNodeId { get; set; }
        public RetailLinkApptDeliveryRequestDestinationNodeDets destinationNodeDets { get; set; }
        public RetailLinkApptDeliveryRequestDestinationSubNodeDets destinationSubNodeDets { get; set; }
        public string expectedDeliveryDate { get; set; }
        public string inventoryTransfer { get; set; }
        public string inventoryTypeId { get; set; }
        public RetailLinkApptDeliveryRequestManageWindowDets manageWindowDets { get; set; }
        public string paymentTermId { get; set; }
        public string poCutOff { get; set; }
        public string recurringDeliveryIndicator { get; set; }
        public string scacCarrierID { get; set; }
        public List<RetailLinkApptDeliveryRequestShipment> shipment { get; set; }
        public RetailLinkApptDeliveryRequest()
        {
            shipment = new List<RetailLinkApptDeliveryRequestShipment>();
        }
    }
}
