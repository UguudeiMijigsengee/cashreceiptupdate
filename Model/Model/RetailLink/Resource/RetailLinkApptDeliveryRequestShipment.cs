using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkApptDeliveryRequestShipment
    {
        public string billOfLading { get; set; }
        public string convInd { get; set; }
        public string cutOffDate { get; set; }
        public int inventoryTypeId { get; set; }
        public bool isDummy { get; set; }
        public RetailLinkApptDeliveryRequestShipmentLoadTypeDets loadTypeDets { get; set; }
        public int loadTypeId { get; set; }
        public string obsoleteIndicator { get; set; }
        public int poAvailableQty { get; set; }
        public int poCaseQty { get; set; }
        public string poEvent { get; set; }
        public int poLines { get; set; }
        public int poLoadSequenceNbr { get; set; }
        public string poMabd { get; set; }
        public string poNumber { get; set; }
        public string poPaymentTerm { get; set; }
        public int poType { get; set; }
        public int proNumber { get; set; }
        public RetailLinkApptDeliveryRequestShipmentPurchaseOrderDets purchaseOrderDets { get; set; }
        public string specialPoType { get; set; }
        public string vendorNbr { get; set; }
        public string weight { get; set; }
    }
}
