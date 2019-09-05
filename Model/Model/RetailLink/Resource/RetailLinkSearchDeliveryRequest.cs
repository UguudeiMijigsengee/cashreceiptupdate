using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model.RetailLink.Resource
{
    public class RetailLinkSearchDeliveryRequest
    {
        public string appointmentFromDate { get; set; }
        public string appointmentToDate { get; set; }
        public string appointmentWindow { get; set; }
        public string buId { get; set; }
        public string defaultSearch { get; set; }
        public List<object> deliveryNumber { get; set; }
        public string deliveryStatusId { get; set; }
        public object deliveryTimeIndicator { get; set; }
        public string deliveryTypeId { get; set; }
        public string desNodeList { get; set; }
        public string destinationSubNodeId { get; set; }
        public bool inventoryTransfer { get; set; }
        public string organization { get; set; }
        public string originNodeId { get; set; }
        public string originSubNodeId { get; set; }
        public string paymentTermId { get; set; }
        public string poNbr { get; set; }
        public bool returns { get; set; }
        public string scacCarrierID { get; set; }
        public string trailer { get; set; }
        public string userTypeName { get; set; }
        public RetailLinkSearchDeliveryRequest()
        {
            appointmentFromDate = "";
            appointmentToDate = "";
            appointmentWindow = "";
            buId = "";
            defaultSearch = "Y";
            deliveryStatusId = "";
            deliveryTypeId = "";
            desNodeList = "";
            destinationSubNodeId = "";
            inventoryTransfer = false;
            originNodeId = "";
            originSubNodeId = "";
            returns = false;
            scacCarrierID = "";
            trailer = "";
            deliveryNumber = new List<object>();
        }
    }
}
