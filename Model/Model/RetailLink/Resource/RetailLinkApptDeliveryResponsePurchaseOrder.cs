using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkApptDeliveryResponsePurchaseOrder
    {
        public string channelMethod { get; set; }
        public string convInd { get; set; }
        public string hzmtInd { get; set; }
        public string inventoryTypeId { get; set; }
        public string itemNumber { get; set; }
        public string itemWhseAreaCode { get; set; }
        public string mabd { get; set; }
        public string nodeNumber { get; set; }
        public string omsUpdate { get; set; }
        public string poCancelDate { get; set; }
        public string poCompanyCode { get; set; }
        public string poCountryCode { get; set; }
        public string poCubeQty { get; set; }
        public string poCubeUOM { get; set; }
        public string poDeptNo { get; set; }
        public string poEventCode { get; set; }
        public string poLineCount { get; set; }
        public string poNumber { get; set; }
        public string poOrderDate { get; set; }
        public string poOrderQty { get; set; }
        public string poPaymentType { get; set; }
        public string poShipDate { get; set; }
        public string poStatus { get; set; }
        public string poType { get; set; }
        public string poWeight { get; set; }
        public string poWeightUOM { get; set; }
        public string powhseAreaCode { get; set; }
        public string sequenceId { get; set; }
        public RetailLinkApptDeliveryResponsePurchaseOrderVendor vendor { get; set; }
        public string vendorId { get; set; }
    }
}
