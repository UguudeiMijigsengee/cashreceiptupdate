using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkApptDeliveryResponseShipment
    {
        public string billOfLading { get; set; }
        public string convInd { get; set; }
        public string createdBy { get; set; }
        public string createdTs { get; set; }
        public string cutOffDate { get; set; }
        public string deliveryId { get; set; }
        public string hazmatIndicator { get; set; }
        public string inventoryTypeId { get; set; }
        public bool isDummy { get; set; }
        public string lastUpdatedBy { get; set; }
        public string lastUpdatedTs { get; set; }
        public RetailLinkApptDeliveryResponseShipmentLoadTypeDets loadTypeDets { get; set; }
        public string loadTypeId { get; set; }
        public string obsoleteIndicator { get; set; }
        public string poAvailableQty { get; set; }
        public string poCaseQty { get; set; }
        public string poEvent { get; set; }
        public string poLines { get; set; }
        public string poLoadSequenceNbr { get; set; }
        public string poMabd { get; set; }
        public string poNumber { get; set; }
        public string poType { get; set; }
        public string proNumber { get; set; }
        public RetailLinkApptDeliveryResponseShipmentPurchaseOrderDets purchaseOrderDets { get; set; }
        public string specialPoType { get; set; }
        public string vendorNbr { get; set; }
        public string weight { get; set; }
    }
}
