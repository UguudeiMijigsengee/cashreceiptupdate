using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkApptDeliveryResponse
    {
        public bool aisFlag { get; set; }
        public string appointment { get; set; }
        public string appointmentDate { get; set; }
        public string buId { get; set; }
        public RetailLinkApptDeliveryResponseBusinessUnitDets businessUnitDets { get; set; }
        public string capacityConfigId { get; set; }
        public RetailLinkApptDeliveryResponseCarrierDets carrierDets { get; set; }
        public string carrierId { get; set; }
        public string carrierName { get; set; }
        public string commodity { get; set; }
        public string conveyableInd { get; set; }
        public string createUserId { get; set; }
        public string createdBy { get; set; }
        public string delReturns { get; set; }
        public string deliveryId { get; set; }
        public string deliveryNumber { get; set; }
        public string deliveryStatus { get; set; }
        public string deliveryStatusId { get; set; }
        public RetailLinkApptDeliveryResponseDeliveryTypeDets deliveryTypeDets { get; set; }
        public string deliveryTypeId { get; set; }
        public string desNodeId { get; set; }
        public string destinationDcCountry { get; set; }
        public RetailLinkApptDeliveryResponseDestinationNodeDets destinationNodeDets { get; set; }
        public RetailLinkApptDeliveryResponseDestinationSubNodeDets destinationSubNodeDets { get; set; }
        public string eventCode { get; set; }
        public string expectedDeliveryDate { get; set; }
        public string expectedQty { get; set; }
        public string hazmatIndicator { get; set; }
        public string inventoryTransfer { get; set; }
        public string inventoryTypeId { get; set; }
        public string langCode { get; set; }
        public string lastUpdatedBy { get; set; }
        public RetailLinkApptDeliveryResponseManageWindowDets manageWindowDets { get; set; }
        public string obsoleteIndicator { get; set; }        
        public string paymentTerm { get; set; }
        public string paymentTermId { get; set; }
        public string poCount { get; set; }       
        public string poCutOff { get; set; }
        public List<RetailLinkApptDeliveryResponsePurchaseOrder> purchaseOrders { get; set; }
        public List<RetailLinkApptDeliveryResponsePurchaseOrder2> purchaseorder { get; set; }
        public string recurringDeliveryIndicator { get; set; }
        public bool sbFlag { get; set; }
        public string sbIndicator { get; set; }
        public bool sbmerge { get; set; }          
        public string scacCarrierID { get; set; }
        public bool scheduleOnApplicablilityCriteria { get; set; }
        public bool selectedSlotBasedOnApplicability { get; set; }
        public List<RetailLinkApptDeliveryResponseShipment> shipment { get; set; }
        public string userModifiedIndicator { get; set; }
        public string userName { get; set; }
        public string userType { get; set; }           
    }
}
